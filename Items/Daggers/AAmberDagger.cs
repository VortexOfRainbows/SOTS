using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Daggers
{
	public class AAmberDagger : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amber Kunai");
			Tooltip.SetDefault("Utility!");
		}
		public override void SetDefaults()
		{

			item.damage = 0;
			item.thrown = true;
			item.width = 24;
			item.height = 24;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;            
			item.shoot = 171; 
            item.shootSpeed = 12;
			item.noUseGraphic = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DemoniteBar, 1);
			recipe.AddIngredient(ItemID.Amber, 1);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddIngredient(null, "GoblinRockBar", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}