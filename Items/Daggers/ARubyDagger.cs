using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Daggers
{
	public class ARubyDagger : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ruby Kunai");
			Tooltip.SetDefault("Blood");
		}
		public override void SetDefaults()
		{

			item.damage = 4;
			item.thrown = true;
			item.width = 24;
			item.height = 24;
			item.useTime = 6;
			item.useAnimation = 6;
			item.useStyle = 1;
			item.knockBack = 0.2f;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("VampireShot"); 
            item.shootSpeed = 23;
			item.noUseGraphic = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LifeCrystal, 1);
			recipe.AddIngredient(ItemID.Ruby, 1);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddIngredient(null, "GoblinRockBar", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}