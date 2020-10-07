using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{
	public class FriendlyOtherworldlyBolt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phase Bolt");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 7;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
		}
		public override void SetDefaults()
		{
			projectile.width = 34;
			projectile.height = 22;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.timeLeft = 720;
			projectile.tileCollide = true;
			projectile.penetrate = 1;
			projectile.ranged = true;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("Projectiles/Otherworld/OtherworldlyBolt");
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
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++)
			{
				int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 242);
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 4f;
				Main.dust[dust].velocity += projectile.velocity;
				Main.dust[dust].noGravity = true;
			}
		}
		bool runOnce = true;
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		public override void AI()
		{
			projectile.ai[0]++;
			Lighting.AddLight(projectile.Center, 0.75f, 0.25f, 0.75f);
			if(runOnce)
			{
				projectile.rotation = projectile.velocity.ToRotation();
				runOnce = false;
			}
			Vector2 varyingVelocity = new Vector2(1.5f, 0).RotatedBy(MathHelper.ToRadians(projectile.ai[0] * 2));
			projectile.position += projectile.velocity + new Vector2(varyingVelocity.X, 0).RotatedBy(projectile.rotation);
		}
	}
}