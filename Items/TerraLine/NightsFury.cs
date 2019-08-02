using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.TerraLine
{
	public class NightsFury : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Night's Fury");
			Tooltip.SetDefault("Bow of the night");
		}
		public override void SetDefaults()
		{
            item.damage = 50;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 16;     //gun image width
            item.height = 42;   //gun image  height
            item.useTime = 32;  //how fast 
            item.useAnimation = 32;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 4;
            item.value = 250000;
            item.rare = 6;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("NightsArrow"); 
            item.shootSpeed = 100;
			item.reuseDelay = 12;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DemonBow, 1);
			recipe.AddIngredient(ItemID.BeesKnees, 1);
			recipe.AddIngredient(null, "TheSkull", 1);
			recipe.AddIngredient(null, "Sharanga", 1);

			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
