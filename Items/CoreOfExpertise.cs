using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class CoreOfExpertise: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pre-Hardmode Core of Expertise");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{

			item.width = 42;
			item.height = 42;
			item.value = 0;
			item.rare = 8;
			item.expert = true;
			item.maxStack = 99;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.EoCShield, 1);
			recipe.AddIngredient(ItemID.RoyalGel, 1);
			recipe.AddIngredient(ItemID.WormScarf, 1);
			recipe.AddIngredient(ItemID.BrainOfConfusion, 1);
			recipe.AddIngredient(ItemID.HiveBackpack, 1);
			recipe.AddIngredient(ItemID.BoneGlove, 1);
			recipe.AddIngredient(null, "PinkyCore", 1);
			recipe.AddIngredient(null, "CrypticKnife", 1);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
		}
	}
}