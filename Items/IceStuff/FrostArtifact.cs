using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
// If you are using c# 6, you can use: "using static Terraria.Localization.GameCulture;" which would mean you could just write "DisplayName.AddTranslation(German, "");"
using Terraria.Localization;

namespace SOTS.Items.IceStuff
{
	public class FrostArtifact : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Frost Artifact");
			Tooltip.SetDefault("Has a slot for a key");
		}

		public override void SetDefaults()
		{
			item.width = 44;
			item.height = 26;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = 9;
			item.consumable = true;
			item.createTile = mod.TileType("FrostArtifactTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "HardIceBrick", 25);
			recipe.AddIngredient(null, "FrostedKey", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}