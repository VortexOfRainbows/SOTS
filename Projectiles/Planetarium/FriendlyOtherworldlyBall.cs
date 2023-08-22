using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Projectiles.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Planetarium
{
	public class FriendlyOtherworldlyBall : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Phase Ball");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}
		public override void SetDefaults()
		{
			Projectile.width = 34;
			Projectile.height = 34;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.timeLeft = 30;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Ranged;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			float width = Projectile.width * Projectile.scale;
			float height = Projectile.width * Projectile.scale;
			hitbox = new Rectangle((int)(Projectile.Center.X - width/2), (int)(Projectile.Center.Y - height/2), (int)width, (int)height);
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
		public override void PostDraw(Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < 6; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				Main.spriteBatch.Draw(texture, new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(Projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * (1f - (Projectile.alpha / 255f)), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
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
			SOTSUtils.PlaySound(SoundID.NPCHit53, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.75f);
			if (Projectile.owner == Main.myPlayer)
			{
				for(int i = 0; i < 8; i++)
				{
					Vector2 circular = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(i * 45));
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, circular.X, circular.Y, ModContent.ProjectileType<FriendlyOtherworldlyBolt>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
				}
				for (int i = 0; i < 3; i++)
				{
					Vector2 circular = new Vector2(2, 0).RotatedBy(MathHelper.ToRadians(i + Main.rand.Next(120) + 120 * i));
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, circular.X, circular.Y, ModContent.ProjectileType<PhaseColumn>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 2, 0);
				}
			}
			for (int i = 0; i < 35; i++)
			{
				Vector2 circularLocation = new Vector2(12, 0);
				resetVector2(ref circularLocation, i);
				int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 3), 0, 0, ModContent.DustType<CopyDust4>());
				Main.dust[dust].velocity = circularLocation;
				Main.dust[dust].velocity *= 3.5f;
				Main.dust[dust].scale *= 5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].fadeIn = 0.2f;
				Main.dust[dust].color = new Color(100, 80, 200);

				resetVector2(ref circularLocation, i);
				dust = Dust.NewDust(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 3), 0, 0, ModContent.DustType<CopyDust4>());
				Main.dust[dust].velocity = circularLocation;
				Main.dust[dust].velocity *= 6f;
				Main.dust[dust].scale *= 4f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].fadeIn = 0.2f;
				Main.dust[dust].color = new Color(100, 80, 200);

				resetVector2(ref circularLocation, i);
				dust = Dust.NewDust(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 3), 0, 0, ModContent.DustType<CopyDust4>());
				Main.dust[dust].velocity = circularLocation;
				Main.dust[dust].velocity *= 8f;
				Main.dust[dust].scale *= 3f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].fadeIn = 0.2f;
				Main.dust[dust].color = new Color(100, 80, 200);

				resetVector2(ref circularLocation, i);
				dust = Dust.NewDust(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 3), 0, 0, ModContent.DustType<CopyDust4>());
				Main.dust[dust].velocity = circularLocation;
				Main.dust[dust].velocity *= 12f;
				Main.dust[dust].scale *= 2f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].fadeIn = 0.2f;
				Main.dust[dust].color = new Color(100, 80, 200);
			}
		}
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, 0.75f, 0.25f, 0.75f);
			Projectile.scale += ((62f / 34f) - 1) / 30f;
		}
	}
}