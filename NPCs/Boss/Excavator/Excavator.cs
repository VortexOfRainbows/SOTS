using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private NPC owner => Main.npc[(int)NPC.ai[3]];
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
        public float AI1
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
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
            }
            else
            {
                body = ModContent.Request<Texture2D>($"{dir}tail").Value;
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
                    for (int j = -1; j <= 1; j += 2)
                    {
                        DrawArmIK(other, spriteBatch, screenPos, j);
                    }
                }
                if(body != null)
                    spriteBatch.Draw(body, other.Center - screenPos, null, drawColor, other.rotation, bodyOrigin, other.scale * scale, other.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            }
        }
        public void DrawArmIK(NPC other, SpriteBatch spriteBatch, Vector2 screenPos, int dir)
        {
            int j = dir;
            Texture2D body = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/body").Value;
            Texture2D arm = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/arm").Value;
            Texture2D hand = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/hand").Value;
            Vector2 armOrigin = new Vector2(50, 14);
            Vector2 revArmOrigin = new Vector2(arm.Width - armOrigin.X, armOrigin.Y);
            Vector2 handOrigin = new Vector2(hand.Width / 2, hand.Height);
            float armRotation = other.rotation;
            Vector2 armPosition = new Vector2((-body.Width / 2 + 4) * j, -body.Height / 2 + 16).RotatedBy(armRotation) + other.Center;
            Color drawColor = Lighting.GetColor(armPosition.ToTileCoordinates(), Color.White);

            float r = other.ai[0] * j + j * 45;
            float outwardSize = 38 - 16 * MathF.Sin(MathHelper.ToRadians(r + 90 * j));
            Vector2 targetHandPos = new Vector2(-(body.Width / 2 + outwardSize) * j, -102).RotatedBy(armRotation) + other.Center;
            Vector2 circular = new Vector2(38, 0).RotatedBy(MathHelper.ToRadians(r));
            circular.X *= 0.25f;
            circular = circular.RotatedBy(armRotation);
            targetHandPos += circular;


            float A = hand.Height; //size of hand
            float B = arm.Width - 20; //size of arm
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
            Vector2 endToMid = -startToEnd.RotatedBy(angleB * dir);
            Vector2 startToMid = startToEnd.RotatedBy(-angleA * dir);
            
            //Visual representations of the IK happening
            //spriteBatch.Draw(SOTSUtils.WhitePixel, end - screenPos, null, drawColor, 0, Vector2.One, other.scale * 4, SpriteEffects.None, 0);
            //spriteBatch.Draw(SOTSUtils.WhitePixel, end - screenPos, null, drawColor, endToMid.ToRotation(), new Vector2(0, 1), new Vector2(A * 0.5f, 2), SpriteEffects.None, 0);
            //spriteBatch.Draw(SOTSUtils.WhitePixel, start - screenPos, null, drawColor, 0, Vector2.One, other.scale * 4, SpriteEffects.None, 0);
            //spriteBatch.Draw(SOTSUtils.WhitePixel, start - screenPos, null, drawColor, startToMid.ToRotation(), new Vector2(0, 1), new Vector2(B * 0.5f, 2), SpriteEffects.None, 0);
            
            spriteBatch.Draw(hand, end - screenPos, null, drawColor, endToMid.ToRotation() + MathHelper.PiOver2, handOrigin, other.scale, j == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(arm, start - screenPos, null, drawColor, startToMid.ToRotation() + (j == -1 ? MathF.PI : 0), j == -1 ? armOrigin : revArmOrigin, other.scale, j == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            //Visual location of the actual center of the arm
            //Vector2 realEnd = end + new Vector2(5, 4 * j).RotatedBy(endToMid.ToRotation());
            //spriteBatch.Draw(SOTSUtils.WhitePixel, realEnd - screenPos, null, Color.Red, 0, Vector2.One, other.scale * 4, SpriteEffects.None, 0);
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
            neckSegments = new List<Vector2>();
        }
        private int[] segments = [-1, -1, -1, -1, -1, -1, -1, -1];
        public override bool PreAI()
        {
            UpdateNeckSegments();
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];
            if (player.dead || Vector2.Distance(player.Center, NPC.Center) > 4800)
            {
                NPC.active = false;
                return false;
            }
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

            Vector2 toPlayer = player.Center - NPC.Center;

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
        public override void SendExtraAI(BinaryWriter writer)
        {

        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {

        }
        public override void PostAI()
        {
            NPC.velocity *= 0.95f;
            if(Main.mouseRight)
            {
                Vector2 toMouse = Main.MouseWorld - NPC.Center;
                NPC.velocity += toMouse * 0.001f;
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