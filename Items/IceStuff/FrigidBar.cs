using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.IceStuff
{
	public class FrigidBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Bar");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 22;
            item.value = Item.sellPrice(0, 0, 2, 50);
			item.rare = 2;
			item.maxStack = 99;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Goblinsteel", 1);
			recipe.AddIngredient(null, "FragmentOfPermafrost", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
        }
	}
}