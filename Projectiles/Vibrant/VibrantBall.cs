using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Vibrant 
{    
    public class VibrantBall : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant Ball");
		}
        public override void SetDefaults()
        {
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.tileCollide = true;
			projectile.penetrate = 1;
			projectile.width = 34;
			projectile.height = 34;
			projectile.alpha = 0;
			projectile.timeLeft = 30;
		}
		public override void Kill(int timeLeft)
		{
            Main.PlaySound(2, (int)(projectile.Center.X), (int)(projectile.Center.Y), 28, 0.6f);

			for(int i = 0; i < 20; i++)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 44);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 1.2f;
				Main.dust[num1].alpha = 200;
			}
			if (Main.myPlayer == projectile.owner)
			{
				for(int j = 0; j < 3; j++)
				{
					for (int i = 2; i < 7; i++)
					{
						Vector2 toReach = new Vector2(i * 48, (j - 1) * 24).RotatedBy(projectile.velocity.ToRotation());
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, toReach.X / 20f, toReach.Y / 20f, mod.ProjectileType("VibrantBolt"), projectile.damage, 0, projectile.owner);
					}
				}
			}
		}
		public override void AI()
		{
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 44);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			Main.dust[num1].alpha = 200;

			projectile.velocity *= 0.94f;
			projectile.rotation += 0.6f;
		}
	}
}
		
			