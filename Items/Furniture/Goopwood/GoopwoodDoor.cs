using SOTS.Items.Slime;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Furniture.Goopwood
{
	public class GoopwoodDoor : ModItem
	{
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.rare = ItemRarityID.Blue;
			item.width = 20;
			item.height = 36;
			item.createTile = ModContent.TileType<GoopwoodDoorClosed>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
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