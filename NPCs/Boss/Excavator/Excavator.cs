using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.WorldgenHelpers;
using System;
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
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.netUpdate = true;
                }
            }
 
            if (Main.npc[(int)NPC.ai[1]].active && NPC.ai[1] < (double)Main.npc.Length)
            {
                //float percent = 1f - (float)i / segments.Count;
                //Vector2 toPrev = prev - segments[i];
                //float normalMovement = toPrev.Length() * 0.6f - wormingAmount;
                //if (normalMovement > 0)
                //    segments[i] += toPrev.SNormalize() * normalMovement;
                //segments[i] = Vector2.Lerp(segments[i], prev, 0.035f);
                //segments[i] += new Vector2(0, 0.3f + 0.02f * i + NPC.velocity.Y * percent * 0.3f);

                Vector2 nextSegment = Main.npc[(int)NPC.ai[1]].Center;
                Vector2 toNext = nextSegment - NPC.Center;
                NPC.rotation = toNext.ToRotation() + 1.57f;
                float length = toNext.Length();
                toNext = toNext.SNormalize() * toNext.Length();
                float dist = (length - NPC.width * 1.5f) / length;
                toNext *= dist;
                NPC.velocity = Vector2.Zero;
                NPC.Center = NPC.Center + toNext;
                NPC.spriteDirection = Main.npc[(int)NPC.ai[3]].spriteDirection;
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
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D head = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/head").Value;
            Texture2D arm = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/arm").Value;
            Texture2D hand = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/hand").Value;
            Texture2D body = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/body").Value;
            Vector2 origin = new Vector2(head.Width * 0.5f, head.Height * 0.5f);
            Vector2 bodyOrigin = body.Size() / 2;
            Vector2 armOrigin = new Vector2(7, arm.Height / 2);
            Vector2 handOrigin = new Vector2(19, 13);
            Vector2 revArmOrigin = new Vector2(arm.Width - 7, arm.Height / 2);
            Vector2 revHandOrigin = new Vector2(hand.Width - 19, 13);
            for (int i = 0; i < segments.Length; i++)
            {
                int segment = segments[i];
                if (segment > 0)
                {
                    NPC other = Main.npc[segment];
                    for(int j = -1; j <= 1; j += 2)
                    {
                        float armRotation = other.rotation;
                        Vector2 armPosition = new Vector2(-body.Width / 2 * j, 0).RotatedBy(armRotation) - screenPos + other.Center;
                        spriteBatch.Draw(arm, armPosition, null, drawColor, armRotation, j == -1 ? armOrigin : revArmOrigin, other.scale, j == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                        Vector2 handPosition = armPosition + new Vector2((arm.Width - 14) * -j, 0).RotatedBy(armRotation);
                        spriteBatch.Draw(hand, handPosition, null, drawColor, armRotation + MathF.PI, j == -1 ? handOrigin : revHandOrigin, other.scale, j == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                    }
                    spriteBatch.Draw(body, other.Center - screenPos, null, drawColor, other.rotation, bodyOrigin, other.scale, other.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
                }
            }
            spriteBatch.Draw(head, NPC.Center - screenPos, null, drawColor, NPC.rotation, origin, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            return false;
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
        }
        private int[] segments = [-1, -1, -1, -1];
        public override bool PreAI()
        {
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
                    int WormLength = 1;
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