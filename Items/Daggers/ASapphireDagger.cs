using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Daggers
{
	public class ASapphireDagger : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sapphirre Kunai");
			Tooltip.SetDefault("Light");
		}
		public override void SetDefaults()
		{

			item.damage = 5;
			item.thrown = true;
			item.width = 24;
			item.height = 24;
			item.useTime = 9;
			item.useAnimation = 9;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;            
			item.shoot = 310; 
            item.shootSpeed = 40;
			item.noUseGraphic = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.GlowingMushroom, 35);
			recipe.AddIngredient(ItemID.Sapphire, 1);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddIngredient(null, "GoblinRockBar", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}