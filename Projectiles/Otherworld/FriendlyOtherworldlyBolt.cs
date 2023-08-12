using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{
	public class FriendlyOtherworldlyBolt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Phase Bolt");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}
		public override void SetDefaults()
		{
			Projectile.width = 34;
			Projectile.height = 22;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.timeLeft = 720;
			Projectile.tileCollide = true;
			Projectile.penetrate = 1;
			Projectile.DamageType = DamageClass.Ranged;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color = new Color(110, 110, 110, 0);
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				color = Projectile.GetAlpha(color) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * 0.5f;
				for (int j = 0; j < 5; j++)
				{
					float x = Main.rand.Next(-10, 11) * 0.1f;
					float y = Main.rand.Next(-10, 11) * 0.1f;
					if(!Projectile.oldPos[k].Equals(Projectile.position))
					{
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, Projectile.rotation, drawOrigin, Projectile.scale * (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length, SpriteEffects.None, 0f);
					}
				}
			}
			return false;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++)
			{
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>());
				Main.dust[dust].velocity *= 1.4f;
				Main.dust[dust].scale *= 1.75f;
				Main.dust[dust].velocity += Projectile.velocity;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].fadeIn = 0.2f;
				Main.dust[dust].color = new Color(100, 80, 200);
			}
		}
		bool runOnce = true;
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		public override void AI()
		{
			Projectile.ai[0]++;
			Lighting.AddLight(Projectile.Center, 0.75f, 0.25f, 0.75f);
			if(runOnce)
			{
				Projectile.rotation = Projectile.velocity.ToRotation();
				runOnce = false;
			}
			Vector2 varyingVelocity = new Vector2(1.5f, 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[0] * 2));
			Projectile.position += Projectile.velocity + new Vector2(varyingVelocity.X, 0).RotatedBy(Projectile.rotation);
		}
	}
}