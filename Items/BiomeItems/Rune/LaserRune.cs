using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems.Rune
{
	public class LaserRune : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Laser Rune");
			Tooltip.SetDefault("Increases damage by 5\nIncreases firerate by 5\nIncreases projectile speed by 5\nDecreases shotspread\nStacks up to 2\nResponds to a clone opportunity\nD tier rune, conflicts with other D tiers");
		}
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 8;
			item.value = 50000;
			item.rare = 6;
			item.maxStack = 2;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SingleShotRune", 7);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this,1);
			recipe.AddRecipe();
		}
	}
}