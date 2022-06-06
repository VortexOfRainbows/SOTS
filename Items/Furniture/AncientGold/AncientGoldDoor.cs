using SOTS.Items.Pyramid;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Furniture.AncientGold
{
	public class AncientGoldDoor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Gold Door");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.width = 18;
			Item.height = 32;
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<AncientGoldDoorClosed>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<RoyalGoldBrick>(), 6).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class AncientGoldDoorClosed : CompleteDoor<AncientGoldDoor, AncientGoldDoorClosed.AncientGoldDoorOpen>
	{
		public class AncientGoldDoorOpen : OpenVariant<AncientGoldDoorClosed>
		{
		}
	}
}