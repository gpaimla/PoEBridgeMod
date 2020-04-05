using PoEBridgeMod.Items;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;


namespace PoEBridgeMod.Prefixes.PrefixTypes
{
    interface IProjOnHitPrefix
    {
        void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit);
    }
}
