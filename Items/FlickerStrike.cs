using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PoEBridgeMod.Projectiles;
using System.Diagnostics;

namespace PoEBridgeMod.Items
{
	public class FlickerStrike : ModItem
	{
		Stopwatch timeFromLastTeleport = new Stopwatch();
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Meele damage");
		}

		public override void SetDefaults()
		{
			item.damage = 0;
			item.useTime = 1;
			item.useAnimation = 1;
			item.noMelee = true;
			item.useStyle = 4; // holding up
			item.knockBack = 0;
			item.value = 10000;
			item.rare = 2;
			// item.shoot = ModContent.ProjectileType<FlickerWeaponProj>();
			item.channel = true;
			item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DirtBlock, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool UseItem(Player player)
		{
			Item itemToFlickerWith = player.inventory[0];
			if (itemToFlickerWith.melee) { 
				if (!this.timeFromLastTeleport.IsRunning || this.timeFromLastTeleport.ElapsedMilliseconds > (2*(itemToFlickerWith.useTime)*3)) {
					this.timeFromLastTeleport.Restart();
					float distanceToTarget = 300f;
					bool target = false;
					NPC targetNPC = new NPC();
					Vector2 targetCenter = player.position;
					for (int i = 0; i < Main.maxNPCs; i++)
					{
						NPC npc = Main.npc[i];
						if (npc.CanBeChasedBy()) { 
							float between = Vector2.Distance(npc.Center, player.Center);
							bool inRange = between < 300f;
							bool lineOfSight = Collision.CanHit(player.position, player.width, player.height, npc.position, npc.width, npc.height);
							if (((target && between > distanceToTarget && inRange) || !target && inRange) && (lineOfSight))
							{
								distanceToTarget = between;
								targetCenter = npc.position;
								target = true;
								targetNPC = npc;
							}
						}
					}

					if (target)
					{
						player.Teleport(targetCenter);
						int damageToDeal = itemToFlickerWith.damage;
						int critChance = Main.player[Main.myPlayer].meleeCrit - Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem].crit + itemToFlickerWith.crit;
						ItemLoader.GetWeaponCrit(Main.HoverItem, player, ref critChance);
						PlayerHooks.GetWeaponCrit(player, Main.HoverItem, ref critChance);
						bool crit = false;
						if ((float)Main.rand.Next(1, 101) <= critChance)
						{
							crit = true;
							player.addDPS(damageToDeal * 2);
						}
						else
						{
							player.addDPS(damageToDeal);
						}

						player.ApplyDamageToNPC(targetNPC, damageToDeal, 0, player.direction, crit);
						return true;
					}
				}
			}
			return false;
		}

	}
}