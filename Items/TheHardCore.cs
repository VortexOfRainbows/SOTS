using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class TheHardCore: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hard Core");
			Tooltip.SetDefault("I hope you had a good time grinding");
		}
		public override void SetDefaults()
		{

			item.width = 62;
			item.height = 62;
			item.value = 0;
			item.rare = 8;	
			item.expert = true;
			item.maxStack = 99;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "CoreOfCreation", 1);
			recipe.AddIngredient(null, "CoreOfExpertise1", 1);
			recipe.AddIngredient(null, "CoreOfStatus", 1);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
		}
	}
}