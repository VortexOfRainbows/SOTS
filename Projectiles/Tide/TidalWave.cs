using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Tide
{    
    public class TidalWave : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tidal Wave");
		}
        public override void SetDefaults()
        {
			projectile.penetrate = -1;
			projectile.width = 24;
			projectile.height = 24;
			projectile.timeLeft = 255;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.tileCollide = false;
		}
		bool runOnce = true;
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void AI()
		{
			UpdateList();
			length += projectile.velocity.Length();
			projectile.position += projectile.velocity;
			projectile.ai[1]++;
			projectile.alpha++;
			if (runOnce)
			{
				for (int i = 0; i < 4; i++)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 267);
					dust.color = new Color(64, 72, 178);
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.scale *= 2f;
					dust.velocity *= 1.1f;
					dust.velocity += projectile.velocity * 1.1f;
				}
				runOnce = false;
            }
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.15f / 255f, (255 - projectile.alpha) * 0.25f / 255f, (255 - projectile.alpha) * 0.65f / 255f);
			projectile.rotation += 0.04f;
		}
		float length = 0;
		const float finalDegree = 23f;
		List<Vector2> ParticlePos = new List<Vector2>();
		public void UpdateList()
		{
			if ((int)projectile.ai[0] >= 0)
			{
				ParticlePos = new List<Vector2>();
				Vector2 origin = projectile.Center - projectile.velocity.SafeNormalize(Vector2.Zero) * (-6 + length);
				float rotation = projectile.velocity.ToRotation();
				float C = 2 * (float)Math.PI * length;
				float oneLength = 360f / C * 10;
				float mult = 0f;
				for (float i = oneLength + finalDegree; i < 180; i += oneLength)
				{
					if (mult < 1)
						mult += 0.05f * oneLength;
					float waveValue = new Vector2(8 * mult, 0).RotatedBy(MathHelper.ToRadians(i * 12 + projectile.ai[1])).X;
					Vector2 drawArea = origin + new Vector2(length + waveValue, 0).RotatedBy(MathHelper.ToRadians(i) + rotation);
					ParticlePos.Add(drawArea);
				}
				mult = 0f;
				for (float i = 360 - oneLength; i > 180; i -= oneLength)
				{
					if(mult < 1)
						mult += 0.05f * oneLength;
					float waveValue = new Vector2(8 * mult, 0).RotatedBy(MathHelper.ToRadians(i * 12 + projectile.ai[1])).X;
					Vector2 drawArea = origin + new Vector2(length + waveValue, 0).RotatedBy(MathHelper.ToRadians(i) + rotation);
					ParticlePos.Add(drawArea);
				}
			}
		}
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			for (int i = 0; i < ParticlePos.Count; i++)
            {
				Rectangle hitbox = new Rectangle((int)ParticlePos[i].X - 5, (int)ParticlePos[i].Y - 5, 10, 10);
				if (hitbox.Intersects(targetHitbox))
					return true;
			}
			if (projHitbox.Intersects(targetHitbox))
				return true;
			return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("Projectiles/Tide/TidalConstructTrail");
			if ((int)projectile.ai[0] >= 0)
			{
				for(int i = 0; i < ParticlePos.Count; i++)
				{
					spriteBatch.Draw(texture, ParticlePos[i] - Main.screenPosition, null, new Color(200, 200, 255, 0) * (1f - (projectile.alpha / 255f)), projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0f);
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
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(projectile.Center.Y - (int)Main.screenPosition.Y) + y),
				null, color * (1f - (projectile.alpha / 255f)), projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
	}
}
		
			