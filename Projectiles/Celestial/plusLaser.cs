using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Celestial
{    
    public class plusLaser : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unholy Light");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(158);
            aiType = 158; 
            projectile.width = 34;
            projectile.height = 34; 
            projectile.timeLeft = 120;
            projectile.penetrate = 1; 
            projectile.friendly = false; 
            projectile.hostile = false; 
            projectile.tileCollide = true;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
            projectile.aiStyle = 1;
			projectile.alpha = 100;
		}
		public override void AI()
		{
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.1f / 255f, (255 - projectile.alpha) * 0.9f / 255f, (255 - projectile.alpha) * 0.3f / 255f);
			if(projectile.timeLeft >= 30)
			{
			projectile.scale += 0.01f;
			}
			else
			{
			projectile.scale -= 0.03f;
			
				int proj = Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, 24, 0, mod.ProjectileType("GreenCellBlast"), projectile.damage, 0, 0);
				Main.projectile[proj].timeLeft = 30;
				
				proj = Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, 0, -24, mod.ProjectileType("GreenCellBlast"), projectile.damage, 0, 0);
				Main.projectile[proj].timeLeft = 30; 
				
				proj = Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, -24, 0, mod.ProjectileType("GreenCellBlast"), projectile.damage, 0, 0);
				Main.projectile[proj].timeLeft = 30;
				
				proj = Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, 0, 24, mod.ProjectileType("GreenCellBlast"), projectile.damage, 0, 0);
				Main.projectile[proj].timeLeft = 30;
			}
			projectile.rotation = 0;
			
		}
	}
}