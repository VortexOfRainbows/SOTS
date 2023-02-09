using Microsoft.Xna.Framework;
using SOTS.Items.Pyramid;
using Terraria;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture.AncientGold
{
	public class AncientGoldBed : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Gold Bed");
			Tooltip.SetDefault("'For naps fit for a king'");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.Size = new Vector2(32, 22);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<AncientGoldBedTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<RoyalGoldBrick>(), 15).AddIngredient(ItemID.Silk, 5).AddTile(TileID.Sawmill).Register();
		}
	}
	public class AncientGoldBedTile : Bed<AncientGoldBed>
	{

	}
}