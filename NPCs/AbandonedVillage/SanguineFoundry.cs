using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using ReLogic.Content;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

using SOTS.Projectiles.AbandonedVillage;

namespace SOTS.NPCs.AbandonedVillage
{
    public class SanguineFoundry : ModNPC  
    {
		bool HasJumped = false;

		private static Asset<Texture2D> GlowTexture;

		public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 11;
        }

        public override void SetDefaults()
		{
            NPC.lifeMax = 55;
            NPC.damage = 24;
            NPC.defense = 5;
            NPC.width = 54;
			NPC.height = 74;
            NPC.npcSlots = 1f;
			NPC.knockBackResist = 0.25f;
            NPC.value = Item.buyPrice(0, 0, 0, 50);
			NPC.noGravity = false;
			NPC.noTileCollide = false;
			NPC.HitSound = SoundID.Item95 with { Volume = 0.8f, Pitch = 1.3f };
			NPC.DeathSound = SoundID.NPCDeath1;
            NPC.aiStyle = 66;
		}

        public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter++;
			if (NPC.frameCounter > 5)
			{
				NPC.frame.Y = NPC.frame.Y + frameHeight;
				NPC.frameCounter = 0;
			}

			if (NPC.ai[2] <= 400)
			{
				if (NPC.velocity.Y == 0)
				{
					if (NPC.frame.Y >= frameHeight * 4)
					{
						NPC.frame.Y = 0 * frameHeight;
					}
				}
				else
				{
					if (NPC.velocity.Y < 0)
					{
						if (NPC.frame.Y >= frameHeight * 2)
						{
							NPC.frame.Y = 1 * frameHeight;
						}
					}
					else
					{
						if (NPC.frame.Y >= frameHeight * 4)
						{
							NPC.frame.Y = 3 * frameHeight;
						}
					}
				}
			}
			else
			{
				if (NPC.frame.Y < frameHeight * 5)
				{
					NPC.frame.Y = 4 * frameHeight;
				}
				
				if (NPC.frame.Y >= frameHeight * 11)
				{
					NPC.frame.Y = 4 * frameHeight;
				}
			}
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            GlowTexture ??= ModContent.Request<Texture2D>("SOTS/NPCs/AbandonedVillage/SanguineFoundryGlow");

            Main.EntitySpriteDraw(GlowTexture.Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY + 4), 
            NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, SpriteEffects.None, 0);
        }

        public override void AI()
		{
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];

			if (NPC.velocity.Y == 0)
			{
				NPC.ai[2]++;
			}

			if (NPC.ai[2] <= 400)
			{
				JumpToTarget(player, 120, Main.rand.Next(0, 61));
			}
			else
			{
				if (NPC.ai[2] == 430)
				{
					for (int numProjs = 0; numProjs <= 3; numProjs++)
					{
						Vector2 RandomVelocity = new Vector2(Main.rand.NextFloat(-5f, 6f), Main.rand.NextFloat(-8f, -3f));
						Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y - 35), RandomVelocity, ModContent.ProjectileType<FoundryFire>(), NPC.GetBaseDamage() / 2, 0f, Main.myPlayer);
					}
				}

				if (NPC.ai[2] >= 445)
				{
					NPC.ai[2] = 0;
					NPC.netUpdate = true;
				}
			}
        }

		public void JumpToTarget(Player target, int JumpHeight, int TimeBeforeNextJump)
		{
			NPC.ai[0]++;

			//set where the it should be jumping towards
			Vector2 JumpTo = new(target.Center.X, NPC.Center.Y - JumpHeight);

			//set velocity and speed
			Vector2 velocity = JumpTo - NPC.Center;
			velocity.Normalize();

			int JumpSpeed = Main.rand.Next(13, 18);

			float speed = MathHelper.Clamp(velocity.Length() / 36, 10, JumpSpeed);

			NPC.velocity.X *= NPC.velocity.Y <= 0 ? 0.98f : 0.95f;

			//actual jumping
			if (NPC.ai[0] >= TimeBeforeNextJump)
			{
				NPC.ai[1]++;

				if (NPC.velocity == Vector2.Zero)
				{
					if ((NPC.ai[1] == 10 && !HasJumped))
					{
						if (target.Distance(NPC.Center) <= 450f)
						{
							SoundEngine.PlaySound(SoundID.Item95 with { Volume = 0.8f, Pitch = 1.05f }, NPC.Center);
						}

						velocity.Y -= 0.25f;

						HasJumped = true;

						NPC.netUpdate = true;
					}
				}

				if (NPC.ai[1] < 15 && HasJumped)
				{
					NPC.velocity = velocity * speed;
				}
			}

			//loop ai
			if (NPC.ai[0] >= TimeBeforeNextJump + 150)
			{
				HasJumped = false;

				NPC.ai[0] = 0;
				NPC.ai[1] = 0;
			}
		}

		public override void HitEffect(NPC.HitInfo hit) 
        {
            if (NPC.life <= 0) 
            {
                for (int numGores = 1; numGores <= 2; numGores++)
                {
                    if (Main.netMode != NetmodeID.Server) 
                    {
						//spawn gores here
                    }
                }
            }
        }
    }
}