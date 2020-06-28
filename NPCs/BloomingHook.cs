using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
			get => npc.ai[0];
			set => npc.ai[0] = value;
		}
		private float aiCounter
		{
			get => npc.ai[1];
			set => npc.ai[1] = value;
		}
		private float aiCounter2
		{
			get => npc.ai[2];
			set => npc.ai[2] = value;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blooming Hook");
		}
		public override void SetDefaults()
		{
			//npc.CloneDefaults(NPCID.BlackSlime);
			npc.aiStyle = -1;
            npc.lifeMax = 6;  
            npc.damage = 10; 
            npc.defense = 0;  
            npc.knockBackResist = 0.5f;
            npc.width = 38;
            npc.height = 38;
			Main.npcFrameCount[npc.type] = 14;  
            npc.value = 0;
            npc.npcSlots = 0f;
			npc.noGravity = true;
			npc.alpha = 75;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/BloomingVine");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			NPC owner = Main.npc[(int)ownerID];
			if (owner.type == mod.NPCType("NatureSlime") && owner.active)
			{
				Vector2 distanceToOwner = npc.Center - owner.Center;
				float radius = distanceToOwner.Length() / 2;
				if (distanceToOwner.X < 0)
				{
					radius = -radius;
				}
				Vector2 centerOfCircle = owner.Center + distanceToOwner/2;
				float startingRadians = distanceToOwner.ToRotation();
				for(int i = 9; i > 0; i--)
				{
					Vector2 rotationPos = new Vector2(radius, 0).RotatedBy(MathHelper.ToRadians(18 * i));
					rotationPos.Y /= 4f;
					rotationPos = rotationPos.RotatedBy(startingRadians);
					Vector2 pos = rotationPos += centerOfCircle;
					Vector2 dynamicAddition = new Vector2(2.5f, 0).RotatedBy(MathHelper.ToRadians(i * 36 + aiCounter * 2));
					Vector2 drawPos = pos - Main.screenPosition;
					spriteBatch.Draw(texture, drawPos + dynamicAddition, null, npc.GetAlpha(drawColor), MathHelper.ToRadians(18 * i - 45) + startingRadians, drawOrigin, npc.scale, SpriteEffects.None, 0f); 
				}
			}
			return true;
		}
		public override bool PreAI()
		{
			npc.TargetClosest(true);
			return true;
		}
		int frame = 0;
		Vector2 rotateVector = new Vector2(12, 0);
		public override void AI()
		{
			Player player = Main.player[npc.target];
			NPC owner = Main.npc[(int)ownerID];
			aiCounter++;
			if (owner.type != mod.NPCType("NatureSlime") || !owner.active)
			{
				npc.active = false;
			}
			Vector2 distanceToOwner = owner.Center - npc.Center;
			Vector2 distanceToOwner2 = owner.Center - npc.Center;
			Vector2 distanceToTarget = player.Center - npc.Center;
			Vector2 distanceToTarget2 = player.Center - npc.Center;

			distanceToTarget.Normalize();
			rotateVector += distanceToTarget * 1;
			rotateVector = new Vector2(12, 0).RotatedBy(rotateVector.ToRotation());

			if(distanceToOwner.Length() >= 64)
			{
				distanceToOwner.Normalize();
				npc.velocity = distanceToOwner * (distanceToOwner2.Length() - 64);
			}
			else if (npc.Center.Y > owner.Center.Y)
			{
				npc.velocity.Y = -1;
			}
			else
			{
				Vector2 dynamicAddition = new Vector2(0.4f, 0).RotatedBy(MathHelper.ToRadians(aiCounter * 2));
				Vector2 added = new Vector2(1.2f, 0).RotatedBy(npc.rotation);
				if (distanceToTarget2.Length() > 256f)
				{
					added = Vector2.Zero;
				}
				npc.velocity = added + dynamicAddition;
			}


			float overlapVelocity = 0.4f;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				// Fix overlap with other minions
				NPC other = Main.npc[i];
				if (i != npc.whoAmI && other.active && (int)other.ai[0] == (int)ownerID && Math.Abs(npc.position.X - other.position.X) + Math.Abs(npc.position.Y - other.position.Y) < npc.width && other.type == npc.type)
				{
					if (npc.position.X < other.position.X) npc.velocity.X -= overlapVelocity;
					else npc.velocity.X += overlapVelocity;

					if (npc.position.Y < other.position.Y) npc.velocity.Y -= overlapVelocity;
					else npc.velocity.Y += overlapVelocity;
				}
			}


			npc.rotation = rotateVector.ToRotation();
			aiCounter2++;
			if (aiCounter2 >= 210 && (distanceToTarget2.Length() < 196f || frame != 0))
			{
				npc.frameCounter++;
				if (npc.frameCounter >= 6)
				{
					frame++;
					if (frame == 7)
					{
						if (Main.netMode != 1)
						{
							int damage = npc.damage / 2;
							if (Main.expertMode)
							{
								damage = (int)(damage / Main.expertDamage);
							}
							Projectile.NewProjectile(npc.Center, rotateVector * 0.4f, mod.ProjectileType("FlowerBolt"), damage, 0, Main.myPlayer);
							npc.netUpdate = true;
						}
					}
					if (frame >= 13)
					{
						aiCounter2 = 0;
						frame = 0;
					}
					npc.frameCounter = 0;
				}
			}
			npc.frame = new Rectangle(0, npc.height * frame, npc.width, npc.height);
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)npc.lifeMax * 30.0)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.Grass, (float)hitDirection * 0.75f, -1f, npc.alpha, default, 1f);
					if(Main.rand.NextBool(4))
						Dust.NewDust(npc.position, npc.width, npc.height, 231, (float)hitDirection, -1f, npc.alpha, default, 1f);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 15; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.Grass, (float)(1.5f * hitDirection), -2f, npc.alpha, default, 1f);
					if (Main.rand.NextBool(2))
						Dust.NewDust(npc.position, npc.width, npc.height, 231, (float)hitDirection, -1f, npc.alpha, default, 1f);
				}
			}
		}
		public override bool PreNPCLoot()
		{
			return false;
		}
	}
}