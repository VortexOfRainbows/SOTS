using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class CoreOfExpertise1: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hardmode Core of Expertise");
			Tooltip.SetDefault("*You are filled with determination");
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
			recipe.AddIngredient(ItemID.FlyingDutchmanTrophy, 1);
			recipe.AddIngredient(ItemID.MartianSaucerTrophy, 1);
			recipe.AddIngredient(ItemID.MinecartMech, 1);
			recipe.AddIngredient(ItemID.SporeSac, 1);
			recipe.AddIngredient(ItemID.ShinyStone, 1);
			recipe.AddIngredient(ItemID.ShrimpyTruffle, 1);
			recipe.AddIngredient(null, "DesertEye", 1);
			recipe.AddIngredient(null, "IceDeployer", 1);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
		}
	}
}