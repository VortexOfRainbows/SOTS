using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.NPCs.Boss.Polaris
{   
    public class BulletSnakeBody : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bullet Snake");
		}
        public override void SetDefaults()
        {
            npc.width = 26;
            npc.height = 34;   
            npc.damage = 60;
            npc.defense = 30;
            npc.lifeMax = 20000;  
            npc.knockBackResist = 0.0f;
            npc.noTileCollide = true;
            npc.netAlways = true;
            npc.noGravity = true;
            npc.dontCountMe = true;
            npc.value = 0;
            npc.buffImmune[BuffID.Frostburn] = true;
            npc.buffImmune[BuffID.Ichor] = true;
            npc.buffImmune[BuffID.OnFire] = true;
        }
        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.damage = (int)(npc.damage * 0.75f);
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
                    NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, npc.whoAmI, -1f, 0.0f, 0.0f, 0, 0, 0);
                }
            }
 
            if (npc.ai[1] < (double)Main.npc.Length)
            {
                NPC lastNpc = Main.npc[(int)npc.ai[1]];
                Vector2 npcCenter = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                float dirX = lastNpc.position.X + (float)(lastNpc.width / 2) - npcCenter.X;
                float dirY = lastNpc.position.Y + (float)(lastNpc.height / 2) - npcCenter.Y;
                npc.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                float height = npc.height - 2;
                if (lastNpc.type == ModContent.NPCType<BulletSnakeWing>())
                    height -= 4;
                float dist = (length - height) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;
                npc.velocity = Vector2.Zero;
                npc.position.X = npc.position.X + posX;
                npc.position.Y = npc.position.Y + posY;
                if (dirX > 0)
                    npc.direction = 1;
                else
                    npc.direction = -1;
            }
            npc.spriteDirection = npc.direction;
            return false;
        }
 
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Main.npcTexture[npc.type];
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, null, Color.White, npc.rotation, origin, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : 0, 0);
            return false;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
		public override void PostAI()
		{
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.9f / 255f, (255 - npc.alpha) * 0.1f / 255f, (255 - npc.alpha) * 0.3f / 255f);
			npc.timeLeft = 100;
		}
    }
}