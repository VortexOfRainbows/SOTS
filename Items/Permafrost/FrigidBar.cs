using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	public class FrigidBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Bar");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.IronBar);
			item.width = 24;
			item.height = 22;
            item.value = Item.sellPrice(0, 0, 6, 0);
			item.rare = ItemRarityID.Green;
			item.maxStack = 99;
			item.placeStyle = 8;
			item.createTile = ModContent.TileType<TheBars>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FrigidIce>(), 10);
			recipe.AddIngredient(ModContent.ItemType<Goblinsteel>(), 1);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfPermafrost>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
        }
	}
}