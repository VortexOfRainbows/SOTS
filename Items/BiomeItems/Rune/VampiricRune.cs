using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems.Rune
{
	public class VampiricRune : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vampiric Rune");
			Tooltip.SetDefault("Decreases damage by 2\nIncreases firerate by 2\nAttacks gain 5% lifesteal\nStacks up to 4\nC tier rune, conflicts with other C tiers");
		}
		public override void SetDefaults()
		{

			item.width = 22;
			item.height = 22;
			item.value = 50000;
			item.rare = 6;
			item.maxStack = 4;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "OilRune", 1);
			recipe.AddIngredient(null, "BaseRune", 2);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this,2);
			recipe.AddRecipe();
		}
	}
}