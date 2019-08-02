using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Daggers
{
	public class ATopazDagger : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Topaz Kunai");
			Tooltip.SetDefault("Gross");
		}
		public override void SetDefaults()
		{

			item.damage = 3;
			item.thrown = true;
			item.width = 24;
			item.height = 24;
			item.useTime = 7;
			item.useAnimation = 7;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 3;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;            
			item.shoot = 280; 
            item.shootSpeed = 22;
			item.noUseGraphic = true;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Torch, 10);
			recipe.AddIngredient(ItemID.Topaz, 1);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddIngredient(null, "GoblinRockBar", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}