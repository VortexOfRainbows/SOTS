using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems
{
	public class MargritBoomer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Margrit Boomer");
			Tooltip.SetDefault("Deflects enemy projectiles");
		}
		public override void SetDefaults()
		{
            item.damage = 18; 
            item.thrown = true;   //originally thrown but changed so i dont have to worry about how much damage the split will do
            item.width = 18;   
            item.height = 34;  
            item.useTime = 23;  
            item.useAnimation = 23;
            item.useStyle = 1;    
            item.noMelee = true; 
            item.knockBack = 1;
            item.value = 110000;
            item.rare = 6;
			item.consumable = false;
			item.maxStack = 1;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot =  mod.ProjectileType("MargritBoomer"); 
            item.shootSpeed = 12.5f;
			item.noUseGraphic = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "MargritCore", 1);
			recipe.AddIngredient(null, "ObsidianScale", 18);
			recipe.AddIngredient(3081, 30);
			recipe.AddIngredient(3086, 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
