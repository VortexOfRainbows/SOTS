using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Daggers
{
	public class AEmeraldDagger : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Emerald Kunai");
			Tooltip.SetDefault("Cursed");
		}
		public override void SetDefaults()
		{

			item.damage = 8;
			item.thrown = true;
			item.width = 24;
			item.height = 24;
			item.useTime = 12;
			item.useAnimation = 12;
			item.useStyle = 1;
			item.knockBack = 0;
			item.value = 10000;
			item.rare = 3;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;            
			item.shoot = 478; 
            item.shootSpeed = 30;
			item.noUseGraphic = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.JungleSpores, 3);
			recipe.AddIngredient(ItemID.Emerald, 1);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddIngredient(null, "GoblinRockBar", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}