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

namespace SOTS.Projectiles.Minions
{
	public class EtherealFlame : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ethereal Flame");
			Main.projFrames[projectile.type] = 5;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;

			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			// Don't mistake this with "if this is true, then it will automatically home". It is just for damage reduction for certain NPCs
			ProjectileID.Sets.Homing[projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;  
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;   
		}

		public sealed override void SetDefaults()
		{
			projectile.width = 28;
			projectile.height = 52;
			projectile.tileCollide = false;

			projectile.friendly = true;
			projectile.minion = true;
			projectile.minionSlots = 1f;
			projectile.penetrate = -1;
		}
		public override bool? CanCutTiles()
		{
			return false;
		}
		public override bool MinionContactDamage()
		{
			return true;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Minions/EtherealFlame" + (projectile.frame + 1));
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, null, color * 0.5f, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}	
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[projectile.owner];
			projectile.ai[0] = 1;
			projectile.ai[1] = 1;
			if(target.life <= 2500)
			{
            target.immune[projectile.owner] = 5;
			}
			else
			{
            target.immune[projectile.owner] = 3;
			}
			projectile.netUpdate = true;
			if(Main.myPlayer == projectile.owner)
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("SmallStellarHitbox"), 0, 0, Main.myPlayer);
		}
		bool atNewLocation = true;
		Vector2 toLocation = new Vector2(0, 0);
		public override void AI() 
		{
			Player player = Main.player[projectile.owner];
		
			projectile.netUpdate = true;
			#region Active check
			if (player.dead || !player.active) 
			{
				player.ClearBuff(mod.BuffType("Ethereal"));
			}
			if (player.HasBuff(mod.BuffType("Ethereal")))
			{
				projectile.timeLeft = 2;
			}
			#endregion

			#region General behavior
			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 64f;
			
			if(atNewLocation)
			{
				toLocation = new Vector2(48, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
				atNewLocation = false;
			}
			idlePosition += toLocation;
			
			Vector2 vectorToIdlePosition = idlePosition - projectile.Center;
			float distanceToIdlePosition = vectorToIdlePosition.Length();
			if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 2000f) 
			{
				projectile.position = idlePosition;
				projectile.velocity *= 0.1f;
				projectile.netUpdate = true;
			}
			#endregion

			#region Find target
			float distanceFromTarget = 1000f;
			Vector2 targetCenter = projectile.Center;
			bool foundTarget = false;
			int targetWidth = 0;

			// This code is required if your minion weapon has the targeting feature
			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, player.Center);
				
				if (between < 1600f) 
				{
					distanceFromTarget = between;
					targetWidth = npc.width;
					targetCenter = npc.Center;
					foundTarget = true;
				}
			}
			if (!foundTarget) 
			{
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy()) 
					{
						float between = Vector2.Distance(npc.Center, player.Center);
						bool inRange = between < distanceFromTarget;
						bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
						
						bool closeThroughWall = between < 800f; //should attack semi-reliably through walls
						if (inRange && (lineOfSight || closeThroughWall))
						{
							distanceFromTarget = between;
							targetWidth = npc.width;
							targetCenter = npc.Center;
							foundTarget = true;
						}
					}
				}
			}
			projectile.friendly = foundTarget;
			#endregion

			#region Movement
			float speed = 62f;
			float inertia = 17f;

			if (foundTarget) 
			{
				if (projectile.ai[0] == 0) 
				{
					projectile.friendly = true;
					Vector2 direction = targetCenter - projectile.Center;
					direction.Normalize();
					direction *= speed;
					projectile.velocity = (projectile.velocity * (inertia - 1) + direction) / inertia;
				}
				else
				{
					projectile.ai[0]++;
					projectile.friendly = false;
					projectile.velocity *= 0.97f;
				}
				if(projectile.ai[0] == 11)
				{
					projectile.velocity *= 0f;
					Vector2 nextLocation = new Vector2(56 + targetWidth/1.2f, 0).RotatedBy(Math.Atan2(projectile.Center.Y - targetCenter.Y, projectile.Center.X - targetCenter.X) - MathHelper.ToRadians(Main.rand.Next(105)));
					projectile.position = new Vector2(targetCenter.X + nextLocation.X - projectile.width/2, targetCenter.Y + nextLocation.Y - projectile.height/2);
				}
				if(projectile.ai[0] > 16)
				{
					projectile.ai[0] = 0;
				}
			}
			else 
			{
				projectile.ai[0] = 0;
				if (distanceToIdlePosition > 600f)
				{
					speed = 30f;
					inertia = 20f;
				}
				else
				{
					speed = 16.5f;
					inertia = 30f;
				}
				if (distanceToIdlePosition > 20f) 
				{
					vectorToIdlePosition.Normalize();
					vectorToIdlePosition *= speed;
					projectile.velocity = (projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
				}
				else
				{
					atNewLocation = true;
				}
			}
			projectile.alpha = (int)projectile.ai[0] * 26;
			#endregion

			#region Animation and visuals
			projectile.rotation = projectile.velocity.X * 0.05f;

			int frameSpeed = 6;
			projectile.frameCounter++;
			if (projectile.frameCounter >= frameSpeed) {
				projectile.frameCounter = 0;
				projectile.frame++;
				if (projectile.frame >= Main.projFrames[projectile.type]) {
					projectile.frame = 0;
				}
			}
			if(projectile.ai[1] == 1)
			{
				/*
				int color = Main.rand.Next(2);
				float size = 60f;
				float starPosX = projectile.Center.X - size/2f;
				float starPosY = projectile.Center.Y - size/6f;
				for(int i = 0; i < 5; i ++)
				{
					float rads = MathHelper.ToRadians(144 * i);
					for(float j = 0; j < size; j += 3f)
					{
						int num1 = Dust.NewDust(new Vector2(starPosX, starPosY), 0, 0, color == 0 ? 88 : 21);
						Main.dust[num1].noGravity = true;
						Main.dust[num1].velocity *= 0.1f;
						
						Vector2 rotationDirection = new Vector2(3f, 0).RotatedBy(rads);
						starPosX += rotationDirection.X;
						starPosY += rotationDirection.Y;
					}
				}
				Main.PlaySound(SoundID.Item9, (int)(projectile.Center.X), (int)(projectile.Center.Y));
				*/
				projectile.ai[1] = 0;
			}
			Lighting.AddLight(projectile.Center, Color.White.ToVector3() * 0.78f);
			#endregion
		}
	}
}