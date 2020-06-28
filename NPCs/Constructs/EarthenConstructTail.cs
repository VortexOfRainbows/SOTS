using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.NPCs.Constructs
{   
    public class EarthenConstructTail : ModNPC
    {	int ai1 = 0;
	int ai2 = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Earthen Construct");
		}
        public override void SetDefaults()
        {
            npc.width = 36;           
            npc.height = 40;        
            npc.damage = 20;
            npc.defense = 0;
            npc.lifeMax = 20000;  
            npc.knockBackResist = 0.0f;
            npc.behindTiles = true;
            npc.noTileCollide = true;
            npc.netAlways = true;
            npc.noGravity = true;
            npc.dontCountMe = true;
            npc.value = 100;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
        }
 
        public override bool PreAI()
        {
            if (npc.ai[3] > 0)
                npc.realLife = (int)npc.ai[3];
            if (npc.target < 0 || npc.target == byte.MaxValue || Main.player[npc.target].dead)
                npc.TargetClosest(true);
            if (Main.player[npc.target].dead && npc.timeLeft > 300)
                npc.timeLeft = 300;
 
            if (Main.netMode != 1)
            {
                if (!Main.npc[(int)npc.ai[1]].active)
                {
                    npc.life = 0;
                    npc.HitEffect(0, 10.0);
                    npc.active = false;
                    NetMessage.SendData(28, -1, -1, null, npc.whoAmI, -1f, 0.0f, 0.0f, 0, 0, 0);
                }
            }
            if(npc.ai[2] != -1)
            {
                npc.scale = 5f / (npc.ai[2] + 6);
                npc.width = (int)(npc.width * npc.scale);
                npc.height = (int)(npc.height * npc.scale);
                npc.ai[2] = -1;
            }
 
            if (npc.ai[1] < (double)Main.npc.Length)
            {
                Vector2 npcCenter = npc.Center;
                // Then using that center, we calculate the direction towards the 'parent NPC' of this NPC.
                float dirX = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2f) - npcCenter.X;
                float dirY = Main.npc[(int)npc.ai[1]].position.Y + (float)(Main.npc[(int)npc.ai[1]].height / 2f) - npcCenter.Y;
                // We then use Atan2 to get a correct rotation towards that parent NPC.
                npc.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
                // We also get the length of the direction vector.
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                // We calculate a new, correct distance.
                float dist = (length - (float)npc.width) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;
 
                // Reset the velocity of this NPC, because we don't want it to move on its own
                npc.velocity = Vector2.Zero;
                // And set this NPCs position accordingly to that of this NPCs parent NPC.
                npc.position.X = npc.position.X + posX;
                npc.position.Y = npc.position.Y + posY;
            }
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("NPCs/Constructs/EarthenConstructTailHead");
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Texture2D texture2 = mod.GetTexture("NPCs/Constructs/EarthenConstruct");
            Vector2 origin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
            Main.spriteBatch.Draw(texture2, npc.Center - Main.screenPosition, new Rectangle?(), drawColor, npc.rotation - MathHelper.ToRadians(npc.localAI[0]), origin2, npc.scale + 0.04f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, new Rectangle?(), drawColor, npc.rotation - MathHelper.ToRadians(90), origin, npc.scale + 0.04f, SpriteEffects.None, 0);
            return false;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;       //this make that the npc does not have a health bar
        }
		public override void PostAI()
		{
            npc.localAI[0] += 3;
			npc.timeLeft = 100;
		}
    }
}