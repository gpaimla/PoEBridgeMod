using PoEBridgeMod.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;

namespace PoEBridgeMod
{
	public class PoEBridgeMod : Mod
	{


		internal UserInterface ClarissaUserInterface;
		public PoEBridgeMod()
		{
		}

		public override void Load()
		{
			if (!Main.dedServ) {
				ClarissaUserInterface = new UserInterface();
			}
		}
		public override void UpdateUI(GameTime gameTime)
		{
			ClarissaUserInterface?.Update(gameTime);
		}
		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
			if (inventoryIndex != -1)
			{
				layers.Insert(inventoryIndex, new LegacyGameInterfaceLayer(
					"PoEBridgeMod: Clarissa UI",
					delegate {
						// If the current UIState of the UserInterface is null, nothing will draw. We don't need to track a separate .visible value.
						ClarissaUserInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}