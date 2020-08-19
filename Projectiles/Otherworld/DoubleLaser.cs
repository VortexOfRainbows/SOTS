using System;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{
	public class DoubleLaser : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Double Laser");
		}

		public override void SetDefaults() 
		{
			projectile.width = 16;
			projectile.height = 20;
			projectile.timeLeft = 120;
			projectile.magic = true;
			projectile.penetrate = -1;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		public override void AI() 
		{
			projectile.ai[0] += 3;
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.8f / 255f, (255 - projectile.alpha) * 0.8f / 255f, (255 - projectile.alpha) * 0.8f / 255f);
			//projectile.Center = npc.Center;
			projectile.alpha += 3;
			if (projectile.alpha > 255) {
				projectile.Kill();
			}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 5;
        }
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) 
		{
			return false;
		}
		int previousDistance = 120;
		public int isHittingEnemy(Vector2 position)
		{
			float point = 0f;
			Rectangle targetHitbox = new Rectangle(0, 0, 0, 0);
			for(int i = 0; i < Main.npc.Length; i++)
			{
				NPC npc = Main.npc[i];
				if(npc.active && !npc.friendly)
				{
					targetHitbox = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
					if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), position - new Vector2(16, 0), position + new Vector2(16, 0), 32f, ref point))
					{
						return npc.whoAmI;
					}
				}
			}
			return -1;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Otherworld/DoubleLaser");
			Texture2D texture2 = ModContent.GetTexture("SOTS/Projectiles/Otherworld/DoubleLaserEnd");
			bool dust = false;
			if(projectile.alpha < 5)
			{
				dust = true;
			}
			Vector2 unit = new Vector2(projectile.velocity.X, projectile.velocity.Y);
			Vector2 currentPos = projectile.Center;
			float radianDir = (float)Math.Atan2(unit.Y, unit.X);
			lightColor = new Color(110, 110, 110, 0) * ((255 - projectile.alpha) / 255f);
			float size = 0f;
			float rotate = projectile.ai[0];
			for (int Distance = 0; Distance < 120; Distance++)
			{
				int npcID = isHittingEnemy(currentPos);
				bool collidingNPC = npcID >= 0;
				float additionalEnd = 0;
				if (Distance == 119)
				{
					additionalEnd = 9;
				}
				Vector2 drawPos = currentPos - Main.screenPosition;
				Vector2 position = currentPos;
				int i = (int)(position.X / 16);
				int j = (int)(position.Y / 16);
				if ((Main.tile[i, j].active() && Main.tileSolidTop[Main.tile[i, j].type] == false && Main.tileSolid[Main.tile[i, j].type] == true && Distance < 119) || collidingNPC)
				{
					Vector2 additional = new Vector2(additionalEnd, 0f).RotatedBy(radianDir);
					currentPos += additional;
					for (int k = 0; k < 10f - (8f * (255 - projectile.alpha) / 255f); k++)
					{
						int dust1 = Dust.NewDust(position + new Vector2(-4, -4), 0, 0, 91, 0, 0, projectile.alpha, default, 1.25f);
						Main.dust[dust1].noGravity = true;
						Main.dust[dust1].velocity *= 1.5f;
						Main.dust[dust1].alpha = projectile.alpha;
						Main.dust[dust1].velocity += projectile.velocity / 2f;
					}
					if(collidingNPC)
					{
						NPC npc = Main.npc[npcID];
						if(npc.immune[projectile.owner] <= 0)
						{
							if (projectile.owner == Main.myPlayer)
								Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, mod.ProjectileType("DoubleLaserExplosion"), projectile.damage, projectile.knockBack, projectile.owner);
						}
					}
					previousDistance = Distance;
					Distance = 119;
				}
				Vector2 rotateVector = new Vector2(1.75f, 0).RotatedBy(MathHelper.ToRadians(rotate));
				if (size < 0.9f)
					size += 0.02f;
				rotate += 3;
				Vector2 laserVelo = new Vector2((14.25f + additionalEnd) * (0.1f + size), 0f).RotatedBy(radianDir) + rotateVector;
				currentPos.X += laserVelo.X;
				currentPos.Y += laserVelo.Y;
				Lighting.AddLight(position, lightColor.R / 255f, lightColor.G / 255f, lightColor.B / 255f);
				for (int s = 0; s < 6; s++)
				{
					float x = Main.rand.Next(-10, 11) * 0.125f;
					float y = Main.rand.Next(-10, 11) * 0.125f;
					if(Distance == 119)
						spriteBatch.Draw(texture2, drawPos + new Vector2(x, y), null, lightColor, (float)Math.Atan2(unit.Y, unit.X), new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f), 0.2f + size, SpriteEffects.None, 0f);
					else
						spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, lightColor, (float)Math.Atan2(unit.Y, unit.X), new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 0.2f + size, SpriteEffects.None, 0f);
				}
				if(dust || Main.rand.Next(40) == 0)
				{
					int num1 = Dust.NewDust(new Vector2(position.X - 4, position.Y - 4), projectile.width, projectile.height, 91);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity *= 1.75f;
					Main.dust[num1].scale = 1.1f;
				}
			}
			return false;
		}
	}
}