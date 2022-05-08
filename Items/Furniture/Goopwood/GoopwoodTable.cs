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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Wormwood>(), 8);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class GoopwoodTableTile : Table<GoopwoodTable>
	{

	}
}