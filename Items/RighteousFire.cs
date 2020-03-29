using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;

namespace PoEBridgeMod.Items
{
    class RighteousFire : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("feel the Bern");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.accessory = true;
			item.value = Item.sellPrice(silver: 30);
			item.rare = ItemRarityID.Blue;
		}
		public override int ChoosePrefix(UnifiedRandom rand)
		{
			return rand.Next(new int[] { PrefixID.Arcane, PrefixID.Lucky, PrefixID.Menacing, PrefixID.Quick, PrefixID.Violent, PrefixID.Warding });
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<PoEBridgeModPlayer>().RighteousFire = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ManaCrystal, 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
