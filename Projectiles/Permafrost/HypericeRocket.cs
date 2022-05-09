using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Permafrost 
{    
    public class HypericeRocket : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hyperice Rocket");
		}
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(14);
            aiType = 14; 
			Projectile.penetrate = 1;
			Projectile.width = 30;
			Projectile.height = 18;
			Projectile.alpha = 0;
			Projectile.timeLeft = 640;
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, (int)Projectile.Center.X, (int)Projectile.Center.Y);
			for (int i = 0; i < 2; i++)
			{
				int goreIndex = Gore.NewGore(new Vector2(Projectile.position.X, Projectile.position.Y), default(Vector2), Main.rand.Next(61,64), 1f);	
				Main.gore[goreIndex].scale = 0.55f;
			}
			if(Main.myPlayer == Projectile.owner)
			{
				for (int i = 0; i < 4; i++)
				{
					Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(15 - (10 * i)));
					Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("IceCluster"), Projectile.damage, 0, Projectile.owner);
				}
			}
		}
		public override void AI()
		{
			/*
			Vector2 trailLoc = new Vector2(18, 0).RotatedBy(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X));
			int num1 = Dust.NewDust(new Vector2(Projectile.Center.X - trailLoc.X - 2, Projectile.Center.Y - trailLoc.Y - 2), 2, 2, 235);
			
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			*/
			Projectile.timeLeft -= Main.rand.Next(40);
			Projectile.timeLeft -= Math.Abs((int)(Projectile.velocity.X * 0.8f));
			Projectile.timeLeft -= Math.Abs((int)(Projectile.velocity.Y * 0.8f));
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X);
		}
	}
}
		
			