using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems.Rune
{
	public class ThunderRune : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thunder Rune");
			Tooltip.SetDefault("Increases damage by 10\nAttacks induce paralysis\nStacks up to 1\nC tier rune, conflicts with other C tiers");
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
			recipe.AddIngredient(null, "RailRune", 1);
			recipe.AddIngredient(null, "BaseRune", 2);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}