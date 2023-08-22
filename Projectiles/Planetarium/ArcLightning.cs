using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Planetarium
{    
    public class ArcLightning : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Thundershock Lightning"); //3 different ways of saying electricity
		}
		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 3600;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.alpha = 120;
			Projectile.scale = 1f;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (Projectile.alpha >= 150)
			{
				return false;
			}
			float scale = Projectile.scale;
			float width = Projectile.width * scale;
			Vector2 last = Projectile.Center;
			projHitbox = new Rectangle((int)last.X - (int)width / 2, (int)last.Y - (int)width / 2, (int)width, (int)width);
			if (projHitbox.Intersects(targetHitbox))
			{
				return true;
			}
			for (int i = 0; i < trailPos.Count; i++)
			{
				Vector2 pos = trailPos[i];
				projHitbox = new Rectangle((int)pos.X - (int)width / 2, (int)pos.Y - (int)width / 2, (int)width, (int)width);
				if (projHitbox.Intersects(targetHitbox))
				{
					return true;
				}
				if(Vector2.Distance(pos, last) > width * 2 + 16)
                {
					Vector2 inBetween = Vector2.Lerp(pos, last, 0.5f);
					projHitbox = new Rectangle((int)inBetween.X - (int)width / 2, (int)inBetween.Y - (int)width / 2, (int)width, (int)width);
					if (projHitbox.Intersects(targetHitbox))
					{
						return true;
					}
				}
				last = pos;
			}
			return false;
			//return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, endPoint, 8f, ref point);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center;
			Color color = new Color(132, 148, 180, 0) * ((255 - Projectile.alpha) / 255f);
			for (int k = 0; k < trailPos.Count; k++)
			{
				if (trailPos[k] == Vector2.Zero)
				{
					return false;
				}
				float scale = Projectile.scale * 0.7f;
				scale *= 1.3f - 1.2f * (k / (float)dist);
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				float max = betweenPositions.Length() / (texture.Width * scale * 0.33f);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					if (trailPos[k] != Projectile.Center)
						for(int a = 0; a < 2; a++)
							Main.spriteBatch.Draw(texture, drawPos + Main.rand.NextVector2Circular(2, 2), null, color, betweenPositions.ToRotation() + MathHelper.ToRadians(90), drawOrigin, scale, SpriteEffects.None, 0f);
				}
				previousPosition = currentPos;
			}
			return false;
		}
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		List<Vector2> trailPos = new List<Vector2>();
		int dist = 120;
		bool runOnce = true;
		float speed = 28f;
		public void runStartCalculations()
        {
			Vector2 location = Projectile.Center;
			Vector2 originalVelo = Projectile.velocity.SafeNormalize(Vector2.Zero) * speed;
			float defaultDeviation = 45f;
			int startingDeviation = 0;
			int startingPreference = Projectile.direction;
			if (Projectile.ai[0] == 1)
			{
				startingPreference = -Projectile.direction;
				Projectile.scale = 0.5f;
				speed = 22f;
				defaultDeviation = 70f;
			}
			for (int i = 0; i < dist; i++)
			{
				location += originalVelo;
				trailPos.Add(location + originalVelo.RotatedBy(MathHelper.ToRadians(45 * startingPreference + Main.rand.NextFloat(-defaultDeviation, defaultDeviation) * startingDeviation)) * 1.5f);
				int u = (int)location.X / 16;
				int j = (int)location.Y / 16;
				if (!WorldGen.InWorld(u, j, 20) || Main.tile[u, j].HasTile && Main.tileSolidTop[Main.tile[u, j].TileType] == false && Main.tileSolid[Main.tile[u, j].TileType] == true)
				{
					for(int l = 0; l < 20; l++)
					{
						int dust = Dust.NewDust(new Vector2(location.X - 16, location.Y - 16), 24, 24, DustID.Electric);
						Main.dust[dust].scale *= 1f;
						Main.dust[dust].velocity *= 1f;
						Main.dust[dust].noGravity = true;
					}
					trailPos.RemoveAt(i);
					break;
				}
				if(i % 2 == 0)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(location.X - 16, location.Y - 16), 24, 24, DustID.Electric);
					dust.scale *= 1.1f;
					dust.velocity *= 1.4f;
					dust.noGravity = true;
				}
				if (speed > 6)
					speed *= 0.98f;
				if(defaultDeviation > 30)
					defaultDeviation *= 0.98f;
				originalVelo = originalVelo.SafeNormalize(Vector2.Zero) * speed;
				startingPreference = 0;
				startingDeviation = 1;
			}
		}
		public override void AI()
		{
			if (runOnce)
			{
				Projectile.position += Projectile.velocity.SafeNormalize(Vector2.Zero) * speed;
				runStartCalculations();
				runOnce = false;
			}
			Projectile.alpha += 4;
			if (Projectile.alpha >= 255)
				Projectile.Kill();
		}
	}
}
		