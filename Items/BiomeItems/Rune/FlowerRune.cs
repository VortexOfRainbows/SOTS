using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems.Rune
{
	public class FlowerRune : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flower Rune");
			Tooltip.SetDefault("Fire an intense spread of bullets\nDecreases damage by 1\nDecreases firerate by 2\nIncreases projectile speed by 2.5\nStacks up to 4\nResponds to a clone opportunity\nA tier rune");
		}
		public override void SetDefaults()
		{

			item.width = 44;
			item.height = 44;
			item.value = 50000;
			item.rare = 6;
			item.maxStack = 4;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SingleShotRune", 3);
			recipe.AddIngredient(null, "ShotgunRune", 1);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}