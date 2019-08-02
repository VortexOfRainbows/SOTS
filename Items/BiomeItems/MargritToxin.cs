using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems
{
	public class MargritToxin : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Margrit Toxin");
			Tooltip.SetDefault("Inflicts enemies with the Margrit Toxin debuff\nEnemies that die while affected will fire out more Margrit Toxins in all directions");
		}
		public override void SetDefaults()
		{
            item.damage = 33; 
            item.thrown = false;   //originally thrown but changed so i dont have to worry about how much damage the split will do
            item.width = 38;   
            item.height = 38;  
            item.useTime = 8;  
            item.useAnimation = 24;
            item.useStyle = 1;    
            item.noMelee = true; 
            item.knockBack = 1;
            item.value = 333;
            item.rare = 6;
			item.consumable = true;
			item.maxStack = 999;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.shoot =  mod.ProjectileType("MargritToxin"); 
            item.shootSpeed = 13.5f;
			item.noUseGraphic = true;
		}public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "MargritCore", 1);
			recipe.AddIngredient(null, "ObsidianScale", 4);
			recipe.AddIngredient(3081, 4);
			recipe.AddIngredient(3086, 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 333);
			recipe.AddRecipe();
		}
	}
}
