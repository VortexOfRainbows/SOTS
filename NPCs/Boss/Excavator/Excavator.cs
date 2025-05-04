using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.AbandonedVillage;
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
        public override void SendExtraAI(BinaryWriter writer)
        {

        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {

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
            NPC.defense = 12;
            NPC.dontTakeDamage = true;
        }
    }
    public class ExcavatorDrillTail : ExcavatorTail
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.damage = 40;
            NPC.dontTakeDamage = false;
        }
    }
    public class Excavator : ModNPC
    {
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(MoveStyle);
            for(int i = 0; i < segments.Length; ++i)
            writer.Write(segments[i]);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            MoveStyle = reader.ReadSingle();
            for (int i = 0; i < segments.Length; ++i)
              segments[i] = reader.ReadInt32();
        }
        public float AIPhase
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }
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
        public List<Vector2> neckSegments;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D head = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/head").Value;
            Texture2D headGlow = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/headGlow").Value;
            Texture2D neck = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/neck").Value;
            Texture2D neckGlow = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/neckGlow").Value;
            Vector2 origin = new Vector2(head.Width * 0.5f, head.Height * 0.5f);
            Vector2 neckOrigin = new Vector2(neck.Width * 0.5f, neck.Height);


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
                    previousPos = child.Center + new Vector2(0, child.height / 2 - 20).RotatedBy(child.rotation);
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
        public void UpdateNeckSegments()
        {
            Vector3 glow = new Vector3(1f, .55f, .05f);
            Lighting.AddLight(NPC.Center, glow * 0.3f);
            float neckHeight = 26;
            while (neckSegments.Count < 9)
            {
                neckSegments.Add(NPC.Center);
            }
            Vector2 previous = NPC.Center;
            for(int i = 0; i < neckSegments.Count; ++i)
            {
                Vector2 toBody = previous - neckSegments[i];
                float length = toBody.Length();
                if(length > neckHeight)
                {
                    Vector2 targetPosition = neckSegments[i] + toBody.SNormalize() * (length - neckHeight);
                    neckSegments[i] = Vector2.Lerp(neckSegments[i], targetPosition, 0.64f);
                }
                previous = neckSegments[i];
                if (i == 2 && segments[1] != -1)
                {
                    NPC child = Main.npc[segments[1]];
                    previous = child.Center + new Vector2(0, child.height / 2 - 20).RotatedBy(child.rotation);
                }
                if (i == 2)
                {
                    neckHeight = 16;
                }
                else if (i > 2)
                    neckHeight *= 0.94f;
                Lighting.AddLight(neckSegments[i], glow * 0.2f);
            }

            for (int i = 0; i < segments.Length; i++)
            {
                int segment = segments[i];
                if (segment > 0)
                {
                    NPC other = Main.npc[segment];
                    if (other.ModNPC is ExcavatorTail)
                    {
                        UpdateChildPos(other, 1 + i);
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
                    prev = other.Center + new Vector2(0, other.height / 2 - 20).RotatedBy(other.rotation);
                }
                Vector2 nextSegment = neckSegments[neckPos];
                Vector2 toPrev = prev - nextSegment;
                child.rotation = toPrev.ToRotation() + 1.57f;
                Vector2 rotationOrigin = new Vector2(0, neckPos == 2 ? (-NPC.height / 2) : (-child.height / 2 + 19)).RotatedBy(child.rotation);
                child.velocity = Vector2.Zero;
                child.Center = nextSegment - rotationOrigin;
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
                body = i == segments.Length - 1 ? ModContent.Request<Texture2D>($"{dir}tailDrill").Value : ModContent.Request<Texture2D>($"{dir}tail").Value;
                bodyTop = ModContent.Request<Texture2D>($"{dir}tailTop").Value;
                //bodyGlow = ModContent.Request<Texture2D>($"{dir}tailGlow").Value;
                scale *= MathF.Pow(0.94f, i - 2);
            }
            Vector2 bodyOrigin = bodyTop.Size() / 2;
            if(top)
            {
                if (other.ModNPC is ExcavatorBody)
                {
                    if(bodyTop != null)
                        spriteBatch.Draw(bodyTop, other.Center - screenPos, null, drawColor, other.rotation, bodyTop.Size() / 2, other.scale * scale, other.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
                    if(bodyGlow != null)
                        spriteBatch.Draw(bodyGlow, other.Center - screenPos, null, Color.White, other.rotation, bodyTop.Size() / 2, other.scale * scale, other.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
                }
            }
            else
            {
                if(arms)
                {
                    DrawLeg(other, spriteBatch, screenPos, 3);
                    DrawLeg(other, spriteBatch, screenPos, -3);
                    for (int j = -2; j <= 2; ++j)
                    {
                        if (j != 0)
                            DrawArmIK(other, spriteBatch, screenPos, j);
                    }
                }
                if (legs)
                    for (int j = -2; j <= 2; ++j)
                        if (j != 0)
                            DrawLeg(other, spriteBatch, screenPos, j);
                if(body != null)
                    spriteBatch.Draw(body, other.Center - screenPos, null, drawColor, other.rotation, bodyOrigin, other.scale * scale, other.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            }
        }
        public static void DrawArmIK(NPC other, SpriteBatch spriteBatch, Vector2 screenPos, int dir)
        {
            bool isBigArm = MathF.Abs(dir) == 2;
            int j = SOTSUtils.SignNoZero(dir);
            Texture2D body = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/body").Value;
            Texture2D arm = ModContent.Request<Texture2D>(isBigArm ? "SOTS/NPCs/Boss/Excavator/bigArmLeft" : "SOTS/NPCs/Boss/Excavator/arm").Value;
            Texture2D hand = null;
            int armType = 2;
            if(armType == 1)
            {
                hand = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/handSaw").Value;
            }
            else if (armType == 2)
            {
                hand = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/handNoWeapon").Value;
            }
            else
            {
                hand = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/hand").Value;
            }
            Vector2 armOrigin = isBigArm ? new Vector2(95, 47): new Vector2(50, 14);
            Vector2 revArmOrigin = new Vector2(arm.Width - armOrigin.X, armOrigin.Y);
            Vector2 handOrigin = new Vector2(hand.Width / 2, hand.Height);
            float armRotation = other.rotation;
            Vector2 armPosition = isBigArm ? new Vector2((-body.Width / 2 + 13) * j, body.Height / 2 - 31) : new Vector2((-body.Width / 2 + 4) * j, -body.Height / 2 + 16);
            armPosition = armPosition.RotatedBy(armRotation) + other.Center;
            Color drawColor = Lighting.GetColor(armPosition.ToTileCoordinates(), Color.White);

            float r = other.ai[0] * j + (isBigArm ? (j == -2 ? 45 : 135) : (j * 45));
            float outwardSize = (isBigArm ? 72 : 38) - 16 * MathF.Sin(MathHelper.ToRadians(r + 90 * j));
            Vector2 targetHandPos = new Vector2(-(body.Width / 2 + outwardSize) * j, isBigArm ? -30 : -100).RotatedBy(armRotation);
            targetHandPos = targetHandPos + other.Center;
            Vector2 circular = new Vector2(isBigArm ? 32 : 20, 0).RotatedBy(MathHelper.ToRadians(r));
            circular.X *= 0.25f;
            circular = circular.RotatedBy(armRotation);
            targetHandPos += circular;


            float A = isBigArm ? hand.Height : hand.Height; //size of hand
            float B = isBigArm ? arm.Width - 36 : arm.Width - 20; //size of arm
            Vector2 end = targetHandPos;
            Vector2 start = armPosition;
            if(end.Distance(start) > (A + B))
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
            Vector2 endToMid = -startToEnd.RotatedBy(angleB * j);
            Vector2 startToMid = startToEnd.RotatedBy(-angleA * j);
            float endHandRot = endToMid.ToRotation();
            float endArmRot = startToMid.ToRotation();
            if (isBigArm)
            {
                end -= new Vector2(0, 13 * j).RotatedBy(endArmRot);
            }
            else
            {
                end += new Vector2(1, 0).RotatedBy(endArmRot);
            }

            if (isBigArm)
            {
                Vector2 mid = startToMid.SNormalize() * 84 + start;
                mid -= new Vector2(0, 14 * j).RotatedBy(endArmRot);
                Texture2D bigDrill = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/bigDrill").Value;
                Vector2 drillOrigin = new Vector2(42, 72);
                Vector2 revDrillOrigin = new Vector2(bigDrill.Width - drillOrigin.X, drillOrigin.Y);
                float deg = -j * ((j == 1 ? 180 : 0) + -70);
                spriteBatch.Draw(bigDrill, mid - screenPos, null, drawColor, endHandRot + MathHelper.ToRadians(deg), j == 1 ? drillOrigin : revDrillOrigin, other.scale, j == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            }

            spriteBatch.Draw(hand, end - screenPos, null, drawColor, endHandRot + MathHelper.PiOver2, handOrigin, other.scale, j == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(arm, start - screenPos, null, drawColor, endArmRot + (j == -1 ? MathF.PI : 0), j == -1 ? armOrigin : revArmOrigin, other.scale, j == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            //Visual representations of the IK happening
            //spriteBatch.Draw(SOTSUtils.WhitePixel, end - screenPos, null, drawColor, 0, Vector2.One, other.scale * 4, SpriteEffects.None, 0);
            //spriteBatch.Draw(SOTSUtils.WhitePixel, end - screenPos, null, drawColor, endToMid.ToRotation(), new Vector2(0, 1), new Vector2(A * 0.5f, 2), SpriteEffects.None, 0);
            //spriteBatch.Draw(SOTSUtils.WhitePixel, start - screenPos, null, drawColor, 0, Vector2.One, other.scale * 4, SpriteEffects.None, 0);
            //spriteBatch.Draw(SOTSUtils.WhitePixel, start - screenPos, null, drawColor, startToMid.ToRotation(), new Vector2(0, 1), new Vector2(B * 0.5f, 2), SpriteEffects.None, 0);

            //Visual location of the actual center of the arm
            //Vector2 realEnd = end + new Vector2(5, 4 * j).RotatedBy(endToMid.ToRotation());
            //spriteBatch.Draw(SOTSUtils.WhitePixel, realEnd - screenPos, null, Color.Red, 0, Vector2.One, other.scale * 4, SpriteEffects.None, 0);
        }
        public void DrawLeg(NPC other, SpriteBatch spriteBatch, Vector2 screenPos, int i)
        {
            int j = SOTSUtils.SignNoZero(i);
            i = Math.Abs(i) - 1;
            float r = WalkCounter * 2.0f;
            float legSwayAmt = i == 0 ? 22 : 18;
            float legMoveSin = MathF.Sin(MathHelper.ToRadians(r + i * 120 + (j == -1 ? 180 : 0)));
            legMoveSin = (legMoveSin * 0.2f + 0.8f * MathF.Sign(legMoveSin) * MathF.Sqrt(MathF.Abs(legMoveSin))) * legSwayAmt * j;
            int separation = i == 2 ?  12 : i * 32;
            float scale = i == 2 ? 0.9f : 0.8f;
            float rotation = i == 2 ? -12.5f : i == 0 ? 5 : -6.25f;
            int outward = i == 1 ? 26 : i == 2 ? 48 : 22;
            Texture2D arm = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/leg").Value;
            Vector2 legOrig = new Vector2(73, 15);
            Vector2 revLegOrig = new Vector2(arm.Width - legOrig.X, legOrig.Y);
            float legRot = other.rotation;
            Vector2 armPosition = new Vector2(outward * j, 18 - separation);
            armPosition = armPosition.RotatedBy(legRot) + other.Center;
            Color drawColor = Lighting.GetColor(armPosition.ToTileCoordinates(), new Color(210, 210, 210));
            legRot += MathHelper.ToRadians(legMoveSin);
            spriteBatch.Draw(arm, armPosition - screenPos, null, drawColor, legRot + MathHelper.ToRadians(rotation * j), j == -1 ? legOrig : revLegOrig, other.scale * scale, j == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

        }
        public override string Texture => "SOTS/NPCs/Boss/Excavator/head";
        public override void SetStaticDefaults()
        {
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
            NPC.lifeMax = 5000;
            NPC.damage = 25;
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
        }
        private int[] segments = [-1, -1, -1, -1, -1, -1, -1, -1];
        private int DespawnCounter = 0;
        private Player target => Main.player[NPC.target];
        public bool DespawnCheck()
        {
            if (target.dead || Vector2.Distance(target.Center, NPC.Center) > 4800)
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
            return false;
        }
        private void WormSetup()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (NPC.ai[0] == 0)
                {
                    NPC.realLife = NPC.whoAmI;
                    int latestNPC = NPC.whoAmI;
                    int WormLength = segments.Length;
                    for (int i = 0; i < WormLength; i++)
                    {
                        int type = i == 0 ? ModContent.NPCType<ExcavatorBody>() : i == 1 ? ModContent.NPCType<ExcavatorBody2>() : i == segments.Length - 1 ? ModContent.NPCType<ExcavatorDrillTail>() : ModContent.NPCType<ExcavatorTail>();
                        latestNPC = NPC.NewNPC(NPC.GetSource_Misc("SOTS:WormEnemy"), (int)NPC.Center.X, (int)NPC.Center.Y, type, NPC.whoAmI, i * 180f, latestNPC);
                        Main.npc[latestNPC].realLife = NPC.whoAmI;
                        Main.npc[latestNPC].ai[3] = NPC.whoAmI;
                        Main.npc[latestNPC].ai[2] = i + 1;
                        segments[i] = latestNPC;
                    }
                    NPC.ai[0] = 1;
                }
                NPC.netUpdate = true;
            }
        }
        private void IdleMoveStyle()
        {
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
            if(MoveStyle == 0)
            {
                float dist = toPlayer.Length();
                if(dist > 200)
                {
                    NPC.velocity += toPlayer.SNormalize() * (0.6f + dist * 0.001f);
                    NPC.velocity *= 0.925f;
                }
            }
        }
        public override bool PreAI()
        {
            Vector2 toPlayer = target.Center - NPC.Center;
            UpdateNeckSegments();
            if (DespawnCheck())
                return false;
            NPC.TargetClosest(true);
            WormSetup();
            IdleMoveStyle();
            AI1++;
            if(AI1 > 120)
            {
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Main.rand.NextVector2Circular(4, 4), ModContent.ProjectileType<ExcavatorRocket>(), NPC.GetBaseDamage() / 2, 1, Main.myPlayer, target.Center.X, target.Center.Y);
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Main.rand.NextVector2Circular(4, 4), ModContent.ProjectileType<ExcavatorOrb>(), NPC.GetBaseDamage() / 2, 1, Main.myPlayer);
                }
                AI1 = -70;
            }

            if(NPC.velocity.LengthSquared() > 0.1f)
            {
                float movementTargetR = NPC.velocity.ToRotation();
                float distToPlayer = toPlayer.Length();
                float playerFacingTargetR = toPlayer.ToRotation();
                float lerpAmt = 0.1f * MathF.Max(1 - distToPlayer / 480f, 0);
                NPC.rotation = SOTSUtils.AngularLerp(movementTargetR, playerFacingTargetR, lerpAmt);
            }
            return false;
        }
        public override void PostAI()
        {
            Vector2 trueVelo = NPC.position - NPC.oldPosition;
            float speed = trueVelo.Length();
            if (speed < 1000)
            {
                WalkCounter += 2 * MathF.Sqrt(speed);
            }
            if (!NPC.active)
                return;
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
    }
}