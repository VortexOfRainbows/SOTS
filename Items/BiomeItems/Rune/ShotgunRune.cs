using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems.Rune
{
	public class ShotgunRune : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shotgun Rune");
			Tooltip.SetDefault("Fire a spread of bullets\nDecreases damage by 2\nDecreases firerate by 1\nIncreases projectile speed by 1\nStacks up to 8\nResponds to a clone opportunity\n\nA tier rune");
		}
		public override void SetDefaults()
		{

			item.width = 40;
			item.height = 36;
			item.value = 50000;
			item.rare = 6;
			item.maxStack = 8;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SingleShotRune", 2);
			recipe.AddIngredient(null, "BaseRune", 1);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}