using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;


namespace SOTS.Items.Planetarium
{
	public class Pallas : ModItem
	{	int boost = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pallas");
			Tooltip.SetDefault("Rapidly accelerates projectile speed and increases projectile damage while in air");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 22;     
            item.height = 32;   
            item.value = 1000000;
            item.rare = 9;
			item.accessory = true;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			boost++;
			for(int j = 0; j < 1000; j++)
			{
				Projectile projectile = Main.projectile[j];
					if(projectile.friendly == true && projectile.hostile == false && player == Main.player[projectile.owner] && projectile.damage > 1 && !projectile.minion && !projectile.sentry)
					{
						projectile.velocity.X *= 1.0575f;
						projectile.velocity.Y *= 1.0575f;
						if(boost >= 25)
						{
							if(projectile.damage < 200)
							{
							projectile.damage = (int)(projectile.damage * 1.25f);
							}
						projectile.damage += 5;
						}
					}
								
				
			}
			
			if(boost >= 25)
			{
			boost = 0;
			}
		
			
			
		}
	}
}
