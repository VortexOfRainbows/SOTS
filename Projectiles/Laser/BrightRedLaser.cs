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
			// DisplayName.SetDefault("Artifact Laser");
		}
		public override void SetDefaults() 
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.timeLeft = 60;
			Projectile.penetrate = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}
		public override void AI() 
		{
			//Projectile.Center = npc.Center;
			if((int)Projectile.localAI[0] == 0)
				SOTSUtils.PlaySound(SoundID.Item12, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.7f);
			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] == 2f) {
				Projectile.damage = 0;
				Projectile.alpha += 75;
			}
			if (Projectile.localAI[0] > 15f) {
				Projectile.Kill();
			}
			Projectile.alpha += 10;
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 3;
			if(Projectile.CountsAsClass(DamageClass.Melee)) 
				target.immune[Projectile.owner] = 0;
			Projectile.damage--;
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
				
				if(Main.tile[i, j].HasTile && Main.tileSolidTop[Main.tile[i, j ].TileType] == false && Main.tileSolid[Main.tile[i, j ].TileType] == true)
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
			Player player  = Main.player[Projectile.owner];
			Vector2 endPoint = new Vector2(Projectile.ai[0], Projectile.ai[1]);
			Vector2 unit = endPoint - Projectile.Center;
			float length = unit.Length();
			unit.Normalize();
			for (float Distance = 0; Distance <= length; Distance += 7f) {
				Distance += Main.rand.Next(5);
				Vector2 drawPos = Projectile.Center + unit * Distance - Main.screenPosition;
				
				Vector2 position = Projectile.Center + unit * Distance;	
				int i = (int)(position.X / 16);
				int j =	(int)(position.Y / 16);
				if(Main.tile[i, j].HasTile && Main.tileSolidTop[Main.tile[i, j ].TileType] == false && Main.tileSolid[Main.tile[i, j ].TileType] == true)
				{
					Distance -= 6f;
					break;
				}
				Color alpha = new Color(255, 0, 0) * ((255 - Projectile.alpha) / 255f);
				//Color alpha = ((255 - Projectile.alpha) / 255f);
				Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, null, alpha, Distance, new Vector2(5, 5), (0.01f * (float)Main.rand.Next(50,151)), SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}