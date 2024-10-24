using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.NPCs.AbandonedVillage;
using SOTS.NPCs.Gizmos;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SOTS.Common.GlobalNPCs
{
    public class FamishedCarrier : GlobalNPC
    {
        public static Texture2D texture => ModContent.Request<Texture2D>(Famished.SeedTexture).Value;
        public static int MaxLength => 30;
        private float OrbitalCounter => SOTSWorld.GlobalCounter * 0.5f + MyCounter * 1.0f;
        private bool InFront(float counter) => counter % 360 > 180;
        private float Sinusoid => 0.5f - 0.5f * MathF.Sin(MathHelper.ToRadians(OrbitalCounter));
        private bool Infected = false;
        private bool RunOnce = true;
        private Vector2 SeedPos = Vector2.Zero;
        private Vector2 myVelo
        {
            get
            {
                if(OldPositions != null && OldPositions.Count > 1)
                {
                    return SeedPos - OldPositions[1];
                }
                return Vector2.Zero;
            }
        }
        private List<Vector2> OldPositions;
        private int MyCounter = 0;
        private void Draw(SpriteBatch spriteBatch, Vector2 screenPos, bool inFront)
        {
            if (OldPositions == null)
                return;
            Texture2D pixel = SOTSUtils.WhitePixel;
            Vector2 origin = new Vector2(0, 1);
            Vector2 previous = SeedPos;
            Color c = Famished.GlowColor;
            float rotation = myVelo.X * 0.055f;
            for (int i = 0; i < OldPositions.Count; i++)
            {
                if (OldPositions[i] == Vector2.Zero)
                    break;
                Vector2 center = OldPositions[i];
                float perc = 1 - i / (float)MaxLength;
                Vector2 toPrev = previous - center;
                if ((InFront(OrbitalCounter - 3 * i) == inFront && i > 15) || (InFront(OrbitalCounter) == inFront && i <= 15))
                    spriteBatch.Draw(pixel, center - Main.screenPosition, null, c * perc, toPrev.ToRotation(), origin, new Vector2(toPrev.Length() / 2f, 2.5f * perc), SpriteEffects.None, 0f);
                previous = center;
            }
            if(InFront(OrbitalCounter) == inFront)
            {
                origin = texture.Size() / 2;
                float scale = 0.7f * (0.8f + 0.3f * Sinusoid);
                c *= 0.5f * (0.5f + 0.5f * Sinusoid);
                for (int i = 0; i < 6; i++)
                {
                    Vector2 circular = new Vector2(1 + 2 * Sinusoid, 0).RotatedBy(i / 6f * MathHelper.TwoPi + MathHelper.ToRadians(SOTSWorld.GlobalCounter * 1.5f) + 0f);
                    spriteBatch.Draw(texture, SeedPos - screenPos + circular, null, c, rotation, origin, scale, SpriteEffects.None, 0f);
                }
                c = Lighting.GetColor((SeedPos / 16f).ToPoint());
                spriteBatch.Draw(texture, SeedPos - screenPos, null, c, rotation, origin, scale, SpriteEffects.None, 0f);
            }
        }
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Draw(spriteBatch, screenPos, false);
            return true;
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Draw(spriteBatch, screenPos, true);
        }
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            if(lateInstantiation)
            {
                bool validNPC = entity.type == ModContent.NPCType<CorpseBloom>()
                    || entity.type == ModContent.NPCType<EarthenGizmo>();
                return validNPC;
            }
            return false;
        }
        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            bitWriter.WriteBit(Infected);
            binaryWriter.Write(MyCounter);
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            Infected = bitReader.ReadBit();
            MyCounter = binaryReader.ReadInt32();
        }
        public override bool PreAI(NPC npc)
        {
            if(RunOnce)
            {
                RunOnce = false;
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    bool underground = npc.Center.Y > Main.rockLayer * 16 - 240;
                    if (underground)
                    {
                        int chanceToBeInfected = 4;
                        int totalFamishedInTheWorld = 0;
                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            NPC npc2 = Main.npc[i];
                            if (npc2.active && (npc2.type == ModContent.NPCType<Famished>() || (npc2.TryGetGlobalNPC(out FamishedCarrier fc) && fc.Infected)))
                            {
                                totalFamishedInTheWorld++;
                            }
                        }
                        chanceToBeInfected = (int)MathF.Pow(chanceToBeInfected, 1 + 0.5f * totalFamishedInTheWorld) + totalFamishedInTheWorld;
                        Infected = Main.rand.NextBool(chanceToBeInfected);
                        npc.netUpdate = true;
                    }
                }
            }
            if(Infected)
            {
                if (OldPositions == null)
                    OldPositions = new List<Vector2>();
                if(SeedPos == Vector2.Zero)
                {
                    SeedPos = npc.Center;
                }
                else
                {
                    MyCounter++;
                    if (SOTSWorld.GlobalCounter % 2 == 0)
                        OldPositions.Insert(0, SeedPos + new Vector2(0, 2));
                    if (OldPositions.Count > MaxLength)
                        OldPositions.RemoveAt(MaxLength);
                    Vector2 circular = new Vector2(32 + npc.width * 0.4f, 0).RotatedBy(MathHelper.ToRadians(OrbitalCounter));
                    circular.Y *= 0.25f;
                    circular = circular.RotatedBy(MathHelper.ToRadians(15 * MathF.Cos(MathHelper.ToRadians(OrbitalCounter * 2.3f))));
                    Vector2 positionToTravelTo = npc.Center + circular;

                    if(OldPositions.Count > 7 && Main.rand.NextBool(7))
                        PixelDust.Spawn(OldPositions[7], 0, 0, Main.rand.NextVector2Circular(0.5f, 0.5f), Famished.GlowColor * 0.5f, 5).scale = Main.rand.NextFloat(0.5f, 1.0f);
                    SeedPos = Vector2.Lerp(SeedPos, positionToTravelTo, 0.1f);
                    if (Main.rand.NextBool(8))
                    {
                        int type = WorldGen.crimson ? ModContent.DustType<FamishedDustCrimson>() : ModContent.DustType<FamishedDustCorruption>();
                        Dust d = Dust.NewDustDirect(SeedPos + new Vector2(-5, 3), 0, 0, type);
                        d.scale *= 0.6f + 0.1f * Sinusoid;
                        d.velocity *= 0.25f;
                    }
                    if(MyCounter % 120 == 0 && npc.life < npc.lifeMax)
                    {
                        int maxGainableHealth = Math.Min(npc.lifeMax - npc.life, Famished.LifePerBlock);
                        npc.life += maxGainableHealth;
                        if (Main.netMode != NetmodeID.Server)
                        {
                            SOTSUtils.PlaySound(SoundID.Item177, SeedPos, 0.8f, -0.4f);
                            CombatText.NewText(npc.Hitbox, CombatText.HealLife, maxGainableHealth, false, true);
                            Vector2 toNpc = npc.Center - SeedPos;
                            for(float i = 0; i < 0.9f; i += 0.025f)
                            {
                                Vector2 pos = Vector2.Lerp(SeedPos, npc.Center, i);
                                PixelDust.Spawn(pos, 0, 0, Main.rand.NextVector2Circular(0.3f, 0.3f) + toNpc.SNormalize() *  Main.rand.NextFloat(), Famished.GlowColor, 8).scale = Main.rand.NextFloat(1f, 1.5f);
                                PixelDust.Spawn(npc.position, npc.width, npc.height, Main.rand.NextVector2Circular(0.75f, 0.75f) + npc.velocity, Famished.GlowColor, 10).scale = Main.rand.NextFloat(1f, 1.5f);
                            }
                        }
                    }
                }
            }
            return true;
        }
        public override void OnKill(NPC npc)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient && Infected)
            {
                NPC npc2 = NPC.NewNPCDirect(npc.GetSource_Death(), (int)SeedPos.X, (int)SeedPos.Y, ModContent.NPCType<Famished>());
                npc2.velocity += Main.rand.NextVector2Circular(5, 5) + myVelo.SNormalize() * Main.rand.NextFloat(3f, 9f) + new Vector2(0, -2);
                npc2.netUpdate = true;
            }
            if(Infected && OldPositions != null)
            {
                Vector2 previous = SeedPos;
                for (int i = 0; i < OldPositions.Count; i++)
                {
                    if (OldPositions[i] == Vector2.Zero)
                        break;
                    Vector2 center = OldPositions[i];
                    float perc = 1 - i / (float)MaxLength;
                    PixelDust.Spawn(center, 0, 0, Main.rand.NextVector2Circular(0.2f, 0.2f) * (0.2f + perc) + (previous - center) * 0.2f, Famished.GlowColor * (0.2f + 0.5f * perc), 4).scale = 0.5f + 2.0f * perc;
                    previous = center;
                }
            }
        }
    }
}
