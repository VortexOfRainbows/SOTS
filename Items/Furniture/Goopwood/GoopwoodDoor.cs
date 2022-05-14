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
			Recipe recipe = new Recipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Wormwood>(), 6);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
    public class GoopwoodDoorClosed : CompleteDoor<GoopwoodDoor, GoopwoodDoorClosed.GoopwoodDoorOpen>
    {
        public class GoopwoodDoorOpen : OpenVariant<GoopwoodDoorClosed>
        {

        }
    }
}