using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class PyramidSlab : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pyramid Slab");
			Tooltip.SetDefault("A slab from an ancient burial site, it may be hard to break");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.rare = ItemRarityID.Orange;
			item.consumable = true;
			item.createTile = ModContent.TileType<PyramidSlabTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SandstoneSlab, 2);
			recipe.AddTile(TileID.Autohammer);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SandstoneSlab, 2);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(ItemID.SandstoneSlab, 2);
			recipe.AddRecipe();
		}
	}
}