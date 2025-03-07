using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ReLogic.Content;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.AbandonedVillage;
using System.IO;
using SOTS.Dusts;

namespace SOTS.NPCs.AbandonedVillage
{
    public class SanguineFoundry : ModNPC  
    {
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(HasJumped);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			HasJumped = reader.ReadBoolean();
        }
        private  bool HasJumped = false;
		private static Asset<Texture2D> GlowTexture;
		public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 11;
        }
        public override void SetDefaults()
		{
            NPC.lifeMax = 60;
            NPC.damage = 30;
            NPC.defense = 14;
            NPC.width = 48;
			NPC.height = 48;
            NPC.npcSlots = 1f;
			NPC.knockBackResist = 0.25f;
            NPC.value = Item.buyPrice(0, 0, 0, 50);
			NPC.noGravity = false;
			NPC.noTileCollide = false;
			NPC.HitSound = SoundID.Item95 with { Volume = 0.8f, Pitch = 1.3f };
			NPC.DeathSound = SoundID.NPCDeath1;
            NPC.aiStyle = 66;
			NPC.scale = 1.1f;
		}
        public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter++;
			if (NPC.frameCounter > 7)
			{
				NPC.frame.Y = NPC.frame.Y + frameHeight;
				NPC.frameCounter = 0;
			}

			if (NPC.ai[2] <= 300)
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

            Main.EntitySpriteDraw(GlowTexture.Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY - 10), 
            NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }
        public override void AI()
		{
            NPC.TargetClosest(true);
            NPC.spriteDirection = -NPC.direction;
            Player player = Main.player[NPC.target];

			if (NPC.velocity.Y == 0)
			{
				NPC.ai[2]++;
			}

			if (NPC.ai[2] <= 300)
			{
				if (player.Distance(NPC.Center) > 160)
				{
					JumpToTarget(player, 100, Main.rand.Next(0, 61));
				}
				else if (NPC.ai[2] < 300 && NPC.velocity.Y == 0)
                {
                    NPC.ai[2]++;
                }
			}
			else
            {
                if (NPC.ai[2] == 301)
				{
					SOTSUtils.PlaySound(SoundID.Item15, NPC.Center, 1.0f, -0.1f);
				}
				if (NPC.ai[2] == 330)
				{
					if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
						Vector2 toPlayer = player.Center - new Vector2(NPC.Center.X, NPC.position.Y);
                        for (int i = 0; i < 4; i++)
                        {
                            Vector2 RandomVelocity = new Vector2(Main.rand.NextFloat(-i, i) + toPlayer.X * 0.01f, Main.rand.NextFloat(-8f, -6f) + toPlayer.Y * 0.01f);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y - 35), RandomVelocity, ModContent.ProjectileType<FoundryFire>(), NPC.GetBaseDamage() / 2, 0f, Main.myPlayer, i -1);
                        }
                    }
				}

				if (NPC.ai[2] >= 345)
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
			float speed = MathHelper.Clamp(velocity.Length() / 60f + 2f, 6, 12);

			NPC.velocity.X *= NPC.velocity.Y <= 0 ? 0.98f : 0.95f;

			//actual jumping
			if (NPC.ai[0] >= TimeBeforeNextJump)
			{
				NPC.ai[1]++;

				if (NPC.velocity == Vector2.Zero)
				{
					if (NPC.ai[1] == 10 && !HasJumped)
                    {
                        SoundEngine.PlaySound(SoundID.Item95 with { Volume = 0.8f, Pitch = 1.05f }, NPC.Center);
                        velocity.Y -= 0.25f;
						HasJumped = true;
						NPC.netUpdate = true;

						for(int i = 0; i < 30; i++)
						{
							Dust dust = Dust.NewDustDirect(NPC.position + new Vector2(-5, -5 + NPC.height), NPC.width, 0, Main.rand.NextBool() ? ModContent.DustType<FamishedDustCorruption>() : ModContent.DustType<FamishedDustCrimson>(), 0, 0, 0);
							dust.velocity *= 0.6f;
							dust.velocity -= NPC.velocity * 0.8f * speed;
							dust.scale *= 1.5f;
							dust.noGravity = true;
                        }
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