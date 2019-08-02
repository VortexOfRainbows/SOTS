using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chess
{    
    public class SightRetanimini : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Retanimini");
			
		}
		
        public override void SetDefaults()
        {
		
		

projectile.netImportant = true;
			projectile.CloneDefaults(387);
            aiType = 387; //18 is the demon scythe style
            projectile.penetrate = -1; 
			projectile.minion = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = false;
			projectile.minionSlots = 1;
			projectile.alpha = 0;


		}
		public override void AI() //The projectile's AI/ what the projectile does
		{
			Player player = Main.player[projectile.owner];
			Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), 0.6f, 0.9f, 0.3f);
			   wait++;
			if(wait >= 30)
			{
				wait = 0;
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("BulletBubble"), projectile.damage, 0, 0);
				
			}
              

           }
	
			
      

			
			  
		}

		}
	
	

