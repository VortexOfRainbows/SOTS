using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.SoldStuff
{
	public class EnchantedGoggles : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Goggles");
			Tooltip.SetDefault("Grants permanent night owl and shine potion buffs");
		}
		public override void SetDefaults()
		{
      
            item.width = 14;     
            item.height = 8;   
            item.value = 125000;
            item.rare = 4;
			item.accessory = true;
			item.defense = 1;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
			
					player.AddBuff(BuffID.Shine, 300);
					player.AddBuff(BuffID.NightOwl, 300);
			
            
					
		}
		
	}
}
