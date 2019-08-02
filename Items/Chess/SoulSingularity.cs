using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Chess
{
	public class SoulSingularity : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Singularity");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{

			item.width = 40;
			item.height = 40;
			item.value = 0;
			item.rare = 7;
			item.maxStack = 9999;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SoulofMight, 1);
			recipe.AddIngredient(ItemID.SoulofFright, 1);
			recipe.AddIngredient(ItemID.SoulofSight, 1);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}