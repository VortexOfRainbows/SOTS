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
			projectile.timeLeft = 300;
			projectile.penetrate = -1;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}
		public override void AI() 
		{
			//projectile.Center = npc.Center;
			projectile.alpha += projectile.timeLeft <= 11 ? 20 : 0;
			if (projectile.alpha > 200) {
				projectile.damage = 0;
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
				
				if(Main.tile[i, j].active() && Main.tileSolidTop[Main.tile[i, j].type] == false && Main.tileSolid[Main.tile[i, j].type] == true)
				{
					break;
				}
				if(Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, position, 8f, ref point))
				{
					return true;
				}
			}
			return false;
			//return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, endPoint, 8f, ref point);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player  = Main.player[projectile.owner];
			Vector2 endPoint = new Vector2(projectile.ai[0], projectile.ai[1]);
			Vector2 unit = endPoint - projectile.Center;
			float length = unit.Length();
			unit.Normalize();
			for (float Distance = 0; Distance <= length; Distance += 5f) {
				Distance += Main.rand.Next(4);
				Vector2 drawPos = projectile.Center + unit * Distance - Main.screenPosition;
				
				Vector2 position = projectile.Center + unit * Distance;	
				int i = (int)(position.X / 16);
				int j =	(int)(position.Y / 16);
				if(Main.tile[i, j].active() && Main.tileSolidTop[Main.tile[i, j].type] == false && Main.tileSolid[Main.tile[i, j].type] == true)
				{
					Distance -= 6f;
					break;
				}
				Color alpha = new Color(255, 0, 225) * ((255 - projectile.alpha) / 255f);
				//Color alpha = ((255 - projectile.alpha) / 255f);
				if(Distance >= 30)
				{
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, alpha, Distance, new Vector2(4, 4), 1f, SpriteEffects.None, 0f);
				}
			}
			return false;
		}
	}
}