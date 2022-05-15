using SOTS.Items.Slime;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Furniture.Goopwood
{
	public class GoopwoodDoor : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Blue;
			Item.width = 20;
			Item.height = 36;
			Item.createTile = ModContent.TileType<GoopwoodDoorClosed>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Wormwood>(), 6).AddTile(TileID.WorkBenches).Register();
		}
    }
    public class GoopwoodDoorClosed : CompleteDoor<GoopwoodDoor, GoopwoodDoorClosed.GoopwoodDoorOpen>
    {
        public class GoopwoodDoorOpen : OpenVariant<GoopwoodDoorClosed>
        {

        }
    }
}