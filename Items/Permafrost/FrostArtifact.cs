using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
// If you are using c# 6, you can use: "using static Terraria.Localization.GameCulture;" which would mean you could just write "DisplayName.AddTranslation(German, "");"
using Terraria.Localization;

namespace SOTS.Items.Permafrost
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
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.width = 44;
			Item.height = 26;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<FrostArtifactTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<HardIceBrick>(), 100);
			recipe.AddIngredient(ModContent.ItemType<FrostedKey>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}