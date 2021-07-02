using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles 
{    
    public class SpikyPufferfish : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spiky Pufferfish");
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(3);
            aiType = 3; 
			projectile.penetrate = 1;
			projectile.width = 20;
			projectile.height = 20;
			projectile.alpha = 0;
			projectile.timeLeft = 640;
		}
		Vector2 initialVelo;
		bool runOnce = true;
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 14, 0.85f);
			for (int i = 0; i < 5; i++)
			{
				int goreIndex = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);	
				Main.gore[goreIndex].scale = 0.45f;
			}
			if(Main.myPlayer == projectile.owner)
			{
				for(int i = 0; i < 2; i++)
				{
					int direction = i * 2 - 1;
					Vector2 projVelocity = initialVelo.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(20) * direction));
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projVelocity.X * Main.rand.NextFloat(1f, 1.2f), projVelocity.Y * Main.rand.NextFloat(1f, 1.2f), ProjectileID.Bullet, (int)(projectile.damage * 0.6f), projectile.knockBack, Main.myPlayer);
					if (Main.rand.NextBool(2 + i))
					{
						projVelocity = initialVelo.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(35) * direction));
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projVelocity.X * Main.rand.NextFloat(1f, 1.2f), projVelocity.Y * Main.rand.NextFloat(1f, 1.2f), ProjectileID.Bullet, (int)(projectile.damage * 0.6f), projectile.knockBack, Main.myPlayer);
					}
				}
			}
		}
		public override void AI()
		{
			if (runOnce)
			{
				initialVelo = projectile.velocity;
				runOnce = false;
			}
			projectile.timeLeft -= Main.rand.Next(40);
			projectile.timeLeft -= Math.Abs((int)(projectile.velocity.X * 0.8f));
			projectile.timeLeft -= Math.Abs((int)(projectile.velocity.Y * 0.8f));
		}
	}
}
		
			