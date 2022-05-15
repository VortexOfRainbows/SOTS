using Microsoft.Xna.Framework;
using SOTS.Items.Slime;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture.Goopwood
{
	public class GoopwoodTable : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.Size = new Vector2(32, 22);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<GoopwoodTableTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Wormwood>(), 8).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class GoopwoodTableTile : Table<GoopwoodTable>
	{

	}
}