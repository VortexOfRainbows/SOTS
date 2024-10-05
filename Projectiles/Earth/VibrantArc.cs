using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Helpers;
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
			// DisplayName.SetDefault("Vibrant Arc");
		}
        public override void SetDefaults()
        {
			Projectile.tileCollide = true;
			Projectile.width = 16;
			Projectile.height = 16;
            Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 1;
			Projectile.alpha = 0; 
			Projectile.friendly = true;
			Projectile.timeLeft = 3000;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color = ColorHelper.VibrantColorGradient(Projectile.whoAmI * 30);
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			color = Projectile.GetAlpha(color);
			for (int j = 0; j < 3; j++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		int counter = 0;
		public override void AI()
        {
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) - MathHelper.ToRadians(135);
			Projectile.spriteDirection = 1;
			
			Vector2 curve = new Vector2(12f,0).RotatedBy(MathHelper.ToRadians(helixRot * 5f));
			helixRot ++;
			
			for(int i = 0; i < 2; i++)
            {
				int direction = i * 2 - 1;
				float radianDir = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X);
				Vector2 helixPos1 = Projectile.Center + new Vector2(curve.X, 0).RotatedBy(radianDir + direction * MathHelper.ToRadians(90));
				Color color2 = ColorHelper.VibrantColorGradient(Projectile.whoAmI * 30);
                Dust dust = Dust.NewDustDirect(new Vector2(helixPos1.X - 4, helixPos1.Y - 4), 0, 0, ModContent.DustType<CopyDust4>());
                dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 0.75f;
				dust.alpha = Projectile.alpha;
				dust.velocity *= 0.1f;
			}
			if (Projectile.timeLeft % 8 == 0)
			{
				float currentVelo = Projectile.velocity.Length();
				float minDist = 240;
				int target2 = -1;
				float dX;
				float dY;
				float distance;
				float speed = 12.5f + counter;
				if (Projectile.friendly == true && Projectile.hostile == false && Projectile.timeLeft > 110)
				{
					for (int i = 0; i < Main.npc.Length - 1; i++)
					{
						NPC target = Main.npc[i];
						if (!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.active && target.CanBeChasedBy())
						{
							dX = target.Center.X - Projectile.Center.X;
							dY = target.Center.Y - Projectile.Center.Y;
							distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
							bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, target.position, target.width, target.height);
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
							dX = toHit.Center.X - Projectile.Center.X;
							dY = toHit.Center.Y - Projectile.Center.Y;
							distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
							speed /= distance;
							Projectile.velocity += new Vector2(dX * speed, dY * speed);
							Projectile.velocity = new Vector2(currentVelo, 0).RotatedBy(Projectile.velocity.ToRotation());
							counter++;
						}
					}
				}
			}
		}
		public override void OnKill(int timeLeft)
		{
			for(int i = 0; i < 10; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[num1];
				Color color2 = ColorHelper.VibrantColorGradient(Projectile.whoAmI * 30);
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2.25f;
				dust.alpha = Projectile.alpha;
				dust.velocity *= 1.5f;
			}
		}
	}
}