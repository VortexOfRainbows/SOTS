using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Star
{    
    public class Star3 : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14; 
            projectile.timeLeft = 30;
            projectile.penetrate = 1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = true;;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
            projectile.aiStyle = 48; //18 is the demon scythe style
			projectile.alpha = 100;
		}
		public override void Kill(int timeLeft)
		{	
			int numberProjectiles = 1;
              for (int i = 0; i < numberProjectiles; i++)
              {
			
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 8, 8,  mod.ProjectileType("Star4"), (int)(projectile.damage * .1), projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -8, -8,  mod.ProjectileType("Star4"), (int)(projectile.damage * .1), projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 8, -8,  mod.ProjectileType("Star4"), (int)(projectile.damage * .1), projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -8, 8,  mod.ProjectileType("Star4"), (int)(projectile.damage * .1), projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0, 10,  mod.ProjectileType("Star4"), (int)(projectile.damage * .1), projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0, -10,  mod.ProjectileType("Star4"), (int)(projectile.damage * .1), projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 10, 0,  mod.ProjectileType("Star4"), (int)(projectile.damage * .1), projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -10, 0,  mod.ProjectileType("Star4"), (int)(projectile.damage * .1), projectile.knockBack, Main.myPlayer);
			  
			  }
			
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player  = Main.player[target.target];	
			if(target.type == mod.NPCType("Libra"))
			{
				Main.NewText("... Here we go again...", 255, 255, 255);
					
				
					Item.NewItem((int)target.position.X, (int)target.position.Y, target.width, target.height, (mod.ItemType("VMaterial")), 1);
					target.timeLeft = 1;
					target.timeLeft--;
					target.life = 0;
					
			}
				
					
			}
}
		
	}
	
