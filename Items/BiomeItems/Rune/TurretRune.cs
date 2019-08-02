using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems.Rune
{
	public class TurretRune : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Turret Rune");
			Tooltip.SetDefault("Decreases firerate by 5\nDecreases projectile speed by 4\nStacks up to 1\nCreates a clone opportunity");
		}
		public override void SetDefaults()
		{

			item.width = 32;
			item.height = 30;
			item.value = 50000;
			item.rare = 6;
			item.maxStack = 1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "OilRune", 4);
			recipe.AddIngredient(null, "FlowerRune", 4);
			recipe.AddIngredient(null, "BaseRune", 4);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}