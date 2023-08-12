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
using SOTS.Buffs.MinionBuffs;

namespace SOTS.Projectiles.Minions
{
	public class EtherealFlame : ModProjectile
	{
        public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ethereal Flame");
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}
		public sealed override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.minionSlots = 1f;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.netImportant = true;
			Projectile.localNPCHitCooldown = 30;
			Projectile.extraUpdates = 2;
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
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D textureTrail = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Minions/EtherealFlameTrail");
			Vector2 drawOrigin2 = new Vector2(textureTrail.Width / 2, 0);
			Vector2 lastPosition = Projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				if (trailPos[k] == Vector2.Zero)
					break;
				float scale = 1f - 0.95f * (k / (float)trailPos.Length);
				Vector2 drawPos = trailPos[k];
				Color color = ColorHelpers.pastelAttempt(MathHelper.ToRadians(colorCounter * 2 + k * 6));
				color.A = 0;
				Vector2 towards = lastPosition - drawPos;
				float lengthTowards = towards.Length() / textureTrail.Height / scale;
				for(int j = 0; j < 2 - (SOTS.Config.lowFidelityMode ? 1 : 0); j++)
				{
					Main.spriteBatch.Draw(textureTrail, drawPos - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, Projectile.GetAlpha(color) * scale * (1 - j * 0.5f), towards.ToRotation() - MathHelper.PiOver2, drawOrigin2, new Vector2(1, lengthTowards) * scale * (1 + j * 0.05f), Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				}
				lastPosition = drawPos;
			}
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			DrawFlames();
			Vector2 drawPos2 = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
			Color color2 = ColorHelpers.pastelAttempt(MathHelper.ToRadians(colorCounter * 2));
			color2.A = 0;
			for(int i = 0; i < 3 - (SOTS.Config.lowFidelityMode ? 1 : 0); i++)
				Main.spriteBatch.Draw(texture, drawPos2 + Main.rand.NextVector2CircularEdge(1, 1), null, Projectile.GetAlpha(color2), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
		public void DrawFlames()
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color;
			for (int i = 0; i < particleList.Count; i++)
			{
				color = particleList[i].color;
				color.A = 0;
				Vector2 drawPos = Projectile.Center + particleList[i].position - Main.screenPosition;
				color = Projectile.GetAlpha(color) * (0.4f + 0.6f * particleList[i].scale);
				Main.spriteBatch.Draw(texture, drawPos, null, color, particleList[i].rotation, drawOrigin, particleList[i].scale * 1.25f, SpriteEffects.None, 0f);
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			Projectile.ai[0] = 1;
			Projectile.netUpdate = true;
			if(Main.myPlayer == Projectile.owner)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<SmallStellarHitbox>(), Projectile.damage, 0, Main.myPlayer, target.whoAmI);
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
			//Projectile.SetDamageBasedOnOriginalDamage(Projectile.owner); //only for void minions
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
				if(!SOTS.Config.lowFidelityMode || Main.rand.NextBool(3))
				{
					if (!Main.rand.NextBool(3))
					{
						Vector2 rotational = new Vector2(0, -3f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-40f, 40f)));
						rotational.X *= 0.5f;
						rotational.Y *= 1f;
						particleList.Add(new ColoredFireParticle(rotational * -1.0f, rotational * 0.8f - Projectile.velocity * 0.05f, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(1.1f, 1.2f), ColorHelpers.pastelAttempt(MathHelper.ToRadians(colorCounter * 2 + Main.rand.NextFloat(-1, 1)), true)));
					}
					if (Main.rand.NextBool(3))
                    {
						Vector2 rotational = new Vector2(0, -1.5f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
						particleList.Add(new ColoredFireParticle(Vector2.Zero, rotational * 0.8f - Projectile.velocity * 0.2f, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(0.9f, 1.0f), ColorHelpers.pastelAttempt(MathHelper.ToRadians(colorCounter * 2 + Main.rand.NextFloat(-1, 1)), true)));
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
			Player player = Main.player[Projectile.owner];
			#region Active check
			if (player.dead || !player.active) 
			{
				player.ClearBuff(ModContent.BuffType<Ethereal>());
			}
			if (player.HasBuff(ModContent.BuffType<Ethereal>()))
			{
				Projectile.timeLeft = 2;
			}
			#endregion

			#region General behavior
			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 128f;
			
			if(atNewLocation)
			{
				if(Projectile.owner == Main.myPlayer)
				{
					Projectile.ai[1] = Main.rand.NextFloat(360);
					Projectile.netUpdate = true;
				}
				atNewLocation = false;
			}
			toLocation = new Vector2(80, 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[1]));
			idlePosition += toLocation;
			
			Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
			float distanceToIdlePosition = vectorToIdlePosition.Length();
			if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 2000f) 
			{
				Projectile.position = idlePosition;
				Projectile.velocity *= 0.1f;
				Projectile.netUpdate = true;
			}
			#endregion

			#region Find target
			float distanceFromTarget = 1200f;
			Vector2 targetCenter = Projectile.Center;
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
						float between2 = Vector2.Distance(npc.Center, Projectile.Center);
						bool inRange = between < distanceFromTarget;
						if (!inRange && between2 < distanceFromTarget * 0.75f)
						{
							between = between2;
							inRange = true;
						}
						bool closeThroughWall = between < 400f;
						bool lineOfSight = false;
						if(!closeThroughWall) 
							lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
						if (inRange && (lineOfSight || closeThroughWall))
						{
							distanceFromTarget = between;
							targetCenter = npc.Center;
							foundTarget = true;
						}
					}
				}
			}
			Projectile.friendly = foundTarget;
			#endregion

			#region Movement
			float speed = 2f;
			float inertia = 15f;
			if (foundTarget)
			{
				Vector2 direction = targetCenter - Projectile.Center;
				direction = direction.SafeNormalize(Vector2.Zero);
				if (Projectile.ai[0] == 0)
				{
					Projectile.velocity *= 0.96f;
					Projectile.friendly = true;
					float mult = direction.Length() / 96f;
					if (mult > 1)
						mult = 1;
					if(Projectile.velocity.Length() < 18)
                    {
						mult += 0.5f;
                    }
					Projectile.velocity += direction * 2f * mult;
					if(Projectile.owner == Main.myPlayer)
                    {
						Projectile.netUpdate = true;
						Projectile.ai[1] = Main.rand.NextFloat(45, 135) * (Main.rand.Next(2) * 2 - 1);
					}
				}
				else
				{
					Projectile.velocity *= 0.9675f;
					Projectile.friendly = false;
					Projectile.velocity += direction.RotatedBy(MathHelper.ToRadians(Projectile.ai[1])) * 0.69f;
					Projectile.ai[0]++;
					if (Projectile.velocity.Length() > 8)
					{
						Projectile.velocity *= 0.9f;
					}
				}
				if(Projectile.ai[0] > 40)
				{
					Projectile.ai[0] = -40;
				}
			}
			else
			{
				if (Projectile.ai[0] > 0)
				{
					Projectile.ai[0]--;
				}
				else if (Projectile.ai[0] < 0)
				{
					Projectile.ai[0]++;
				}
				if (distanceToIdlePosition > 16f) 
				{
					vectorToIdlePosition = vectorToIdlePosition.SafeNormalize(Vector2.Zero) * (speed + 0.0075f * vectorToIdlePosition.Length());
					Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
				}
				else
				{
					atNewLocation = true;
				}
			}
			Projectile.alpha = (int)(255 * (1 - 1f * Math.Cos(1f * MathHelper.Clamp(Projectile.ai[0] / 25f, -1, 1) * MathHelper.PiOver2)));
			Projectile.alpha = (int)MathHelper.Clamp(Projectile.alpha, 0, 255);
			#endregion

			#region Animation and visuals
			Projectile.rotation = Projectile.velocity.X * 0.05f;
			Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 0.78f);
			#endregion
		}
        public override void PostAI()
		{
			Vector2 current = Projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				trailPos[i] = current;
				current = previousPosition;
			}
		}
    }
}