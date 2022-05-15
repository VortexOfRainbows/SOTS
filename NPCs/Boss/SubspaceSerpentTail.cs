using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.NPCs.Boss
{   
    public class SubspaceSerpentTail : ModNPC
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Subspace Serpent");
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * bossLifeScale * 0.75f);  //boss life scale in expertmode
        }
        public override void SetDefaults()
        {
            NPC.width = 48;             
            NPC.height = 36;         
            NPC.damage = 40;
            NPC.defense = 50;
            NPC.lifeMax = 130000;
            NPC.knockBackResist = 0.0f;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
            NPC.noGravity = true;
            NPC.dontCountMe = true;
            NPC.value = 100000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath32;
			music = MusicID.Boss2;
            for (int i = 0; i < Main.maxBuffTypes; i++)
            {
                NPC.buffImmune[i] = true;
            }
            Main.npcFrameCount[NPC.type] = 8;
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return !NPC.dontTakeDamage;
        }
        public override void FindFrame(int frameHeight)
        {
            NPC parent = Main.npc[(int)NPC.ai[1]];
            int frameHeight2 = 36;
            int targetFrame = parent.frame.Y / frameHeight2;
            int currentFrame = NPC.frame.Y / frameHeight;
            if (currentFrame != targetFrame)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter >= 4)
                {
                    currentFrame = targetFrame;
                    NPC.frameCounter = 0;
                }
            }
            else
            {
                NPC.frameCounter = 0;
            }
            NPC.alpha = parent.alpha;
            NPC.dontTakeDamage = parent.dontTakeDamage;
            if (currentFrame > 7)
                currentFrame = 0;
            NPC.frame.Y = currentFrame * frameHeight;

        }
        public override bool CheckActive()
        {
            return false;
        }
        float ai2 = 0;
        public override bool PreAI()
        {
            if (glow > 0)
                glow -= 0.5f;
            if (runOnce)
            {
                for (int i = 0; i < trailPos.Length; i++)
                {
                    trailPos[i] = Vector2.Zero;
                }
                runOnce = false;
            }
            if (NPC.ai[3] > 0)
                NPC.realLife = (int)NPC.ai[3];
            if (NPC.target < 0 || NPC.target == byte.MaxValue || Main.player[NPC.target].dead)
                NPC.TargetClosest(true);
 
            if (Main.netMode != 1)
            {
                if (!Main.npc[(int)NPC.ai[1]].active)
                {
                    NPC.life = 0;
                    NPC.HitEffect(0, 10.0);
                    NPC.active = false;
                    NetMessage.SendData(28, -1, -1, null, NPC.whoAmI, -1f, 0.0f, 0.0f, 0, 0, 0);
                }
            }
 
            if (NPC.ai[1] < (double)Main.npc.Length)
            {
                Vector2 npcCenter = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                float dirX = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - npcCenter.X;
                float dirY = Main.npc[(int)NPC.ai[1]].position.Y + (float)(Main.npc[(int)NPC.ai[1]].height / 2) - npcCenter.Y;
                NPC.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                float dist = (length - NPC.height) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;
                NPC.velocity = Vector2.Zero;
                NPC.position.X = NPC.position.X + posX;
                NPC.position.Y = NPC.position.Y + posY;
            }
            cataloguePos();
            if (Main.netMode != 1)
            {
                NPC.netUpdate = true;
            }
            return false;
        }
        Vector2[] trailPos = new Vector2[12];
        bool runOnce = true;
        float glow = 14f;
        public void cataloguePos()
        {
            Vector2 current = NPC.Center + new Vector2(0, 20).RotatedBy(NPC.rotation);
            Vector2 velo = new Vector2(0, 20).RotatedBy(NPC.rotation) * 0.1f;
            for (int i = 0; i < trailPos.Length; i++)
            {
                if (trailPos[i] != Vector2.Zero)
                    trailPos[i] += velo * (trailPos.Length - i) / (float)trailPos.Length;
                Vector2 previousPosition = trailPos[i];
                trailPos[i] = current;
                current = previousPosition;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/Boss/SubspaceSerpentTailFill").Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, NPC.height * 0.5f);
            NPC head = Main.npc[NPC.realLife];
            SubspaceSerpentHead subHead = head.modNPC as SubspaceSerpentHead;
            bool phase2 = subHead.hasEnteredSecondPhase;
            if (phase2)
            {
                Color color = new Color(phase2 ? 0 : 255, phase2 ? 255 : 0, 0);
                for (int i = 0; i < 2; i++)
                {
                    int direction = i * 2 - 1;
                    Vector2 toTheSide = new Vector2(2 * direction, 0).RotatedBy(NPC.rotation);
                    Main.spriteBatch.Draw(texture, NPC.Center - Main.screenPosition + toTheSide, NPC.frame, color * ((255f - NPC.alpha) / 255f) * ((255f - NPC.alpha) / 255f), NPC.rotation, origin, 1f, SpriteEffects.None, 0);
                }
            }
            DrawTrail();
            texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            origin = new Vector2(texture.Width * 0.5f, NPC.height * 0.5f);
            Main.spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, NPC.frame, lightColor * ((255f - NPC.alpha) / 255f), NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
            texture = Mod.Assets.Request<Texture2D>("NPCs/Boss/SubspaceSerpentTailGlow").Value;
            origin = new Vector2(texture.Width * 0.5f, NPC.height * 0.5f);
            Main.spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, NPC.frame, Color.White * ((255f - NPC.alpha) / 255f), NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
            counter++;
            if (counter > 12)
                counter = 0;
            for (int j = 0; j < 2; j++)
            {
                float bonusAlphaMult = 1 - 1 * (counter / 12f);
                float dir = j * 2 - 1;
                Vector2 offset = new Vector2(counter * 0.8f * dir, 0).RotatedBy(NPC.rotation);
                Main.spriteBatch.Draw(texture, NPC.Center - Main.screenPosition + offset, NPC.frame, new Color(100, 100, 100, 0) * bonusAlphaMult * ((255f - NPC.alpha) / 255f), NPC.rotation, origin, 1.00f, SpriteEffects.None, 0.0f);
            }
            return false;
        }
        public void DrawTrail()
        {
            if (runOnce)
                return;
            Texture2D texture2 = Mod.Assets.Request<Texture2D>("NPCs/Boss/SerpentTailTrail").Value;
            Vector2 drawOrigin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
            Vector2 current = NPC.Center + new Vector2(0, 20).RotatedBy(NPC.rotation);
            Vector2 previousPosition = current;
            Color color = new Color(90, 120, 90, 0);
            for (int k = 0; k < trailPos.Length; k++)
            {
                float scale = NPC.scale * (trailPos.Length - k) / (float)trailPos.Length;
                scale *= 1f;
                if (trailPos[k] == Vector2.Zero)
                {
                    return;
                }
                Vector2 drawPos = trailPos[k] - Main.screenPosition;
                Vector2 currentPos = trailPos[k];
                Vector2 betweenPositions = previousPosition - currentPos;
                if(betweenPositions.Length() > 120)
                {
                    return;
                }
                color *= 0.95f;
                float max = betweenPositions.Length() / (4 * scale);
                for (int i = 0; i < max; i++)
                {
                    drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
                    for (int j = 0; j < 4; j++)
                    {
                        float x = Main.rand.Next(-10, 11) * 0.1f * scale;
                        float y = Main.rand.Next(-10, 11) * 0.1f * scale;
                        if (j <= 1)
                        {
                            x = 0;
                            y = 0;
                        }
                        Main.spriteBatch.Draw(texture2, drawPos + new Vector2(x, y), null, color * ((255f - NPC.alpha) / 255f), NPC.rotation, drawOrigin2, scale, NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f); ;
                    }
                }
                previousPosition = currentPos;
            }
        }
        int counter = 0;
        float ai1 = 0;
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;       //this make that the npc does not have a health bar
        }
		public override void PostAI()
		{
			Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 2.5f / 255f, (255 - NPC.alpha) * 1.6f / 255f, (255 - NPC.alpha) * 2.4f / 255f);
			NPC.timeLeft = 10000000;
		}
    }
}