using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Laser
{
	public class PinkLaser : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			// DisplayName.SetDefault("Pinky Laser");
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
		}

		public override void SetDefaults() 
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.timeLeft = 90;
			Projectile.penetrate = -1;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}
		public override void AI() 
		{
			//Projectile.Center = npc.Center;
			Projectile.alpha += 3;
			if (Projectile.alpha > 160) {
				Projectile.damage = 0;
			}
			if (Projectile.alpha > 250) {
				Projectile.Kill();
			}
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) 
		{
			float point = 0f;
			Vector2 endPoint = new Vector2(Projectile.ai[0], Projectile.ai[1]);
			Vector2 unit = endPoint - Projectile.Center;
			float length = unit.Length();
			unit.Normalize();
			for (float Distance = 0; Distance <= length; Distance += 6f) 
			{
				Vector2 position = Projectile.Center + unit * Distance;	
				int i = (int)(position.X / 16);
				int j =	(int)(position.Y / 16);
				if (!WorldGen.InWorld(i, j, 20) || Main.tile[i, j].HasTile && Main.tileSolidTop[Main.tile[i, j ].TileType] == false && Main.tileSolid[Main.tile[i, j ].TileType] == true && Main.tile[i, j].HasUnactuatedTile)
				{
					break;
				}
				if(Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, position, 10f, ref point))
				{
					return true;
				}
			}
			return false;
			//return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, endPoint, 8f, ref point);
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			bool dust = false;
			if(Projectile.alpha < 5)
			{
				dust = true;
			}
			Player player  = Main.player[Projectile.owner];
			Vector2 endPoint = new Vector2(Projectile.ai[0], Projectile.ai[1]);
			Vector2 unit = endPoint - Projectile.Center;
			float length = unit.Length();
			unit.Normalize();
			lightColor = new Color(255, 255, 255) * ((255 - Projectile.alpha) / 255f);
			for (float Distance = 0; Distance <= length; Distance += 3.5f) 
			{
				Vector2 drawPos = Projectile.Center + unit * Distance - Main.screenPosition;
				Vector2 position = Projectile.Center + unit * Distance;	
				int i = (int)(position.X / 16);
				int j =	(int)(position.Y / 16);
				if (!WorldGen.InWorld(i, j, 20) || Main.tile[i, j].HasTile && Main.tileSolidTop[Main.tile[i, j ].TileType] == false && Main.tileSolid[Main.tile[i, j ].TileType] == true && Main.tile[i, j].HasUnactuatedTile)
				{
					break;
				}
				float size = 0.4f + (Projectile.timeLeft/150f);
				if(Distance < 180)
				{
					size += (180 - Distance)/120f;
				}
				if(Distance >= 40)
				{
					float alphaMult = (255 - Projectile.alpha) / 255f;
					Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
					float rotation = (float)Math.Atan2(unit.Y, unit.X);
					float otherMult = 1f - Distance / length;
					float helix = otherMult * alphaMult * 0.8f * (float)Math.Sin(MathHelper.ToRadians(SOTSWorld.GlobalCounter * -8 + Distance));
					Main.spriteBatch.Draw(texture, drawPos, null, lightColor, rotation, texture.Size() / 2, new Vector2(1.0f, size * (1.2f + helix)), unit.X > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
					Lighting.AddLight(position, (255 - Projectile.alpha) * 0.3f / 255f, (255 - Projectile.alpha) * 0.3f / 255f, (255 - Projectile.alpha) * 0.3f / 255f);
					if(dust)
					{
						int num1 = Dust.NewDust(new Vector2(position.X, position.Y), 0, 0, 72);
						Main.dust[num1].noGravity = true;
						Main.dust[num1].velocity *= 2.5f;
						Main.dust[num1].scale = 2f;
					}
				}
			}
			return false;
		}
	}
}