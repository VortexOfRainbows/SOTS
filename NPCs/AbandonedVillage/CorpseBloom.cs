using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using SOTS.Common.GlobalNPCs;
using System;
using SOTS.Items.Banners;
using SOTS.Projectiles.AbandonedVillage;

namespace SOTS.NPCs.AbandonedVillage
{
	public class CorpseBloom : ModNPC
	{
		public bool Closed => NPC.frame.Y <= 330; //This should include all of the closed frames
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 19;

			NPCID.Sets.NPCBestiaryDrawOffset[NPC.type] = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				Frame = 6
            };

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
		}
		public override void SetDefaults()
		{
			NPC.lifeMax = 50;
            NPC.damage = 35;
            NPC.defense = 16;
			NPC.width = 46;
			NPC.height = 66;
            NPC.npcSlots = 1f;
			NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 0, 1, 0);
            NPC.noGravity = false;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.aiStyle = -1; 
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<CorpsebloomBanner>();
        }
        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
			if(Closed)
            {
                modifiers.SourceDamage *= 0.5f;
                modifiers.FinalDamage -= 2;
            }
        }
        public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.5f;
			double bonusFrameCounter = Math.Sin(Math.Min(NPC.frame.Y / (float)frameHeight / 18f * MathHelper.Pi, MathHelper.Pi)); //Make the animation play faster on the middle frames and slower on outer frames to give an easing effect (not sure how noticeable this is)
			//Main.NewText(bonusFrameCounter);
			NPC.frameCounter += bonusFrameCounter * 0.5f;
            if (NPC.ai[0] == 0)
			{
				if (NPC.frameCounter > 6)
				{
					NPC.frame.Y = NPC.frame.Y + frameHeight;
					NPC.frameCounter -= 6;
				}
				if (NPC.frame.Y >= frameHeight * 4)
				{
					NPC.frame.Y = 0 * frameHeight;
				}
            }
			else
			{
				//opening/attacking animation
				if (NPC.ai[0] == 1)
				{
					if (NPC.frame.Y < frameHeight * 5)
					{
						NPC.frame.Y = 4 * frameHeight;
					}

					if (NPC.frameCounter > 6)
					{
						NPC.frame.Y = NPC.frame.Y + frameHeight;
                        NPC.frameCounter -= 6;
                    }
					if (NPC.frame.Y >= frameHeight * 12)
					{
						NPC.frame.Y = 10 * frameHeight;
					}
				}
				//closing up animation
				else
				{
					if (NPC.frameCounter > 6)
					{
						NPC.frame.Y = NPC.frame.Y + frameHeight;
                        NPC.frameCounter -= 6;
                    }
					if (NPC.frame.Y >= frameHeight * 19)
					{
						NPC.frame.Y = 0 * frameHeight;
					}
				}
			}
		}
        public override void AI()
        {
            //NPC is not awakened
            if (NPC.ai[0] != 1 && Closed)
			{
                NPC.TargetClosest(true);
                NPC.spriteDirection = NPC.direction;
                Vector2 target = new Vector2(NPC.ai[2], NPC.ai[3]);
                if (target.X <= 0 || target.Y <= 0)
                {
                    target = Main.player[NPC.target].Center;
                    NPC.ai[2] = target.X;
                    NPC.ai[3] = target.Y;
                }
            }
            Player player = Main.player[NPC.target];

			//awaken if the player gets close enough
			if (NPC.Distance(player.Center) <= 200f && NPC.ai[0] == 0)
			{
				NPC.ai[0] = 1;
				if (Main.netMode == NetmodeID.Server)
					NPC.netUpdate = true;
			}
			
			//shot out vile spits when activated
			if (NPC.ai[0] == 1)
			{
				NPC.ai[1]++;

				if (NPC.ai[1] > 30 && NPC.ai[1] < 120 && NPC.ai[1] % 10 == 0)
				{
					SOTSUtils.PlaySound(SoundID.NPCDeath9, NPC.Center, 1.2f, -0.1f);

					if(Main.netMode != NetmodeID.MultiplayerClient)
					{
                        Vector2 ShootPosition = new Vector2(NPC.Center.X + NPC.direction * 16, NPC.Center.Y - 5);
						Vector2 target = new Vector2(NPC.ai[2], NPC.ai[3]);
                        Vector2 toPlayer = target - NPC.Center;
                        toPlayer = toPlayer.SafeNormalize(Vector2.Zero);
                        toPlayer.X *= Main.rand.NextFloat(7f, 11f);
                        toPlayer.Y *= Main.rand.NextFloat(-2f, 4f);
						toPlayer.Y -= 1f;
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), ShootPosition, toPlayer, ModContent.ProjectileType<CorpsebloomAcid>(), SOTSNPCs.GetBaseDamage(NPC) / 2, 0, Main.myPlayer);
                    }
				}

				if (NPC.ai[1] > 120)
				{
					NPC.ai[0] = 2;
					NPC.ai[1] = 0;
					NPC.ai[2] = -1;
                    NPC.ai[3] = -1;
                    if (Main.netMode == NetmodeID.Server)
                        NPC.netUpdate = true;
                }
			}
			//NPC is not awakened
			if (NPC.ai[0] == 2)
			{
				if (NPC.frame.Y >= 18 * NPC.height)
				{
					NPC.ai[0] = 0;
                    if (Main.netMode == NetmodeID.Server)
                        NPC.netUpdate = true;
                }
			}
        }
		public override void HitEffect(NPC.HitInfo hit) 
        {
			if (NPC.life <= 0)
			{
				if (Main.netMode != NetmodeID.Server)
				{
					for (int i = 1; i <= 7; i++)
					{
						Vector2 circular = new Vector2(14, 14).RotatedBy(MathHelper.ToRadians(-i * 60));
						circular.X *= NPC.direction;
						if (i == 7)
							circular *= 0;
                        Gore.NewGore(NPC.GetSource_Death(), NPC.Center + circular + new Vector2(10 * NPC.direction, -10) - new Vector2(9, 9), circular * 0.1f + Main.rand.NextVector2Circular(2, 2), ModGores.GoreType("Gores/CorpseBloom/CorpseBloomGore" + i), 1f);
                    }
                    NPC.DeathGore("Gores/CorpseBloom/CorpseBloomGore8", new Vector2(NPC.width / 2, NPC.height * 3 / 4));
                    NPC.DeathGore("Gores/CorpseBloom/CorpseBloomGore9", new Vector2(NPC.width / 2, NPC.height * 3 / 4));
                }
			}
			else
			{

			}
        }
    }
}