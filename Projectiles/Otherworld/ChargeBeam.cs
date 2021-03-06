using log4net.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{
	public class ChargeBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charge Ball");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
		}
		public override void SetDefaults()
		{
			projectile.width = 38;
			projectile.height = 38;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.timeLeft = 300;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.scale = 1f;
			projectile.extraUpdates = 1;
			projectile.alpha = 255;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			float width = projectile.width * projectile.scale;
			float height = projectile.width * projectile.scale;
			width += 2;
			height += 2;
			hitbox = new Rectangle((int)(projectile.Center.X - width/2), (int)(projectile.Center.Y - height/2), (int)width, (int)height);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Color color = new Color(110, 110, 110, 0);
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				color = projectile.GetAlpha(color) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length) * 0.5f;
				for (int j = 0; j < 5; j++)
				{
					float length = Main.rand.Next(-10, 11) * 0.3f * (1 + projectile.scale) * (1 + projectile.scale);
					Vector2 xy = new Vector2(length, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(xy.X, xy.Y), null, color, projectile.rotation, drawOrigin, projectile.scale * (projectile.oldPos.Length - k) / (float)projectile.oldPos.Length, SpriteEffects.None, 0f);
				}
			}
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < 6; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				Main.spriteBatch.Draw(texture, new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * (1f - (projectile.alpha / 255f)), projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
		bool runOnce = true;
        public override void AI()
		{
			if(runOnce)
            {
				runOnce = false;
				projectile.scale = 0;
				projectile.alpha = 0;
            }
			Lighting.AddLight(projectile.Center, 0.25f, 0.45f, 0.75f);
			if(projectile.timeLeft > 200)
            {
				if(projectile.timeLeft % 20 == 0)
				{
					Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 15, 0.7f);
					for (int k = 0; k < 360; k += 10)
					{
						Vector2 circularLocation = new Vector2(-38 * projectile.scale, 0).RotatedBy(MathHelper.ToRadians(k));
						circularLocation += 0.5f * new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2));
						int type = DustID.Electric;
						int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, type);
						Main.dust[num1].noGravity = true;
						Main.dust[num1].scale *= 1f + 0.166f * projectile.scale;
						Main.dust[num1].velocity = -circularLocation * 0.07f;
					}
				}
				projectile.scale += 0.015f;
            }
			else
            {
				if(projectile.timeLeft == 200)
				{
					Main.PlaySound(2, (int)(projectile.Center.X), (int)(projectile.Center.Y), 94);
					if (Main.netMode != 1)
                    {
						int numberProjectiles = 3;
						for (int i = 0; i < numberProjectiles; i++)
						{
							Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(MathHelper.ToRadians((i - 1f) * 3f));
							Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("SpiralDeathBeam"), projectile.damage, 1f, Main.myPlayer, (i * 360f / numberProjectiles));
						}
					}
                }
				projectile.scale -= 0.0075f;
            }
		}
	}
}