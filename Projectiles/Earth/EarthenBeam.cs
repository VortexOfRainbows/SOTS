using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Earth
{
	public class EarthenBeam : ModProjectile
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
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.extraUpdates = 12;
			Projectile.timeLeft = 6000;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Earth/EarthenTrail").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color = new Color(100, 100, 100, 0);
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				color = Projectile.GetAlpha(color) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * 0.5f;
				for (int j = 0; j < 5; j++)
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
			Color color = new Color(100, 100, 100, 0);
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
			Lighting.AddLight(Projectile.Center, 2.55f, 1.90f, 0);
			if (inititate == 0)
			{
				inititate++;
				Terraria.Audio.SoundEngine.PlaySound(2, Projectile.Center, 60);
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
			if(Projectile.Center.Y > Projectile.ai[1])
			{
				Projectile.tileCollide = true;
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			triggerUpdate();
			return false;
		}
		public void triggerUpdate()
		{
			Projectile.ai[0] = 1;
			Projectile.velocity *= 0;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				Projectile.netUpdate = true;
				Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, Mod.Find<ModProjectile>("EarthenRing").Type, Projectile.damage, 0, Main.myPlayer);
			}
		}
	}
}