using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{
	public class FriendlyOtherworldlyBall : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phase Ball");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 7;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
		}
		public override void SetDefaults()
		{
			projectile.width = 34;
			projectile.height = 34;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.timeLeft = 30;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			float width = projectile.width * projectile.scale;
			float height = projectile.width * projectile.scale;
			hitbox = new Rectangle((int)(projectile.Center.X - width/2), (int)(projectile.Center.Y - height/2), (int)width, (int)height);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("Projectiles/Otherworld/OtherworldlyBall");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Color color = new Color(110, 110, 110, 0);
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				color = projectile.GetAlpha(color) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length) * 0.5f;
				for (int j = 0; j < 5; j++)
				{
					float x = Main.rand.Next(-10, 11) * 0.1f;
					float y = Main.rand.Next(-10, 11) * 0.1f;
					if(!projectile.oldPos[k].Equals(projectile.position))
					{
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, projectile.rotation, drawOrigin, projectile.scale * (projectile.oldPos.Length - k) / (float)projectile.oldPos.Length, SpriteEffects.None, 0f);
					}
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
				Main.spriteBatch.Draw(texture, new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * (1f - (projectile.alpha / 255f)), projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		public void resetVector2(ref Vector2 loc, int i)
		{
			loc = new Vector2(10, 0).RotatedBy(MathHelper.ToRadians(i * 9));
			loc.X += Main.rand.Next(-5, 6);
			loc.Y += Main.rand.Next(-5, 6);
			loc *= 0.1f;
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 14, 0.6f);
			if (projectile.owner == Main.myPlayer)
			{
				for(int i = 0; i < 8; i++)
				{
					Vector2 circular = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(i * 45));
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, circular.X, circular.Y, mod.ProjectileType("FriendlyOtherworldlyBolt"), projectile.damage, projectile.knockBack, Main.myPlayer);
				}
			}
			for (int i = 0; i < 35; i++)
			{
				Vector2 circularLocation = new Vector2(12, 0);
				resetVector2(ref circularLocation, i);
				int dust = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 3), 0, 0, 242);
				Main.dust[dust].velocity = circularLocation;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 7f;
				Main.dust[dust].noGravity = true;

				resetVector2(ref circularLocation, i);
				dust = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 3), 0, 0, 242);
				Main.dust[dust].velocity = circularLocation;
				Main.dust[dust].velocity *= 4f;
				Main.dust[dust].scale *= 6f;
				Main.dust[dust].noGravity = true;

				resetVector2(ref circularLocation, i);
				dust = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 3), 0, 0, 242);
				Main.dust[dust].velocity = circularLocation;
				Main.dust[dust].velocity *= 6.5f;
				Main.dust[dust].scale *= 5f;
				Main.dust[dust].noGravity = true;

				resetVector2(ref circularLocation, i);
				dust = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 3), 0, 0, 242);
				Main.dust[dust].velocity = circularLocation;
				Main.dust[dust].velocity *= 10f;
				Main.dust[dust].scale *= 4f;
				Main.dust[dust].noGravity = true;
			}
		}
		public override void AI()
		{
			Lighting.AddLight(projectile.Center, 0.75f, 0.25f, 0.75f);
			projectile.scale += ((62f / 34f) - 1) / 30f;
		}
	}
}