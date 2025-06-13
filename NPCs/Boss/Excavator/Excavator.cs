using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.NPCs.Gizmos;
using SOTS.Projectiles.AbandonedVillage;
using SOTS.WorldgenHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss.Excavator
{
    public class ExcavatorBody : ModNPC
    {
        public override string Texture => "SOTS/NPCs/Boss/Excavator/body";
        public override void SetStaticDefaults()
        {
            //NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            //NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            //NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
            //NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
        }
        public override void SetDefaults()
        {
            NPC.width = 90;           
            NPC.height = 90;        
            NPC.damage = 24;
            NPC.defense = 20;
            NPC.lifeMax = 20000;  
            NPC.knockBackResist = 0.0f;
            NPC.behindTiles = true;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.dontCountMe = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
        }
        public override void ModifyHoverBoundingBox(ref Rectangle boundingBox)
        {
            boundingBox = NPC.Hitbox;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server)
                return;
            //if (NPC.life <= 0)
            //{
            //    for (int k = 0; k < 10; k++)
            //    {
            //        Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Lead, 2.5f * (float)hit.HitDirection, -2.5f, 0, default(Color), 0.7f);
            //    }
            //    if(Main.rand.NextBool(3))
            //        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/EarthenConstructGore1"), 1f);
            //    if (Main.rand.NextBool(3))
            //        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/EarthenConstructGore3"), 1f);
            //    if (Main.rand.NextBool(3))
            //        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/EarthenConstructGore4"), 1f);
            //    if (Main.rand.NextBool(3))
            //        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/EarthenConstructGore5"), 1f);
            //    for (int i = 0; i < 4; i++)
            //        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Main.rand.Next(61, 64), 1f);
            //}
        }
        public override bool PreAI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!Main.npc[(int)NPC.ai[1]].active)
                {
                    NPC.life = 0;
                    NPC.HitEffect(0, 10.0);
                    NPC.active = false;
                    NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, NPC.whoAmI, -1f, 0.0f, 0.0f, 0, 0, 0);
                    return false;
                }
            }
            if (NPC.ai[3] > 0)
            {
                NPC.realLife = (int)NPC.ai[3];
            }
            if (NPC.ai[2] >= 0)
            {
                NPC.scale = 1;
                NPC.width = (int)(NPC.width * NPC.scale);
                NPC.height = (int)(NPC.height * NPC.scale);
                NPC.ai[2] = -NPC.ai[2];
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.netUpdate = true;
                }
            }

            Vector2 trueVelo = NPC.position - NPC.oldPosition;
            float speed = trueVelo.Length();
            if(speed < 1000)
            {
                NPC.ai[0] += 2 * MathF.Sqrt(speed);
            }
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            //Texture2D texture = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/body").Value;
            //Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            //spriteBatch.Draw(texture, NPC.Center - screenPos, null, drawColor, NPC.rotation - MathHelper.ToRadians(90), origin, NPC.scale + 0.04f, NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            return false;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;
    }
    public class ExcavatorBody2 : ExcavatorBody
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.width = 60;
            NPC.height = 60;
            NPC.damage = 20;
            NPC.defense = 20;
        }
    }
    public class ExcavatorTail : ExcavatorBody
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.width = 40;
            NPC.height = 40;
            NPC.damage = 12;
            NPC.defense = 24;
            NPC.dontTakeDamage = true;
            NPC.alpha = 255;
        }
    }
    public class ExcavatorDrillTail : ExcavatorTail
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.damage = 40;
            NPC.dontTakeDamage = false;
            NPC.alpha = 255;
        }
    }
    [AutoloadBossHead]
    public class Excavator : ModNPC
    {
        public static readonly int EnergyBallPhase = 1;
        public static readonly int LaserPhase = 2;
        public static readonly int SawPhase = 3;
        public static readonly int RocketPhase = 4;
        public static readonly int SecondPhaseTransition = 5;
        public static readonly int DrillDashPhase = 6;
        public class ExcavatorArm(Excavator owner, int dir, bool bigArm = false)
        {
            public float SawBladeRotation;
            public float sawBladePercent;
            public static void DoArmJoint(ref Vector2 start, ref Vector2 end, float A, float B, int dir, out float endArmRot, out float endHandRot)
            {
                if (end.Distance(start) > (A + B))
                {
                    end = start + (end - start).SNormalize() * (A + B);
                }
                if (end.Distance(start) < 32)
                {
                    end = start + (end - start).SNormalize() * 32;
                }
                Vector2 startToEnd = end - start;
                float C = startToEnd.Length();
                float angleA = C - A - B > 0 ? 0 : MathF.Acos((B * B + C * C - A * A) / (2 * B * C));
                float angleB = C - A - B > 0 ? 0 : MathF.Acos((A * A + C * C - B * B) / (2 * A * C));
                Vector2 endToMid = -startToEnd.RotatedBy(angleB * dir);
                Vector2 startToMid = startToEnd.RotatedBy(-angleA * dir);
                endHandRot = endToMid.ToRotation();
                endArmRot = startToMid.ToRotation();
            }
            public void DoArmIK(NPC other, SpriteBatch spriteBatch, Vector2 screenPos, int dir, bool draw = true)
            {
                int j = SOTSUtils.SignNoZero(dir);
                Texture2D body = null;
                Texture2D arm = null;
                Texture2D hand = null;
                Texture2D handOverheat = null;
                Texture2D handGlow = null;
                Texture2D handSaw = null;
                if (draw)
                {
                    body = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/body").Value;
                    arm = ModContent.Request<Texture2D>(isBigArm ? "SOTS/NPCs/Boss/Excavator/bigArmLeft" : "SOTS/NPCs/Boss/Excavator/arm").Value;
                    if (isBigArm)
                    {
                        hand = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/handDrill").Value;
                        handGlow = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/handDrillGlow").Value;
                    }
                    else
                    {
                        string handVal = "SOTS/NPCs/Boss/Excavator/hand";
                        if (ArmType == 1)
                        {
                            handVal = "SOTS/NPCs/Boss/Excavator/handSaw";
                            handSaw = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/saw").Value;
                            handGlow = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/handSawGlow").Value;
                        }
                        else if (ArmType == 2)
                            handVal = "SOTS/NPCs/Boss/Excavator/handNoWeapon";
                        else
                            handGlow = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/handGlow").Value;
                        hand = ModContent.Request<Texture2D>(handVal).Value;
                        handOverheat = ModContent.Request<Texture2D>(handVal + "Overheat").Value;
                    }
                }
                int armWidth = isBigArm ? 118 : 56;
                float handWidth = isBigArm ? 46 : ArmType == 1 ? 26 : ArmType == 2 ? 34 : 38;
                float handHeight = isBigArm ? 132 : this.handHeight;
                int bodyWidth = 126;
                int bodyHeight = 104;
                Vector2 armOrigin = isBigArm ? new Vector2(95, 47) : new Vector2(50, 14);
                Vector2 revArmOrigin = new(armWidth - armOrigin.X, armOrigin.Y);
                Vector2 handOrigin = new(handWidth / 2, handHeight);
                float armRotation = other.rotation;
                Vector2 armPosition = isBigArm ? new Vector2((-bodyWidth / 2 + 13) * j, bodyHeight / 2 - 31) : new Vector2((-bodyWidth / 2 + 4) * j, -bodyHeight / 2 + 16);
                armPosition = armPosition.RotatedBy(armRotation) + other.Center;
                Color drawColor = Lighting.GetColor(armPosition.ToTileCoordinates(), Color.White);

                float r = other.ai[0] * 1.2f * j + (isBigArm ? (j == -2 ? 45 : 135) : (j * 45));
                float outwardSize = (isBigArm ? 80 : 38) - (isBigArm ? 4 : 16) * MathF.Sin(MathHelper.ToRadians(r + 90 * j));
                if (!isBigArm)
                {
                    outwardSize *= 1 - armTargetPercent * 0.9f;
                }
                Vector2 targetHandPos = new Vector2(-(bodyWidth / 2 + outwardSize) * j, isBigArm ? -70 : -100).RotatedBy(armRotation);
                Vector2 circular = new Vector2(isBigArm ? 8 : 20, 0).RotatedBy(MathHelper.ToRadians(r));
                circular.X *= 0.25f;
                circular = circular.RotatedBy(armRotation);
                if (!isBigArm)
                    circular *= 1 - armTargetPercent * 0.8f;
                float sin = 0;
                if (!isBigArm && armTargetPercent > 0)
                {
                    float percent = ArmType == 1 ? MathF.Sqrt(MathF.Abs(MathF.Sin(sawBladePercent * MathF.PI))) : 1;
                    float bonusMax = ArmType == 1 ? 52 * percent : 24;
                    float bonusRate = ArmType == 1 ? 18 : 24;
                    float bonusMin = ArmType == 1 ? 12 : 0;
                    Vector2 toArm = Vector2.Lerp(armTarget, forceArmTarget, forceArmTargetPercent) - armPosition;
                    float angle = MathHelper.WrapAngle(toArm.ToRotation() - armRotation);
                    bool targetBehindMe = angle > 0;
                    sin = MathF.Sin(-angle);
                    if (!targetBehindMe)
                    {
                        targetHandPos += sin * armTargetPercent * toArm.SNormalize() * MathF.Min(bonusMax, bonusMin + (armTarget - armPosition).Length() / bonusRate);
                    }
                }
                targetHandPos += circular + other.Center;
                if(sin != 0)
                    targetHandPos = Vector2.Lerp(targetHandPos, forceArmTarget, forceArmTargetPercent * sin);
                float A = isBigArm ? handHeight : handHeight; //size of hand
                float B = isBigArm ? armWidth - 34 : armWidth - 20; //size of arm
                Vector2 end = targetHandPos;
                Vector2 start = armPosition;
                DoArmJoint(ref start, ref end, A, B, j, out float endArmRot, out float endHandRot);
                Vector2 mid = start + new Vector2(B, 0).RotatedBy(endArmRot);
                if (isBigArm)
                {
                    end -= new Vector2(0, 21 * j).RotatedBy(endArmRot);
                }
                else
                {
                    int num = dir == -1 ? 1 : 0;
                    end += new Vector2(1, 0).RotatedBy(endArmRot);

                    Vector2 midToTarget = -armTarget + mid;
                    float toPR = MathHelper.WrapAngle(midToTarget.ToRotation() - endArmRot);
                    if (j == -1)
                    {
                        if (toPR < 0 && toPR < -MathHelper.PiOver2)
                            toPR += MathHelper.TwoPi;
                        toPR = MathHelper.Clamp(toPR, MathHelper.ToRadians(30), MathF.PI);
                    }
                    else
                    {
                        if (toPR > 0 && toPR > MathHelper.PiOver2)
                            toPR -= MathHelper.TwoPi;
                        toPR = MathHelper.Clamp(toPR, -MathF.PI, -MathHelper.ToRadians(30));
                    }
                    toPR += endArmRot;
                    float rot = -endHandRot + toPR;
                    if (!draw)
                    {
                        ArmAngleSpecial = SOTSUtils.AngularLerp(ArmAngleSpecial, rot, 0.12f * armTargetPercent);
                        ArmAngleSpecial = SOTSUtils.AngularLerp(ArmAngleSpecial, 0, 0.12f * (1 - armTargetPercent));
                    }
                    end = end.RotatedBy(ArmAngleSpecial, mid);
                    endHandRot += ArmAngleSpecial;


                    //Main.NewText(MathHelper.WrapAngle(toPR - endArmRot));
                }

                //if (isBigArm)
                //{
                //Vector2 mid = startToMid.SNormalize() * 84 + start;
                //mid -= new Vector2(0, 14 * j).RotatedBy(endArmRot);
                //int bigDrillWidth = 38;
                //Vector2 drillOrigin = new(42, 72);
                //Vector2 revDrillOrigin = new(bigDrillWidth - drillOrigin.X, drillOrigin.Y);
                //float deg = -j * ((j == 1 ? 180 : 0) + -70);
                //if(draw)
                //{
                //Texture2D bigDrill = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/bigDrill").Value;
                //spriteBatch.Draw(bigDrill, mid - screenPos, null, drawColor, endHandRot + MathHelper.ToRadians(deg), j == 1 ? drillOrigin : revDrillOrigin, other.scale, j == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                //}
                //}

                if (draw)
                {
                    if (ArmType == 1 && handSaw != null)
                    {
                        float scalePercent = sawBladeTimer >= 150 ? 1 : MathF.Min(sawBladePercent * 6.667f, 1f);
                        scalePercent += MathF.Sin(scalePercent * scalePercent * MathF.PI) * 0.4f;
                        float percent = sawBladeTimer >= 150 ? 1 : sawBladePercent;
                        float rotation = MathHelper.ToRadians(SawBladeRotation);
                        Vector2 sawOrigin = handSaw.Size() / 2;
                        for (int i = 0; i < 6; ++i)
                        {
                            Vector2 circular2 = new Vector2(4 * percent, 0).RotatedBy(MathHelper.ToRadians(i * 60 + SOTSWorld.GlobalCounter * j));
                            spriteBatch.Draw(handSaw, circular2 + end - screenPos + new Vector2(5, 0).RotatedBy(endHandRot), null, ExcavatorOrb.Color * 0.5f * percent, rotation * j, sawOrigin, other.scale * scalePercent, j == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                        }
                        spriteBatch.Draw(handSaw, end - screenPos + new Vector2(5, 0).RotatedBy(endHandRot), null, drawColor, rotation * j, sawOrigin, other.scale * scalePercent, j == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                    }
                    spriteBatch.Draw(hand, end - screenPos, null, drawColor, endHandRot + MathHelper.PiOver2, handOrigin, other.scale, j == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                    if (ArmType == 0 || isBigArm)
                        spriteBatch.Draw(handGlow, end - screenPos, null, Color.White, endHandRot + MathHelper.PiOver2, handOrigin, other.scale, j == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                    if (!isBigArm)
                    {
                        float percent = ArmSwitchTimer / 60f;
                        if (percent > 0)
                        {
                            for (int i = 0; i < 6; ++i)
                            {
                                Vector2 circular2 = new Vector2(3 + percent, 0).RotatedBy((percent + i / 3f) * MathF.PI);
                                spriteBatch.Draw(handOverheat, circular2 + end - screenPos, null, ExcavatorOrb.Color * 0.45f * percent, endHandRot + MathHelper.PiOver2, handOrigin, other.scale, j == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                            }
                        }
                    }
                    spriteBatch.Draw(arm, start - screenPos, null, drawColor, endArmRot + (j == -1 ? MathF.PI : 0), j == -1 ? armOrigin : revArmOrigin, other.scale, j == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                    if (isBigArm)
                        spriteBatch.Draw(ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/bigArmLeftGlow").Value, start - screenPos, null, Color.White,
                            endArmRot + (j == -1 ? MathF.PI : 0), j == -1 ? armOrigin : revArmOrigin, other.scale, j == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                    //Dust.NewDust(end, 0, 0, DustID.LifeDrain);
                }
                else
                {
                    handPos = end;
                    handNorm = new Vector2(-1, 0).RotatedBy(endHandRot);
                }

                //Visual representations of the IK happening
                //if (draw)
                //{
                //    drawColor *= 0.4f;
                //    spriteBatch.Draw(SOTSUtils.WhitePixel, end - screenPos, null, drawColor, 0, Vector2.One, other.scale * 4, SpriteEffects.None, 0);
                //    spriteBatch.Draw(SOTSUtils.WhitePixel, end - screenPos, null, drawColor, MathF.PI + endHandRot, new Vector2(0, 1), new Vector2(500, 1), SpriteEffects.None, 0);
                //    spriteBatch.Draw(SOTSUtils.WhitePixel, end - screenPos, null, drawColor, endHandRot, new Vector2(0, 1), new Vector2(A * 0.5f, 2), SpriteEffects.None, 0);
                //    spriteBatch.Draw(SOTSUtils.WhitePixel, targetHandPos - circular - screenPos, null, Color.Red, 0, Vector2.One, other.scale * 4, SpriteEffects.None, 0);
                //    spriteBatch.Draw(SOTSUtils.WhitePixel, targetHandPos - screenPos, null, Color.Red, 0, Vector2.One, other.scale * 4, SpriteEffects.None, 0);
                //    spriteBatch.Draw(SOTSUtils.WhitePixel, start - screenPos, null, drawColor, 0, Vector2.One, other.scale * 4, SpriteEffects.None, 0);
                //    spriteBatch.Draw(SOTSUtils.WhitePixel, start - screenPos, null, drawColor, endArmRot, new Vector2(0, 1), new Vector2(B * 0.5f, 2), SpriteEffects.None, 0);
                //    spriteBatch.Draw(SOTSUtils.WhitePixel, mid - screenPos, null, Color.Yellow, 0, Vector2.One, other.scale * 4, SpriteEffects.None, 0);
                //    spriteBatch.Draw(SOTSUtils.WhitePixel, forceArmTarget - screenPos, null, Color.Green, 0, Vector2.One, other.scale * 4, SpriteEffects.None, 0);
                //}

                //Visual location of the actual center of the arm
                //Vector2 realEnd = end + new Vector2(5, 4 * j).RotatedBy(endToMid.ToRotation());
                //spriteBatch.Draw(SOTSUtils.WhitePixel, realEnd - screenPos, null, Color.Red, 0, Vector2.One, other.scale * 4, SpriteEffects.None, 0);
            }
            public void SwitchArm(int i)
            {
                NextArmType = i;
                if (ArmType != i)
                {
                    if(ArmType != 1 || sawBladeTimer == 0)
                        ArmSwitchTimer++;
                    if (ArmSwitchTimer > 60)
                    {
                        ArmSwitchTimer = 0;
                        if (Main.netMode != NetmodeID.Server)
                        {
                            Vector2 size = ArmType == 0 ? new Vector2(-19, -16) : ArmType == 1 ? new Vector2(-7, -21) : new Vector2(-17, -25);
                            float xOff = 20;
                            float r = handNorm.ToRotation();
                            Vector2 offset = size - (handNorm * xOff);
                            Gore g = Gore.NewGoreDirect(NPC.GetSource_Death(), handPos + offset, NPC.velocity + handNorm * Main.rand.NextFloat(), ModGores.GoreType("Gores/Excavator/handGore" + (ArmType + 1)), 1f);
                            g.rotation = r - MathHelper.PiOver2;
                            SOTSUtils.PlaySound(SoundID.Item62, handPos, 1, 0.5f, 0);
                            for (int k = 0; k < 17; k++)
                            {
                                Dust d = PixelDust.Spawn(handPos, 0, 0, Main.rand.NextVector2Circular(3, 3) + handNorm * Main.rand.NextFloat(-2, 3) + NPC.velocity * Main.rand.NextFloat(0.1f, 1.5f), ExcavatorOrb.Color, 5);
                                d.scale *= Main.rand.NextFloat(1, 3);
                                if (k % 2 == 0)
                                {
                                    d = Dust.NewDustDirect(handPos - new Vector2(5), 0, 0, DustID.Smoke);
                                    d.velocity += handNorm * Main.rand.NextFloat(-2, 3) + NPC.velocity * Main.rand.NextFloat(0.1f, 1.5f);
                                    d.velocity *= 0.5f;
                                    d.scale += Main.rand.NextFloat(0.4f);
                                }
                                if (Main.rand.NextBool(6))
                                {
                                    g = Gore.NewGoreDirect(NPC.GetSource_Death(), handPos + offset, NPC.velocity + handNorm * Main.rand.NextFloat(), Main.rand.NextFromList(GoreID.Smoke1, GoreID.Smoke2, GoreID.Smoke3), 1f);
                                    g.scale *= 0.75f;
                                    g.velocity *= 0.5f;
                                }
                            }
                        }
                        if (ArmType != 1 && NextArmType == 1)
                        {
                            sawBladeTimer = 0;
                            if (dir == -1)
                                sawBladeTimer = -71;
                        }
                        ArmType = i;
                    }
                }
                else
                    ArmSwitchTimer = 0;
            }
            public void TargetArm(Vector2 pos, bool force = false)
            {
                if(force)
                {
                    forceArmTarget = pos;
                    forceArmTargetting = true;
                }
                armTargetting = true;
                armTarget = pos;
            }
            public void AdjustHandSize()
            {
                float targetH = ArmType == 1 ? 76 : ArmType == 2 ? 82 : 64;
                handHeight = MathHelper.Lerp(handHeight, targetH, 0.1f);
            }
            public NPC NPC => owner.NPC;
            public Vector2 handPos;
            public Vector2 handNorm;
            public Vector2 armTarget;
            public Vector2 forceArmTarget;
            public float armTargetPercent, forceArmTargetPercent;
            public bool armTargetting, forceArmTargetting;
            public float ArmSwitchTimer;
            public int ArmType = 2, NextArmType = 2;
            public float handWidth;
            public float handHeight;
            public float ArmAngleSpecial;
            public float sawBladeTimer = 0;
            public bool isBigArm = bigArm;
            public Excavator owner = owner;
            public int dir = dir;
            public void PreUpdate()
            {
                if (ArmSwitchTimer > 0)
                    SwitchArm(NextArmType);
                else
                    AdjustHandSize();
            }
            public void PostUpdate()
            {
                if (armTargetting)
                    armTargetPercent = MathHelper.Lerp(armTargetPercent + 0.01f, 1, 0.08f);
                else
                    armTargetPercent = MathHelper.Lerp(armTargetPercent - 0.01f, 0, 0.09f);

                if (forceArmTargetting)
                    forceArmTargetPercent = MathHelper.Lerp(forceArmTargetPercent + 0.01f, 1, 0.1f);
                else
                    forceArmTargetPercent = MathHelper.Lerp(forceArmTargetPercent - 0.01f, 0, 0.15f);

                armTargetPercent = MathHelper.Clamp(armTargetPercent, 0, 1);
                forceArmTargetPercent = MathHelper.Clamp(forceArmTargetPercent, 0, 1);
                armTargetting = forceArmTargetting = false;
            }
            public bool sawSwingingDown = false;
            public void SawUpdate()
            {
                sawSwingingDown = false;
                sawBladePercent = 0;
                if (sawBladeTimer >= 150)
                {
                    TargetArm(owner.target.Center, false);
                    sawBladePercent = (sawBladeTimer - 150) / 30f;
                    if(sawBladeTimer == 150)
                    {
                        SOTSUtils.PlaySound(SoundID.Item22, handPos, 0.7f, -0.3f);
                    }
                    sawBladeTimer++;
                    if (sawBladeTimer > 180)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), handPos, handNorm * 14.5f + NPC.velocity, ModContent.ProjectileType<ExcavatorSaw>(), NPC.GetBaseDamage() / 2, 1, Main.myPlayer);
                        sawBladeTimer = 0;
                        SOTSUtils.PlaySound(SoundID.Item23, handPos, 0.7f, -0.3f);
                        SOTSUtils.PlaySound(SoundID.Item23, handPos, 0.7f, -0.3f);
                    }
                    SawBladeRotation = sawBladePercent * 480;
                }
                else if(sawBladeTimer > 0 || (sawBladeTimer == 0 && NextArmType == 1))
                {
                    int bodyWidth = 126;
                    int bodyHeight = 104;
                    NPC body = Main.npc[owner.segments[0]];
                    float percent = sawBladeTimer / 150f;
                    sawBladeTimer++;
                    float sin = MathF.Sin(percent * percent * MathF.PI);
                    float iPercent = 1 - percent;
                    sawBladePercent = percent;

                    Vector2 armPosition = new Vector2((-bodyWidth / 2 + 4) * dir, -bodyHeight / 2 + 16);
                    armPosition = armPosition.RotatedBy(body.rotation) + body.Center;
                    float afterMidPercent = MathF.Max(0, percent * 2 - 1);
                    float r = MathHelper.ToRadians(90 * sin - 40 * afterMidPercent) * -dir;
                    float maxExtension = 96 + 64 * percent;
                    Vector2 handSwingPos = armPosition + new Vector2(0, -maxExtension).RotatedBy(body.rotation + r) * 0.8f;
                    TargetArm(handSwingPos, true);
                    if (afterMidPercent > 0)
                    {
                        if(afterMidPercent > 0.6f)
                            sawSwingingDown = true;
                        sawBladeTimer++;
                    }
                    if (sawBladeTimer >= 150)
                    {
                        sawBladeTimer = 150;
                    }
                    SawBladeRotation = sawBladePercent * sawBladePercent * 2400;
                }
                else if(NextArmType == 1)
                {
                    sawBladeTimer++;
                }
            }
        }
        public ExcavatorArm[] arms;
        public ExcavatorArm[] GenArms()
        {
            return [new(this, 1), new(this, -1), new(this, 2, true), new(this, -2, true)]; //This is a really funny looking declaration
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(MoveStyle);
            for(int i = 0; i < segments.Length; ++i)
                writer.Write(segments[i]);
            writer.Write(AI3);
            writer.Write(AI4);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            MoveStyle = reader.ReadSingle();
            for (int i = 0; i < segments.Length; ++i)
              segments[i] = reader.ReadInt32();
            AI3 = reader.ReadSingle();
            AI4 = reader.ReadSingle();
        }
        public float AIPhase
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }
        public float NextAIPhase = -1;
        public float MoveStyle = 0;
        public float WalkCounter
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }
        public float AI1
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }
        public float AI2
        {
            get => NPC.ai[3];
            set => NPC.ai[3] = value;
        }
        public float AI3;
        public float AI4;
        public static float TelegraphSize => 2400;
        public float TelegraphCounter = 0;
        public float TelegraphFadeOut = 0;
        public Vector2 leftTelegraph => new(TelegraphLocation.X - TelegraphSize * 0.85f, TelegraphLocation.Y);
        public Vector2 rightTelegraph => new(TelegraphLocation.X + TelegraphSize * 0.85f, TelegraphLocation.Y);
        public Vector2 ClosestTelegraphEndLocation()
        {
            float toLeft = leftTelegraph.Distance(NPC.Center);
            float toRight = rightTelegraph.Distance(NPC.Center);
            if (toLeft > toRight)
                return rightTelegraph;
            else
                return leftTelegraph;
        }
        public Vector2 FarthestTelegraphEndLocation()
        {
            float toLeft = leftTelegraph.Distance(NPC.Center);
            float toRight = rightTelegraph.Distance(NPC.Center);
            if (toLeft > toRight)
                return leftTelegraph;
            else
                return rightTelegraph;
        }
        public Vector2 TelegraphLocation;
        public bool NeedsToGoIntoPhase2 => !InSecondPhase && NPC.life < NPC.lifeMax / 2f;
        public bool InSecondPhase = false;
        public List<Vector2> neckSegments;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            DrawDashTelegraph(spriteBatch, screenPos);
            Texture2D head = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/head").Value;
            Texture2D headGlow = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/headGlow").Value;
            Texture2D neck = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/neck").Value;
            Texture2D neckGlow = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/neckGlow").Value;
            Vector2 origin = new(head.Width * 0.5f, head.Height * 0.5f);
            Vector2 neckOrigin = new(neck.Width * 0.5f, neck.Height);


            for (int i = segments.Length - 1; i >= 0; --i)
            {
                int segment = segments[i];
                if (segment > 0)
                {
                    NPC other = Main.npc[segment];
                    if (other.ModNPC is ExcavatorBody)
                        DrawBody(other, spriteBatch, screenPos, i, false);
                }
            }
            Vector2 previousPos = NPC.Center;
            for (int i = 0; i < neckSegments.Count; ++i)
            {
                Vector2 segment = neckSegments[i];
                Vector2 toPrev = previousPos - segment;

                float length = toPrev.Length() + 4;
                Vector2 scale = new Vector2(1, length / neck.Height);
                spriteBatch.Draw(neck, segment - screenPos, null, Lighting.GetColor(segment.ToTileCoordinates()), toPrev.ToRotation() + MathHelper.PiOver2, neckOrigin, scale, SpriteEffects.None, 0);
                spriteBatch.Draw(neckGlow, segment - screenPos, null, Color.White, toPrev.ToRotation() + MathHelper.PiOver2, neckOrigin, scale, SpriteEffects.None, 0);

                if (i == 2 && segments[1] != -1)
                {
                    NPC child = Main.npc[segments[1]];
                    previousPos = child.Center + new Vector2(0, child.height / 2 - 3).RotatedBy(child.rotation);
                }
                else
                    previousPos = segment;
            }

            for (int i = segments.Length - 1; i >= 0; --i)
            {
                int segment = segments[i];
                if (segment >= 0)
                {
                    NPC other = Main.npc[segment];
                    if (other.ModNPC is ExcavatorBody)
                        DrawBody(other, spriteBatch, screenPos, i, true);
                }
            }
            spriteBatch.Draw(head, NPC.Center - screenPos, null, drawColor, NPC.rotation + 1.57f, origin, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            spriteBatch.Draw(headGlow, NPC.Center - screenPos, null, Color.White, NPC.rotation + 1.57f, origin, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            return false;
        }
        public void DrawDashTelegraph(SpriteBatch spriteBatch, Vector2 screenPos)
        {
            float fadeOutPercent = 1 - MathF.Min(TelegraphFadeOut / 40f, 1);
            if (fadeOutPercent <= 0 || TelegraphCounter == 0)
                return;
            float telegraphSize = TelegraphSize;
            Vector2 size1 = new(telegraphSize, 1);
            float percent2 = TelegraphCounter / 100f;
            Texture2D texture = SOTSUtils.WhitePixel;
            Vector2 origin = new(1, 1);
            Color white = Color.White;
            Color c = ExcavatorOrb.Color;
            int start = (int)(24 * percent2);
            int end = (int)(start + 24 * percent2);
            float between = end - start;
            float minColor = 0.14f;
            float maxColor = 1 - minColor;
            Vector2 position = Vector2.Lerp(new Vector2(target.Center.X, NPC.Center.Y), TelegraphLocation, percent2 * percent2);
            for(int i = -1; i <= 1; i += 2)
            {
                for (int j = start; j <= end; ++j)
                {
                    float percent = j >= start ? (MathF.Abs(j - start) / between) : 0;
                    float trippleP = percent * percent * percent;
                    percent = trippleP * maxColor + minColor;
                    Vector2 pos = position + new Vector2(0, 2 * j * i);
                    Color c2 = Color.Lerp(c, white, trippleP) * percent * fadeOutPercent * percent2;
                    spriteBatch.Draw(texture, pos - screenPos, null, c2, 0, origin, size1, SpriteEffects.None, 0);
                }
                if(i == 1)
                {
                    spriteBatch.Draw(texture, position - screenPos, null, c * minColor * percent2 * fadeOutPercent, 0, origin, new Vector2(telegraphSize, start * 2 - 1), SpriteEffects.None, 0);
                }
            }
        }
        public void DashTelegraphDust()
        {
            float fadeOutPercent = 1 - MathF.Min(TelegraphFadeOut / 40f, 1);
            if (fadeOutPercent <= 0 || TelegraphCounter == 0)
                return;
            float telegraphSize = TelegraphSize;
            float percent2 = TelegraphCounter / 100f;
            if (percent2 <= 0)
                return;
            Vector2 position = Vector2.Lerp(new Vector2(target.Center.X, NPC.Center.Y), TelegraphLocation, percent2 * percent2);
            int start = (int)(24 * percent2);
            int end = (int)(start + 24 * percent2);
            for(int i = 0; i < (float)(7 * fadeOutPercent); ++i)
            {
                float m = i <= 1 ? 0 : Main.rand.NextFloat(1);
                for (int j = -1; j <= 1; j += 2)
                {
                    Vector2 pos = position + new Vector2(Main.rand.NextFloat(-telegraphSize, telegraphSize), 2 * end * j * m);
                    PixelDust.Spawn(pos, 0, 0, Main.rand.NextVector2Circular(1, 1) + new Vector2(Main.rand.NextFloat(-0.2f, 0.2f), 2 * j), Color.Lerp(ExcavatorOrb.Color, Color.White, percent2 * Main.rand.NextFloat(1)) * fadeOutPercent, 5);
                }
            }
            Vector3 lColor = ExcavatorOrb.Color.ToVector3() * 0.2f * percent2 * fadeOutPercent;
            for (float pY = 0; pY <= 1; pY += 0.2f)
            {
                for (float p = 0; p <= 1; p += 0.005f)
                {
                    Vector2 position2 = new(MathHelper.Lerp(position.X - telegraphSize, position.X + telegraphSize, p), MathHelper.Lerp(position.Y - end * 2, position.Y + end * 2, pY));
                    Lighting.AddLight(position2, lColor);
                }
            }
        }
        public void UpdateNeckSegments()
        {
            NPC.Center += NPC.velocity;
            Vector3 glow = new Vector3(1f, .55f, .05f);
            Lighting.AddLight(NPC.Center, glow * 0.3f);
            float neckHeight = 26;
            while (neckSegments.Count < 8)
            {
                neckSegments.Add(NPC.Center);
            }
            Vector2 previous2 = NPC.Center;
            Vector2 previous = NPC.Center;
            float lerpAmt = 0.64f;
            float tailAngleCutoff = MathHelper.ToRadians(8);
            for (int i = 0; i < neckSegments.Count; ++i)
            {
                Vector2 toBody = previous - neckSegments[i];
                float length = toBody.Length();
                if(length > neckHeight)
                {
                    if(i > 2) //tail segments
                    {
                        Vector2 prevToPrev = previous2 - previous;
                        float prevR = prevToPrev.ToRotation();
                        float curR = toBody.ToRotation();
                        float diffR = MathHelper.WrapAngle(prevR - curR);
                        float targetR = curR;
                        if(Math.Abs(diffR) > tailAngleCutoff)
                        {
                            targetR = SOTSUtils.AngularLerp(targetR, prevR - MathF.Sign(diffR) * tailAngleCutoff, 0.6f);
                        }
                        //if (i == 4)
                        //{
                        //    Main.NewText(diffR * 180f / MathF.PI);
                        //}
                        Vector2 targetPosition = previous - new Vector2(neckHeight * 0.99f, 0).RotatedBy(targetR);
                        neckSegments[i] = Vector2.Lerp(neckSegments[i], targetPosition, lerpAmt);
                    }
                    else
                    {
                        Vector2 targetPosition = neckSegments[i] + toBody.SNormalize() * (length - neckHeight);
                        neckSegments[i] = Vector2.Lerp(neckSegments[i], targetPosition, lerpAmt);
                    }
                    //neckSegments[i] = targetPosition;
                }
                previous2 = previous;
                previous = neckSegments[i];
                if (i == 2 && segments[1] != -1)
                {
                    NPC child = Main.npc[segments[1]];
                    previous = child.Center + new Vector2(0, child.height / 2 - 3).RotatedBy(child.rotation);
                }
                if (i == 2)
                {
                    neckHeight = 16;
                    lerpAmt = 0.8f;
                }
                else if (i > 2)
                {
                    neckHeight = 31;
                    neckHeight *= MathF.Pow(0.94f, i - 2);
                }
                Lighting.AddLight(neckSegments[i], glow * 0.2f);
            }
            for (int j = 0; j < segments.Length; j++)
            {
                int segment = segments[j];
                if (segment >= 0)
                {
                    NPC other = Main.npc[segment];
                    if (other.ModNPC is ExcavatorTail)
                    {
                        if(j == segments.Length - 3)
                            other.dontTakeDamage = false;
                        UpdateChildPos(other, 1 + j);
                    }
                    else if (other.ModNPC is ExcavatorBody2)
                    {
                        UpdateChildPos(other, -1);
                    }
                    else if (other.ModNPC is ExcavatorBody)
                    {
                        UpdateChildPos(other, 2);
                    }
                }
            }
            ProcessRecoil();
            NPC.Center -= NPC.velocity;
        }
        public void ProcessRecoil()
        {
            for (int i = 0; i < neckSegments.Count; i++)
                neckSegments[i] += recoil;
            for (int i = 0; i < segments.Length; i++)
            {
                int segment = segments[i];
                if (segment > 0)
                {
                    NPC other = Main.npc[segment];
                    other.position += recoil * 1f;
                }
            }
            NPC.position += recoil * 1.1f;
            recoil *= 0.94f;
        }
        public void UpdateChildPos(NPC child, int neckPos)
        {
            if(neckPos < 0 && segments[0] != -1)
            {
                NPC other = Main.npc[segments[0]];
                Vector2 nextSegment = other.Center + new Vector2(0, other.height - 9).RotatedBy(other.rotation);
                child.rotation = SOTSUtils.AngularLerp(child.rotation, (other.Center - child.Center).ToRotation() + 1.57f, 0.5f);
                child.velocity = Vector2.Zero;
                child.Center = nextSegment;
            }
            else
            {
                Vector2 prev = neckSegments[neckPos - 1];
                if (neckPos == 3 && segments[1] != -1)
                {
                    NPC other = Main.npc[segments[1]];
                    prev = other.Center + new Vector2(0, other.height / 2 - 3).RotatedBy(other.rotation);
                }
                Vector2 nextSegment = neckSegments[neckPos];
                Vector2 toPrev = prev - nextSegment;
                child.rotation = toPrev.ToRotation() + 1.57f;
                Vector2 rotationOrigin = new Vector2(0, neckPos == 2 ? (-NPC.height / 2) : (-child.height / 2 + 19)).RotatedBy(child.rotation);
                child.velocity = Vector2.Zero;
                child.Center = nextSegment - rotationOrigin;
                if (neckPos > 2)
                    child.Center += NPC.velocity * 0.6f;
            }
        }
        public void DrawBody(NPC other, SpriteBatch spriteBatch, Vector2 screenPos, int i, bool top)
        {
            Color drawColor = Lighting.GetColor(other.Center.ToTileCoordinates(), Color.White);
            string dir = "SOTS/NPCs/Boss/Excavator/";
            Texture2D body = null;
            Texture2D bodyTop = null;
            Texture2D bodyGlow = null;
            bool arms = false;
            bool legs = false;
            float scale = 1;
            if (i == 0)
            {
                body = ModContent.Request<Texture2D>($"{dir}body").Value;
                bodyTop = ModContent.Request<Texture2D>($"{dir}bodytop").Value;
                bodyGlow = ModContent.Request<Texture2D>($"{dir}bodyGlow").Value;
                arms = true;
            }
            else if (i == 1)
            {
                bodyTop = ModContent.Request<Texture2D>($"{dir}body2").Value;
                bodyGlow = ModContent.Request<Texture2D>($"{dir}body2Glow").Value;
                legs = true;
            }
            else
            {
                bodyTop = i == segments.Length - 1 ? 
                    ModContent.Request<Texture2D>($"{dir}tailDrill").Value :
                    i % 2 == 0 ? ModContent.Request<Texture2D>($"{dir}tail").Value :
                    ModContent.Request<Texture2D>($"{dir}tail2").Value;
                //bodyTop = ModContent.Request<Texture2D>($"{dir}tailTop").Value;
                bodyGlow = i == segments.Length - 1 ? ModContent.Request<Texture2D>($"{dir}tailDrillGlow").Value : ModContent.Request<Texture2D>(i % 2 == 0 ? $"{dir}tailGlow" : $"{dir}tail2Glow").Value;
                scale *= MathF.Pow(0.94f, i - 2);
            }
            Vector2 bodyOrigin = bodyTop.Size() / 2;
            if(i == segments.Length - 1)
            {
                bodyOrigin = new Vector2(bodyTop.Width / 2, bodyTop.Height / 2 - 18);
            }
            if(top)
            {
                if (other.ModNPC is ExcavatorBody)
                {
                    if(bodyTop != null)
                        spriteBatch.Draw(bodyTop, other.Center - screenPos, null, drawColor, other.rotation, bodyOrigin, other.scale * scale, other.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
                    if(bodyGlow != null)
                        spriteBatch.Draw(bodyGlow, other.Center - screenPos, null, Color.White, other.rotation, bodyOrigin, other.scale * scale, other.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
                }
            }
            else
            {
                if(arms)
                {
                    DrawLeg(other, spriteBatch, screenPos, 3, -1);
                    DrawLeg(other, spriteBatch, screenPos, -3, -1);
                    foreach(ExcavatorArm arm in this.arms)
                        arm.DoArmIK(other, spriteBatch, screenPos, arm.dir, true);
                }
                if (legs) {
                    for (int k = -1; k <= 1; k += 2)
                        for (int j = 1; j <= 2; ++j)
                            DrawLeg(other, spriteBatch, screenPos, j * k);
                }
                if(body != null)
                    spriteBatch.Draw(body, other.Center - screenPos, null, drawColor, other.rotation, bodyOrigin, other.scale * scale, other.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            }
        }
        public void DrawLeg(NPC other, SpriteBatch spriteBatch, Vector2 screenPos, int i, int dir = 1)
        {
            int j = SOTSUtils.SignNoZero(i);
            i = Math.Abs(i) - 1;
            float r = WalkCounter * 1.2f;
            //float legSwayAmt = i == 0 ? 22 : 18;
            //float legMoveSin = MathF.Sin(MathHelper.ToRadians(r + i * 120 + (j == -1 ? 180 : 0)));
            //legMoveSin = (legMoveSin * 0.2f + 0.8f * MathF.Sign(legMoveSin) * MathF.Sqrt(MathF.Abs(legMoveSin))) * legSwayAmt * j;
            int separation = i == 2 ?  12 : i * 32;
            //float scale = i == 2 ? 0.9f : 0.8f;
            //float rotation = i == 2 ? -12.5f : i == 0 ? 5 : -6.25f;
            int outward = i == 1 ? 26 : i == 2 ? 48 : 22;
            //arm = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/leg").Value;
            //Vector2 legOrig = new Vector2(73, 15);
            //Vector2 revLegOrig = new Vector2(arm.Width - legOrig.X, legOrig.Y);
            float legRot = other.rotation;
            Vector2 armPosition = new Vector2(outward * j, 18 - separation);
            armPosition = armPosition.RotatedBy(legRot) + other.Center;
            //legRot += MathHelper.ToRadians(legMoveSin);
            //spriteBatch.Draw(arm, armPosition - screenPos, null, drawColor, legRot + MathHelper.ToRadians(rotation * j), j == -1 ? legOrig : revLegOrig, other.scale * scale, j == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            float A = 72; //size of hand
            float B = 36; //size of arm
            Vector2 circular = new Vector2(50 * j, 0).RotatedBy(MathHelper.ToRadians(r + i * 120 + (j == dir ? 180 : 0)) * j * dir);
            Vector2 offset = new Vector2(82 * j, 0);
            if (i == 0) {
                offset = new Vector2(70 * j, 32);
                circular.X *= 0.5f;
                circular *= 0.8f;
            }
            if(i == 1)
            {
                circular.X *= 0.25f;
            }
            if (i == 2)
            {
                offset = new Vector2(70 * j, -32);
                circular.X *= 0.5f;
                circular *= 0.8f;
            }
            Vector2 target = armPosition + new Vector2(offset.X + circular.X, offset.Y + circular.Y).RotatedBy(other.rotation);
            Vector2 end = target;
            Vector2 start = armPosition;
            ExcavatorArm.DoArmJoint(ref start, ref end, A, B, j * dir, out float endArmRot, out float endHandRot);
            //spriteBatch.Draw(SOTSUtils.WhitePixel, end - screenPos, null, drawColor, 0, Vector2.One, other.scale * 4, SpriteEffects.None, 0);
            //spriteBatch.Draw(SOTSUtils.WhitePixel, end - screenPos, null, drawColor, endHandRot, new Vector2(0, 1), new Vector2(A * 0.5f, 2), SpriteEffects.None, 0);
            //spriteBatch.Draw(SOTSUtils.WhitePixel, target - screenPos, null, Color.White, 0, Vector2.One, other.scale * 4, SpriteEffects.None, 0);
            //spriteBatch.Draw(SOTSUtils.WhitePixel, start - screenPos, null, drawColor, 0, Vector2.One, other.scale * 4, SpriteEffects.None, 0);
            //spriteBatch.Draw(SOTSUtils.WhitePixel, start - screenPos, null, drawColor, endArmRot, new Vector2(0, 1), new Vector2(B * 0.5f, 2), SpriteEffects.None, 0);

            Texture2D hand = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/handNoWeapon").Value;
            Texture2D arm = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/arm").Value;
            Vector2 armOrigin = new Vector2(50, 14);
            Vector2 revArmOrigin = new(arm.Width - armOrigin.X, armOrigin.Y);
            Vector2 handOrigin = new(hand.Width / 2, hand.Height);
            Color drawColor = Lighting.GetColor(start.ToTileCoordinates(), new Color(210, 210, 210));
            spriteBatch.Draw(hand, end - screenPos, null, drawColor, endHandRot + MathHelper.PiOver2, handOrigin, other.scale, j == -dir ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(arm, start - screenPos, null, drawColor, endArmRot + (j == -dir ? MathF.PI : 0), j == -dir ? armOrigin : revArmOrigin, other.scale, j == -dir ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }
        public override string Texture => "SOTS/NPCs/Boss/Excavator/head";
        public override void SetStaticDefaults()
        {
            NPCID.Sets.MustAlwaysDraw[Type] = true;
            //NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
            //NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            //{
            //    CustomTexturePath = "SOTS/NPCs/Constructs/EarthenConstructHead",
            //    PortraitScale = 1.1f
            //};
            //NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            //NPCID.Sets.MPAllowedEnemies[Type] = true;
            //NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            //NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
            //NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
        }
        public override void SetDefaults()
        {
            NPC.lifeMax = 14000;
            NPC.damage = 42;
            NPC.defense = 20;
            NPC.knockBackResist = 0f;
            NPC.width = 68;
            NPC.height = 68;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.value = Item.buyPrice();
            NPC.npcSlots = 3f;
            NPC.behindTiles = true;
            NPC.aiStyle = -1;
            NPC.boss = true;
            neckSegments = [];
            arms = GenArms();
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * balance * bossAdjustment * 0.75f); 
            NPC.damage = (int)(NPC.damage * 0.75f);
        }
        private int[] segments = [-1, -1, -1, -1, -1, -1, -1];
        private int DespawnCounter = 0;
        public Vector2 recoil = Vector2.Zero;
        private Player target => Main.player[NPC.target];
        public bool DespawnCheck()
        {
            if (target.dead || Vector2.Distance(target.Center, NPC.Center) > 6400 || !target.SOTSPlayer().AbandonedVillageBiome)
            {
                DespawnCounter++;
            }
            else if(DespawnCounter > 0)
            {
                DespawnCounter--;
            }
            if (DespawnCounter >= 600)
            {
                NPC.active = false;
                return true;
            }
            else
            {
                NPC.DiscourageDespawn(1000);
            }
            return false;
        }
        private void WormSetup()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (AIPhase == 0)
                {
                    NPC.realLife = NPC.whoAmI;
                    int latestNPC = NPC.whoAmI;
                    int WormLength = segments.Length;
                    for (int i = 0; i < WormLength; i++)
                    {
                        int type = i == 0 ? ModContent.NPCType<ExcavatorBody>() : 
                            i == 1 ? ModContent.NPCType<ExcavatorBody2>() : 
                            i == segments.Length - 1 ? ModContent.NPCType<ExcavatorDrillTail>() 
                            : ModContent.NPCType<ExcavatorTail>();
                        latestNPC = NPC.NewNPC(NPC.GetSource_Misc("SOTS:WormEnemy"), (int)NPC.Center.X, (int)NPC.Center.Y, type, NPC.whoAmI, i * 180f, latestNPC);
                        Main.npc[latestNPC].realLife = NPC.whoAmI;
                        Main.npc[latestNPC].ai[3] = NPC.whoAmI;
                        Main.npc[latestNPC].ai[2] = i + 1;
                        segments[i] = latestNPC;
                    }
                    AIPhase = 1;
                }
                NPC.netUpdate = true;
            }
        }
        private void IdleMoveStyle()
        {
            float prevSpeed = NPC.velocity.Length();
            Vector2 toPlayer = target.Center - NPC.Center;
            if(MoveStyle == -2) //Debug move style = right click to move to player
            {
                NPC.velocity *= 0.95f;
                if (Main.mouseRight)
                {
                    Vector2 toMouse = Main.MouseWorld - NPC.Center;
                    NPC.velocity += toMouse * 0.001f;
                }
            }
            int dir = (int)MoveStyle % 2;
            if (MoveStyle == 4)
            {
                Vector2 targetPosition = ClosestTelegraphEndLocation();
                toPlayer = targetPosition - NPC.Center;
                float dist = toPlayer.Length();
                NPC.velocity += toPlayer.SNormalize() * (0.55f + dist * 0.0025f);
                NPC.velocity *= 0.95f;
            }
            if (MoveStyle == 0 || MoveStyle == 1)
            {
                float dist = toPlayer.Length();
                if(dist > 240)
                {
                    NPC.velocity += toPlayer.SNormalize().RotatedBy(MathHelper.ToRadians(-30) * dir) * (0.6f + dist * 0.001f);
                    NPC.velocity *= 0.925f;
                }
                else
                {
                    NPC.velocity += toPlayer.SNormalize().RotatedBy(MathHelper.ToRadians(-31) * dir) * (0.2f + dist * 0.001f);
                    NPC.velocity *= 0.9f;
                }
            }
            if (MoveStyle == 2)
            {
                float dist = toPlayer.Length();
                if (dist > 200)
                    NPC.velocity += toPlayer.SNormalize() * (0.25f + dist * 0.001f);
                else
                    NPC.velocity += toPlayer.SNormalize() * (0.125f + dist * 0.0004f);
                NPC.velocity *= 0.9f;
            }
            if (MoveStyle == 3)
            {
                float dist = toPlayer.Length();
                if (dist > 320)
                {
                    Vector2 targetPos = new Vector2(target.Center.X - MathF.Sign(toPlayer.X) * 320, target.Center.Y);
                    toPlayer = targetPos - NPC.Center;
                    NPC.velocity += toPlayer.SNormalize() * (0.25f + dist * 0.001f);
                }
                else
                {
                    NPC.velocity += toPlayer.SNormalize() * (0.035f + dist * 0.00035f);
                }
                NPC.velocity *= 0.9f;
            }
            //Main.NewText(MoveStyle);
        }
        public void SwitchArm(int type, int index = -1)
        {
            if(index == -1)
            {
                SwitchArm(type, 0);
                SwitchArm(type, 1);
            }
            else
            {
                arms[index].SwitchArm(type);
            }
        }
        public void TargetArm(Vector2 area, int index = -1)
        {
            if (index == -1)
            {
                TargetArm(area, 0);
                TargetArm(area, 1);
            }
            else
            {
                arms[index].TargetArm(area);
            }
        }
        public bool ArmsFinishedSwitching()
        {
            foreach(ExcavatorArm arm in arms)
                if (arm.NextArmType != arm.ArmType)
                    return false;
            return true;
        }
        public bool SwingingSawArm()
        {
            foreach (ExcavatorArm arm in arms)
                if (arm.sawSwingingDown)
                    return true;
            return false;
        }
        public void LaunchRocket(int dir, Vector2 targetPosition = default, bool sound = true, float precisionMult = 1.0f)
        {
            NPC body = Main.npc[segments[0]];
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 missileSiloSpots = body.Center + new Vector2(-30 * dir, 23).RotatedBy(body.rotation);
                Vector2 oppositeVelo = new Vector2(0, 0.5f).RotatedBy(body.rotation);
                if(targetPosition == default)
                    targetPosition = target.Center + Main.rand.NextVector2Circular(320, 320) * precisionMult + target.velocity * 15f;
                Projectile.NewProjectile(NPC.GetSource_FromThis(), missileSiloSpots, Main.rand.NextVector2CircularEdge(precisionMult, precisionMult) + oppositeVelo, ModContent.ProjectileType<ExcavatorRocket>(), NPC.GetBaseDamage() / 2, 1, Main.myPlayer, targetPosition.X, targetPosition.Y);
                //PixelDust.Spawn(body.Center + new Vector2(-30 * dir, 23).RotatedBy(body.rotation), 0, 0, Vector2.Zero, Color.White, 5).scale = 1;
            }
            if(sound)
                SOTSUtils.PlaySound(SoundID.Item61, body.Center, 1.1f, 0.1f);
        }
        public override bool PreAI()
        {
            DashTelegraphDust();
            foreach (ExcavatorArm arm in arms)
                arm.PreUpdate();
            UpdateNeckSegments();
            NPC.TargetClosest(true);
            Vector2 toPlayer = target.Center - NPC.Center;
            Vector2 norm = toPlayer.SNormalize();
            if (DespawnCheck())
                return false;
            WormSetup();
            IdleMoveStyle();
            AIPhase = DrillDashPhase;
            if (segments.Length > 0)
            {
                int segment = segments[0];
                if (segment >= 0)
                {
                    NPC other = Main.npc[segment];
                    if (other.ModNPC is ExcavatorBody)
                        foreach (ExcavatorArm arm in this.arms)
                            arm.DoArmIK(other, null, Main.screenPosition, arm.dir, false);
                }
            }
            if(AIPhase == DrillDashPhase)
            {
                MoveStyle = 3;
                TargetArm(NPC.Center, 2);
                TargetArm(NPC.Center, 3);
            }
            if (AIPhase == EnergyBallPhase)
            {
                AI1++;
                if (AI2 == -1)
                {
                    recoil = -toPlayer.SNormalize() * 8;
                    NPC.velocity *= 0.1f;
                    AI2 = 0;
                }
                if (AI1 > 120)
                {
                    MoveStyle = 2;
                    if (AI1 >= 150)
                    {
                        if (AI1 == 150)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Main.rand.NextVector2Circular(4, 4), ModContent.ProjectileType<ExcavatorOrb>(), NPC.GetBaseDamage() / 2, 1, Main.myPlayer, NPC.whoAmI);
                            if (InSecondPhase && AI3 < 1)
                            {
                                AI1 = -30;
                                AI3 = 1;
                            }
                        }
                    }
                    if (AI1 > 450)
                    {
                        SwapPhase(LaserPhase);
                    }
                }
            }
            if(AIPhase == LaserPhase)
            {
                MoveStyle = 2;
                int totalShots = 20;
                if (AI2 >= totalShots)
                {
                    if(InSecondPhase)
                        SwapPhase(SawPhase);
                    else
                        SwapPhase(RocketPhase);
                    SwitchArm(2);
                }
                else
                    SwitchArm(0);
                if (ArmsFinishedSwitching())
                {
                    AI1++;
                    int fireRate = Main.expertMode ? 14 : 16;
                    bool nextShotIsBig = AI4 >= 3;
                    if (nextShotIsBig)
                    {
                        fireRate = Main.expertMode ? 50 : 60;
                    }
                    else if (InSecondPhase)
                    {
                        fireRate -= 5;
                        NPC.velocity *= 0.995f;
                    }
                    if (AI1 >= 97)
                        TargetArm(target.Center);
                    if (AI1 >= 104)
                    {
                        AI1 -= fireRate;
                        if(!nextShotIsBig)
                        {
                            float speed = 3.5f;
                            if (InSecondPhase)
                            {
                                AI4++;
                                speed = 6f;
                            }
                            for (int i = 0; i < 2; ++i)
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), arms[i].handPos, arms[i].handNorm * speed + NPC.velocity, ModContent.ProjectileType<ExcavatorBolt>(), NPC.GetBaseDamage() / 2, 1, Main.myPlayer);
                                SOTSUtils.PlaySound(SoundID.Item91, arms[i].handPos, 1.0f, -0.4f);
                            }
                        }
                        else
                        {
                            AI4 = 0;
                            for (int i = 0; i < 2; ++i)
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), arms[i].handPos, arms[i].handNorm * 3f + NPC.velocity, ModContent.ProjectileType<ExcavatorBoltBig>(), (int)(NPC.GetBaseDamage() / 2 * 1.5f), 1, Main.myPlayer);
                                SOTSUtils.PlaySound(SoundID.Item91, arms[i].handPos, 1.0f, -0.7f);
                            }
                            recoil -= toPlayer.SNormalize() * 8;
                        }
                        AI2++;
                    }
                }
            }
            if(AIPhase == RocketPhase)
            {
                MoveStyle = 3;
                SwitchArm(2);
                if (ArmsFinishedSwitching())
                    ++AI1;
                int fireCooldown = Main.expertMode ? 11 : 13;
                int totalRockets = Main.expertMode ? 30 : 24;
                if(InSecondPhase)
                {
                    totalRockets = 3;
                    fireCooldown = 20;
                }
                if (AI1 > 70)
                {
                    int dir = (int)AI2 % 2 * 2 - 1;
                    if (!InSecondPhase)
                    {
                        LaunchRocket(dir);
                    }
                    else
                    {
                        for(int i = 0; i < 5; ++i)
                        {
                            LaunchRocket(dir, precisionMult: 0.75f);
                        }
                    }
                    AI1 -= fireCooldown;
                    AI2++;
                }
                if(AI2 > totalRockets)
                {
                    if(InSecondPhase)
                        SwapPhase((int)NextAIPhase);
                    else
                        SwapPhase(SawPhase);
                }
            }
            if(AIPhase == SawPhase)
            {
                int total = InSecondPhase ? 6 : 8;
                //Main.NewText(AI3);
                if (AI3 > total)
                {
                    SwitchArm(2);
                    if (ArmsFinishedSwitching())
                    {
                        SwapPhase(InSecondPhase ? SecondPhaseTransition : EnergyBallPhase);
                    }
                }
                else
                {
                    SwitchArm(1);
                }
                if (ArmsFinishedSwitching() || AI1 > 0)
                {
                    AI1++;
                    if (AI1 > 100)
                    {
                        for (int i = 0; i < 2; ++i)
                        {
                            arms[i].SawUpdate();
                            if (arms[i].sawBladeTimer <= 30 && arms[i].sawBladeTimer > 0 && arms[i].sawBladeTimer % 10 == 0 && AI3 <= total && InSecondPhase)
                            {
                                LaunchRocket(i * 2 - 1, precisionMult: 0.4f);
                            }
                        }
                    }
                }
                MoveStyle = 2;
                bool armSwinging = SwingingSawArm();
                if (armSwinging)
                {
                    if(AI2 == 0)
                    {
                        AI3++;
                        AI2++;
                        SOTSUtils.PlaySound(SoundID.Item71, NPC.Center, 1, -0.3f);
                    }
                    recoil *= 0.95f;
                    NPC.velocity *= 0.95f;
                    recoil += norm * 1.2f;
                    NPC.velocity += norm * 0.4f + toPlayer * 0.0008f;
                    for (int i = 0; i < 3; i++)
                    {
                        NPC other = i == 2 ? NPC : Main.npc[segments[i]];
                        Dust d= PixelDust.Spawn(other.position, other.width, other.height, -recoil * Main.rand.NextFloat(1) - NPC.velocity * Main.rand.NextFloat() + Main.rand.NextVector2Circular(1, 1), ExcavatorOrb.Color, 3);
                        d.scale = Main.rand.NextFloat(1, 2);
                    }
                }
                else
                {
                    NPC.velocity *= 0.875f;
                    AI2 = 0;
                }
            }
            if (AIPhase == SecondPhaseTransition)
            {
                if (AI1++ <= 10)
                {
                    TelegraphCounter = TelegraphFadeOut = 0;
                    TelegraphLocation = target.Center;
                }
                if (AI1 > 1200)
                {
                    AI1 = 0;
                    SwapPhase(EnergyBallPhase);
                }
                bool isDrill = AI3 % 2 == 0;
                if (AI1 > 20)
                {
                    int dashTime = 110;
                    if (AI1 > dashTime)
                    {
                        if (AI1 > dashTime + 50)
                            TelegraphFadeOut++;
                        Vector2 otherSide = AI2 == -1 ? leftTelegraph : rightTelegraph;
                        MoveStyle = -1;
                        NPC.velocity.X += AI2 * 1.9f;
                        NPC.velocity.Y += MathF.Sign(TelegraphLocation.Y - NPC.Center.Y) * 0.4f;
                        NPC.velocity += (otherSide - NPC.Center).SNormalize() * 0.7f;
                        NPC.velocity *= 0.925f;
                        if (Vector2.Distance(otherSide, NPC.Center) < 140)
                        {
                            AI1 = 0;
                            if(AI3 >= 1)
                            {
                                AI1 = 1200;
                                return true;
                            }
                            AI3++;
                        }
                        if (AI1 > dashTime + 50)
                        {
                            if(isDrill)
                            {
                                if (AI1 % 15 == 0)
                                {
                                    SOTSUtils.PlaySound(SoundID.Item23, NPC.Center, 2f, 0.5f, 0.05f);
                                }
                                int dropRate = Main.expertMode ? 8 : 9;
                                if (AI1 % dropRate == 0)
                                {
                                    Vector2 spawnLocation = NPC.Center + new Vector2(0, -120);
                                    Point p = spawnLocation.ToTileCoordinates();
                                    CollapseBlock.Spawn(NPC.GetSource_FromThis(), p.X, p.Y, NPC.GetBaseDamage() / 2);
                                    SOTSUtils.PlaySound(SoundID.Tink, NPC.Center, 0.8f, -0.4f, 0.05f);
                                }
                            }
                            else
                            {
                                int dropRate = Main.expertMode ? 5 : 6;
                                if (AI1 % dropRate == 0)
                                {
                                    int dir = (int)AI4 % 2 * 2 - 1;
                                    Vector2 target = NPC.Center;
                                    Point p;
                                    for (int i = 0; i < 40; ++i)
                                    {
                                        target += new Vector2(0, 16 * dir);
                                        p = target.ToTileCoordinates();
                                        if (SOTSWorldgenHelper.TrueTileSolid(p.X, p.Y, false))
                                            break;
                                    }
                                    LaunchRocket(dir, target, dir == 1);
                                    ++AI4;
                                }
                            }
                        }
                    }
                    else
                    {
                        MoveStyle = 4;
                        Vector2 toFarthest = FarthestTelegraphEndLocation() - NPC.Center;
                        AI2 = MathF.Sign(toFarthest.X);
                    }
                }
                else
                {
                    MoveStyle = 2;
                }
                if (TelegraphCounter < 100)
                {
                    if(isDrill)
                    {
                        Point p = (target.Center + new Vector2(0, -32)).ToTileCoordinates();
                        for (int j = 0; j < 65; ++j)
                        {
                            --p.Y;
                            if (SOTSWorldgenHelper.TrueTileSolid(p.X, p.Y, true) && SOTSWorldgenHelper.TrueTileSolid(p.X, p.Y - 1, true) && SOTSWorldgenHelper.TrueTileSolid(p.X, p.Y - 2, true))
                            {
                                TelegraphLocation = Vector2.Lerp(TelegraphLocation, p.ToWorldCoordinates() + new Vector2(0, 96), 0.2f);
                                break;
                            }
                        }
                    }
                    else
                    {
                        TelegraphLocation = Vector2.Lerp(TelegraphLocation, target.Center, 0.3f);
                    }
                    ++TelegraphCounter;
                }
                NPC.velocity *= 0.95f;
            }
            else
            {
                TelegraphCounter = TelegraphFadeOut = 0;
                TelegraphLocation = target.Center;
            }    
            /*
            one where excavator will use the laser and saw at the same time
            and it would also have a desparation phase with an unstable "Gula" spirit which is an Evil+Earthen spirit that explodes violently
            */
            return false;
        }
        public override void PostAI()
        {
            if (!NPC.active)
                return;
            Vector2 toPlayer = target.Center - NPC.Center;
            if(segments.Length > 0)
                AvoidCollision();
            if (NPC.velocity.LengthSquared() > 0.1f)
            {
                float movementTargetR = NPC.velocity.ToRotation();
                float distToPlayer = toPlayer.Length();
                float playerFacingTargetR = toPlayer.ToRotation();
                float lerpAmt = 0.05f * MathF.Max(1 - distToPlayer / 480f, 0);
                NPC.rotation = SOTSUtils.AngularLerp(movementTargetR, playerFacingTargetR, lerpAmt);
            }
            Vector2 trueVelo = NPC.position - NPC.oldPosition;
            float speed = trueVelo.Length();
            if (speed < 1000)
            {
                WalkCounter += 2 * MathF.Sqrt(speed);
            }
            foreach (ExcavatorArm arm in arms)
                arm.PostUpdate();
        }
        public void AvoidCollision()
        {
            NPC body = Main.npc[segments[0]];
            //Rectangle me = NPC.Hitbox;
            //Rectangle other = body.Hitbox;
            Vector2 toPlayer = target.Center - body.Center;
            Vector2 toBody = body.Center - NPC.Center;
            float len = 64;
            float dist = toBody.Length();
            if (dist < len)
            {
                float distNeeded = len - toBody.Length();
                NPC.Center -= toBody.SNormalize() * distNeeded;
            }
            if(NPC.velocity.LengthSquared() > 0.01f)
            {
                float nextRotation = NPC.velocity.ToRotation();
                float bodyRotation = body.rotation - MathHelper.PiOver2;
                float rotationDifference = bodyRotation - nextRotation;
                rotationDifference = MathHelper.WrapAngle(rotationDifference);
                float maxTurnAngle = MathHelper.ToRadians(64);
                float speed = 1.0f;
                if (MathF.Abs(rotationDifference) > maxTurnAngle)
                {
                    rotationDifference = (MathF.Abs(rotationDifference) - maxTurnAngle) * MathF.Sign(rotationDifference);
                    float percent = MathF.Min(MathF.Abs(MathHelper.ToDegrees(rotationDifference)) / 90f, 1);
                    float playerDist = target.Distance(body.Center);
                    float playerPercent = MathF.Min(1, playerDist / 160f);
                    recoil -= new Vector2(1, 0).RotatedBy(nextRotation * 1.0f) * MathF.Sqrt(percent) * .3f * playerPercent * playerPercent;
                    speed += percent;
                }
                else
                {
                    rotationDifference = 0;
                }
                NPC.velocity = NPC.velocity.RotatedBy(rotationDifference) * speed;
            }
        }
        public void SwapPhase(int phase)
        {
            AI1 = AI2 = AI3 = AI4 = 0;
            if(NeedsToGoIntoPhase2)
            {
                if (!InSecondPhase && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for(int i = -2; i <= 2; ++i)
                    {
                        Vector2 spawn = target.Center + new Vector2(i * 800, 540);
                        NPC.NewNPCDirect(NPC.GetSource_FromAI(), spawn, ModContent.NPCType<EarthenGizmo>());
                    }
                }
                AIPhase = SecondPhaseTransition;
                InSecondPhase = true;
            }
            else if(InSecondPhase)
            {
                if (NextAIPhase != phase && AIPhase != SecondPhaseTransition)
                {
                    NextAIPhase = phase;
                    AIPhase = RocketPhase;
                }
                else
                {
                    AIPhase = phase;
                }
            }
            else
            {
                AIPhase = phase;
            }
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f; 
            return null;
        }
        public void CreateGore(int HitDirection)
        {
            for (int k = 0; k < 20; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Lead, 2.5f * (float)HitDirection, -2.5f, 0, default(Color), 0.7f);
            }
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/EarthenConstructGore1"), 1f);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/EarthenConstructGore2"), 1f);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/EarthenConstructGore3"), 1f);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/EarthenConstructGore4"), 1f);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/EarthenConstructGore5"), 1f);
            for (int i = 0; i < 9; i++)
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Main.rand.Next(61, 64), 1f);
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
            {
                //CreateGore(hit.HitDirection);
            }
        }
        public override void OnKill()
        {
            SOTSWorld.downedExcavator = true;
            /*if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                int type = ModContent.NPCType<EarthenSpirit>();
                int j = NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Center.Y, type, 0, 0, 0);
                Main.npc[j].velocity.Y = -10f;
                Main.npc[j].netUpdate = true;

                int n = NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Center.Y, type, 0, 0, 1, j);
                Main.npc[n].velocity = new Vector2(1, -9f);
                Main.npc[n].netUpdate = true;

                n = NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Center.Y, type, 0, 0, 2, j);
                Main.npc[n].velocity = new Vector2(-1, -9f);
                Main.npc[n].netUpdate = true;

                n = NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Center.Y, type, 0, 0, 3, j);
                Main.npc[n].velocity = new Vector2(2, -8f);
                Main.npc[n].netUpdate = true;

                n = NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Center.Y, type, 0, 0, 4, j);
                Main.npc[n].velocity = new Vector2(-2, -8f);
                Main.npc[n].netUpdate = true;
            }*/
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FragmentOfEarth>(), 1, 4, 7));
        }
        public override bool ModifyCollisionData(Rectangle victimHitbox, ref int immunityCooldownSlot, ref MultipliableFloat damageMultiplier, ref Rectangle npcHitbox)
        {
            if(arms != null)
            {
                for (int i = 0; i < 4; ++i)
                {
                    int size = arms[i].isBigArm ? 48 : arms[i].ArmType == 1 ? 32 : 16;
                    Vector2 pos = arms[i].isBigArm ? arms[i].handPos - arms[i].handNorm * 30 : arms[i].handPos;
                    Rectangle r = new Rectangle((int)pos.X - size / 2, (int)pos.Y - size / 2, size, size);
                    if(r.Intersects(victimHitbox))
                    {
                        npcHitbox = victimHitbox;
                    }
                }
            }
            return false;
        }
    }
}