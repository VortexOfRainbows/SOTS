using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Nature
{
	public class PinkyBomber : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pinky Bomber");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			Main.projFrames[projectile.type] = 4;
		}
		public override void SetDefaults()
		{
			projectile.width = 46;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.timeLeft = 6000;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.alpha = 40;
			projectile.extraUpdates = 3;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(new Color(120, 110, 110, 0)) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, new Rectangle(0, projectile.height * projectile.frame, projectile.width, projectile.height), color * 0.6f, projectile.rotation, drawOrigin, projectile.scale * 0.33f + 0.67f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length), projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
			return true;
		}
		bool runOnce = true;
		int counter = 0;
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			Lighting.AddLight(projectile.Center, 0.75f, 0.2f, 0.1f);
			if (runOnce)
			{
				runOnce = false;
				SoundEngine.PlaySound(2, player.Center, 60);
				Vector2 toLocation = new Vector2(projectile.ai[0], projectile.ai[1]);
				Vector2 goTo = toLocation - projectile.Center;
				goTo = goTo.SafeNormalize(Vector2.Zero);
				float shouldBeSpeed = 16f;
				projectile.velocity = goTo;
				float xSpeed = shouldBeSpeed / Math.Abs(goTo.X);
				projectile.velocity = goTo * xSpeed;
				counter += Main.rand.Next(-5, 6);
			}
			if(counter % 4 == 0)
            {
				projectile.frame = (projectile.frame + 1) % 4;
            }
			if(counter == 120)
            {
				if(Main.myPlayer == projectile.owner)
                {
					for (int i = 1; i < 3 + Main.rand.Next(2); i++)
						Projectile.NewProjectile(projectile.Center + new Vector2(0, 16), new Vector2(Main.rand.NextFloat(-1.5f, 1.5f) * i, -1 + i * 1.25f), ModContent.ProjectileType<PeanutBomb>(), projectile.damage, projectile.knockBack, Main.myPlayer, projectile.Center.Y + 480);
                }
			}
			counter++;
			projectile.rotation = projectile.velocity.ToRotation();
			if(projectile.velocity.X < 0)
            {
				projectile.rotation +=	(float)Math.PI;
				projectile.spriteDirection = -1;
            }
			else
			{
				projectile.spriteDirection = 1;
			}
		}
	}
}