using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles 
{    
    public class CrackedGate : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crack The Sky");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(158);
            aiType = 158; //18 is the demon scythe style
            projectile.penetrate = -1; 
            projectile.width = 30;
            projectile.height = 30; 
            projectile.tileCollide = false;
            projectile.thrown = true; 
			projectile.timeLeft = 600;
			projectile.alpha = 0;
		}
		
		public override void AI()
        {
			if(projectile.owner == Main.myPlayer)
			{
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, 0, -12, mod.ProjectileType("CrackedBeam"), (int)(projectile.damage * 1f), 0, 0);
				wait = 0;
				Main.PlaySound(2,(int)projectile.position.X, (int)projectile.position.Y, 9);
			
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 30, 30, 160);
			}
		}
	}
}
			