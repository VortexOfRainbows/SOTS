using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems.Rune
{
	public class RailRune : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rail Rune");
			Tooltip.SetDefault("Accelerates projectile damage and speed during travel\nStacks up to 1\nC tier rune, conflicts with other C tiers");
		}
		public override void SetDefaults()
		{

			item.width = 22;
			item.height = 22;
			item.value = 50000;
			item.rare = 6;
			item.maxStack = 1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "LaserRune", 1);
			recipe.AddIngredient(null, "BaseRune", 8);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}