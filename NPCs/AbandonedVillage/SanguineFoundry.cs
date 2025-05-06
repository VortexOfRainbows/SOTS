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
using SOTS.Items.Banners;
using Terraria.GameContent;
using System;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Fragments;
using Terraria.GameContent.ItemDropRules;

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
        private bool HasJumped = false;
		private static Asset<Texture2D> GlowTexture;
		public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 11;
        }
        public override void SetDefaults()
		{
            NPC.lifeMax = 60;
            NPC.damage = 25;
            NPC.defense = 14;
            NPC.width = 48;
			NPC.height = 48;
            NPC.npcSlots = 1f;
			NPC.knockBackResist = 0.1f;
            NPC.value = Item.buyPrice(0, 0, 1, 0);
			NPC.noGravity = false;
            NPC.noTileCollide = false;
			NPC.HitSound = SoundID.Item95 with { Volume = 0.8f, Pitch = 1.3f };
			NPC.DeathSound = SoundID.NPCDeath1;
            NPC.aiStyle = 0;
			NPC.scale = 1.1f;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<FurnaceBanner>();
        }
        private readonly int framesUntilShoot = 6;
        private readonly int frameSpeed = 7;
        public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter++;
			if (NPC.frameCounter > frameSpeed)
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
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = TextureAssets.Npc[Type].Value;
            if (NPC.ai[2] > 300 && NPC.ai[2] < 300 + frameSpeed * (framesUntilShoot - 1))
			{
				float percent = (NPC.ai[2] - 300) / (frameSpeed * (framesUntilShoot - 1));
                Color color2 = Color.Lerp(Color.Red, Color.Gold, 0.5f);
                color2.A = 0;
                for (int i = 0; i < 8; i++)
                {
                    Vector2 circular = new Vector2(14 - 14 * percent, 0).RotatedBy(i * MathHelper.PiOver4 + MathHelper.PiOver2 * percent);
                    Main.EntitySpriteDraw(texture, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY - 10) + circular, NPC.frame, color2 * 1.2f * percent * (1 - percent), NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                }
            }
			return true;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            GlowTexture ??= ModContent.Request<Texture2D>("SOTS/NPCs/AbandonedVillage/SanguineFoundryGlow");

            Main.EntitySpriteDraw(GlowTexture.Value, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY - 10), 
            NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }
        public override void AI()
		{
            NPC.TargetClosest(true);
            NPC.spriteDirection = -NPC.direction;
            Player player = Main.player[NPC.target];
            NPC.velocity.X *= NPC.velocity.Y <= 0 ? 0.98f : 0.95f;

            if (NPC.velocity.Y == 0)
			{
				NPC.ai[2]++;
			}

			if (NPC.ai[2] <= 300)
			{
				if (player.Distance(NPC.Center) > 160)
				{
					JumpToTarget(player, 115, 30);
				}
				else if (NPC.ai[2] < 300 && NPC.velocity.Y == 0)
                {
                    NPC.ai[2]++;
                }
			}
			else if(Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
            {
                if (NPC.ai[2] == 301)
				{
					SOTSUtils.PlaySound(SoundID.Item15, NPC.Center, 1.0f, -0.1f);
				}
				if (NPC.ai[2] == 300 + (framesUntilShoot - 1) * frameSpeed)
				{
					if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
						Vector2 toPlayer = player.Center - new Vector2(NPC.Center.X, NPC.position.Y);
                        for (int i = 0; i < 4; i++)
                        {
                            Vector2 RandomVelocity = new Vector2(Main.rand.NextFloat(-i, i) + toPlayer.X * 0.015f, Main.rand.NextFloat(-8f, -6f) + toPlayer.Y * 0.01f);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y - 35), RandomVelocity, ModContent.ProjectileType<FoundryFire>(), NPC.GetBaseDamage() / 2, 0f, Main.myPlayer, i - 1);
                        }
                    }
				}

				if (NPC.ai[2] >= 310 + framesUntilShoot * frameSpeed)
                {
					NPC.ai[2] = 0;
					NPC.netUpdate = true;
				}
			}
			else
            {
                NPC.ai[2] -= 100;
                NPC.netUpdate = true;
            }
        }

		public void JumpToTarget(Player target, int JumpHeight, int TimeBeforeNextJump)
		{
			NPC.ai[0]++;

			//set where the it should be jumping towards
			Vector2 JumpTo = new(target.Center.X, NPC.Center.Y * 0.5f + target.position.Y * 0.5f - JumpHeight);

			//set velocity and speed
			Vector2 velocity = JumpTo - NPC.Center;
			velocity.Normalize();
			float speed = MathHelper.Clamp(velocity.Length() / 60f + 5f, 3, 16);


			//actual jumping
			if (NPC.ai[0] >= TimeBeforeNextJump)
			{
				NPC.ai[1]++;

				if (NPC.velocity == Vector2.Zero)
				{
					if (NPC.ai[1] == 10 && !HasJumped)
                    {
                        SoundEngine.PlaySound(SoundID.Item95 with { Volume = 0.8f, Pitch = 1.05f }, NPC.Center);
                        velocity.Y -= 0.45f;
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
                NPC.netUpdate = true;
            }
		}

		public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server)
            {
				return;
			}
			if (NPC.life <= 0)
            {
				for (int i = 0; i < 40; i++)
				{
					Dust dust = Dust.NewDustDirect(NPC.position + new Vector2(-5, -5), NPC.width, NPC.height, Main.rand.NextBool() ? ModContent.DustType<FamishedDustCorruption>() : ModContent.DustType<FamishedDustCrimson>(), hit.HitDirection, -1f, 0);
				    dust.velocity *= 0.9f;
					dust.scale *= 1.2f;
				    dust.noGravity = false;
				}
				Vector2 velo = NPC.oldVelocity + new Vector2(hit.HitDirection, -1);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(10, 0), velo, ModGores.GoreType($"Gores/Foundry/SanguineFoundryGore1"), NPC.scale * 0.95f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 18), velo, ModGores.GoreType($"Gores/Foundry/SanguineFoundryGore2"), NPC.scale * 0.95f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 36), velo, ModGores.GoreType($"Gores/Foundry/SanguineFoundryGore3"), NPC.scale * 0.95f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(24, 16), velo, ModGores.GoreType($"Gores/Foundry/SanguineFoundryGore4"), NPC.scale * 0.95f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(20, 32), velo, ModGores.GoreType($"Gores/Foundry/SanguineFoundryGore5"), NPC.scale * 0.95f);
            }
			else
			{
				int count = 2 + hit.Damage / 2;
                for (int i = 0; i < count; i++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position + new Vector2(-5, -5), NPC.width, NPC.height, Main.rand.NextBool() ? ModContent.DustType<FamishedDustCorruption>() : ModContent.DustType<FamishedDustCrimson>(), hit.HitDirection, -1f, 0);
                    dust.velocity *= 0.9f;
                    dust.scale *= 1.2f;
                    dust.noGravity = false;
                }
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Vertebrae, 2));
            npcLoot.Add(ItemDropRule.Common(ItemID.RottenChunk, 5));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FragmentOfEvil>(), 5));
            npcLoot.Add(ItemDropRule.Common(ItemID.Spaghetti, 20));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldKey>(), 100));
        }
    }
}