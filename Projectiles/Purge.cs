using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class Purge : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Purge");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 38;
            projectile.height = 38; 
            projectile.timeLeft = 255;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
            projectile.aiStyle = 3; //18 is the demon scythe style
			projectile.alpha = 0;
		}
		public override void Kill(int timeLeft)
		{
				if(projectile.owner == Main.myPlayer)
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.Next(-3, 4), Main.rand.Next(-3, 4), mod.ProjectileType("Dracula2"), (int)(projectile.damage * 1) + 1, 0, 0);
		}
		public override void AI()
		{
			
		projectile.alpha++;
			if(projectile.timeLeft % 5 == 0)
			{
				for(int i = 0; i < 360; i += 45)
				{
				Vector2 circularLocation = new Vector2(-20, 0).RotatedBy(MathHelper.ToRadians(i));
				
				int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 258);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				
				}
			}
		}
	}
	
}