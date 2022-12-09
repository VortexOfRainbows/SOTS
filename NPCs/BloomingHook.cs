using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Nature;
using System;
using System.ComponentModel;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs
{
	public class BloomingHook : ModNPC
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
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blooming Hook");
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Hide = true
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}
		public override void SetDefaults()
		{
			//npc.CloneDefaults(NPCID.BlackSlime);
			NPC.aiStyle =-1;
            NPC.lifeMax = 6;  
            NPC.damage = 10; 
            NPC.defense = 0;  
            NPC.knockBackResist = 0.5f;
            NPC.width = 38;
            NPC.height = 38;
			Main.npcFrameCount[NPC.type] = 14;  
            NPC.value = 0;
            NPC.npcSlots = 0f;
			NPC.noGravity = true;
			NPC.alpha = 75;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
			//Banner = NPC.type;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/BloomingVine");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			NPC owner = Main.npc[(int)ownerID];
			if (owner.type == ModContent.NPCType<NatureSlime>() && owner.active)
			{
				Vector2 distanceToOwner = NPC.Center - owner.Center;
				float radius = distanceToOwner.Length() / 2;
				if (distanceToOwner.X < 0)
				{
					radius = -radius;
				}
				Vector2 centerOfCircle = owner.Center + distanceToOwner / 2;
				float startingRadians = distanceToOwner.ToRotation();
				for(int i = 9; i > 1; i--)
				{
					Vector2 rotationPos = new Vector2(radius, 0).RotatedBy(MathHelper.ToRadians(18 * i));
					rotationPos.Y /= 4f;
					rotationPos = rotationPos.RotatedBy(startingRadians);
					Vector2 pos = rotationPos += centerOfCircle;
					Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(18 * i));
					Vector2 dynamicAddition = new Vector2(0.5f + 2.0f * circular.Y, 0).RotatedBy(MathHelper.ToRadians(i * 36 + aiCounter * 2));
					Vector2 drawPos = pos - screenPos;
					spriteBatch.Draw(texture, drawPos + dynamicAddition, null, NPC.GetAlpha(drawColor), MathHelper.ToRadians(18 * i - 45) + startingRadians, drawOrigin, NPC.scale * 0.9f, SpriteEffects.None, 0f); 
				}
			}
			return true;
		}
		public override bool PreAI()
		{
			NPC.TargetClosest(true);
			return true;
		}
		int frame = 0;
		Vector2 rotateVector = new Vector2(12, 0);
		public override void AI()
		{
			Player player = Main.player[NPC.target];
			NPC owner = Main.npc[(int)ownerID];
			aiCounter++;
			if (owner.type != ModContent.NPCType<NatureSlime>() || !owner.active)
			{
				NPC.active = false;
			}
			Vector2 distanceToOwner = owner.Center - NPC.Center;
			Vector2 distanceToOwner2 = owner.Center - NPC.Center;
			Vector2 distanceToTarget = player.Center - NPC.Center;
			Vector2 distanceToTarget2 = player.Center - NPC.Center;

			distanceToTarget.Normalize();
			rotateVector += distanceToTarget * 1;
			rotateVector = new Vector2(12, 0).RotatedBy(rotateVector.ToRotation());

			if(distanceToOwner.Length() >= 64)
			{
				distanceToOwner.Normalize();
				NPC.velocity = distanceToOwner * (distanceToOwner2.Length() - 64);
			}
			else if (NPC.Center.Y > owner.Center.Y)
			{
				NPC.velocity.Y = -1;
			}
			else
			{
				Vector2 dynamicAddition = new Vector2(0.4f, 0).RotatedBy(MathHelper.ToRadians(aiCounter * 2));
				Vector2 added = new Vector2(1.2f, 0).RotatedBy(NPC.rotation);
				if (distanceToTarget2.Length() > 256f)
				{
					added = Vector2.Zero;
				}
				NPC.velocity = added + dynamicAddition;
			}


			float overlapVelocity = 0.4f;
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


			NPC.rotation = rotateVector.ToRotation() + MathHelper.ToRadians(90) + NPC.velocity.X * 0.075f;
			aiCounter2++;
			if (aiCounter2 >= 210 && (distanceToTarget2.Length() < 196f || frame != 0))
			{
				NPC.frameCounter++;
				if (NPC.frameCounter >= 6)
				{
					frame++;
					if (frame == 7)
					{
						SOTSUtils.PlaySound(SoundID.Item30, (int)NPC.Center.X, (int)NPC.Center.Y, 0.4f, -0.4f);
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							int damage = Common.GlobalNPCs.SOTSNPCs.GetBaseDamage(NPC) / 2;
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, rotateVector * 0.45f, ModContent.ProjectileType<FlowerBolt>(), damage, 0, Main.myPlayer);
							NPC.netUpdate = true;
						}
					}
					if (frame >= 13)
					{
						aiCounter2 = 0;
						frame = 0;
					}
					NPC.frameCounter = 0;
				}
			}
			NPC.frame = new Rectangle(0, NPC.height * frame, NPC.width, NPC.height);
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)NPC.lifeMax * 30.0)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Grass, (float)hitDirection * 0.75f, -1f, NPC.alpha, default, 1f);
					if(Main.rand.NextBool(4))
						Dust.NewDust(NPC.position, NPC.width, NPC.height, 231, (float)hitDirection, -1f, NPC.alpha, default, 1f);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 15; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Grass, (float)(1.5f * hitDirection), -2f, NPC.alpha, default, 1f);
					if (Main.rand.NextBool(2))
						Dust.NewDust(NPC.position, NPC.width, NPC.height, 231, (float)hitDirection, -1f, NPC.alpha, default, 1f);
				}
			}
		}
		public override bool PreKill()
		{
			return false;
		}
	}
}