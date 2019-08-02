using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems.Rune
{
	public class BoomerangRune : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boomerang Rune");
			Tooltip.SetDefault("Decreases firerate by 4\nDecreases projectile speed by 2.5\nStacks up to 1\nCreates a clone opportunity");
		}
		public override void SetDefaults()
		{

			item.width = 24;
			item.height = 24;
			item.value = 50000;
			item.rare = 6;
			item.maxStack = 1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BaseRune", 20);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}