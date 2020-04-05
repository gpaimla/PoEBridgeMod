using PoEBridgeMod.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using PoEBridgeMod.Items;
using static Terraria.ModLoader.ModContent;

namespace PoEBridgeMod.UI
{
    internal class ClarissaUIIdentify : UIState
    {
        private VanillaItemSlotWrapper _vanillaItemSlot;
		public override void OnInitialize()
		{
			_vanillaItemSlot = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem, 0.85f)
			{
				Left = { Pixels = 50 },
				Top = { Pixels = 270 },
				ValidItemFunc = item => item.IsAir || !item.IsAir && item.Prefix(-3)
			};
			Append(_vanillaItemSlot);
		}
		public override void OnDeactivate()
		{
			if (!_vanillaItemSlot.Item.IsAir)
			{
				Main.LocalPlayer.QuickSpawnClonedItem(_vanillaItemSlot.Item, _vanillaItemSlot.Item.stack);
				_vanillaItemSlot.Item.TurnToAir();
			}
		}
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (Main.LocalPlayer.talkNPC == -1 || Main.npc[Main.LocalPlayer.talkNPC].type != NPCType<Clarissa>())
			{
				GetInstance<PoEBridgeMod>().ClarissaUserInterface.SetState(null);
			}
		}

		private bool tickPlayed;
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);

			Main.HidePlayerCraftingMenu = true;

			const int slotX = 50;
			const int slotY = 270;
			if (!_vanillaItemSlot.Item.IsAir)
			{
				int identifyPrice = Item.buyPrice(0, 5, 0, 0);

				string costText = Language.GetTextValue("LegacyInterface.46") + ": ";
				string coinsText = "";
				int[] coins = Utils.CoinsSplit(identifyPrice);
				if (coins[3] > 0)
				{
					coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinPlatinum).Hex3() + ":" + coins[3] + " " + Language.GetTextValue("LegacyInterface.15") + "] ";
				}
				if (coins[2] > 0)
				{
					coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinGold).Hex3() + ":" + coins[2] + " " + Language.GetTextValue("LegacyInterface.16") + "] ";
				}
				if (coins[1] > 0)
				{
					coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinSilver).Hex3() + ":" + coins[1] + " " + Language.GetTextValue("LegacyInterface.17") + "] ";
				}
				if (coins[0] > 0)
				{
					coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinCopper).Hex3() + ":" + coins[0] + " " + Language.GetTextValue("LegacyInterface.18") + "] ";
				}
				ItemSlot.DrawSavings(Main.spriteBatch, slotX + 130, Main.instance.invBottom, true);
				ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, costText, new Vector2(slotX + 50, slotY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
				ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, coinsText, new Vector2(slotX + 50 + Main.fontMouseText.MeasureString(costText).X, (float)slotY), Color.White, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
				int reforgeX = slotX + 70;
				int reforgeY = slotY + 40;
				bool hoveringOverReforgeButton = Main.mouseX > reforgeX - 15 && Main.mouseX < reforgeX + 15 && Main.mouseY > reforgeY - 15 && Main.mouseY < reforgeY + 15 && !PlayerInput.IgnoreMouseInterface;
				Texture2D reforgeTexture = Main.reforgeTexture[hoveringOverReforgeButton ? 1 : 0];
				Main.spriteBatch.Draw(reforgeTexture, new Vector2(reforgeX, reforgeY), null, Color.White, 0f, reforgeTexture.Size() / 2f, 0.8f, SpriteEffects.None, 0f);
				if (hoveringOverReforgeButton)
				{
					Main.hoverItemName = Language.GetTextValue("LegacyInterface.19");
					if (!tickPlayed)
					{
						Main.PlaySound(12, -1, -1, 1, 1f, 0f);
					}
					tickPlayed = true;
					Main.LocalPlayer.mouseInterface = true;
					if (Main.mouseLeftRelease && Main.mouseLeft && Main.LocalPlayer.CanBuyItem(identifyPrice, -1) && ItemLoader.PreReforge(_vanillaItemSlot.Item))
					{
						Main.LocalPlayer.BuyItem(identifyPrice, -1);
						bool favorited = _vanillaItemSlot.Item.favorited;
						int stack = _vanillaItemSlot.Item.stack;
						Item reforgeItem = new Item();
						reforgeItem.netDefaults(_vanillaItemSlot.Item.netID);
						Main.NewText(_vanillaItemSlot.Item.prefix);
						reforgeItem = reforgeItem.CloneWithModdedDataFrom(_vanillaItemSlot.Item);
						reforgeItem.Prefix(GetInstance<PoEBridgeMod>().PrefixType("Sockets"));
						_vanillaItemSlot.Item = reforgeItem.Clone();
						_vanillaItemSlot.Item.position.X = Main.LocalPlayer.position.X + (float)(Main.LocalPlayer.width / 2) - (float)(_vanillaItemSlot.Item.width / 2);
						_vanillaItemSlot.Item.position.Y = Main.LocalPlayer.position.Y + (float)(Main.LocalPlayer.height / 2) - (float)(_vanillaItemSlot.Item.height / 2);
						_vanillaItemSlot.Item.favorited = favorited;
						_vanillaItemSlot.Item.stack = stack;
						Main.PlaySound(SoundID.Item37, -1, -1);
					}
				}
				else
				{
					tickPlayed = false;
				}
			}
			if (_vanillaItemSlot.Item.IsAir)
			{
				string message = "Place item here";
				ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, message, new Vector2(slotX + 50, 280), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
			}
		}

	}
}
