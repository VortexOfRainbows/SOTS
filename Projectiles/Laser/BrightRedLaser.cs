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
	public class BrightRedLaser : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Artifact Laser");
		}

		public override void SetDefaults() 
		{
			projectile.width = 10;
			projectile.height = 10;
			projectile.timeLeft = 60;
			projectile.penetrate = -1;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}
		public override void AI() 
		{
			//projectile.Center = npc.Center;
			if((int)projectile.localAI[0] == 0)
				SoundEngine.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 12, 0.7f);
			projectile.localAI[0] += 1f;
			if (projectile.localAI[0] == 2f) {
				projectile.damage = 0;
				projectile.alpha += 75;
			}
			if (projectile.localAI[0] > 15f) {
				projectile.Kill();
			}
			projectile.alpha += 10;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 3;
			if(projectile.melee) target.immune[projectile.owner] = 0;
			
			projectile.damage--;
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
			Player player  = Main.player[projectile.owner];
			Vector2 endPoint = new Vector2(projectile.ai[0], projectile.ai[1]);
			Vector2 unit = endPoint - projectile.Center;
			float length = unit.Length();
			unit.Normalize();
			for (float Distance = 0; Distance <= length; Distance += 7f) {
				Distance += Main.rand.Next(5);
				Vector2 drawPos = projectile.Center + unit * Distance - Main.screenPosition;
				
				Vector2 position = projectile.Center + unit * Distance;	
				int i = (int)(position.X / 16);
				int j =	(int)(position.Y / 16);
				if(Main.tile[i, j].active() && Main.tileSolidTop[Main.tile[i, j].type] == false && Main.tileSolid[Main.tile[i, j].type] == true)
				{
					Distance -= 6f;
					break;
				}
				Color alpha = new Color(255, 0, 0) * ((255 - projectile.alpha) / 255f);
				//Color alpha = ((255 - projectile.alpha) / 255f);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, alpha, Distance, new Vector2(5, 5), (0.01f * (float)Main.rand.Next(50,151)), SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}