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
using PoEBridgeMod.Items;
using PoEBridgeMod.Prefixes;
using PoEBridgeMod.Prefixes.PrefixTypes;

namespace PoEBridgeMod
{
    class PoEBridgeModPlayer : ModPlayer
    {
        public bool RighteousFire;

		public string oldName = null;
		// ResetEffects is used to reset effects back to their default value. Terraria resets all effects every frame back to defaults so we will follow this design. 
		// (You might think to set a variable when an item is equipped and unassign the value when the item in unequipped, but Terraria is not designed that way.)
		public override void ResetEffects()
		{
			RighteousFire = false;
		}
		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
		{
			if(player.HeldItem.GetGlobalItem<GemPrefixGlobalItem>().prefixType != null)
			{
				ModPrefix prefix = this.mod.GetPrefix(player.HeldItem.GetGlobalItem<GemPrefixGlobalItem>().prefixType);
				if (prefix is IProjOnHitPrefix iHPrefix)
				{
					iHPrefix.OnHitNPCWithProj(proj, target, damage, knockback, crit);
				} 
			}
			base.OnHitNPCWithProj(proj, target, damage, knockback, crit);
		}

		public override void PreUpdate()
		{

			if (RighteousFire){
				Lighting.AddLight((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f), 0.65f, 0.4f, 0.1f);
				int num = 24;
				float num2 = 200f;
				bool flag = player.infernoCounter % 60 == 0;
				int damage = 10;
				if (player.whoAmI == Main.myPlayer)
				{
					for (int l = 0; l < 200; l++)
					{
						NPC nPC = Main.npc[l];
						if (nPC.active && !nPC.friendly && nPC.damage > 0 && !nPC.dontTakeDamage && !nPC.buffImmune[num] && Vector2.Distance(player.Center, nPC.Center) <= num2)
						{
							if (nPC.FindBuffIndex(num) == -1)
							{
								nPC.AddBuff(num, 120, false);
							}
							if (flag)
							{
								player.ApplyDamageToNPC(nPC, damage, 0f, 0, false);
							}
						}
					}
					if (player.hostile)
					{
						for (int m = 0; m < 255; m++)
						{
							Player playerFound = Main.player[m];
							if (playerFound != player && player.active && !player.dead && player.hostile && !player.buffImmune[num] && (player.team != player.team || player.team == 0) && Vector2.Distance(player.Center, player.Center) <= num2)
							{
								if (player.FindBuffIndex(num) == -1)
								{
									player.AddBuff(num, 120, true);
								}
								if (flag)
								{
									player.Hurt(PlayerDeathReason.LegacyEmpty(), damage, 0, true, false, false, -1);
									if (Main.netMode != 0)
									{
										PlayerDeathReason reason = PlayerDeathReason.ByPlayer(player.whoAmI);
										NetMessage.SendPlayerHurt(m, reason, damage, 0, false, true, 0, -1, -1);
									}
								}
							}
						}
					}
				}
			}
		}
	}
}

