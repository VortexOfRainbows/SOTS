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
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Orange;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<PyramidSlabTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.SandstoneSlab, 2).AddTile(TileID.Autohammer).Register();
			CreateRecipe(1).AddIngredient(ItemID.SandstoneSlab, 2).AddTile(TileID.LunarCraftingStation).Register();
			CreateRecipe(2).AddIngredient(this, 1).AddTile(TileID.WorkBenches).ReplaceResult(ItemID.SandstoneSlab);
		}
	}
}