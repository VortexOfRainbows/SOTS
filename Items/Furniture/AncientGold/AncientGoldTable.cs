using Microsoft.Xna.Framework;
using SOTS.Items.Pyramid;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture.AncientGold
{
	public class AncientGoldTable : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Gold Table");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.Size = new Vector2(38, 24);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<AncientGoldTableTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<RoyalGoldBrick>(), 8).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class AncientGoldTableTile : Table<AncientGoldTable>
	{

	}
}