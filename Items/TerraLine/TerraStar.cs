using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.TerraLine
{
	public class TerraStar: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terrarian Star");
			Tooltip.SetDefault("V");
		}
		public override void SetDefaults()
		{
            item.damage = 60;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 48;     //gun image width
            item.height = 24;   //gun image  height
            item.useTime = 12;  //how fast 
            item.useAnimation = 12;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 10000000;
            item.rare = 4;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("Star3"); 
            item.shootSpeed = 16;
			item.expert = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "TrueNightsFury", 1);
			recipe.AddIngredient(null, "TrueHallowedRepeater", 1);
			recipe.AddIngredient(null, "TheHardCore", 3);
			recipe.AddIngredient(ItemID.ScourgeoftheCorruptor, 1);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
