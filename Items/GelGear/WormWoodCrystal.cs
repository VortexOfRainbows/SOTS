using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GelGear
{
	public class WormWoodCrystal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Worm Wood Crystal");
			Tooltip.SetDefault("Throws a crystal that explodes into gelatinous projectiles");
		}
		public override void SetDefaults()
		{
            item.damage = 12; 
            item.thrown = true; 
            item.width = 22; 
            item.height = 22; 
            item.useTime = 35;  
            item.useAnimation = 35;
            item.useStyle = 1;    
            item.noMelee = true; 
            item.knockBack = 1;
            item.value = Item.sellPrice(0, 0, 0, 60);
            item.rare = 4;
			item.consumable = true;
			item.maxStack = 999;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot =  mod.ProjectileType("PinkyCrystal"); 
            item.shootSpeed = 12;
			item.noUseGraphic = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "WormWoodCore", 1);
			recipe.AddIngredient(null, "SlimeyFeather", 4);
			recipe.AddIngredient(null, "GelBar", 4);
			recipe.AddIngredient(ItemID.Wood, 4);
			recipe.AddIngredient(ItemID.PinkGel, 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 333);
			recipe.AddRecipe();
		}
	}
}
