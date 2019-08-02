using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chess
{    
    public class SightSpazmamini : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Spazmamini");
			
		}
		
        public override void SetDefaults()
        {
		
		

projectile.netImportant = true;
			projectile.CloneDefaults(388);
            aiType = 388; //18 is the demon scythe style
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
			
			
              

           }
        public override void OnHitNPC(NPC target, int damage, float knockBack, bool crit)
        {
			Player owner = Main.player[projectile.owner];
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 5, 0, 228, damage, knockBack, 0);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -5, 0, 228, damage, knockBack, 0);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 5, 228, damage, knockBack, 0);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, -5, 228, damage, knockBack, 0);
            target.immune[projectile.owner] = 0;
        }
	
			
      

			
			  
		}
		}
	
	

