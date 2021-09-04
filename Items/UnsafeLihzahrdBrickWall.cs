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
			item.CloneDefaults(ItemID.StoneWall);
			item.width = 24;
			item.height = 24;
			item.rare = 9;
			item.createWall = WallID.LihzahrdBrickUnsafe;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LihzahrdBrick, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LihzahrdBrickWall, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}