using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PoEBridgeMod
{
    class PoEBridgeModPlayer : ModPlayer
    {
        public bool RighteousFire;
		// ResetEffects is used to reset effects back to their default value. Terraria resets all effects every frame back to defaults so we will follow this design. 
		// (You might think to set a variable when an item is equipped and unassign the value when the item in unequipped, but Terraria is not designed that way.)
		public override void ResetEffects()
		{
			RighteousFire = false;
		}
		public static readonly PlayerLayer MiscEffects = new PlayerLayer("ExampleMod", "MiscEffects", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo) {
			if (drawInfo.shadow != 0f)
			{
				return;
			}
			Player drawPlayer = drawInfo.drawPlayer;
			Mod mod = ModLoader.GetMod("PoEBridgeMod");
			PoEBridgeModPlayer modPlayer = drawPlayer.GetModPlayer<PoEBridgeModPlayer>();
			if (modPlayer.RighteousFire)
			{
				Texture2D texture = mod.GetTexture("Items/RighteousFire");
				int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
				int drawY = (int)(drawInfo.position.Y - 4f - Main.screenPosition.Y);
				DrawData data = new DrawData(texture, new Vector2(drawX, drawY), null, Lighting.GetColor((int)((drawInfo.position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.position.Y - 4f - texture.Height / 2f) / 16f)), 0f, new Vector2(texture.Width / 2f, texture.Height), 1f, SpriteEffects.None, 0);
				Main.playerDrawData.Add(data);
			}
		});
		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			MiscEffects.visible = true;
			layers.Add(MiscEffects);
		}

	}
}

