using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles 
{    
    public class EctoRocket : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ectoplasm Glyph");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(616);
            aiType = 616; //18 is the demon scythe style
            projectile.width = 28;
            projectile.height = 40; 
            projectile.timeLeft = 6000;
            projectile.penetrate = 1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = true;;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
		}
		
		public override void AI()
        {
			
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 18, 32, 206);
			
        }
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player  = Main.player[target.target];	
			if(target.type == mod.NPCType("Libra"))
			{
				{
					Main.NewText("Took a long time to get this one didn't it?", 255, 255, 255);
					
				}
					Item.NewItem((int)target.Center.X, (int)target.Center.Y, target.width, target.height, (mod.ItemType("OMaterial")), 1);
					target.timeLeft = 1;
					target.timeLeft--;
					target.life = 0;
					
					}
		}
		public override void Kill(int timeLeft)
		{
				if(projectile.owner == Main.myPlayer)
				{
					Projectile.NewProjectile(projectile.Center.X + 48, projectile.Center.Y + 0, 4, 0,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					
					Projectile.NewProjectile(projectile.Center.X + 40, projectile.Center.Y + 8, 4, 0,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					
					Projectile.NewProjectile(projectile.Center.X + 32, projectile.Center.Y + 16, 4, 0,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X + 24, projectile.Center.Y + 24, 4, 0,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X + 16, projectile.Center.Y + 32, 4, 0,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					
					Projectile.NewProjectile(projectile.Center.X + 40, projectile.Center.Y - 8, 4, 0,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					
					Projectile.NewProjectile(projectile.Center.X + 32, projectile.Center.Y - 16, 4, 0,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X + 24, projectile.Center.Y - 24, 4, 0,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X + 16, projectile.Center.Y - 32, 4, 0,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					
					
					Projectile.NewProjectile(projectile.Center.X - 48, projectile.Center.Y + 0, -4, 0,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					
					Projectile.NewProjectile(projectile.Center.X - 40, projectile.Center.Y + 8, -4, 0,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					
					Projectile.NewProjectile(projectile.Center.X - 32, projectile.Center.Y + 16, -4, 0,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X - 24, projectile.Center.Y + 24, -4, 0,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X - 16, projectile.Center.Y + 32, -4, 0,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					
					Projectile.NewProjectile(projectile.Center.X - 40, projectile.Center.Y - 8, -4, 0,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					
					Projectile.NewProjectile(projectile.Center.X - 32, projectile.Center.Y - 16, -4, 0,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X - 24, projectile.Center.Y - 24, -4, 0,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X - 16, projectile.Center.Y - 32, -4, 0,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					
					//up		
					
					
					Projectile.NewProjectile(projectile.Center.X + 0, projectile.Center.Y + 48, 0, 4,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					
					Projectile.NewProjectile(projectile.Center.X + 8, projectile.Center.Y + 40, 0, 4,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					
					Projectile.NewProjectile(projectile.Center.X + 16, projectile.Center.Y + 32, 0, 4,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X + 24, projectile.Center.Y + 24, 0, 4,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X + 32, projectile.Center.Y + 16, 0, 4,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					
					Projectile.NewProjectile(projectile.Center.X - 8, projectile.Center.Y + 40, 0, 4,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					
					Projectile.NewProjectile(projectile.Center.X - 16, projectile.Center.Y + 32, 0, 4,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X - 24, projectile.Center.Y + 24, 0, 4,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X - 32, projectile.Center.Y + 16, 0, 4,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					
					
					Projectile.NewProjectile(projectile.Center.X + 0, projectile.Center.Y - 48, 0, -4,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					
					Projectile.NewProjectile(projectile.Center.X + 8, projectile.Center.Y - 40, 0, -4,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					
					Projectile.NewProjectile(projectile.Center.X + 16, projectile.Center.Y - 32, 0, -4,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X + 24, projectile.Center.Y - 24, 0,-4,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X + 32, projectile.Center.Y - 16, 0, -4,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					
					Projectile.NewProjectile(projectile.Center.X - 8, projectile.Center.Y - 40, 0, -4,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					
					Projectile.NewProjectile(projectile.Center.X - 16, projectile.Center.Y - 32, 0, -4,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X - 24, projectile.Center.Y - 24, 0, -4,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X - 32, projectile.Center.Y - 16, 0, -4,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					
					
					
					
					
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 4, 4,   mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -4, -4,   mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 4, -4,   mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -4, 4,   mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 4,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, -4,   mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 4, 0,   mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -4, 0,   mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					
					
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -2, 4,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -2, -4,   mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 4, -2,   mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -4, -2,   mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 2, 4,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 2, -4,   mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 4, 2,   mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -4, 2,   mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					
					
					Projectile.NewProjectile(projectile.Center.X + 48, projectile.Center.Y + 48, 4, 4,   mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X - 48, projectile.Center.Y - 48, -4, -4,   mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X + 48, projectile.Center.Y - 48, 4, -4,   mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X - 48, projectile.Center.Y + 48, -4, 4,   mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y + 48, 0, 4,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 48, 0, -4,   mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X + 48, projectile.Center.Y, 4, 0,   mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X - 48, projectile.Center.Y, -4, 0,   mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					
					
					Projectile.NewProjectile(projectile.Center.X + 24, projectile.Center.Y - 48, -2, 4,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X + 24, projectile.Center.Y + 48, -2, -4,   mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X - 48, projectile.Center.Y + 24, 4, -2,   mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X + 48, projectile.Center.Y + 24, -4, -2,   mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X - 24, projectile.Center.Y - 48, 2, 4,  mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X - 24, projectile.Center.Y + 48, 2, -4,   mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X - 48, projectile.Center.Y - 24, 4, 2,   mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X + 48, projectile.Center.Y - 24, -4, 2,   mod.ProjectileType("EctoSpark"), (int)(projectile.damage * .3), projectile.knockBack, Main.myPlayer);

			}
		}
	}	
}
			