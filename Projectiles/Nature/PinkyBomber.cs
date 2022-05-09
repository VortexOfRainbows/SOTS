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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			Main.projFrames[Projectile.type] = 4;
		}
		public override void SetDefaults()
		{
			Projectile.width = 46;
			Projectile.height = 32;
			Projectile.friendly = true;
			Projectile.ranged = true;
			Projectile.timeLeft = 6000;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.alpha = 40;
			Projectile.extraUpdates = 3;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(new Color(120, 110, 110, 0)) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), color * 0.6f, Projectile.rotation, drawOrigin, Projectile.scale * 0.33f + 0.67f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
			return true;
		}
		bool runOnce = true;
		int counter = 0;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Lighting.AddLight(Projectile.Center, 0.75f, 0.2f, 0.1f);
			if (runOnce)
			{
				runOnce = false;
				SoundEngine.PlaySound(2, player.Center, 60);
				Vector2 toLocation = new Vector2(Projectile.ai[0], Projectile.ai[1]);
				Vector2 goTo = toLocation - Projectile.Center;
				goTo = goTo.SafeNormalize(Vector2.Zero);
				float shouldBeSpeed = 16f;
				Projectile.velocity = goTo;
				float xSpeed = shouldBeSpeed / Math.Abs(goTo.X);
				Projectile.velocity = goTo * xSpeed;
				counter += Main.rand.Next(-5, 6);
			}
			if(counter % 4 == 0)
            {
				Projectile.frame = (Projectile.frame + 1) % 4;
            }
			if(counter == 120)
            {
				if(Main.myPlayer == Projectile.owner)
                {
					for (int i = 1; i < 3 + Main.rand.Next(2); i++)
						Projectile.NewProjectile(Projectile.Center + new Vector2(0, 16), new Vector2(Main.rand.NextFloat(-1.5f, 1.5f) * i, -1 + i * 1.25f), ModContent.ProjectileType<PeanutBomb>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, Projectile.Center.Y + 480);
                }
			}
			counter++;
			Projectile.rotation = Projectile.velocity.ToRotation();
			if(Projectile.velocity.X < 0)
            {
				Projectile.rotation +=	(float)Math.PI;
				Projectile.spriteDirection = -1;
            }
			else
			{
				Projectile.spriteDirection = 1;
			}
		}
	}
}