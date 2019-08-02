using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Daggers
{
	public class AAmethystDagger : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amethyst Kunai");
			Tooltip.SetDefault("Purpur");
		}
		public override void SetDefaults()
		{

			item.damage = 22;
			item.thrown = true;
			item.width = 24;
			item.height = 24;
			item.useTime = 18;
			item.useAnimation = 24;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;            
			item.shoot = 156; 
            item.shootSpeed = 12;
			item.noUseGraphic = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wood, 10);
			recipe.AddIngredient(ItemID.Amethyst, 1);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddIngredient(null, "GoblinRockBar", 1);

			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}