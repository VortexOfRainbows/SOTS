using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;

namespace SOTS.Projectiles.Minions
{
	public class EarthenSpirit : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Earthen Spirit");
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
			projectile.width = 34;
			projectile.height = 34;
			projectile.tileCollide = false;

			projectile.friendly = true;
			projectile.minion = true;
			projectile.minionSlots = 1f;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.alpha);
			writer.Write(readyToFight);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.alpha = reader.ReadInt32();
			readyToFight = reader.ReadBoolean();
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
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, null, color * 0.5f, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Texture2D texture2 = mod.GetTexture("Projectiles/Minions/EarthenSpiritReticle");
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < 9; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.25f;
				float y = Main.rand.Next(-10, 11) * 0.25f;
				Main.spriteBatch.Draw(texture, new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * ((255 - projectile.alpha) / 255f), 0f, drawOrigin, projectile.scale, SpriteEffects.None, 0f);

				x = Main.rand.Next(-10, 11) * 0.125f;
				y = Main.rand.Next(-10, 11) * 0.125f;
				float reticleAlpha = projectile.ai[0] / 60f;
				if(reticleAlpha > 1)
				{
					reticleAlpha = 1;
				}
				if(projectile.velocity.Length() <= 10f)
					Main.spriteBatch.Draw(texture2, new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * reticleAlpha, MathHelper.ToRadians((projectile.ai[0] + 2) * 6f), drawOrigin, projectile.scale * reticleAlpha, SpriteEffects.None, 0f);
			}
		}
		bool readyToFight = false;
		public void dustSound()
		{
			Main.PlaySound(2, (int)(projectile.Center.X), (int)(projectile.Center.Y), 14, 0.4f);
			for (int i = 0; i < 360; i += 20)
			{
				Vector2 circularLocation = new Vector2(5, 0).RotatedBy(MathHelper.ToRadians(i));

				int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 222);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].scale = 1.2f;
				Main.dust[num1].velocity = circularLocation * 0.25f + new Vector2(Main.rand.Next(-20, 21), Main.rand.Next(-20, 21)) * 0.1f;


				num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 222);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].scale = 1.5f;
				Main.dust[num1].velocity = circularLocation * 0.45f + new Vector2(Main.rand.Next(-20, 21), Main.rand.Next(-20, 21)) * 0.2f;
			}
		}
		public override void AI() 
		{
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");

			#region Active check
			if (player.dead || !player.active) 
			{
				player.ClearBuff(mod.BuffType("SpiritAid"));
			}
			if (player.HasBuff(mod.BuffType("SpiritAid")))
			{
				projectile.timeLeft = 2;
			}
			#endregion

			#region General behavior
			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 96f;

			bool found = false;
			int ofTotal = 0;
			int total = 0;
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if (projectile.type == proj.type && proj.active && projectile.active && proj.owner == projectile.owner)
				{
					if (proj == projectile)
					{
						found = true;
					}
					if (!found)
						ofTotal++;
					total++;
				}
			}
			#endregion

			#region Find target
			float distanceFromTarget = 800f;
			Vector2 targetCenter = projectile.Center;
			bool foundTarget = false;

			// This code is required if your minion weapon has the targeting feature
			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, projectile.Center);
				float between2 = Vector2.Distance(npc.Center, player.Center);

				if (between2 < distanceFromTarget) 
				{
					distanceFromTarget = between;
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
						float between = Vector2.Distance(npc.Center, projectile.Center);
						float between2 = Vector2.Distance(npc.Center, player.Center);
						bool inRange = between < distanceFromTarget;
						bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
						
						bool closeThroughWall = between < 100f; //should attack semi-reliably through walls
						if (inRange && (lineOfSight || closeThroughWall) && between2 < 1000f)
						{
							distanceFromTarget = between;
							targetCenter = npc.Center;
							foundTarget = true;
						}
					}
				}
			}
			#endregion

			#region Movement
			float speed = 10f;

			if (foundTarget)
			{
				if (projectile.alpha >= 255)
					speed = 50f;

				Vector2 direction = targetCenter - projectile.Center;
				float distance = direction.Length() + 0.1f;
				if (distance > 1.1f)
				{
					direction = direction.SafeNormalize(Vector2.Zero);
					if (distance > speed)
					{
						distance = speed;
					}
					direction *= distance;
					projectile.velocity = direction;
				}
				else
				{
					projectile.velocity *= 0f;
				}
				projectile.alpha += 10;

				if (projectile.alpha > 255)
				{
					projectile.alpha = 255;	
					if (readyToFight)
						projectile.ai[0]++;

					if (total != 0 && (int)modPlayer.orbitalCounter % 60 == (int)(ofTotal * 60f / total + 0.5f) % 60)
					{
						if (readyToFight)
						{
							projectile.ai[0] = 0;
							projectile.alpha = 0;
							projectile.friendly = true;
							dustSound();
							readyToFight = false;
						}
						else
						{
							readyToFight = true;
						}
					}
				}
				else
				{
					projectile.friendly = false;
				}
			}
			else
			{
				projectile.ai[0] = 0;
				readyToFight = false;
				projectile.alpha -= 8;
				if(projectile.alpha < 0)
				{
					projectile.alpha = 0;
				}

				Vector2 toPlayer = idlePosition - projectile.Center;
				float distance = toPlayer.Length();
				speed = 10f;
				projectile.velocity = new Vector2(-speed, 0).RotatedBy(Math.Atan2(projectile.Center.Y - idlePosition.Y, projectile.Center.X - idlePosition.X));

				if (distance < 256)
				{
					Vector2 rotateCenter = new Vector2(64, 0).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter + (ofTotal * 360f / total)));
					rotateCenter += idlePosition;
					Vector2 toRotate = rotateCenter - projectile.Center;
					float dist2 = toRotate.Length();
					if (dist2 > 12)
					{
						dist2 = 12;
					}
					projectile.velocity = new Vector2(-dist2, 0).RotatedBy(Math.Atan2(projectile.Center.Y - rotateCenter.Y, projectile.Center.X - rotateCenter.X));
				}

				Vector2 vectorToIdlePosition = idlePosition - projectile.Center;
				float distanceToIdlePosition = vectorToIdlePosition.Length();
				if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 1400f)
				{
					projectile.ai[0] = 0;
					readyToFight = false;
					projectile.alpha = 0;
					projectile.position = idlePosition;
					projectile.velocity *= 0.1f;
					projectile.netUpdate = true;
				}
			}
			#endregion

			#region Animation and visuals
			Lighting.AddLight(projectile.Center, 2.4f * 0.5f * ((255 - projectile.alpha) / 255f), 2.2f * 0.5f * ((255 - projectile.alpha) / 255f), 1.4f * 0.5f * ((255 - projectile.alpha) / 255f));
			#endregion

			if (Main.myPlayer == player.whoAmI)
			{
				projectile.netUpdate = true;
			}
		}
	}
}