using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using SOTS.Dusts;

namespace SOTS.Projectiles.Celestial
{
	public class PurplePunch : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Purple Fist");
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = 0;
			projectile.melee = true;
			projectile.friendly = true;
			projectile.width = 56;
			projectile.height = 30;
			projectile.timeLeft = 70;
			projectile.penetrate = 4;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.alpha = 40;
			Main.projFrames[projectile.type] = 5;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 25;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width / 2, projectile.height / 2);
			Color color = Color.Black;
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(3.5f, 5), 0).RotatedBy(MathHelper.ToRadians(i));
				color = new Color(170, 100, 190, 0);
				Main.spriteBatch.Draw(texture, projectile.Center + circular - Main.screenPosition, new Rectangle(0, projectile.height * projectile.frame, projectile.width, projectile.height), color * ((255f - projectile.alpha) / 255f), projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);
			}
			color = new Color(160, 100, 180);
			Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle(0, projectile.height * projectile.frame, projectile.width, projectile.height), color, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);
			return false;
		}
		public override void AI()
		{
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 2.5f / 275f, (255 - projectile.alpha) * 1.6f / 275f, (255 - projectile.alpha) * 2.4f / 275f);
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
			projectile.velocity *= 0.99f;
			projectile.frameCounter++;
			if (projectile.frameCounter >= 3)
			{
				projectile.frameCounter = 0;
				projectile.frame = (projectile.frame + 1) % 5;
			}
			projectile.alpha += 3;
			float minDist = 720;
			int target2 = -1;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			float speed = 0.275f;
			if (projectile.friendly == true && projectile.hostile == false)
			{
				for (int i = 0; i < Main.npc.Length; i++)
				{
					NPC target = Main.npc[i];
					if (!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.active && target.CanBeChasedBy())
					{
						dX = target.Center.X - projectile.Center.X;
						dY = target.Center.Y - projectile.Center.Y;
						distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
						if (distance < minDist)
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
						projectile.velocity *= 0.99f;
						projectile.velocity += new Vector2(dX * speed, dY * speed);
					}
				}
			}
			if (Main.rand.NextBool(5))
			{
				Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
				dust.velocity *= 0.2f;
				dust.velocity -= 2 * projectile.velocity.SafeNormalize(Vector2.Zero);
				dust.scale *= 2;
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
				dust.color = new Color(170, 100, 190, 0);
				dust.alpha = 100;
			}
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 24; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
				dust.velocity *= 1.2f;
				dust.velocity += 5 * projectile.velocity.SafeNormalize(Vector2.Zero);
				dust.scale *= 2;
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
				dust.color = new Color(170, 100, 190, 0);
				dust.alpha = 100;
			}
			base.Kill(timeLeft);
		}
	}
}
		