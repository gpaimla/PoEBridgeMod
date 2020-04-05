using Microsoft.Xna.Framework;
using PoEBridgeMod.Prefixes;
using System;
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
		public int socketNumber;
		public GemPrefixGlobalItem()
		{
			originalOwner = "";
			prefixType = "";
			socketNumber = 0;
		}

		public override bool InstancePerEntity => true;
		public bool didIShoot = false;
		public override bool NeedsSaving(Item item)
		{
			return prefixType.Length > 0 || prefixType.Length > 0 || socketNumber > 0;
		}
		public override TagCompound Save(Item item)
		{
			return new TagCompound {
				{"originalOwner", originalOwner},
				{"prefixType", prefixType},
				{"socketNumber", socketNumber},
			};
		}
		public override void Load(Item item, TagCompound tag)
		{
			originalOwner = tag.GetString("originalOwner");
			prefixType = tag.GetString("prefixType");
			socketNumber = tag.GetInt("socketNumber");
		}
		public override GlobalItem Clone(Item item, Item itemClone)
		{
			GemPrefixGlobalItem myClone = (GemPrefixGlobalItem)base.Clone(item, itemClone);
			myClone.originalOwner = originalOwner;
			myClone.prefixType = prefixType;
			myClone.socketNumber = socketNumber;
			return myClone;
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			// TODO
			if (!item.social && item.prefix > 0)
			{
				if(socketNumber > 0) { 
					TooltipLine line = new TooltipLine(mod, "Sockets", "Sockets: " + socketNumber)
					{
						overrideColor = Color.LimeGreen	
					
					};
					tooltips.Add(line);
				}
				TooltipLine line2 = new TooltipLine(mod, "TornadoShot", "Prefixes: " + prefixType)
				{
					overrideColor = Color.LimeGreen
				};
				tooltips.Add(line2);
			}
			if (originalOwner.Length > 0)
			{
				TooltipLine line = new TooltipLine(mod, "CraftedBy", "Crafted by: " + originalOwner)
				{
					overrideColor = Color.LimeGreen
				};
				tooltips.Add(line);
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
			// need to think what im gonna even do here, bows, easy to do
			if (this.didIShoot)
			{
				this.didIShoot = false;
				return true; // false
			}
			if (item.GetGlobalItem<GemPrefixGlobalItem>().prefixType != null)
			{
				ModPrefix prefix = this.mod.GetPrefix(prefixType);
				if (prefix is IRangePrefix irPrefix)
				{
					// irPrefix.CustomShoot(item, player, position, speedX, speedY, damage, knockBack);
					return true; // false
				}
			}

			return true;
		}

		public override bool CanUseItem(Item item, Player player)
		{
			// need to think what im gonna even do here, guns, need custom timer, clunky, hard
			if (item.GetGlobalItem<GemPrefixGlobalItem>().prefixType != null)
			{
				ModPrefix prefix = this.mod.GetPrefix(prefixType);
				if (prefix is IRangePrefix irPrefix)
				{
					// terraria source code for angles
					if (item.useAmmo > 0)
					{
						bool canShoot = true;
						int shoot = item.shoot;
						float shootSpeed = item.shootSpeed;
						int damage = item.damage;
						float knockBack = item.knockBack;
						player.PickAmmo(item, ref shoot, ref shootSpeed, ref canShoot, ref damage, ref knockBack, true);
					}
					float num75 = item.shootSpeed;
					Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
					float num81 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
					float num82 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
					if (player.gravDir == -1f)
					{
						num82 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector2.Y;
					}
					float num83 = (float)Math.Sqrt((double)(num81 * num81 + num82 * num82));
					float num84 = num83;
					if ((float.IsNaN(num81) && float.IsNaN(num82)) || (num81 == 0f && num82 == 0f))
					{
						num81 = (float)player.direction;
						num82 = 0f;
						num83 = num75;
					}
					else
					{
						num83 = num75 / num83;
					}
					float num151 = num81;
					float num152 = num82;
					float num153 = 0.05f * (float)3;
					num151 += (float)Main.rand.Next(-35, 36) * num153;
					num152 += (float)Main.rand.Next(-35, 36) * num153;
					num83 = (float)Math.Sqrt((double)(num151 * num151 + num152 * num152));
					num83 = num75 / num83;
					num151 *= num83;
					num152 *= num83;
					float x4 = vector2.X;
					float y4 = vector2.Y;
					// irPrefix.CustomShoot(item, player, new Vector2(x4,y4), num151, num152, item.damage, item.knockBack);
					this.didIShoot = true;
				}
				else
				{
					this.didIShoot = false;

				}
			}
			return base.CanUseItem(item, player);
		}

	}
}
