using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems.Rune
{
	public class CloudRune : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cloud Rune");
			Tooltip.SetDefault("Makes projectiles hover over their enemies\nStacks up to 1\nD tier rune, conflicts with other D tiers");
		}
		public override void SetDefaults()
		{

			item.width = 14;
			item.height = 28;
			item.value = 50000;
			item.rare = 6;
			item.maxStack = 1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "RocketRune", 1);
			recipe.AddIngredient(null, "BaseRune", 4);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}