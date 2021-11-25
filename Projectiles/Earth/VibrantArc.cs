using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Void;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Earth
{    
    public class VibrantArc : ModProjectile 
    {	
		int helixRot = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant Arc");
		}
        public override void SetDefaults()
        {
			projectile.tileCollide = true;
			projectile.width = 16;
			projectile.height = 16;
            projectile.magic = true;
			projectile.penetrate = 1;
			projectile.alpha = 0; 
			projectile.friendly = true;
			projectile.timeLeft = 3000;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color = VoidPlayer.VibrantColorAttempt(projectile.whoAmI * 30);
			Vector2 drawPos = projectile.Center - Main.screenPosition;
			color = projectile.GetAlpha(color);
			for (int j = 0; j < 3; j++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		int counter = 0;
		public override void AI()
        {
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) - MathHelper.ToRadians(135);
			projectile.spriteDirection = 1;
			
			Vector2 curve = new Vector2(12f,0).RotatedBy(MathHelper.ToRadians(helixRot * 5f));
			helixRot ++;
			
			for(int i = 0; i < 2; i++)
            {
				int direction = i * 2 - 1;
				float radianDir = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
				Vector2 helixPos1 = projectile.Center + new Vector2(curve.X, 0).RotatedBy(radianDir + direction * MathHelper.ToRadians(90));
				int num1 = Dust.NewDust(new Vector2(helixPos1.X - 4, helixPos1.Y - 4), 0, 0, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[num1];
				Color color2 = VoidPlayer.VibrantColorAttempt(projectile.whoAmI * 30);
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 0.75f;
				dust.alpha = projectile.alpha;
				dust.velocity *= 0.1f;
			}
			if (projectile.timeLeft % 8 == 0)
			{
				float currentVelo = projectile.velocity.Length();
				float minDist = 240;
				int target2 = -1;
				float dX;
				float dY;
				float distance;
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
			for(int i = 0; i < 10; i++)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[num1];
				Color color2 = VoidPlayer.VibrantColorAttempt(projectile.whoAmI * 30);
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2.25f;
				dust.alpha = projectile.alpha;
				dust.velocity *= 1.5f;
			}
		}
	}
}