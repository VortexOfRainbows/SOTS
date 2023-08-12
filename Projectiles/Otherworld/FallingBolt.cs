using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{
	public class FallingBolt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Falling Bolt");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 60;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}
		public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.extraUpdates = 7;
			Projectile.timeLeft = 480;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Otherworld/CurveBoltTrail").Value;
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
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, Projectile.rotation, drawOrigin, (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length, SpriteEffects.None, 0f);
					}
				}
			}
			return false;
		}
		public override void PostDraw(Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < 6; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				Main.spriteBatch.Draw(texture, new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(Projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * (1f - (Projectile.alpha / 255f)), Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
		}
		public override void Kill(int timeLeft)
		{
			SOTSUtils.PlaySound(SoundID.Item14, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.6f);
			if (Projectile.owner == Main.myPlayer)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<AvaritianExplosion>(), Projectile.damage, 0, Main.myPlayer);
			}
		}
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, 0.55f, 0.55f, 0.75f);
			Projectile.tileCollide = false;
			Projectile.rotation += 0.5f;
			if(Projectile.timeLeft == 240)
			{
				for (int i = 0; i < 10; i++)
				{
					int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Electric, 0, 0, Projectile.alpha, default, 1.25f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.5f;
				}
				Projectile.position = new Vector2(Projectile.ai[0], Projectile.ai[1] - 600) - new Vector2(Projectile.width / 2, Projectile.height / 2);
				Projectile.velocity = -1.5f * Projectile.velocity;
				for (int i = 0; i < 10; i++)
				{
					int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Electric, 0, 0, Projectile.alpha, default, 1.25f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.5f;
				}
			}
		}
	}
}