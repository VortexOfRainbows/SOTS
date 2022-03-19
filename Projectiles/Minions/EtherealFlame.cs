using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Buffs;
using SOTS.Projectiles.Celestial;
using System.Collections.Generic;
using SOTS.Void;
using static SOTS.CurseHelper;
using System.IO;

namespace SOTS.Projectiles.Minions
{
	public class EtherealFlame : ModProjectile
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ethereal Flame");
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
		}
		public sealed override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 22;
			projectile.tileCollide = false;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.minionSlots = 1f;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.netImportant = true;
			projectile.localNPCHitCooldown = 30;
			projectile.extraUpdates = 2;
		}
		Vector2[] trailPos = new Vector2[24];
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
			Texture2D textureTrail = ModContent.GetTexture("SOTS/Projectiles/Minions/EtherealFlameTrail");
			Vector2 drawOrigin2 = new Vector2(textureTrail.Width / 2, 0);
			Vector2 lastPosition = projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				if (trailPos[k] == Vector2.Zero)
					break;
				float scale = 1f - 0.95f * (k / (float)trailPos.Length);
				Vector2 drawPos = trailPos[k];
				Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(colorCounter * 2 + k * 6));
				color.A = 0;
				Vector2 towards = lastPosition - drawPos;
				float lengthTowards = towards.Length() / textureTrail.Height / scale;
				for(int j = 0; j < 2; j++)
				{
					spriteBatch.Draw(textureTrail, drawPos - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), null, projectile.GetAlpha(color) * scale * (1 - j * 0.5f), towards.ToRotation() - MathHelper.PiOver2, drawOrigin2, new Vector2(1, lengthTowards) * scale * (1 + j * 0.05f), projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				}
				lastPosition = drawPos;
			}
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			DrawFlames();
			Vector2 drawPos2 = projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);
			Color color2 = VoidPlayer.pastelAttempt(MathHelper.ToRadians(colorCounter * 2));
			color2.A = 0;
			for(int i = 0; i < 3; i++)
				spriteBatch.Draw(texture, drawPos2 + Main.rand.NextVector2CircularEdge(1, 1), null, projectile.GetAlpha(color2), projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
		public void DrawFlames()
		{
			Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color;
			for (int i = 0; i < particleList.Count; i++)
			{
				color = particleList[i].color;
				color.A = 0;
				Vector2 drawPos = projectile.Center + particleList[i].position - Main.screenPosition;
				color = projectile.GetAlpha(color) * (0.4f + 0.6f * particleList[i].scale);
				Main.spriteBatch.Draw(texture, drawPos, null, color, particleList[i].rotation, drawOrigin, particleList[i].scale * 1.25f, SpriteEffects.None, 0f);
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			projectile.ai[0] = 1;
			projectile.netUpdate = true;
			if(Main.myPlayer == projectile.owner)
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, ModContent.ProjectileType<SmallStellarHitbox>(), projectile.damage, 0, Main.myPlayer, target.whoAmI);
		}
		bool atNewLocation = true;
		Vector2 toLocation = new Vector2(0, 0);
		public List<ColoredFireParticle> particleList = new List<ColoredFireParticle>();
		public void cataloguePos()
		{
			for (int i = 0; i < particleList.Count; i++)
			{
				ColoredFireParticle particle = particleList[i];
				particle.Update();
				if (!particle.active)
				{
					particleList.RemoveAt(i);
					i--;
				}
			}
		}
		bool runOnce = true;
		public override bool PreAI()
		{
			if(runOnce)
			{
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
            }
			if (Main.netMode != NetmodeID.Server)
			{
				if(!SOTS.Config.lowFidelityMode || Main.rand.NextBool(2))
				{
					if (!Main.rand.NextBool(3))
					{
						Vector2 rotational = new Vector2(0, -3f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-40f, 40f)));
						rotational.X *= 0.5f;
						rotational.Y *= 1f;
						particleList.Add(new ColoredFireParticle(rotational * -1.0f, rotational * 0.8f - projectile.velocity * 0.05f, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(1.1f, 1.2f), VoidPlayer.pastelAttempt(MathHelper.ToRadians(colorCounter * 2 + Main.rand.NextFloat(-1, 1)), true)));
					}
					if (Main.rand.NextBool(3))
                    {
						Vector2 rotational = new Vector2(0, -1.5f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
						particleList.Add(new ColoredFireParticle(Vector2.Zero, rotational * 0.8f - projectile.velocity * 0.2f, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(0.9f, 1.0f), VoidPlayer.pastelAttempt(MathHelper.ToRadians(colorCounter * 2 + Main.rand.NextFloat(-1, 1)), true)));
					}
				}
				colorCounter++;
				cataloguePos();
			}
			return base.PreAI();
		}
		int colorCounter = 0;
		public override void AI() 
		{
			Player player = Main.player[projectile.owner];
			#region Active check
			if (player.dead || !player.active) 
			{
				player.ClearBuff(ModContent.BuffType<Ethereal>());
			}
			if (player.HasBuff(ModContent.BuffType<Ethereal>()))
			{
				projectile.timeLeft = 2;
			}
			#endregion

			#region General behavior
			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 128f;
			
			if(atNewLocation)
			{
				if(projectile.owner == Main.myPlayer)
				{
					projectile.ai[1] = Main.rand.NextFloat(360);
					projectile.netUpdate = true;
				}
				atNewLocation = false;
			}
			toLocation = new Vector2(80, 0).RotatedBy(MathHelper.ToRadians(projectile.ai[1]));
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
			float distanceFromTarget = 1200f;
			Vector2 targetCenter = projectile.Center;
			bool foundTarget = false;
			// This code is required if your minion weapon has the targeting feature
			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, player.Center);
				if (between < 1800f) 
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
						float between = Vector2.Distance(npc.Center, player.Center);
						float between2 = Vector2.Distance(npc.Center, projectile.Center);
						bool inRange = between < distanceFromTarget;
						if (!inRange && between2 < distanceFromTarget * 0.75f)
						{
							between = between2;
							inRange = true;
						}
						bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);

						bool closeThroughWall = between < 400f;
						if (inRange && (lineOfSight || closeThroughWall))
						{
							distanceFromTarget = between;
							targetCenter = npc.Center;
							foundTarget = true;
						}
					}
				}
			}
			projectile.friendly = foundTarget;
			#endregion

			#region Movement
			float speed = 2f;
			float inertia = 15f;
			if (foundTarget)
			{
				Vector2 direction = targetCenter - projectile.Center;
				direction = direction.SafeNormalize(Vector2.Zero);
				if (projectile.ai[0] == 0)
				{
					projectile.velocity *= 0.96f;
					projectile.friendly = true;
					float mult = direction.Length() / 96f;
					if (mult > 1)
						mult = 1;
					if(projectile.velocity.Length() < 18)
                    {
						mult += 0.5f;
                    }
					projectile.velocity += direction * 2f * mult;
					if(projectile.owner == Main.myPlayer)
                    {
						projectile.netUpdate = true;
						projectile.ai[1] = Main.rand.NextFloat(45, 135) * (Main.rand.Next(2) * 2 - 1);
					}
				}
				else
				{
					projectile.velocity *= 0.9675f;
					projectile.friendly = false;
					projectile.velocity += direction.RotatedBy(MathHelper.ToRadians(projectile.ai[1])) * 0.69f;
					projectile.ai[0]++;
					if (projectile.velocity.Length() > 8)
					{
						projectile.velocity *= 0.9f;
					}
				}
				if(projectile.ai[0] > 40)
				{
					projectile.ai[0] = -40;
				}
			}
			else
			{
				if (projectile.ai[0] > 0)
				{
					projectile.ai[0]--;
				}
				else if (projectile.ai[0] < 0)
				{
					projectile.ai[0]++;
				}
				if (distanceToIdlePosition > 16f) 
				{
					vectorToIdlePosition = vectorToIdlePosition.SafeNormalize(Vector2.Zero) * (speed + 0.0075f * vectorToIdlePosition.Length());
					projectile.velocity = (projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
				}
				else
				{
					atNewLocation = true;
				}
			}
			projectile.alpha = (int)(255 * (1 - 1f * Math.Cos(1f * MathHelper.Clamp(projectile.ai[0] / 25f, -1, 1) * MathHelper.PiOver2)));
			projectile.alpha = (int)MathHelper.Clamp(projectile.alpha, 0, 255);
			#endregion

			#region Animation and visuals
			projectile.rotation = projectile.velocity.X * 0.05f;
			Lighting.AddLight(projectile.Center, Color.White.ToVector3() * 0.78f);
			#endregion
		}
        public override void PostAI()
		{
			Vector2 current = projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				trailPos[i] = current;
				current = previousPosition;
			}
		}
    }
}