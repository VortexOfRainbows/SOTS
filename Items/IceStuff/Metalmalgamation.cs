using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;                    
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.IceStuff
{
    public class Metalmalgamation : ModItem   
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Metalmalgamation");
			Tooltip.SetDefault("'An aura of death surrounds the foul childrens' toy'");
		}
 
 
        public override void SetDefaults()
        {
 
            item.damage = 55;
            item.melee = true; 
            item.useTime = 22;  
            item.useAnimation = 22;   
            item.useStyle = 5;
            item.channel = true;
            item.knockBack = 3.5f;
            item.value = Item.sellPrice(0, 7, 0, 0);
			item.rare = 7;
            item.autoReuse = false; 
            item.shoot = mod.ProjectileType("Metalmalgamation"); 
            item.noUseGraphic = true; 
            item.noMelee = true;
            item.UseSound = SoundID.Item1; 
	
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "AbsoluteBar", 11);
			recipe.SetResult(this);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipe();
		}
    }
}