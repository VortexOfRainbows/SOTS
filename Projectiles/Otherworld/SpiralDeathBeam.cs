using System;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{
	public class SpiralDeathBeam : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Spiral Death Beam");
		}
		public override void SetDefaults() 
		{
			Projectile.width = 16;
			Projectile.height = 20;
			Projectile.timeLeft = 100;
			// Projectile.magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Projectile.penetrate = -1;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		float alpha = 0;
		public override void AI() 
		{
			Projectile.ai[0] += 2.65f;
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.8f / 255f, (255 - Projectile.alpha) * 0.8f / 255f, (255 - Projectile.alpha) * 0.8f / 255f);
			//Projectile.Center = npc.Center;
			alpha += 2.55f;
			Projectile.alpha = (int)alpha;
			if(Projectile.alpha > 215)
            {
				Projectile.friendly = false;
				Projectile.hostile = false;
            }
			if (Projectile.alpha > 255) {
				Projectile.Kill();
			}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 5;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			int maxDist = 200;
			float point = 0f;
			Vector2 unit = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);
			Vector2 currentPos = Projectile.Center;
			float radianDir = (float)Math.Atan2(unit.Y, unit.X);
			float size = 0f;
			float rotate = Projectile.ai[0];
			for (int Distance = 0; Distance < maxDist; Distance++)
			{
				float additionalEnd = 0;
				if (Distance == maxDist - 1)
				{
					additionalEnd = 9;
				}
				Vector2 position = currentPos;
				int i = (int)(position.X / 16);
				int j = (int)(position.Y / 16);
				if (!WorldGen.InWorld(i, j, 20) || (Main.tile[i, j].HasTile && Main.tileSolidTop[Main.tile[i, j ].TileType] == false && Main.tileSolid[Main.tile[i, j ].TileType] == true && Distance < maxDist - 1))
				{
					Vector2 additional = new Vector2(additionalEnd, 0f).RotatedBy(radianDir);
					currentPos += additional;
					Distance = maxDist - 1;
				}
				Vector2 rotateVector = new Vector2(1.75f, 0).RotatedBy(MathHelper.ToRadians(rotate));
				if (size < 0.9f)
					size += 0.02f;
				rotate += 720f / maxDist;
				Vector2 laserVelo = new Vector2((14.25f + additionalEnd) * (0.1f + size), 0f).RotatedBy(radianDir) + rotateVector;
				currentPos.X += laserVelo.X;
				currentPos.Y += laserVelo.Y;
				if (Main.tile[i, j].HasTile && Main.tileSolidTop[Main.tile[i, j ].TileType] == false && Main.tileSolid[Main.tile[i, j ].TileType] == true)
				{
					break;
				}
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), currentPos - new Vector2(8, 0), currentPos + new Vector2(8, 0), 20f, ref point))
				{
					return true;
				}
			}
			return false;
			//return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, endPoint, 8f, ref point);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			int maxDist = 200;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Otherworld/SpiralDeathBeam");
			Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Otherworld/SpiralDeathBeamEnd");
			bool dust = false;
			if(Projectile.alpha < 5)
			{
				dust = true;
			}
			Vector2 unit = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);
			Vector2 currentPos = Projectile.Center;
			float radianDir = (float)Math.Atan2(unit.Y, unit.X);
			lightColor = new Color(110, 110, 110, 0) * ((255 - Projectile.alpha) / 255f);
			float size = 0f;
			float rotate = Projectile.ai[0];
			for (int Distance = 0; Distance < maxDist; Distance++)
			{
				float additionalEnd = 0;
				if (Distance == maxDist - 1)
				{
					additionalEnd = 9;
				}
				Vector2 drawPos = currentPos - Main.screenPosition;
				Vector2 position = currentPos;
				int i = (int)(position.X / 16);
				int j = (int)(position.Y / 16);
				if (!WorldGen.InWorld(i, j, 20) || (Main.tile[i, j].HasTile && Main.tileSolidTop[Main.tile[i, j ].TileType] == false && Main.tileSolid[Main.tile[i, j ].TileType] == true && Distance < maxDist - 1))
				{
					Vector2 additional = new Vector2(additionalEnd, 0f).RotatedBy(radianDir);
					currentPos += additional;
					int dust1 = Dust.NewDust(position + new Vector2(-4, -4), 0, 0, DustID.Electric, 0, 0, Projectile.alpha, default, 1.25f);
					Main.dust[dust1].noGravity = true;
					Main.dust[dust1].velocity *= 1.5f;
					Main.dust[dust1].alpha = Projectile.alpha;
					Main.dust[dust1].velocity += Projectile.velocity / 2f;
					Distance = maxDist - 1;
				}
				Vector2 rotateVector = new Vector2(1.75f, 0).RotatedBy(MathHelper.ToRadians(rotate));
				if (size < 0.9f)
					size += 0.02f;
				rotate += 720f / maxDist;
				Vector2 laserVelo = new Vector2((14.25f + additionalEnd) * (0.1f + size), 0f).RotatedBy(radianDir) + rotateVector;
				currentPos.X += laserVelo.X;
				currentPos.Y += laserVelo.Y;
				Lighting.AddLight(position, lightColor.R / 255f, lightColor.G / 255f, lightColor.B / 255f);
				for (int s = 0; s < 5; s++)
				{
					float x = Main.rand.Next(-10, 11) * 0.125f;
					float y = Main.rand.Next(-10, 11) * 0.125f;
					if(Distance == maxDist - 1)
						spriteBatch.Draw(texture2, drawPos + new Vector2(x, y), null, lightColor, (float)Math.Atan2(unit.Y, unit.X), new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f), 0.2f + size, SpriteEffects.None, 0f);
					else
						spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, lightColor, (float)Math.Atan2(unit.Y, unit.X), new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 0.2f + size, SpriteEffects.None, 0f);
				}
				if(dust || Main.rand.Next(120) == 0)
				{
					int num1 = Dust.NewDust(new Vector2(position.X - 4, position.Y - 4), Projectile.width, Projectile.height, DustID.Electric);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity *= 1.75f;
					Main.dust[num1].scale = 1.1f;
				}
			}
			return false;
		}
	}
}