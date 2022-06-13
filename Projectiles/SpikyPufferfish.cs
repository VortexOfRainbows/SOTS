using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;

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
			Projectile.CloneDefaults(3);
            AIType = 3; 
			Projectile.penetrate = 1;
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.alpha = 0;
			Projectile.timeLeft = 640;
		}
		Vector2 initialVelo;
		bool runOnce = true;
		public override void Kill(int timeLeft)
		{
			SOTSUtils.PlaySound(SoundID.Item14, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.65f);
			for (int i = 0; i < 5; i++)
			{
				int goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X, Projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);	
				Main.gore[goreIndex].scale = 0.45f;
			}
			if(Main.myPlayer == Projectile.owner)
			{
				for(int i = 0; i < 2; i++)
				{
					int direction = i * 2 - 1;
					Vector2 projVelocity = initialVelo.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(20) * direction));
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, projVelocity.X * Main.rand.NextFloat(1f, 1.2f), projVelocity.Y * Main.rand.NextFloat(1f, 1.2f), ProjectileID.Bullet, (int)(Projectile.damage * 0.6f), Projectile.knockBack, Main.myPlayer);
					if (Main.rand.NextBool(2 + i))
					{
						projVelocity = initialVelo.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(35) * direction));
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, projVelocity.X * Main.rand.NextFloat(1f, 1.2f), projVelocity.Y * Main.rand.NextFloat(1f, 1.2f), ProjectileID.Bullet, (int)(Projectile.damage * 0.6f), Projectile.knockBack, Main.myPlayer);
					}
				}
			}
		}
		public override void AI()
		{
			if (runOnce)
			{
				initialVelo = Projectile.velocity;
				runOnce = false;
			}
			Projectile.timeLeft -= Main.rand.Next(40);
			Projectile.timeLeft -= Math.Abs((int)(Projectile.velocity.X * 0.8f));
			Projectile.timeLeft -= Math.Abs((int)(Projectile.velocity.Y * 0.8f));
		}
	}
}
		
			