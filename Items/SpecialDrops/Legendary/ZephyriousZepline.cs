using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;                    
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.SpecialDrops.Legendary 
{
    public class ZephyriousZepline : ModItem   
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zephyrious Zepline");
			Tooltip.SetDefault("Levels up as you progress\nLegendary Fish");
		}
 
 
        public override void SetDefaults()
        {
 
            item.damage = 15;
            item.melee = true; 
            item.useTime = 22;  
            item.useAnimation = 22;   
            item.useStyle = 5;
            item.channel = true;
            item.knockBack = 2f;
            item.value = Item.sellPrice(1, 25, 0, 0);
			item.rare = 9;
            item.autoReuse = false; 
            item.shoot = mod.ProjectileType("Zepline"); 
            item.noUseGraphic = true; 
            item.noMelee = true;
            item.UseSound = SoundID.Item1; 
	
        }public override void UpdateInventory(Player player)
		{
			
			item.damage = 15;
			item.useTime = 22;
			item.useAnimation = 22;
			
			if(SOTSWorld.legendLevel == 1)
			{
				item.damage = 19;
				item.useTime = 20;
				item.useAnimation = 20;
			}
			if(SOTSWorld.legendLevel == 2)
			{
				item.damage = 23;
				item.useTime = 18;
				item.useAnimation = 18;
			}
			if(SOTSWorld.legendLevel == 3)
			{
				item.damage = 28;
				item.useTime = 17;
				item.useAnimation = 17;
			}
			if(SOTSWorld.legendLevel == 4)
			{
				item.damage = 32;
				item.useTime = 15;
				item.useAnimation = 15;
			}
			if(SOTSWorld.legendLevel == 5)
			{
				item.damage = 40;
				item.useTime = 15;
				item.useAnimation = 15;
			}
			if(SOTSWorld.legendLevel == 6)
			{
				item.damage = 45;
				item.useTime = 13;
				item.useAnimation = 13;
			}
			if(SOTSWorld.legendLevel == 7)
			{
				item.damage = 50;
				item.useTime = 12;
				item.useAnimation = 12;
			}
			if(SOTSWorld.legendLevel == 8)
			{
				item.damage = 58;
				item.useTime = 12;
				item.useAnimation = 12;
			}
			if(SOTSWorld.legendLevel == 9)
			{
				item.damage = 64;
				item.useTime = 12;
				item.useAnimation = 12;
			}
			if(SOTSWorld.legendLevel == 10)
			{
				item.damage = 72;
				item.useTime = 10;
				item.useAnimation = 10;
			}
			if(SOTSWorld.legendLevel == 11)
			{
				item.damage = 80;
				item.useTime = 10;
				item.useAnimation = 10;
			}
			if(SOTSWorld.legendLevel == 12)
			{
				item.damage = 87;
				item.useTime = 8;
				item.useAnimation = 8;
			}
			if(SOTSWorld.legendLevel == 13)
			{
				item.damage = 90;
				item.useTime = 5;
				item.useAnimation = 5;
			}
			if(SOTSWorld.legendLevel == 14)
			{
				item.damage = 94;
				item.useTime = 5;
				item.useAnimation = 5;
			}
			if(SOTSWorld.legendLevel == 14)
			{
				item.damage = 100;
				item.useTime = 5;
				item.useAnimation = 5;
			}
			if(SOTSWorld.legendLevel == 15)
			{
				item.damage = 108;
				item.useTime = 5;
				item.useAnimation = 5;
			}
			if(SOTSWorld.legendLevel == 16)
			{
				item.damage = 116;
				item.useTime = 5;
				item.useAnimation = 5;
			}
			if(SOTSWorld.legendLevel == 17)
			{
				item.damage = 124;
				item.useTime = 5;
				item.useAnimation = 5;
			}
			if(SOTSWorld.legendLevel == 18)
			{
				item.damage = 130;
				item.useTime = 5;
				item.useAnimation = 5;
			}
			if(SOTSWorld.legendLevel == 19)
			{
				item.damage = 138;
				item.useTime = 5;
				item.useAnimation = 5;
			}
			if(SOTSWorld.legendLevel == 20)
			{
				item.damage = 150;
				item.useTime = 5;
				item.useAnimation = 5;
			}
			if(SOTSWorld.legendLevel == 21)
			{
				item.damage = 158;
				item.useTime = 5;
				item.useAnimation = 5;
			}
			if(SOTSWorld.legendLevel == 22)
			{
				item.damage = 164;
				item.useTime = 5;
				item.useAnimation = 5;
			}
			if(SOTSWorld.legendLevel == 23)
			{
				item.damage = 175;
				item.useTime = 5;
				item.useAnimation = 5;
			}
			if(SOTSWorld.legendLevel == 24)
			{
				item.damage = 190;
				item.useTime = 5;
				item.useAnimation = 5;
			}
			if(SOTSWorld.legendLevel == 25)
			{
				item.damage = 220;
				item.useTime = 5;
				item.useAnimation = 5;
			}
			
		}
    }
}