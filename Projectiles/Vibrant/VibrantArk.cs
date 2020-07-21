using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Vibrant
{    
    public class VibrantArk : ModProjectile 
    {	
		int helixRot = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant");
			
		}
		
        public override void SetDefaults()
        {
			projectile.tileCollide = true;
			projectile.width = 12;
			projectile.height = 12;
            projectile.magic = true;
			projectile.penetrate = 1;
			projectile.alpha = 0; 
			projectile.friendly = true;
			projectile.timeLeft = 3000;
		}
		int counter = 0;
		public override void AI()
        {
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 1.5f / 255f, (255 - projectile.alpha) * 1.75f / 255f, (255 - projectile.alpha) * 0.2f / 255f);
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) - MathHelper.ToRadians(135);
			projectile.spriteDirection = 1;
			
			Vector2 curve = new Vector2(12f,0).RotatedBy(MathHelper.ToRadians(helixRot * 5f));
			helixRot ++;
			
			float radianDir = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
			Vector2 helixPos1 = projectile.Center + new Vector2(curve.X, 0).RotatedBy(radianDir + MathHelper.ToRadians(90));
			int num1 = Dust.NewDust(new Vector2(helixPos1.X - 4, helixPos1.Y - 4), 4, 4, 44);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.2f;
			Main.dust[num1].alpha = 200;

			Vector2 helixPos2 = projectile.Center + new Vector2(curve.X, 0).RotatedBy(radianDir - MathHelper.ToRadians(90));
			num1 = Dust.NewDust(new Vector2(helixPos2.X - 4, helixPos2.Y - 4), 4, 4, 44);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.2f;
			Main.dust[num1].alpha = 200;

			if (projectile.timeLeft % 12 == 0)
			{
				float currentVelo = projectile.velocity.Length();
				float minDist = 360;
				int target2 = -1;
				float dX = 0f;
				float dY = 0f;
				float distance = 0;
				float speed = 12.5f + counter;
				if (projectile.friendly == true && projectile.hostile == false && projectile.timeLeft > 110)
				{
					for (int i = 0; i < Main.npc.Length - 1; i++)
					{
						NPC target = Main.npc[i];
						if (!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.active && target.CanBeChasedBy())
						{
							dX = target.Center.X - projectile.Center.X;
							dY = target.Center.Y - projectile.Center.Y;
							distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
							bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, target.position, target.width, target.height);
							if (distance < minDist && lineOfSight)
							{
								minDist = distance;
								target2 = i;
							}
						}
					}

					if (target2 != -1)
					{
						NPC toHit = Main.npc[target2];
						if (toHit.active == true)
						{
							dX = toHit.Center.X - projectile.Center.X;
							dY = toHit.Center.Y - projectile.Center.Y;
							distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
							speed /= distance;

							projectile.velocity += new Vector2(dX * speed, dY * speed);
							projectile.velocity = new Vector2(currentVelo, 0).RotatedBy(projectile.velocity.ToRotation());
							counter++;
						}
					}
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 12; i++)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 44);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].alpha = 200;
			}
		}
	}
}