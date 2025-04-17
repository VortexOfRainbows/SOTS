using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using ReLogic.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using SOTS.Items.Banners;
using SOTS.Dusts;
using System.Collections.Generic;
using SOTS.WorldgenHelpers;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Fragments;
using Terraria.GameContent.ItemDropRules;

namespace SOTS.NPCs.AbandonedVillage
{
    public class BallOWorms : BallOGuts
    {
        private static Asset<Texture2D> NPCTexture;
        private static Asset<Texture2D> PieceOfBallTexture;
        private Vector2 WormTrailStartPos => NPC.Center + new Vector2(0, 16);
        private List<Vector2> segments = new List<Vector2>();
        private void UpdateSegments()
        {
            while(segments.Count < 10)
            {
                segments.Add(WormTrailStartPos + new Vector2(1 * NPC.direction, 0.1f) * segments.Count);
            }
            Vector2 prev = WormTrailStartPos;
            float wormingAmount = 11;
            for (int i = 0; i < segments.Count; i++)
            {
                float percent = 1f - (float)i / segments.Count;
                Vector2 toPrev = prev - segments[i];
                float normalMovement = toPrev.Length() * 0.6f - wormingAmount;
                if (normalMovement > 0)
                    segments[i] += toPrev.SNormalize() * normalMovement;
                segments[i] = Vector2.Lerp(segments[i], prev, 0.035f);
                segments[i] += new Vector2(0, 0.3f + 0.02f * i + NPC.velocity.Y * percent * 0.3f);
                int x = (int)segments[i].X / 16;
                int y = (int)segments[i].Y / 16;
                if(SOTSWorldgenHelper.TrueTileSolid(x, y))
                {
                    Vector2? tileCollidePos = SOTSTile.GetWorldPositionOnTile(x, y, 0, segments[i].X - x * 16, segments[i].Y - y * 16, true);
                    if(tileCollidePos != null && tileCollidePos.Value.Y < segments[i].Y)
                    {
                        segments[i] = Vector2.Lerp(segments[i], tileCollidePos.Value, 0.3f);
                    }
                }
                prev = segments[i];
            }
        }
        private void DrawSegments(SpriteBatch spriteBatch, Vector2 screenPos)
        {
            Texture2D worm = ModContent.Request<Texture2D>("SOTS/NPCs/AbandonedVillage/BallWorm").Value;
            Vector2 origin = new Vector2(4, worm.Height / 2);
            Vector2 prev = WormTrailStartPos;
            Rectangle frame = new Rectangle(0, 0, worm.Width - 4, worm.Height);
            for (int i = 0; i < segments.Count; i++)
            {
                float percent = 1f - (float)i / segments.Count;
                Vector2 toPrev = prev - segments[i];
                Vector2 position = segments[i] - screenPos + new Vector2(0, -2);
                float r = toPrev.ToRotation();
                float Dist = toPrev.Length();
                int x = (int)segments[i].X / 16;
                int y = (int)segments[i].Y / 16;
                spriteBatch.Draw(worm, position, frame, Lighting.GetColor(x, y), r, origin, new Vector2(Dist / (frame.Width - 4), 1.0f + 0.3f * percent), toPrev.X < 0 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
                prev = segments[i];
            }
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(hasCollidedWithWall);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            hasCollidedWithWall = reader.ReadBoolean();
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if(!target.Hitbox.Intersects(NPC.Hitbox)) //Basically, if the player is hit by the tail and not the main body
            {
                modifiers.SourceDamage *= 0.5f;
            }
        }
        public override void SetDefaults()
		{
            base.SetDefaults(); //DO NOT REMOVE THIS
            NPC.lifeMax -= 5;
            NPC.defense += 2;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<BallOWormsBanner>();
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            DrawSegments(spriteBatch, screenPos);
            NPCTexture ??= ModContent.Request<Texture2D>(Texture);
            PieceOfBallTexture ??= ModContent.Request<Texture2D>("SOTS/NPCs/AbandonedVillage/BallOWormsPieces");

            float stretch = 0f;

			stretch = Math.Abs(stretch) - addedStretch;
			
			if (stretch > 0.5f) //limit how much it can stretch
            {
				stretch = 0.5f;
			}

			if (stretch < -0.5f) //limit how much it can squish
            {
				stretch = -0.5f;
			}

			Vector2 scaleStretch = new Vector2(1f + stretch, 1f - stretch);

            Vector2 drawPosition = new Vector2(NPC.Center.X, NPC.Center.Y) - screenPos + new Vector2(0, NPC.gfxOffY + 4);

            //draw npc manually for stretching
            spriteBatch.Draw(NPCTexture.Value, drawPosition, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2f, scaleStretch, SpriteEffects.None, 0f);

            //theres probably a better way to do this but i didnt feel like spending 6 hours on it
            for (int numFrame = 0; numFrame < 3; numFrame++)
            {
                float Pulsing = (float)Math.Cos((double)(Main.GlobalTimeWrappedHourly % 2.5f / 2.5f * 6f)) / 2f + 0.5f;

                //I LOVE RANDOM NUMBERS
                if (numFrame == 0 || numFrame == 2)
                {
                    Pulsing = (float)Math.Cos((double)(Main.GlobalTimeWrappedHourly % 2.5f / 2.5f * 6f)) / 2f + 0.5f;
                }
                else
                {
                    Pulsing = (float)Math.Sin((double)(Main.GlobalTimeWrappedHourly % 2.5f / 2.5f * 6f)) / 2f + 0.5f;
                }

                Pulsing = MathHelper.Clamp(Pulsing, 0f, 1f);

                spriteBatch.Draw(PieceOfBallTexture.Value, drawPosition, new Rectangle(0, numFrame * NPC.height, NPC.width, NPC.height), 
                drawColor, NPC.rotation, NPC.frame.Size() / 2f, new Vector2(scaleStretch.X + Pulsing / 12, scaleStretch.Y + Pulsing / 12), SpriteEffects.None, 0f);
            }

			return false;
		}
        public override bool PreAI()
        {
            UpdateSegments();
            return true;
        }
        public override bool ModifyCollisionData(Rectangle victimHitbox, ref int immunityCooldownSlot, ref MultipliableFloat damageMultiplier, ref Rectangle npcHitbox)
        {
            bool colliding = NPC.Hitbox.Intersects(victimHitbox);
            if (colliding)
                return true;
            for (int i = 0; i < segments.Count; i++)
            {
                Rectangle hitBox = new Rectangle((int)segments[i].X - 5, (int)segments[i].Y - 3, 10, 6);
                if (hitBox.Intersects(victimHitbox))
                {
                    npcHitbox = victimHitbox;
                    break;
                }
            }
            return false;
        }
        public override void HitEffect(NPC.HitInfo hit) 
        {
            if (Main.netMode == NetmodeID.Server)
                return;
			if (NPC.life <= 0)
            {
                for(int i = 0; i < 5; ++i)
                {
                    Vector2 circular = Main.rand.NextVector2CircularEdge(10, 10);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center + circular - new Vector2(9, 9), circular * 0.135f, ModGores.GoreType("Gores/Ball/BallOWormsGore1"), 1f);
                }
                for (int i = 0; i < 3; ++i)
                {
                    Vector2 circular = Main.rand.NextVector2CircularEdge(10, 10);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center + circular - new Vector2(13, 7), circular * 0.125f, ModGores.GoreType("Gores/Ball/BallOWormsGore3"), 1f);
                }
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center - new Vector2(19, 16), new Vector2(hit.HitDirection, -1), ModGores.GoreType("Gores/Ball/BallOWormsGore2"), 1f);
                for (int i = 0; i < 30; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<FamishedDustCorruption>(), hit.HitDirection, -1f, NPC.alpha, Scale: 1.25f);
                }
                if(segments != null)
                {
                    for (int i = 0; i < segments.Count; i++)
                    {
                        float percent = 1f - (float)i / segments.Count;
                        Gore.NewGore(NPC.GetSource_Death(), segments[i] - new Vector2(11, 5), new Vector2(hit.HitDirection, -Main.rand.NextFloat()), ModGores.GoreType("Gores/Ball/BallWorm"), .8f + 0.3f * percent);
                    }
                }
            }
            else
            {
                int num = 0;
                while (num < hit.Damage / (float)NPC.lifeMax * 60)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<FamishedDustCorruption>(), hit.HitDirection, -1, NPC.alpha, Scale: 1.25f);
                    num++;
                }
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.RottenChunk, 2));
            npcLoot.Add(ItemDropRule.Common(ItemID.Worm, 5, 1, 5));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FragmentOfEvil>(), 5));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldKey>(), 100));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PintOPunch>(), 200));
        }
    }
}