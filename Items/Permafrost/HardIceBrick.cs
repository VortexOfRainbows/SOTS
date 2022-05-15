using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
// If you are using c# 6, you can use: "using static Terraria.Localization.GameCulture;" which would mean you could just write "DisplayName.AddTranslation(German, "");"
using Terraria.Localization;

namespace SOTS.Items.Permafrost
{
	public class HardIceBrick : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hard Ice Brick");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Cyan;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<HardIceBrickTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(50).AddIngredient(ItemID.IceBlock, 50).AddIngredient(ModContent.ItemType<AbsoluteBar>(), 1).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}