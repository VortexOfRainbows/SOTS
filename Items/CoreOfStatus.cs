using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class CoreOfStatus : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Status Core");
			Tooltip.SetDefault("Made with the souls of life");
		}
		public override void SetDefaults()
		{

			item.width = 42;
			item.height = 40;
			item.value = 0;
			item.rare = 8;
			item.expert = true;
			item.maxStack = 99;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "RedPowerChamber", 1);
			recipe.AddIngredient(null, "BluePowerChamber", 1);
			recipe.AddIngredient(ItemID.LifeFruit, 1);
			recipe.AddIngredient(ItemID.DemonHeart, 1);
			recipe.AddIngredient(ItemID.SoulofFlight, 10);
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddIngredient(ItemID.SoulofLight, 10);
			recipe.AddIngredient(ItemID.GoldenCrate, 1);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
		}
	}
}