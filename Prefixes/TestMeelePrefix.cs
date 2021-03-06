﻿using PoEBridgeMod.Items;
using Terraria;
using Terraria.ModLoader;

namespace PoEBridgeMod.Prefixes
{
    class TestMeelePrefix : ModPrefix, IMeelePrefix
	{

		// determines if it can roll at all.
		// use this to control if a prefixes can be rolled or not
		public override bool CanRoll(Item item)
			=> false;

		// change your category this way, defaults to Custom
		public override PrefixCategory Category
			=> PrefixCategory.Melee;

		public TestMeelePrefix()
		{
		}

		// Allow multiple prefix autoloading this way (permutations of the same prefix)
		public override bool Autoload(ref string name)
		{
			if (!base.Autoload(ref name))
			{
				return false;
			}

			mod.AddPrefix("TestMeele", new TestMeelePrefix());
			return false;
		}

		public override void Apply(Item item)
			=> item.GetGlobalItem<GemPrefixGlobalItem>().prefixType = "TestMeele";

	}
}
