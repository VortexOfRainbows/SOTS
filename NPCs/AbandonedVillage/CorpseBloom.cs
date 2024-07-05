using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace SOTS.NPCs.AbandonedVillage
{
	public class CorpseBloom : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 14;

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
            NPC.defense = 15;
			NPC.width = 46;
			NPC.height = 66;
            NPC.npcSlots = 1f;
			NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 0, 1, 0);
            NPC.noGravity = false;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.aiStyle = 0;
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.ai[0] == 0)
			{
				//idle frame
				NPC.frame.Y = 0 * frameHeight;
			}
			else
			{
				NPC.frameCounter++;

				//opening/attacking animation
				if (NPC.ai[0] == 1)
				{
					if (NPC.frameCounter > 8)
					{
						NPC.frame.Y = NPC.frame.Y + frameHeight;
						NPC.frameCounter = 0;
					}
					if (NPC.frame.Y >= frameHeight * 8)
					{
						NPC.frame.Y = 7 * frameHeight;
					}
				}
				//closing up animation
				else
				{
					if (NPC.frameCounter > 8)
					{
						NPC.frame.Y = NPC.frame.Y + frameHeight;
						NPC.frameCounter = 0;
					}
					if (NPC.frame.Y >= frameHeight * 14)
					{
						NPC.frame.Y = 0 * frameHeight;
					}
				}
			}
		}

        public override void AI()
		{
			NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];

            NPC.spriteDirection = NPC.direction;

			//awaken if the player gets close enough
			if (NPC.Distance(player.Center) <= 200f && NPC.ai[0] == 0)
			{
				NPC.ai[0] = 1;
			}
			
			//shot out vile spits when activated
			if (NPC.ai[0] == 1)
			{
				NPC.ai[1]++;

				if (NPC.ai[1] > 60 && NPC.ai[1] < 120 && NPC.ai[1] % 10 == 0)
				{
					SoundEngine.PlaySound(SoundID.NPCDeath9, NPC.Center);

					Vector2 ShootPosition = new Vector2(NPC.Center.X + (NPC.direction == -1 ? -10 : 10), NPC.Center.Y - 6);

					Vector2 ShootSpeed = player.Center - NPC.Center;
                    ShootSpeed.Normalize();
                    ShootSpeed.X *= Main.rand.NextFloat(5f, 8f);
                    ShootSpeed.Y *= Main.rand.NextFloat(-2f, 3f);

                    Projectile.NewProjectile(NPC.GetSource_FromThis(), ShootPosition, ShootSpeed, ModContent.ProjectileType<CorpsebloomAcid>(), NPC.damage / 4, 0, NPC.target);
				}

				if (NPC.ai[1] > 120)
				{
					NPC.ai[0] = 2;
					NPC.ai[1] = 0;
				}
			}

			if (NPC.ai[0] == 2)
			{
				if (NPC.frame.Y >= 13 * NPC.height)
				{
					NPC.ai[0] = 0;
				}
			}
        }

		/*
		//spawn gores and whatnot
		public override void HitEffect(NPC.HitInfo hit) 
        {
			if (NPC.life <= 0) 
            {
            }
        }
		*/
    }
}