using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;
using SOTS.Void;
using SOTS.Buffs;
using SOTS.Buffs.MinionBuffs;
using SOTS.Dusts;
using SOTS.NPCs.Constructs;
using System.Collections.Generic;

namespace SOTS.Projectiles.Minions
{
	public class EvilSpirit : SpiritMinion
	{
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(orbitalCounter);
            base.SendExtraAI(writer); //required
		}
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			orbitalCounter = reader.ReadSingle();
			base.ReceiveExtraAI(reader); //required
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Evil Spirit");
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
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
		List<EvilEye> eyes = new List<EvilEye>();
		float orbitalCounter = 2;
		float range = 36;
		public void UpdateEyes(bool draw = false, int ring = -2)
		{
			for (int i = 0; i < eyes.Count; i++)
			{
				float rangeMult = range / 36f;
				EvilEye eye = eyes[i];
				int direction = -1;
				float rotation = projectile.rotation + MathHelper.ToRadians(orbitalCounter * direction);
				if (draw)
				{
					eye.Draw(projectile.Center, rotation, rangeMult);
				}
				else
				{
					if (i == ring)
					{
						eye.Fire(projectile.Center);
					}
					else
						eye.Update(projectile.Center, rotation, rangeMult);
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			Color color2 = Color.Black;
			for (int i = 0; i < 2; i++)
			{
				for (int k = 0; k < projectile.oldPos.Length; k++)
				{
					Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
					Color color = projectile.GetAlpha(color2) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
					spriteBatch.Draw(texture, drawPos, null, color * 0.5f, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
				}
				color2 = VoidPlayer.EvilColor * 0.5f;
			}
			return false;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color = VoidPlayer.EvilColor * 1.0f;
			if (Main.netMode != NetmodeID.Server) //pretty sure drawcode doesn't run in multiplayer anyways but may as well
				UpdateEyes(true, -2);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.NextFloat(-2.5f, 2.5f);
				float y = Main.rand.NextFloat(-2.5f, 2.5f);
				Main.spriteBatch.Draw(texture, new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, projectile.GetAlpha(color), 0f, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
		}
        public override bool? CanCutTiles()
		{
			return false;
		}
		public override bool MinionContactDamage()
		{
			return true;
		}
		public void dustSound()
		{
			if (Main.myPlayer == projectile.owner)
			{
				Projectile.NewProjectile(projectile.Center, new Vector2(8f, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))), ModContent.ProjectileType<Tide.RippleWaveSummon>(), projectile.damage, 0f, projectile.owner, 1, 0);
			}
			Main.PlaySound(SoundID.Item, (int)(projectile.Center.X), (int)(projectile.Center.Y), 62, 0.7f, -0.2f);
			for (int i = 0; i < 360; i += 10)
			{
				Vector2 circularLocation = new Vector2(Main.rand.NextFloat(4.5f, 6f), 0).RotatedBy(MathHelper.ToRadians(i));
				Dust dust = Dust.NewDustDirect(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
				dust.noGravity = true;
				dust.scale = dust.scale * 0.5f + 2.25f;
				dust.fadeIn = 0.1f;
				dust.color = VoidPlayer.EvilColor * 1.5f;
				dust.velocity *= 0.8f;
				dust.velocity += circularLocation * 1.0f;

				if(Main.rand.NextBool(2))
				{
					circularLocation = new Vector2(Main.rand.NextFloat(3f, 6f), 0).RotatedBy(MathHelper.ToRadians(i));
					dust = Dust.NewDustDirect(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, Main.rand.NextBool(2) ? 62 : 60); //purple and red torch respectively
					dust.noGravity = true;
					dust.scale = dust.scale * 0.5f + 1.55f;
					dust.velocity *= 1.1f;
					dust.velocity += circularLocation * 1.1f;
				}
			}
		}
		bool runOnce = true;
		bool readyToFight = false;
        public override bool PreAI()
		{
			if(runOnce)
			{
				for (int i = 0; i < 8; i++)
				{
					Vector2 circular = new Vector2(range, 0).RotatedBy(MathHelper.ToRadians(i * 45f));
					eyes.Add(new EvilEye(circular, projectile.damage, true));
				}
				runOnce = false;
			}
			UpdateEyes(false, -2);
			return true;
		}
        public override void AI()
		{
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			float rotateAmt = 1;

			#region Active check
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<EvilSpiritAid>());
			}
			if (player.HasBuff(ModContent.BuffType<EvilSpiritAid>()))
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
			float distanceFromTarget = 960f;
			Vector2 targetCenter = projectile.Center;
			bool foundTarget = false;

			// This code is required if your minion weapon has the targeting feature
			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				//float between = Vector2.Distance(npc.Center, projectile.Center);
				float between2 = Vector2.Distance(npc.Center, player.Center);
				if (between2 < distanceFromTarget)
				{
					distanceFromTarget = between2;
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

						bool closeThroughWall = between < 240f; //should attack semi-reliably through walls
						if (inRange && (lineOfSight || closeThroughWall) && between2 < distanceFromTarget)
						{
							distanceFromTarget = between2;
							targetCenter = npc.Center;
							foundTarget = true;
						}
					}
				}
			}
			#endregion

			#region Movement
			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 96f;
			float speed = 4f;
			if (foundTarget)
			{
				if (projectile.alpha >= 255)
				{
					projectile.Center = targetCenter;
				}
				else if (projectile.alpha > 0)
				{
					speed += 16f * (projectile.alpha / 255f);
				}

				Vector2 direction = targetCenter - projectile.Center;
				float distance = direction.Length();
				direction = direction.SafeNormalize(Vector2.Zero);
				if (distance > speed)
				{
					distance = speed;
				}
				direction *= distance;
				projectile.velocity = direction;
				projectile.alpha += 8;
				if (projectile.alpha > 255)
				{
					projectile.alpha = 255;
					if (readyToFight)
						projectile.ai[0]++;
					if (projectile.ai[0] > 240)
					{
						projectile.ai[0] = 0;
						projectile.alpha = 0;
						dustSound();
						readyToFight = false;
					}
					else
					{
						if(projectile.ai[0] >= 60 && projectile.ai[0] < 180)
						{
							int bonus = (int)projectile.ai[0];
							if (bonus > 96)
								bonus = 96;
							range = 32 + bonus;
							int remaining = (int)projectile.ai[0] - 50;
							int id = remaining / 3;
							if(remaining % 3 == 0)
                            {
								UpdateEyes(false, id % 8);
                            }
                        }
						float rangeMult = projectile.ai[0] / 50f;
						if (rangeMult > 1)
							rangeMult = 1;
						float baseR = 32;
						if(projectile.ai[0] > 200)
                        {
							rangeMult = 1 - (projectile.ai[0] - 200) / 40f;
							baseR *= rangeMult;
						}
						float sinusoid = (float)Math.Sin(MathHelper.ToRadians(460 * projectile.ai[0] / 240f));
						range = baseR + 128 * rangeMult + 32 * sinusoid;
						float mult = projectile.ai[0] / 60f;
						if (mult > 1)
							mult = 1;
						rotateAmt += mult * 2f * rangeMult; //speed is at 3 degrees / frame, 
						readyToFight = true;
					}
				}
			}
			else
			{
				GoIdle();
				readyToFight = false;
				if(projectile.ai[0] > 0)
				{
					projectile.ai[0] -= 6f;
				}	
				else
					projectile.ai[0] = 0;
				if (range > 48)
					range -= 2;
				else if (range > 46)
					range = 48;
				projectile.alpha -= 12;
				if (projectile.alpha < 0)
				{
					projectile.alpha = 0;
				}
				Vector2 vectorToIdlePosition = idlePosition - projectile.Center;
				float distanceToIdlePosition = vectorToIdlePosition.Length();
				if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 1400f)
				{
					projectile.ai[0] = 0;
					projectile.alpha = 0;
					projectile.position = idlePosition;
					projectile.velocity *= 0.1f;
					projectile.netUpdate = true;
				}
			}
			#endregion

			Lighting.AddLight(projectile.Center, 2.4f * 0.5f * ((255 - projectile.alpha) / 255f), 2.2f * 0.5f * ((255 - projectile.alpha) / 255f), 1.4f * 0.5f * ((255 - projectile.alpha) / 255f));
			MoveAwayFromOthers();
			if (Main.myPlayer == player.whoAmI)
			{
				projectile.netUpdate = true;
			}
			orbitalCounter += rotateAmt;
		}
	}
}