using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.WorldgenHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Animations;
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
    public class Excavator : ModNPC
    {
        public static int BodyFrequency => 3;
        public List<Vector2> neckSegments;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D head = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/head").Value;
            Texture2D neck = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/neck").Value;
            Texture2D bodyTop = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/bodytop").Value;
            Vector2 origin = new Vector2(head.Width * 0.5f, head.Height * 0.5f);
            Vector2 neckOrigin = new Vector2(neck.Width * 0.5f, neck.Height);


            for (int i = 0; i < segments.Length; i++)
            {
                int segment = segments[i];
                if (segment > 0)
                {
                    NPC other = Main.npc[segment];
                    if (other.ModNPC is ExcavatorBody)
                        DrawBody(other, spriteBatch, screenPos);
                }
            }
            Vector2 previousPos = NPC.Center;
            for (int i = 0; i < neckSegments.Count; ++i)
            {
                Vector2 segment = neckSegments[i];
                Vector2 toPrev = previousPos - segment;

                float length = toPrev.Length() + 4;
                Vector2 scale = new Vector2(1, length / neck.Height);
                spriteBatch.Draw(neck, segment - screenPos, null, drawColor, toPrev.ToRotation() + MathHelper.PiOver2, neckOrigin, scale, SpriteEffects.None, 0);

                previousPos = segment;
                if ((i + 1) % BodyFrequency == 0 && i != 0)
                {
                    previousPos += new Vector2(-74, 0).RotatedBy(toPrev.ToRotation());
                }
            }

            for (int i = 0; i < segments.Length; i++)
            {
                int segment = segments[i];
                if (segment > 0)
                {
                    NPC other = Main.npc[segment];
                    if (other.ModNPC is ExcavatorBody)
                        spriteBatch.Draw(bodyTop, other.Center - screenPos, null, drawColor, other.rotation, bodyTop.Size() / 2, other.scale, other.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
                }
            }
            spriteBatch.Draw(head, NPC.Center - screenPos, null, drawColor, NPC.rotation, origin, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            return false;
        }
        public void UpdateNeckSegments()
        {
            float neckHeight = 26;
            while(neckSegments.Count < segments.Length * BodyFrequency)
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
                    neckSegments[i] = Vector2.Lerp(neckSegments[i], targetPosition, 0.6f);
                }
                previous = neckSegments[i];
                if((i + 1) % BodyFrequency == 0 && i != 0)
                {
                    previous += new Vector2(-74, 0).RotatedBy(toBody.ToRotation());
                }
            }

            for (int i = 0; i < segments.Length; i++)
            {
                int segment = segments[i];
                if (segment > 0)
                {
                    NPC other = Main.npc[segment];
                    if(other.ModNPC is ExcavatorBody)
                    {
                        UpdateChildPos(other, (1 + i) * BodyFrequency - 1);
                    }
                }
            }
        }
        public void UpdateChildPos(NPC child, int neckPos)
        {
            //float percent = 1f - (float)i / segments.Count;
            //Vector2 toPrev = prev - segments[i];
            //float normalMovement = toPrev.Length() * 0.6f - wormingAmount;
            //if (normalMovement > 0)
            //    segments[i] += toPrev.SNormalize() * normalMovement;
            //segments[i] = Vector2.Lerp(segments[i], prev, 0.035f);
            //segments[i] += new Vector2(0, 0.3f + 0.02f * i + NPC.velocity.Y * percent * 0.3f);
            Vector2 nextSegment = neckSegments[neckPos];
            Vector2 toNext = neckSegments[neckPos - 1] - nextSegment;
            child.rotation = toNext.ToRotation() + 1.57f;
            Vector2 rotationOrigin = new Vector2(0, -NPC.height / 2).RotatedBy(child.rotation);
            child.velocity = Vector2.Zero;
            child.Center = nextSegment - rotationOrigin;
        }
        public void DrawBody(NPC other, SpriteBatch spriteBatch, Vector2 screenPos)
        {
            Color drawColor = Lighting.GetColor(other.Center.ToTileCoordinates(), Color.White);
            Texture2D body = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/body").Value;
            Vector2 bodyOrigin = body.Size() / 2;
            for (int j = -1; j <= 1; j += 2)
            {
                DrawArmIK(other, spriteBatch, screenPos, j);
            }
            spriteBatch.Draw(body, other.Center - screenPos, null, drawColor, other.rotation, bodyOrigin, other.scale, other.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
        }
        public void DrawArmIK(NPC other, SpriteBatch spriteBatch, Vector2 screenPos, int dir)
        {
            int j = dir;
            Texture2D body = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/body").Value;
            Texture2D arm = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/arm").Value;
            Texture2D hand = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/hand").Value;
            Vector2 armOrigin = new Vector2(7, arm.Height / 2);
            Vector2 revArmOrigin = new Vector2(arm.Width - 7, arm.Height / 2);
            Vector2 revHandOrigin = new Vector2(hand.Width - 19, 13);
            Vector2 handOrigin = new Vector2(19, 13);
            float armRotation = other.rotation;
            Vector2 armPosition = new Vector2(-body.Width / 2 * j, 0).RotatedBy(armRotation) + other.Center;
            Color drawColor = Lighting.GetColor(armPosition.ToTileCoordinates(), Color.White);
            spriteBatch.Draw(arm, armPosition - screenPos, null, drawColor, armRotation, j == -1 ? armOrigin : revArmOrigin, other.scale, j == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            Vector2 handPosition = armPosition + new Vector2((arm.Width - 14) * -j, 0).RotatedBy(armRotation);
            spriteBatch.Draw(hand, handPosition - screenPos, null, drawColor, armRotation + MathF.PI, j == -1 ? handOrigin : revHandOrigin, other.scale, j == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
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
            NPC.width = 118;
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
        private int[] segments = [-1, -1, -1, -1];
        public override bool PreAI()
        {
            UpdateNeckSegments();
            NPC.TargetClosest(true);
            if (Main.player[NPC.target].dead || Vector2.Distance(Main.player[NPC.target].Center, NPC.Center) > 4800)
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
                    int WormLength = 4;
                    for (int i = 0; i < WormLength; i++)
                    {
                        latestNPC = NPC.NewNPC(NPC.GetSource_Misc("SOTS:WormEnemy"), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<ExcavatorBody>(), NPC.whoAmI, 0, latestNPC);
                        Main.npc[latestNPC].realLife = NPC.whoAmI;
                        Main.npc[latestNPC].ai[3] = NPC.whoAmI;
                        Main.npc[latestNPC].ai[2] = i + 1;
                        segments[i] = latestNPC;
                    }
                    NPC.ai[0] = 1;
                }
                NPC.netUpdate = true;
            }
            NPC.rotation = NPC.velocity.ToRotation() + 1.57f;
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