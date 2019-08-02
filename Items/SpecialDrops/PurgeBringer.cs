using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class PurgeBringer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Purge Bringer");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
            item.damage = 62;  
            item.thrown = true;   
            item.width = 38;    
            item.height = 38;    
            item.useTime = 12;  
            item.useAnimation = 12;
            item.useStyle = 1;    
            item.noMelee = true; 
            item.knockBack = 1;
            item.value = 1000;
            item.rare = 10;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot =  mod.ProjectileType("Purge"); 
            item.shootSpeed = 12;
			item.noUseGraphic = true;
		}
	}
}
