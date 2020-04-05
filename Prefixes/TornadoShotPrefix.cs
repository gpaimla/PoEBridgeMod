using PoEBridgeMod.Items;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using PoEBridgeMod.Projectiles;
using static Terraria.ModLoader.ModContent;

namespace PoEBridgeMod.Prefixes
{
    class TornadoShotPrefix : ModPrefix, IRangePrefix
	{
		public TornadoShotPrefix()
		{
		}

		public override bool CanRoll(Item item)
			=> false;

		public override bool Autoload(ref string name)
		{
			if (!base.Autoload(ref name))
			{
				return false;
			}

			mod.AddPrefix("TornadoShot", new TornadoShotPrefix());
			return false;
		}

		public override void Apply(Item item)
			=> item.GetGlobalItem<GemPrefixGlobalItem>().prefixType = "TornadoShot";

		public void CustomShoot(Item item, Player player, Vector2 position, float speedX, float speedY, int damage, float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ProjectileType<TornadoShotProj>(), damage, knockBack, player.whoAmI);
		}
	}
}
