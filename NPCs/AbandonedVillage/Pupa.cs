using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using Terraria.Audio;
using ReLogic.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

using SOTS.Dusts;

namespace SOTS.NPCs.AbandonedVillage
{
    public class Pupa : ModNPC
    {
		int ScaleTimerLimit = 10;
		float ScaleAmount = 0.05f;

		private static Asset<Texture2D> NPCTexture;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
        }
        
        public override void SetDefaults()
		{
            NPC.lifeMax = 120;
            NPC.damage = 30;
            NPC.defense = 10;
            NPC.width = 38;
			NPC.height = 58;
            NPC.npcSlots = 1f;
			NPC.knockBackResist = 0f;
            NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
            NPC.aiStyle = 3;
            AIType = NPCID.Crab;
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			NPCTexture ??= ModContent.Request<Texture2D>(Texture);

			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			Main.EntitySpriteDraw(NPCTexture.Value, NPC.Center - screenPos, NPC.frame, NPC.GetNPCColorTintedByBuffs(drawColor), NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);

			return false;
		}

        public override void FindFrame(int frameHeight)
        {
            //walking animation
            NPC.frameCounter++;
            if (NPC.frameCounter > 10)
            {
                NPC.frame.Y = NPC.frame.Y + frameHeight;
                NPC.frameCounter = 0;
            }
            if (NPC.frame.Y >= frameHeight * 5)
            {
                NPC.frame.Y = 0 * frameHeight;
            }

            //frame when falling/jumping
            if (NPC.velocity.Y > 0 || NPC.velocity.Y < 0 || NPC.localAI[0] > 0)
            {
                NPC.frame.Y = 2 * frameHeight;
            }
        }
        
        public override void AI()
		{
            NPC.spriteDirection = NPC.direction;

            if (NPC.localAI[0] == 0)
            {
                foreach (Player player in Main.ActivePlayers)
                {
                    if (player.Distance(NPC.Center) < 100)
                    {
                        NPC.localAI[0] = 1;
                    }
                }
            }
            else
            {
                NPC.aiStyle = -1;
                NPC.velocity.X = 0;

                NPC.localAI[0]++;
				NPC.localAI[1]++;
				if (NPC.localAI[1] < 2)
				{
					NPC.scale -= 0.12f;
				}
				if (NPC.localAI[1] >= 2)
				{
					NPC.scale += 0.12f;
				}

				if (NPC.localAI[1] > 4)
				{
					NPC.localAI[1] = 0;
					NPC.scale = 1f;
				}

                if (NPC.localAI[0] >= 60)
                {
                    SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, NPC.Center);
                    SoundEngine.PlaySound(SoundID.Item171, NPC.Center);

                    //todo: dust and gores explosion should spawn here

                    for (int i = 0; i < 3; i++)
                    {
                        int Fly = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<PupaFly>());
                        Main.npc[Fly].velocity.X = Main.rand.Next(-5, 6);
                        Main.npc[Fly].velocity.Y = Main.rand.Next(-5, 0);
                    }

                    NPC.active = false;
                }
			}
        }

        public override void HitEffect(NPC.HitInfo hit) 
        {
			if (NPC.life <= 0) 
            {
                //gore here
            }
        }
    }
}