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
			DisplayName.SetDefault("Curve Bolt");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 22;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.extraUpdates = 2;
			projectile.timeLeft = 360;
			projectile.tileCollide = true;
			projectile.penetrate = -1;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Otherworld/CurveBoltTrail").Value;
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
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, projectile.rotation, drawOrigin, (projectile.oldPos.Length - k) / (float)projectile.oldPos.Length, SpriteEffects.None, 0f);
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
		int counter = 0;
		Vector2 spawnLoc = new Vector2(0, 0);
		public override void AI()
		{
			if(spawnLoc.X == 0 && spawnLoc.Y == 0)
			{
				spawnLoc = projectile.Center;
			}
			Player player = Main.player[projectile.owner];
			Lighting.AddLight(projectile.Center, 0.55f, 0.55f, 0.55f);
			counter++;
			projectile.rotation = MathHelper.ToRadians(counter);
			int direction = 1;
			Vector2 distance = new Vector2(projectile.ai[0], projectile.ai[1]) - spawnLoc;
			float distX = projectile.ai[0] - spawnLoc.X;
			if(distX < 0)
			{
				direction = -1;
			}
			distance *= 0.5f;
			Vector2 center = spawnLoc + distance;
			float startRot = distance.ToRotation();
			Vector2 ring = new Vector2(distance.Length(), 0).RotatedBy(MathHelper.ToRadians(direction * counter + 180));
			ring = ring.RotatedBy(startRot);
			projectile.position = ring - new Vector2(projectile.width / 2, projectile.height / 2) + center;
			projectile.velocity = new Vector2(0, 1f); //give a nudge for tile collide
			
		}
	}
}