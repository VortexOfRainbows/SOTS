using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs
{
	public class FluxSlimeBall : ModNPC
	{
		private float ownerID
		{
			get => NPC.ai[0];
			set => NPC.ai[0] = value;
		}
		private float aiCounter
		{
			get => NPC.ai[1];
			set => NPC.ai[1] = value;
		}
		private float aiCounter2
		{
			get => NPC.ai[2];
			set => NPC.ai[2] = value;
		}
		private float UniqueMultiplier
		{
			get => NPC.ai[3];
			set => NPC.ai[3] = value;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flux Slime Hook");
		}
		public override void SetDefaults()
		{
			//npc.CloneDefaults(NPCID.BlackSlime);
			NPC.aiStyle =-1;
            NPC.lifeMax = 30;  
            NPC.damage = 30; 
            NPC.defense = 8;  
            NPC.knockBackResist = 0.5f;
            NPC.width = 18;
            NPC.height = 18;
			Main.npcFrameCount[NPC.type] = 1;  
            NPC.value = 0;
            NPC.npcSlots = 0f;
			NPC.noGravity = true;
			NPC.alpha = 70;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
			//Banner = NPC.type;
		}
		float counter2 = 0;
		float randMult = 1f;
		bool runOnce = true;
		float[] counterArr = new float[6];
		float[] randSeed1 = new float[6];
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/FluxSlimeVine");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			NPC owner = Main.npc[(int)ownerID];
			Vector2 ownerCenter = new Vector2(owner.Center.X, owner.position.Y + 6);
			Vector2 dynamicScaling = new Vector2(40, 0).RotatedBy(MathHelper.ToRadians(aiCounter * 1.1f));
			float moreScaling = 1.15f - 0.25f * Math.Abs(dynamicScaling.X) / 40f;
			if (owner.type == ModContent.NPCType<FluxSlime>() && owner.active)
			{
				Vector2 p0 = ownerCenter;
				Vector2 p1 = ownerCenter - baseVelo.RotatedBy(MathHelper.ToRadians(180 + dynamicScaling.X)) * 3.5f * moreScaling;
				Vector2 p2 = NPC.Center - baseVelo * 8f * moreScaling;
				Vector2 p3 = NPC.Center;
				int segments = 36;
				for (int i = 0; i < segments; i++)
				{
					float t = i / (float)segments;
					Vector2 drawPos2 = SOTS.CalculateBezierPoint(t, p0, p1, p2, p3);
					t = (i + 1) / (float)segments;
					Vector2 drawPosNext = SOTS.CalculateBezierPoint(t, p0, p1, p2, p3);
					float rotation = (drawPos2 - drawPosNext).ToRotation();
					drawColor = Lighting.GetColor((int)drawPos2.X / 16, (int)(drawPos2.Y / 16));
					spriteBatch.Draw(texture, drawPos2 - Main.screenPosition, null, NPC.GetAlpha(drawColor), rotation - MathHelper.ToRadians(90), drawOrigin, NPC.scale, SpriteEffects.None, 0f);
				}
			}
			Vector2 drawPos = NPC.Center - Main.screenPosition;
			if (!runOnce)
			{
				for (int i = 0; i < 6; i++)
				{
					counterArr[i] += randSeed1[i];
					texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/PinkyGrappleSpike");
					Vector2 circular = new Vector2(0, (NPC.width / 2 - 1.5f) * NPC.scale).RotatedBy(MathHelper.ToRadians(i * 60 + counter2 * 0.3f * randMult) + NPC.rotation);
					int frame = 0;
					if (counterArr[i] >= 20)
					{
						frame = 1;
					}
					if (counterArr[i] >= 30)
					{
						frame = 2;
					}
					if (counterArr[i] >= 40)
					{
						frame = 3;
					}
					if (counterArr[i] >= 50)
					{
						frame = 0;
						counterArr[i] = 0;
						randSeed1[i] = Main.rand.NextFloat(0.8f, 1.2f);
					}
					Rectangle FrameSize = new Rectangle(0, texture.Height / 4 * frame, texture.Width, texture.Height / 4);
					spriteBatch.Draw(texture, drawPos + circular, FrameSize, NPC.GetAlpha(drawColor), circular.ToRotation() - MathHelper.ToRadians(90), new Vector2(texture.Width / 2, 3.5f), NPC.scale * 0.8f, SpriteEffects.None, 0f);
				}
			}
			texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			spriteBatch.Draw(texture, drawPos, null, NPC.GetAlpha(drawColor), NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreAI()
		{
			if (runOnce)
			{
				randMult = Main.rand.NextFloat(0.8f, 1.2f) * (Main.rand.Next(2) * 2 - 1);
				if (Main.netMode != NetmodeID.MultiplayerClient)
					NPC.netUpdate = true;
				for (int i = 0; i < counterArr.Length; i++)
				{
					counterArr[i] = 0;
					randSeed1[i] = Main.rand.NextFloat(0.8f, 1.2f);
				}
				runOnce = false;
			}
			NPC.TargetClosest(true);
			return true;
		}
		Vector2 baseVelo = Vector2.Zero;
		Vector2 rotateVector = new Vector2(12, 0);
		Vector2 ownerCenter = Vector2.Zero;
		public override void AI()
		{
			Player player = Main.player[NPC.target];
			NPC owner = Main.npc[(int)ownerID];
			ownerCenter = NPC.Center;
			if (owner.type != ModContent.NPCType<FluxSlime>() || !owner.active)
			{
				NPC.life = 0;
				HitEffect(NPC.direction, NPC.life);
				NPC.active = false;
			}
			ownerCenter = new Vector2(owner.Center.X, owner.position.Y + 6);
			Vector2 distanceToOwner = owner.Center - NPC.Center;
			Vector2 distanceToTarget = player.Center - NPC.Center;
			Vector2 distanceToTarget2 = player.Center - NPC.Center;

			Vector2 dynamicAddition = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(aiCounter));
			distanceToTarget.Normalize();
			rotateVector += distanceToTarget * 1;
			rotateVector = new Vector2(1, 0).RotatedBy(rotateVector.ToRotation());
			NPC.velocity *= 0.98f;
			NPC.velocity.X += distanceToOwner.X * 0.0002f * UniqueMultiplier;
			NPC.velocity.Y += distanceToOwner.Y * 0.0003f * UniqueMultiplier;
			NPC.velocity.Y += -0.03f;
			NPC.velocity += dynamicAddition * 0.01f / UniqueMultiplier;
			NPC.velocity += rotateVector * 0.03f;
			if(aiCounter2 < 0)
			{
				NPC.velocity += rotateVector * 0.03f * (90 - aiCounter2) / 90f;
			}
			baseVelo *= 0.935f;
			baseVelo += NPC.velocity.SafeNormalize(Vector2.Zero) * (float)Math.Sqrt(NPC.velocity.Length());

			float overlapVelocity = 0.5f;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				// Fix overlap with other minions
				NPC other = Main.npc[i];
				if (i != NPC.whoAmI && other.active && (int)other.ai[0] == (int)ownerID && Math.Abs(NPC.position.X - other.position.X) + Math.Abs(NPC.position.Y - other.position.Y) < NPC.width && other.type == NPC.type)
				{
					if (NPC.position.X < other.position.X) NPC.velocity.X -= overlapVelocity;
					else NPC.velocity.X += overlapVelocity;

					if (NPC.position.Y < other.position.Y) NPC.velocity.Y -= overlapVelocity;
					else NPC.velocity.Y += overlapVelocity;
				}
			}


			NPC.rotation = rotateVector.ToRotation();
			aiCounter += 1 * NPC.direction;

			aiCounter2++;
			if (aiCounter2 >= 210 && distanceToTarget2.Length() < 480f)
			{
				NPC.velocity += rotateVector * 7.8f;
				NPC.netUpdate = true;
				aiCounter2 = Main.rand.Next(-90, 30);
				//SoundEngine.PlaySound(2, (int)npc.Center.X, (int)npc.Center.Y, 96, 0.6f);
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)NPC.lifeMax * 30.0)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.PinkSlime, (float)hitDirection * 0.75f, -1f, NPC.alpha, default, 1f);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 15; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.PinkSlime, (float)(1.5f * hitDirection), -2f, NPC.alpha, default, 1f);
				}
				NPC owner = Main.npc[(int)ownerID];
				Vector2 dynamicScaling = new Vector2(40, 0).RotatedBy(MathHelper.ToRadians(aiCounter * 1.1f));
				float moreScaling = 1.15f - 0.25f * Math.Abs(dynamicScaling.X) / 40f;
				Vector2 p0 = ownerCenter;
				Vector2 p1 = ownerCenter - baseVelo.RotatedBy(MathHelper.ToRadians(180 + dynamicScaling.X)) * 3.5f * moreScaling;
				Vector2 p2 = NPC.Center - baseVelo * 8f * moreScaling;
				Vector2 p3 = NPC.Center;
				int segments = 36;
				for (int i = 0; i < segments; i++)
				{
					float t = i / (float)segments;
					Vector2 drawPos2 = SOTS.CalculateBezierPoint(t, p0, p1, p2, p3);

					for (int k = 0; k < Main.rand.Next(1, 3); k++)
					{
						Dust.NewDust(drawPos2 - new Vector2(5), 0, 0, DustID.PinkSlime, (float)(1.5f * hitDirection), -2f, NPC.alpha, default, 1f);
					}
				}
			}
		}
		public override bool PreNPCLoot()
		{
			return false;
		}
	}
}