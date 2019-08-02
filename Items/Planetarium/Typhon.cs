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
	public class Typhon : ModItem
	{	float boost = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Typhon");
			Tooltip.SetDefault("Grants aimbot");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 38;     
            item.height = 70;   
            item.value = 1000000;
            item.rare = 9;
			item.accessory = true;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			for(int j = 0; j < 1000; j++)
			{
				Projectile projectile = Main.projectile[j];
					float minDist = 325;
					int target2 = -1;
					float dX = 0f;
					float dY = 0f;
					float distance = 0;
					float speed = (float) Math.Sqrt((double)(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y));
					if(projectile.friendly == true && projectile.hostile == false && player == Main.player[projectile.owner] && projectile.active)
					{
						for(int i = 0; i < Main.npc.Length - 1; i++)
						{
							NPC target = Main.npc[i];
							if(!target.friendly && target.dontTakeDamage == false)
							{
								dX = target.Center.X - projectile.Center.X;
								dY = target.Center.Y - projectile.Center.Y;
								distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
								if(distance < minDist)
								{
									minDist = distance;
									target2 = i;
								}
							}
						}
						
						if(target2 != -1)
						{
						NPC toHit = Main.npc[target2];
							if(toHit.active == true)
							{
								
							dX = toHit.Center.X - projectile.Center.X;
							dY = toHit.Center.Y - projectile.Center.Y;
							distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
							speed /= distance;
						   
							projectile.velocity = new Vector2(dX * speed, dY * speed);
							projectile.tileCollide = false;
							}
						}
						
						
						
					}
								
				
			}
			
			
		
			
			
		}
	}
}
