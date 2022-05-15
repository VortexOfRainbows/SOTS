using Microsoft.Xna.Framework;
using SOTS.Items.Slime;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture.Goopwood
{
	public class GoopwoodBed : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.Size = new Vector2(32, 22);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<GoopwoodBedTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Wormwood>(), 15).AddIngredient(ItemID.Silk, 5).AddTile(TileID.Sawmill).Register();
		}
	}
	public class GoopwoodBedTile : Bed<GoopwoodBed>
	{

	}
}