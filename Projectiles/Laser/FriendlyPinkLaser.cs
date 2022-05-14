using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Laser
{
	public class FriendlyPinkLaser : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Pinky Laser");
		}

		public override void SetDefaults() 
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.timeLeft = 60;
			Projectile.magic = true;
			Projectile.penetrate = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}
		public override void AI() 
		{
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.8f / 255f, (255 - Projectile.alpha) * 0.8f / 255f, (255 - Projectile.alpha) * 0.8f / 255f);
			//Projectile.Center = npc.Center;
			Projectile.alpha += 4;
			if (Projectile.alpha > 160) {
				Projectile.damage = 0;
			}
			if (Projectile.alpha > 250) {
				Projectile.Kill();
			}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 5;
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

				if (!WorldGen.InWorld(i, j, 20) || Main.tile[i, j].HasTile && Main.tileSolidTop[Main.tile[i, j ].TileType] == false && Main.tileSolid[Main.tile[i, j ].TileType] == true && Main.tile[i, j].nactive())
				{
					break;
				}
				if(Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, position, 32f, ref point))
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
			lightColor = new Color(255,255,255) * ((255 - Projectile.alpha) / 255f);
			for (float Distance = 0; Distance <= length; Distance += 4f) {
				Vector2 drawPos = Projectile.Center + unit * Distance - Main.screenPosition;
				Vector2 position = Projectile.Center + unit * Distance;	
				int i = (int)(position.X / 16);
				int j =	(int)(position.Y / 16);
				if (!WorldGen.InWorld(i, j, 20) || Main.tile[i, j].HasTile && Main.tileSolidTop[Main.tile[i, j ].TileType] == false && Main.tileSolid[Main.tile[i, j ].TileType] == true && Main.tile[i, j].nactive())
				{
					Distance -= 6f;
					break;
				}
				float size = 0.6f + (Projectile.timeLeft/150f);
				if(Distance < 180)
				{
					size += 0.8f * (180 - Distance)/150f;
				}
				if(Distance >= 40)
				{
					spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, null, lightColor, (float)Math.Atan2(unit.Y, unit.X), new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height * 0.5f), size, SpriteEffects.None, 0f);
					if(dust)
					{
						int num1 = Dust.NewDust(new Vector2(position.X - 4, position.Y - 4), Projectile.width, Projectile.height, 72);
						Main.dust[num1].noGravity = true;
						Main.dust[num1].velocity *= 1.75f;
						Main.dust[num1].scale = 1.8f;
					}
				}
			}
			return false;
		}
	}
}