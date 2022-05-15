using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Laser
{
	public class VibrantBlade : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lightspeed Blade");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 180;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.extraUpdates = 12;
			Projectile.timeLeft = 6000;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
		}
		bool bounce = true;
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if(!bounce)
			{
				triggerUpdate();
			}
			else
			{
				if (Projectile.velocity.X != oldVelocity.X)
				{
					Projectile.velocity.X = -oldVelocity.X;
				}
				if (Projectile.velocity.Y != oldVelocity.Y)
				{
					Projectile.velocity.Y = -oldVelocity.Y;
				}
				bounce = false;
			}
			return false;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Laser/VibrantBladeTrail").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color = VoidPlayer.VibrantColorAttempt(k);
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				color = Projectile.GetAlpha(color) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * 0.5f;
				for (int j = 0; j < 7; j++)
				{
					float x = Main.rand.Next(-10, 11) * 0.15f;
					float y = Main.rand.Next(-10, 11) * 0.15f;
					if(!Projectile.oldPos[k].Equals(Projectile.position))
					{
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, Projectile.rotation, drawOrigin, (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length, SpriteEffects.None, 0f);
					}
				}
			}
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = VoidPlayer.EarthColor;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				if (Projectile.ai[0] != 1)
					Main.spriteBatch.Draw(texture, new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(Projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * (1f - (Projectile.alpha / 255f)), Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		int inititate = 0;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Lighting.AddLight(Projectile.Center, 1.5f, 1.75f, 0.2f);
			if (inititate == 0)
			{
				inititate++;
				//SoundEngine.PlaySound(2, Projectile.Center, 60);
				SoundEngine.PlaySound(SoundID.Item, player.Center, 60);
			}
			if(!Projectile.velocity.Equals(new Vector2(0, 0)))
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45);

			if (Projectile.ai[0] == 1)
			{
				Projectile.alpha += 2;
				Projectile.friendly = false;
				if (Projectile.alpha >= 255)
				{
					Projectile.Kill();
				}
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[Projectile.owner] = 0;
			triggerUpdate();
		}
		public void triggerUpdate()
		{
			Projectile.ai[0] = 1;
			Projectile.velocity *= 0;
			Projectile.friendly = false;
			if (Projectile.owner == Main.myPlayer)
			{
				Projectile.netUpdate = true;
				Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<VibrantRing>(), Projectile.damage, Projectile.knockBack * 0.1f, Main.myPlayer);
			}
		}
	}
}