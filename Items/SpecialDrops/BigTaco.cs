using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;                     	
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.SpecialDrops 	
{
    public class BigTaco : ModItem   	
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Big Taco");
			Tooltip.SetDefault("");
		}
 
 
        public override void SetDefaults()
        {
 
            item.damage = 99;	
            item.melee = true;   	
            item.width = 30;
			item.height = 28;
            item.useTime = 22; 	
            item.useAnimation = 22;	
            item.useStyle = 5;	
            item.channel = true;	
            item.knockBack = 2f;	
            item.value = Item.buyPrice(1, 0, 0, 0); 	
            item.rare = 3;	
            item.autoReuse = false;	
            item.shoot = mod.ProjectileType("BigTaco");	       
            item.noUseGraphic = true;	
            item.noMelee = true;	
            item.UseSound = SoundID.Item1;	
			
		}
    }
}