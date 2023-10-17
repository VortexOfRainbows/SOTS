using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
// If you are using c# 6, you can use: "using static Terraria.Localization.GameCulture;" which would mean you could just write "DisplayName.AddTranslation(German, "");"
using Terraria.Localization;

namespace SOTS.Items.Permafrost
{
	public class HardIceBrick : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(100);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Cyan;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<HardIceBrickTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(200).AddIngredient(ItemID.IceBlock, 200).AddIngredient<AbsoluteBar>(1).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}