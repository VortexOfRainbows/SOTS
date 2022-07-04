using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.NPCs.Constructs
{   
    public class EarthenConstructTail : ModNPC
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Earthen Construct");
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }
        public override void SetDefaults()
        {
            NPC.width = 36;           
            NPC.height = 40;        
            NPC.damage = 16;
            NPC.defense = 12;
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
            writer.Write(NPC.scale);
            writer.Write(NPC.width);
            writer.Write(NPC.height);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.scale = reader.ReadSingle();
            NPC.width = reader.ReadInt32();
            NPC.height = reader.ReadInt32();
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 10; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 82, 2.5f * (float)hitDirection, -2.5f, 0, default(Color), 0.7f);
                }
                if(Main.rand.NextBool(3))
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/EarthenConstructGore1"), 1f);
                if (Main.rand.NextBool(3))
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/EarthenConstructGore3"), 1f);
                if (Main.rand.NextBool(3))
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/EarthenConstructGore4"), 1f);
                if (Main.rand.NextBool(3))
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/EarthenConstructGore5"), 1f);
                for (int i = 0; i < 4; i++)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Main.rand.Next(61, 64), 1f);
            }
        }
        Vector2 ownerVelocity = Vector2.Zero;
        public override bool PreAI()
        {
            if (Main.netMode != 1)
            {
                if (!Main.npc[(int)NPC.ai[1]].active)
                {
                    NPC.life = 0;
                    NPC.HitEffect(0, 10.0);
                    NPC.active = false;
                    NetMessage.SendData(28, -1, -1, null, NPC.whoAmI, -1f, 0.0f, 0.0f, 0, 0, 0);
                    return false;
                }
            }
            if (NPC.ai[3] > 0)
            {
                NPC.realLife = (int)NPC.ai[3];
                ownerVelocity = Main.npc[(int)NPC.ai[3]].velocity;
            }
            if (NPC.ai[2] >= 0)
            {
                NPC.scale = 5f / (NPC.ai[2] + 6);
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
                Vector2 npcCenter = NPC.Center;
                float dirX = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2f) - npcCenter.X;
                float dirY = Main.npc[(int)NPC.ai[1]].position.Y + (float)(Main.npc[(int)NPC.ai[1]].height / 2f) - npcCenter.Y;
                NPC.rotation = new Vector2(dirX, dirY).ToRotation() + 1.57f;
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                if (length <= 0)
                    length = 1;
                float dist = (length - (float)NPC.width) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;
                NPC.velocity = Vector2.Zero;
                NPC.position.X = NPC.position.X + posX;
                NPC.position.Y = NPC.position.Y + posY;
                NPC.spriteDirection = Main.npc[(int)NPC.ai[3]].spriteDirection;
            }
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/Constructs/EarthenConstructTailHead").Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Texture2D texture2 = Mod.Assets.Request<Texture2D>("NPCs/Constructs/EarthenConstruct").Value;
            Vector2 origin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
            spriteBatch.Draw(texture2, NPC.Center - screenPos, null, drawColor, NPC.rotation - MathHelper.ToRadians(NPC.localAI[1]), origin2, NPC.scale + 0.04f, NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            spriteBatch.Draw(texture, NPC.Center - screenPos, null, drawColor, NPC.rotation - MathHelper.ToRadians(90), origin, NPC.scale + 0.04f, NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            return false;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;       //this make that the npc does not have a health bar
        }
		public override void PostAI()
		{
            NPC.localAI[1] += (float)Math.Sqrt(ownerVelocity.Length()) * -NPC.spriteDirection;
			NPC.timeLeft = 100;
		}
    }
}