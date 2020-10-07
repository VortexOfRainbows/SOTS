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
using SOTS.Void;

namespace SOTS.Projectiles.Laser
{
	public class PinkLaser : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Pinky Laser");
		}

		public override void SetDefaults() 
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.timeLeft = 120;
			projectile.penetrate = -1;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}
		public override void AI() 
		{
			//projectile.Center = npc.Center;
			projectile.alpha += 2;
			if (projectile.alpha > 160) {
				projectile.damage = 0;
			}
			if (projectile.alpha > 250) {
				projectile.Kill();
			}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 7;
        }
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) 
		{
			float point = 0f;
			Vector2 endPoint = new Vector2(projectile.ai[0], projectile.ai[1]);
			Vector2 unit = endPoint - projectile.Center;
			float length = unit.Length();
			unit.Normalize();
			for (float Distance = 0; Distance <= length; Distance += 6f) 
			{
				Vector2 position = projectile.Center + unit * Distance;	
				int i = (int)(position.X / 16);
				int j =	(int)(position.Y / 16);

				if (!WorldGen.InWorld(i, j, 20) || Main.tile[i, j].active() && Main.tileSolidTop[Main.tile[i, j].type] == false && Main.tileSolid[Main.tile[i, j].type] == true)
				{
					break;
				}
				if(Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, position, 10f, ref point))
				{
					return true;
				}
			}
			return false;
			//return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, endPoint, 8f, ref point);
		}
		
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			bool dust = false;
			if(projectile.alpha < 5)
			{
				dust = true;
			}
			Player player  = Main.player[projectile.owner];
			Vector2 endPoint = new Vector2(projectile.ai[0], projectile.ai[1]);
			Vector2 unit = endPoint - projectile.Center;
			float length = unit.Length();
			unit.Normalize();
			lightColor = new Color(255, 255, 255) * ((255 - projectile.alpha) / 255f);
			for (float Distance = 0; Distance <= length; Distance += 4f) {
				Vector2 drawPos = projectile.Center + unit * Distance - Main.screenPosition;
				Vector2 position = projectile.Center + unit * Distance;	
				int i = (int)(position.X / 16);
				int j =	(int)(position.Y / 16);
				if (!WorldGen.InWorld(i, j, 20) || Main.tile[i, j].active() && Main.tileSolidTop[Main.tile[i, j].type] == false && Main.tileSolid[Main.tile[i, j].type] == true)
				{
					Distance -= 6f;
					break;
				}
				float size = 0.4f + (projectile.timeLeft/150f);
				if(Distance < 180)
				{
					size += (180 - Distance)/120f;
				}
				if(Distance >= 40)
				{
					spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, lightColor, (float)Math.Atan2(unit.Y, unit.X), new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, Main.projectileTexture[projectile.type].Height * 0.5f), size, SpriteEffects.None, 0f);
					Lighting.AddLight(position, (255 - projectile.alpha) * 0.3f / 255f, (255 - projectile.alpha) * 0.3f / 255f, (255 - projectile.alpha) * 0.3f / 255f);
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