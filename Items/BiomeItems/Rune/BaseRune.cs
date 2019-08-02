using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems.Rune
{
	public class BaseRune : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Base Rune");
			Tooltip.SetDefault("Increases damage by 1\nDecreases firerate by 1\nDecreases projectile speed by 1.5\nStacks up to 20\nA tier rune");
		}
		public override void SetDefaults()
		{

			item.width = 22;
			item.height = 30;
			item.value = 50000;
			item.rare = 6;
			item.maxStack = 20;
		}
		public override void AddRecipes()
		{
			//ModRecipe recipe = new ModRecipe(mod);
			//recipe.AddIngredient(null, "SingleShotRune", 2);
			//recipe.AddIngredient(null, "BaseRune", 2);
			//recipe.AddTile(TileID.CrystalBall);
			//recipe.SetResult(this);
			//recipe.AddRecipe();
		}
	}
}