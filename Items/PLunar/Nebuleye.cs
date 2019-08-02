using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.PLunar
{
	public class Nebuleye : ModItem
	{int Probe = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebuleye");
			Tooltip.SetDefault("Let infinity have no bounds\n25% increased magic damage and crit chance");
		}
		public override void SetDefaults()
		{
	
      
            item.width = 16;     
            item.height = 16;   
            item.value = 125000;
            item.rare = 11;

			item.accessory = true;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
			player.magicCrit += 25;
			player.magicDamage += 0.25f;
			Probe++;
			if(Probe == 15)
			{
				Probe = 0;
				
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("NebulousTear"), 102, 0, player.whoAmI);
			}
		}
	}
}