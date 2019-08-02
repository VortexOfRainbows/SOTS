using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems.Rune
{
	public class RocketRune : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rocket Rune");
			Tooltip.SetDefault("Increases damage by 4\nIncreases firerate by 1\nIncreases projectile speed by 3\nGrants homing\nStacks up to 2\nD tier rune, conflicts with other D tiers");
		}
		public override void SetDefaults()
		{

			item.width = 34;
			item.height = 34;
			item.value = 50000;
			item.rare = 6;
			item.maxStack = 2;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SingleShotRune", 8);
			recipe.AddIngredient(null, "ShotgunRune", 3);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}