using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Projectiles
{
	public class ReaperScythe : ModProjectile
	{
		public float start = 0;
		Vector2 cen = Vector2.Zero;
		public override void SetDefaults()
		{
			projectile.width = 26;
			projectile.height = 28;
			projectile.friendly = true;
			projectile.aiStyle = -1;
			projectile.penetrate = 5;      //this is how many enemy this projectile penetrate before disappear
			projectile.extraUpdates = 1;
			projectile.timeLeft = 100;
			Main.projFrames[projectile.type] = 6;
			projectile.tileCollide = false;
			projectile.ignoreWater = false;
		}

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

        public override void AI()
		{
			start = MathHelper.Lerp(start, 10, 0.05f);
			projectile.ai[0]++;
			if (projectile.ai[0] == 20)
            {
				cen = projectile.DirectionTo(Main.MouseWorld) * 0.5f;
            }
			if (projectile.ai[0] > 20 && projectile.ai[0] < 50)
            {
				projectile.velocity += cen * 1.8f;
            }
            else if (projectile.ai[0] < 20)
            {
				projectile.velocity *= 0.95f;
			}
			else if (projectile.ai[0] > 50)
            {
				projectile.velocity *= 0.97f;
            }
			projectile.rotation += MathHelper.ToRadians(projectile.velocity.X);
		}

        public override bool CanDamage()
        {
			return projectile.ai[1] != 25;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[0] = 1;
			target.immune[1] = 1;
			if (projectile.penetrate == 2)
            {
				projectile.ai[1] = 25;
            }
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			for (int i = 1; i < projectile.oldPos.Length; i++)
			{
				projectile.oldPos[i] = projectile.oldPos[i - 1] + (projectile.oldPos[i] - projectile.oldPos[i - 1]).SafeNormalize(Vector2.Zero) * MathHelper.Min(Vector2.Distance(projectile.oldPos[i - 1], projectile.oldPos[i]), start);
			}

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
			for (int i = 0; i < projectile.oldPos.Length; i++)
				{
					spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.oldPos[i] + new Vector2(projectile.width / 2, projectile.height / 2) - Main.screenPosition,
					new Rectangle(0, 0, 26, 28), Color.Lerp(Color.Lerp(Color.BlueViolet, Color.Black, (float)i / projectile.oldPos.Length), Color.Black, projectile.ai[0] / 100), projectile.oldRot[i],
					new Vector2(26 * 0.5f, 28 * 0.5f), Vector2.Lerp(new Vector2(1f, 1f), new Vector2(0, 0), (float)i / projectile.oldPos.Length), SpriteEffects.None, 0f);
				}
			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.position + new Vector2(projectile.width / 2, projectile.height / 2) - Main.screenPosition,
			   new Rectangle(0, 0, 26, 28), Color.Lerp(Color.Purple, Color.Black, projectile.ai[0] / 100), projectile.rotation,
			   new Vector2(26 * 0.5f, 28 * 0.5f), 1f, SpriteEffects.None, 0f);
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
			return false;
		}
	}
}