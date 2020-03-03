using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;                    
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.SpecialDrops
{
    public class ZephyrousZeppelin : ModItem   
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zephyrous Zeppelin");
			Tooltip.SetDefault("Surrounded by a ring of razorwater");
		}
        public override void SetDefaults()
        {
            item.damage = 22;
            item.melee = true; 
            item.useTime = 25;  
            item.useAnimation = 25;   
            item.useStyle = 5;
            item.channel = true;
            item.knockBack = 2f;
            item.value = Item.sellPrice(0, 1, 50, 0);
			item.rare = 4;
            item.autoReuse = false; 
            item.shoot = mod.ProjectileType("Zeppelin"); 
            item.noUseGraphic = true; 
            item.noMelee = true;
            item.UseSound = SoundID.Item1; 
		}
    }
}