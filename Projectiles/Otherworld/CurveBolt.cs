using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{
	public class CurveBolt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Curve Bolt");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.extraUpdates = 2;
			Projectile.timeLeft = 360;
			Projectile.tileCollide = true;
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
		int counter = 0;
		Vector2 spawnLoc = new Vector2(0, 0);
		public override void AI()
		{
			if(spawnLoc.X == 0 && spawnLoc.Y == 0)
			{
				spawnLoc = Projectile.Center;
			}
			Player player = Main.player[Projectile.owner];
			Lighting.AddLight(Projectile.Center, 0.55f, 0.55f, 0.55f);
			counter++;
			Projectile.rotation = MathHelper.ToRadians(counter);
			int direction = 1;
			Vector2 distance = new Vector2(Projectile.ai[0], Projectile.ai[1]) - spawnLoc;
			float distX = Projectile.ai[0] - spawnLoc.X;
			if(distX < 0)
			{
				direction = -1;
			}
			distance *= 0.5f;
			Vector2 center = spawnLoc + distance;
			float startRot = distance.ToRotation();
			Vector2 ring = new Vector2(distance.Length(), 0).RotatedBy(MathHelper.ToRadians(direction * counter + 180));
			ring = ring.RotatedBy(startRot);
			Projectile.position = ring - new Vector2(Projectile.width / 2, Projectile.height / 2) + center;
			Projectile.velocity = new Vector2(0, 1f); //give a nudge for tile collide
		}
	}
}