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
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 180;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 22;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.extraUpdates = 12;
			projectile.timeLeft = 6000;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Earth/EarthenTrail").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Color color = new Color(100, 100, 100, 0);
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				color = projectile.GetAlpha(color) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length) * 0.5f;
				for (int j = 0; j < 5; j++)
				{
					float x = Main.rand.Next(-10, 11) * 0.15f;
					float y = Main.rand.Next(-10, 11) * 0.15f;
					if(!projectile.oldPos[k].Equals(projectile.position))
					{
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, projectile.rotation, drawOrigin, (projectile.oldPos.Length - k) / (float)projectile.oldPos.Length, SpriteEffects.None, 0f);
					}
				}
			}
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				if (projectile.ai[0] != 1)
					Main.spriteBatch.Draw(texture, new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * (1f - (projectile.alpha / 255f)), projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		int inititate = 0;
		public override void AI()
		{
			Lighting.AddLight(projectile.Center, 2.55f, 1.90f, 0);
			if (inititate == 0)
			{
				inititate++;
				SoundEngine.PlaySound(2, projectile.Center, 60);
			}
			if(!projectile.velocity.Equals(new Vector2(0, 0)))
				projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(45);

			if (projectile.ai[0] == 1)
			{
				projectile.alpha += 2;
				projectile.friendly = false;
				if (projectile.alpha >= 255)
				{
					projectile.Kill();
				}
			}
			if(projectile.Center.Y > projectile.ai[1])
			{
				projectile.tileCollide = true;
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			triggerUpdate();
			return false;
		}
		public void triggerUpdate()
		{
			projectile.ai[0] = 1;
			projectile.velocity *= 0;
			projectile.friendly = false;
			projectile.tileCollide = false;
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				projectile.netUpdate = true;
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("EarthenRing"), projectile.damage, 0, Main.myPlayer);
			}
		}
	}
}