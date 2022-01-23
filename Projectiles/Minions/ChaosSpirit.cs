using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using System.IO;
using SOTS.Void;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Minions
{
	public class ChaosSpirit : SpiritMinion
	{
        public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(targetID);
			writer.Write(targetType);
			base.SendExtraAI(writer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
		{
			targetID = reader.ReadInt32();
			targetType = reader.ReadInt32();
			base.ReceiveExtraAI(reader);
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Spirit");
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 12;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public sealed override void SetDefaults()
		{
			projectile.width = 34;
			projectile.height = 34;
			projectile.tileCollide = false;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.ignoreWater = true;
			projectile.localNPCHitCooldown = 10;
		}
		public override bool? CanCutTiles()
		{
			return false;
		}
		public override bool MinionContactDamage()
		{
			return true;
		}
		float[] rotations = new float[2] { 1.56f, 0 };
		float[] compressions = new float[2] { 0.5f, 0.5f };
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player = Main.player[projectile.owner];
			Texture2D texture = mod.GetTexture("Projectiles/Minions/ChaosSpiritRing");
			float furtherCompression = 1f - postChargeCounter / 270f;
			for (int j = 0; j < 2; j++)
				for (int i = 180; i < 360; i += 6)
				{
					Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i + 180 * j));
					Vector2 addition = new Vector2(0, postChargeCounter * (0.5f + 0.5f * j)).RotatedBy(projectile.rotation);
					Vector2 center = projectile.Center + addition;
					Vector2 rotation = new Vector2(26 * (j == 0 ? 1 : furtherCompression), 0).RotatedBy(MathHelper.ToRadians(i + Main.GameUpdateCount));
					rotation.Y *= compressions[j];
					rotation = rotation.RotatedBy(rotations[j] + projectile.rotation);
					Main.spriteBatch.Draw(texture, center - Main.screenPosition + rotation, null, new Color(color.R, color.G, color.B, 0) * (j == 0 ? 1 : furtherCompression) * 0.6f, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 0.75f, SpriteEffects.None, 0f);
				}
			base.PreDraw(spriteBatch, lightColor);
			for (int j = 0; j < 2; j++)
				for (int i = 0; i < 180; i += 6)
				{
					Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i + 180 * j));
					Vector2 addition = new Vector2(0, postChargeCounter * (0.5f + 0.5f * j)).RotatedBy(projectile.rotation);
					Vector2 center = projectile.Center + addition;
					Vector2 rotation = new Vector2(26 * (j == 0 ? 1 : furtherCompression), 0).RotatedBy(MathHelper.ToRadians(i + Main.GameUpdateCount));
					rotation.Y *= compressions[j];
					rotation = rotation.RotatedBy(rotations[j] + projectile.rotation);
					Main.spriteBatch.Draw(texture, center - Main.screenPosition + rotation, null, new Color(color.R, color.G, color.B, 0) * (j == 0 ? 1 : furtherCompression) * 0.6f, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 0.75f, SpriteEffects.None, 0f);
				}

			texture = mod.GetTexture("Projectiles/Minions/ChaosSpiritWing");
			float bonusSpread = .205f * postChargeCounter;
			for (int j = 0; j < 2; j++)
				for (int i = 0; i < 3; i++)
				{
					Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
					Vector2 toPosition = new Vector2((44 - i * 4) * (j * 2 - 1), -20).RotatedBy(MathHelper.ToRadians((i * (21.5f + bonusSpread - i) - wingHeight) * (j * 2 - 1)));
					toPosition = toPosition.RotatedBy(projectile.rotation);
					for (int k = 0; k < 6; k++)
					{
						float scale = 1.0f - 0.25f * i;
						Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 60));
						Vector2 modi = new Vector2(2f * scale, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount));
						Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + toPosition + modi, null, new Color(color.R, color.G, color.B, 0), toPosition.ToRotation(), origin, scale, SpriteEffects.None, 0f);
					}
				}
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Player player = Main.player[projectile.owner];
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < 6; k++)
			{
				Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 60));
				Vector2 modi = new Vector2(2f, 0).RotatedBy(MathHelper.ToRadians(k * 60 + SOTSPlayer.ModPlayer(player).orbitalCounter));
				Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + modi, null, new Color(color.R, color.G, color.B, 0), 0f, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
		}
		float postChargeCounter = 0;
		int charging = 0;
		float wingHeight = 0;
		float counter = 0;
		float[] nextRotations = new float[2];
		float[] nextCompressions = new float[2];
		float[] prevRotations = new float[2];
		float[] prevCompressions = new float[2];
		public void RingStuff()
		{
			if (counter == 0 || charging == -1)
			{
				counter = 0;
				charging = 0;
				for (int i = 0; i < 2; i++)
				{
					nextRotations[i] = Main.rand.NextFloat(-1 * (float)Math.PI, (float)Math.PI);
					nextCompressions[i] = Main.rand.NextFloat(0, 1);
					prevRotations[i] = rotations[i];
					prevCompressions[i] = compressions[i];
				}
			}
			if (charging == 1)
			{
				for (int i = 0; i < 2; i++)
				{
					prevRotations[i] = rotations[i];
					prevCompressions[i] = compressions[i];
					nextCompressions[i] = 0.5f;
					nextRotations[i] = 0;
				}
				charging = 2;
				counter = 0;
			}
			if (counter < 180)
				counter += 5;
			float scale = 0.5f + new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(counter)).X;
			if (counter >= 180)
            {
				if(charging != 2)
					counter = 0;
				else
                {
					counter = 180;
                }
			}
			for (int i = 0; i < 2; i++)
			{
				rotations[i] = lerpMath(prevRotations[i], nextRotations[i], scale);
				compressions[i] = lerpMath(prevCompressions[i], nextCompressions[i], scale);
			}
		}
		private float lerpMath(float point, float point2, float scale)
        {
			return point * scale + point2 * (1f - scale);
        }
		float counter2 = 0;
		float lastWingHeight = 0;
		int targetID = -1;
		int targetType = -1;
		public void WingStuff()
        {
			counter2 += 5;
			float dipAndRise = new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(counter2)).X;
			//dipAndRise *= (float)Math.sqrt(dipAndRise);
			wingHeight = dipAndRise * 30;
			lastWingHeight = wingHeight;
		}
        public override void AI() 
		{
			RingStuff();
			if(charging == 0 || charging == -1)
			{
				WingStuff();
			}
			else
            {
				counter2 = 0;
				wingHeight = lerpMath(lastWingHeight, 15f, 1 - postChargeCounter / 90f);
            }
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			#region Active check
			if (player.dead || !player.active) 
			{
				player.ClearBuff(mod.BuffType("ChaosSpiritAid"));
			}
			if (player.HasBuff(mod.BuffType("ChaosSpiritAid")))
			{
				projectile.timeLeft = 6;
			}
			#endregion

			#region General behavior
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
			if (Main.myPlayer == player.whoAmI)
				projectile.ai[1] = ofTotal;
			#endregion

			#region Find target
			float distanceFromTarget = 1200f;
			bool foundTarget = false;

			// This code is required if your minion weapon has the targeting feature
			if(targetID == -1 && targetType == -1 && postChargeCounter <= 0)
			{
				if (player.HasMinionAttackTargetNPC)
				{
					NPC npc = Main.npc[player.MinionAttackTargetNPC];
					float between = Vector2.Distance(npc.Center, projectile.Center);
					if (between < distanceFromTarget)
					{
						distanceFromTarget = between;
						targetID = npc.whoAmI;
						targetType = npc.type;
						foundTarget = true;
						projectile.netUpdate = true;
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
							bool inRange = between < distanceFromTarget;
							bool lineOfSight = Collision.CanHitLine(player.position, player.width, player.height, npc.position, npc.width, npc.height);
							bool closeThroughWall = between < 240f; //should attack semi-reliably through walls
							if (inRange && (lineOfSight || closeThroughWall) && between < distanceFromTarget)
							{
								distanceFromTarget = between;
								targetID = npc.whoAmI;
								targetType = npc.type;
								projectile.netUpdate = true;
							}
						}
					}
				}
			}
			#endregion

			#region Movement
			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 96f;
			if (targetID != -1 && targetType != -1)
			{
				float speed = -13.5f;
				NPC npc = Main.npc[targetID];
				Vector2 npcCenter = npc.Center;
				if(!(!npc.friendly && npc.dontTakeDamage == false && npc.active && npc.CanBeChasedBy()) || Vector2.Distance(npc.Center, player.Center) > 3200)
				{
					targetID = -1;
					targetType = -1;
					return;
                }
				Vector2 direction = npcCenter - projectile.Center;
				float distance = direction.Length();
				direction = direction.SafeNormalize(Vector2.Zero);
				int intDirection = direction.X > 0 ? 1 : -1;
				Vector2 rotateBy = new Vector2(0, 18 * intDirection).RotatedBy(projectile.rotation);
				rotateBy += direction.SafeNormalize(Vector2.Zero) * intDirection;
				projectile.rotation = rotateBy.ToRotation() - MathHelper.ToRadians(90) * intDirection;
				if(charging == 0)
					charging = 1;
				if (postChargeCounter < 90)
					postChargeCounter++;
				if(postChargeCounter % 30 == 29)
                {
					Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 15, 1.3f);
				}

				direction *= (float)Math.Pow(distance, 1.35) * 0.005f + speed;
				projectile.velocity += direction;
				projectile.velocity *= 0.5f;

				projectile.ai[0]++;
				if (projectile.ai[0] > 120)
				{
					projectile.ai[0] -= 5;
					if (Main.myPlayer == projectile.owner)
					{
						Projectile.NewProjectileDirect(projectile.Center + new Vector2(0, 1).RotatedBy(projectile.rotation) * 45, new Vector2(0, 1).RotatedBy(projectile.rotation) * 3, ModContent.ProjectileType<ChaosBeam>(), projectile.damage, projectile.knockBack, Main.myPlayer, targetID, 0);
					}
					projectile.velocity -= new Vector2(0, 1).RotatedBy(projectile.rotation) * 3.25f;
				}
			}
			else
			{
				projectile.ai[0] = 0;
				if (postChargeCounter > 0)
				{
					GoIdle(4f);
					postChargeCounter--;
				}
				else
				{
					GoIdle();
				}
				if (charging != 0 && charging != -1)
					charging = -1;
				float scale = new Vector2(1f, 0).RotatedBy(MathHelper.ToRadians(counter2 * 0.33f)).X;
				Vector2 direction = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(7 * scale));
				Vector2 rotateBy = new Vector2(18, 0).RotatedBy(projectile.rotation);
				rotateBy += direction.SafeNormalize(Vector2.Zero);
				projectile.rotation = rotateBy.ToRotation();
				Vector2 vectorToIdlePosition = idlePosition - projectile.Center;
				float distanceToIdlePosition = vectorToIdlePosition.Length();
				if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 1400f)
				{
					projectile.position = idlePosition;
					projectile.velocity *= 0.1f;
					projectile.netUpdate = true;
				}
			}
			#endregion

			Lighting.AddLight(projectile.Center, 2.0f * 0.5f * ((255 - projectile.alpha) / 255f), 1.4f * 0.5f * ((255 - projectile.alpha) / 255f), 2.0f * 0.5f * ((255 - projectile.alpha) / 255f));
			MoveAwayFromOthers(true, 0.125f, 3.5f);

			if (Main.myPlayer == player.whoAmI)
			{
				projectile.netUpdate = true;
			}
		}
	}
}