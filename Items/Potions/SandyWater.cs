using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Potions
{
	public class SandyWater : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sandy Water");
			Tooltip.SetDefault("Suffocate your enemies");
		}
		public override void SetDefaults()
		{

			item.damage = 32;
			item.thrown = true;
			item.width = 24;
			item.height = 24;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 4;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;            
			item.shoot = mod.ProjectileType("SandyWater"); 
            item.shootSpeed = 12;
			item.noUseGraphic = true;
            item.consumable = true;       
			item.maxStack = 999;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater, 2);
			recipe.AddIngredient(null, "SandFish", 1);
			recipe.AddIngredient(ItemID.PixieDust, 1);
			recipe.AddTile(13);
			recipe.SetResult(this, 80);
			recipe.AddRecipe();
		}
	}
}