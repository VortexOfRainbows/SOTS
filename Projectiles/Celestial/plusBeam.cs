using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Celestial
{    
    public class plusBeam : ModProjectile 
    {	int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Light");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(158);
            aiType = 158; 
            projectile.width = 34;
            projectile.height = 34; 
            projectile.timeLeft = 90;
            projectile.penetrate = 1; 
            projectile.friendly = false; 
            projectile.hostile = false; 
            projectile.tileCollide = true;;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
            projectile.aiStyle = 1;
			projectile.alpha = 100;
			//projectile.netImportant = true;
		}
		public override void AI()
		{
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * .8f / 355f, (255 - projectile.alpha) * 2.0f / 355f, (255 - projectile.alpha) * 2.5f / 355f);
			
			timer++;
			if(timer <= 35)
			{
			projectile.scale += 0.01f;
			}
			else
			{
			projectile.scale -= 0.01f;
			}
			projectile.rotation = 0;
		}
		public override void Kill(int timeLeft)
		{
			int proj = Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y,0, 5, 435, projectile.damage, 0, 0);
			Main.projectile[proj].timeLeft = 30;
			
			proj = Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, 5, 0, 435, projectile.damage, 0, 0);
			Main.projectile[proj].timeLeft = 30;
			
			proj = Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, 0, -5, 435, projectile.damage, 0, 0);
			Main.projectile[proj].timeLeft = 30;
			
			proj = Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, -5, 0, 435, projectile.damage, 0, 0);
			Main.projectile[proj].timeLeft = 30;
			
		}
	}
}