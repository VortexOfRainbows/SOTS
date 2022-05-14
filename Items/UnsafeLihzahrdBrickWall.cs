using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class UnsafeLihzahrdBrickWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unsafe Lihzahrd Brick Wall");
			Tooltip.SetDefault("Allows Lihzahrd Temple mobs to spawn");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.Red;
			Item.createWall = WallID.LihzahrdBrickUnsafe;
		}
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
			recipe.AddIngredient(ItemID.LihzahrdBrick, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
			
			recipe = new Recipe(mod);
			recipe.AddIngredient(ItemID.LihzahrdBrickWall, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}