using PoEBridgeMod.Items;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using PoEBridgeMod.Projectiles;
using static Terraria.ModLoader.ModContent;

namespace PoEBridgeMod.Prefixes
{
    class SocketPrefix : ModPrefix
    {
		public SocketPrefix()
		{
		}
		// determines if it can roll at all.
		// use this to control if a prefixes can be rolled or not
		public override bool CanRoll(Item item)
			=> false;
		public override PrefixCategory Category
		=> PrefixCategory.AnyWeapon;
		public override bool Autoload(ref string name)
		{
			if (!base.Autoload(ref name))
			{
				return false;
			}

			mod.AddPrefix("Sockets", new SocketPrefix());
			return false;
		}

		public override void Apply(Item item)
			=> item.GetGlobalItem<GemPrefixGlobalItem>().socketNumber = 5;
	}
}
