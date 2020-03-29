using Microsoft.Xna.Framework;
using PoEBridgeMod.Prefixes;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using static Terraria.ModLoader.ModContent;

namespace PoEBridgeMod.Items
{
	public class GemPrefixGlobalItem : GlobalItem
	{
		public string originalOwner;
		public string prefixType;
		public GemPrefixGlobalItem()
		{
			originalOwner = "";
		}

		public override bool InstancePerEntity => true;

		public override GlobalItem Clone(Item item, Item itemClone)
		{
			GemPrefixGlobalItem myClone = (GemPrefixGlobalItem)base.Clone(item, itemClone);
			myClone.originalOwner = originalOwner;
			myClone.prefixType = prefixType;
			return myClone;
		}

		public override int ChoosePrefix(Item item, UnifiedRandom rand)
		{
			if ((item.accessory || item.damage > 0) && item.maxStack == 1 && rand.NextBool(30))
			{
				return mod.PrefixType(rand.Next(2) == 0 ? "Awesome" : "ReallyAwesome");
			}
			return -1;
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			// TODO
			if (!item.social && item.prefix > 0)
			{
					TooltipLine line = new TooltipLine(mod, "PrefixTornadoShot", "Tornado Shot")
					{
						isModifier = true
					};
					tooltips.Add(line);
			}
			if (originalOwner.Length > 0)
			{
				TooltipLine line = new TooltipLine(mod, "CraftedBy", "Crafted by: " + originalOwner)
				{
					overrideColor = Color.LimeGreen
				};
				tooltips.Add(line);

				/*foreach (TooltipLine line2 in tooltips)
				{
					if (line2.mod == "Terraria" && line2.Name == "ItemName")
					{
						line2.text = originalOwner + "'s " + line2.text;
					}
				}*/
			}

			foreach (TooltipLine line3 in tooltips)
			{
				if (line3.mod == "Terraria" && line3.Name == "ItemName")
				{
					line3.text = line3.text + (item.modItem != null ? " [" + item.modItem.mod.DisplayName + "]" : "");
				}
			}
		}

		public override void OnCraft(Item item, Recipe recipe)
		{
			if (item.maxStack == 1)
			{
				originalOwner = Main.LocalPlayer.name;
			}
		}

		public override void NetSend(Item item, BinaryWriter writer)
		{
			writer.Write(originalOwner);
			writer.Write(prefixType);
		}

		public override void NetReceive(Item item, BinaryReader reader)
		{
			originalOwner = reader.ReadString();
			prefixType = reader.ReadString();
		}

		public override bool Shoot(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if(item.GetGlobalItem<GemPrefixGlobalItem>().prefixType != null)
			{
				if (this.mod.GetPrefix(prefixType) is IRangePrefix)
				{
					damage = 1000;
				}
				if (this.mod.GetPrefix(prefixType) is IMeelePrefix)
				{
					damage = 10000;
				}

			}
			return true;
		}
	}
}
