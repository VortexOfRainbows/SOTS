using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Daggers
{
	public class ADiamondDagger : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diamond Kunai");
			Tooltip.SetDefault("Pure");
		}
		public override void SetDefaults()
		{

			item.damage = 6;
			item.thrown = true;
			item.width = 24;
			item.height = 24;
			item.useTime = 8;
			item.useAnimation = 8;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;            
			item.shoot = 10; 
            item.shootSpeed = 20;
			item.noUseGraphic = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.PlatinumBar, 1);
			recipe.AddIngredient(ItemID.Diamond, 1);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddIngredient(null, "GoblinRockBar", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}