using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class UnsafeLihzahrdBrickWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unsafe Lihzahrd Brick Wall");
			Tooltip.SetDefault("Allows Lihzahrd Temple mobs to spawn");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.Red;
			Item.createWall = WallID.LihzahrdBrickUnsafe;
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(ItemID.LihzahrdBrick, 1).AddTile(TileID.WorkBenches).Register();
			CreateRecipe(1).AddIngredient(ItemID.LihzahrdBrickWall, 1).AddTile(TileID.WorkBenches).Register();
		}
	}
}