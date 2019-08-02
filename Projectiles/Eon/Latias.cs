using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Eon
{    
    public class Latias : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Latias");
			
		}
		
        public override void SetDefaults()
        {
		
		

				projectile.netImportant = true;
			projectile.CloneDefaults(317);
            aiType = 317; //18 is the demon scythe style
            projectile.width = 56;
            projectile.height = 44; 
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = false; 
			projectile.minion = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = false;
			projectile.minionSlots = 0;		
			projectile.alpha = 0;


		}
		public override void AI() //The projectile's AI/ what the projectile does
		{
			Player player = Main.player[projectile.owner];
			Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), 0.6f, 0.9f, 0.3f);
			
			
              

           }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player owner = Main.player[projectile.owner];
            target.immune[projectile.owner] = 2;
			int healtheal = Main.rand.Next(0, 3);
			owner.statLife += healtheal;
			if(healtheal > 0)
			owner.HealEffect(healtheal);
        }
	
			
      

			
			  
		}
		}
	
	

