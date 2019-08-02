using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems.Rune
{
	public class SingleShotRune : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shot Rune");
			Tooltip.SetDefault("Increases damage by 3\nIncreases firerate by 1\nIncreases projectile speed by 0.5\nStacks up to 12\nResponds to a clone opportunity\nB tier rune, conflicts with other B tiers");
		}
		public override void SetDefaults()
		{

			item.width = 32;
			item.height = 12;
			item.value = 50000;
			item.rare = 6;
			item.maxStack = 12;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BaseRune", 2);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}