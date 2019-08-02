using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems.Rune
{
	public class OilRune : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Oil Rune");
			Tooltip.SetDefault("Decreases damage by 16\nIncreases firerate by 8\nDecreases shotspread\nProjectiles ignore enemy defense\nStacks up to 1\nB tier rune, conflicts with other B tiers");
		}
		public override void SetDefaults()
		{

			item.width = 40;
			item.height = 36;
			item.value = 50000;
			item.rare = 6;
			item.maxStack = 1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "LaserRune", 1);
			recipe.AddIngredient(null, "FlowerRune", 1);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}