using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.NPCs.Boss.CelestialSerpent
{   
    public class CelestialSerpentBody : ModNPC
    {	
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Celestial Serpent");
		}
        public override void SetDefaults()
        {
            npc.width = 42;     
            npc.height = 36;            
            npc.damage = 50;
            npc.defense = 90;
            npc.lifeMax = 12104310; //arbitrary
            npc.knockBackResist = 0.0f;
            npc.noTileCollide = true;
            npc.netAlways = true;
            npc.boss = true;
            npc.noGravity = true;
            npc.dontCountMe = true;
            npc.value = 10000;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath32;
			music = MusicID.Boss2;
            for(int i = 0; i < Main.maxBuffTypes; i++)
            {
                npc.buffImmune[i] = true;
            }
        }
        float currentDPS = -1;
        float DPSregenRate = 0.1f;
        float maxDPS = 250;
        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {

            if (damage - (defense/2) > currentDPS)
            {
                damage = currentDPS + (defense / 2);
                currentDPS = 0;
            }
            else
            {
                currentDPS -= (float)damage - (defense / 2);
            }
            return true;
        }
        public override bool PreAI()
        {
            if (npc.ai[3] > 0)
                npc.realLife = (int)npc.ai[3];
            if (npc.target < 0 || npc.target == byte.MaxValue || Main.player[npc.target].dead)
                npc.TargetClosest(true);
 
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
 
            if (npc.ai[1] < (double)Main.npc.Length)
            {
                Vector2 npcCenter = npc.Center;
                float dirX = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - npcCenter.X;
                float dirY = Main.npc[(int)npc.ai[1]].position.Y + (float)(Main.npc[(int)npc.ai[1]].height / 2) - npcCenter.Y;
                npc.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                int height = Main.npc[(int)npc.ai[1]].height;
                if (height > 44)
                    height = 44;
                float dist = (length - height) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;
                npc.velocity = Vector2.Zero;
                npc.position.X = npc.position.X + posX;
                npc.position.Y = npc.position.Y + posY;
            }
            return false;
        }
 
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Main.npcTexture[npc.type];
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, new Rectangle?(), drawColor, npc.rotation, origin, npc.scale, SpriteEffects.None, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("NPCs/Boss/CelestialSerpent/DPSBarrier");
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, new Rectangle?(), drawColor * (0.5f + (0.25f * (maxDPS - currentDPS) / maxDPS)), 0, origin, (maxDPS - currentDPS)/maxDPS, SpriteEffects.None, 0);
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
 
            return false;       //this make that the npc does not have a health bar
        }
		public override void PostAI()
		{
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 2.5f / 255f, (255 - npc.alpha) * 1.6f / 255f, (255 - npc.alpha) * 2.4f / 255f);
			npc.timeLeft = 100000;

            if (currentDPS == -1)
                currentDPS = maxDPS;

            if (currentDPS < maxDPS)
            {
                currentDPS += (maxDPS / 60) * DPSregenRate;
            }
            else
            {
                currentDPS = maxDPS;
            }
        }
    }
}