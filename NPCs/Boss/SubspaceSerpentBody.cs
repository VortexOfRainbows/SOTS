using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.NPCs.Boss
{
    public class SubspaceSerpentBody : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Subspace Serpent");
        }
        public override void SetDefaults()
        {
            npc.width = 48;
            npc.height = 34;
            npc.damage = 70;
            npc.defense = 100;
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
            for (int i = 0; i < Main.maxBuffTypes; i++)
            {
                npc.buffImmune[i] = true;
            }
            Main.npcFrameCount[npc.type] = 8;
        }
        float currentDPS = -1;
        private float DPSregenRate = 0.1f;
        float maxDPS = 250;
        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Subspace/SubspaceSerpentBodyGore"), 1f);
            }
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return !npc.dontTakeDamage;
        }
        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            if (damage - (defense / 2) > currentDPS)
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
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(npc.knockBackResist);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            npc.knockBackResist = reader.ReadSingle();
        }
        public override void FindFrame(int frameHeight)
        {
            NPC parent = Main.npc[(int)npc.ai[1]];
            int frameHeight2 = frameHeight;
            if(parent.type == ModContent.NPCType<SubspaceSerpentHead>())
            {
                frameHeight2 = 44;
            }
            npc.alpha = parent.alpha;
            npc.dontTakeDamage = parent.dontTakeDamage;
            int targetFrame = parent.frame.Y / frameHeight2;
            int currentFrame = npc.frame.Y / frameHeight;
            if (currentFrame != targetFrame)
            {
                npc.frameCounter++;
                if (npc.frameCounter >= 4)
                {
                    currentFrame = targetFrame;
                    npc.frameCounter = 0;
                }
            }
            else
            {
                npc.frameCounter = 0;
            }
            if (currentFrame > 7)
                currentFrame = 0;
            npc.frame.Y = currentFrame * frameHeight;

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
                }
                if (!npc.active && Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, npc.whoAmI, -1f, 0f, 0f, 0, 0, 0);
                }
            }

            if (npc.ai[1] < (double)Main.npc.Length)
            {
                Vector2 npcCenter = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                float dirX = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - npcCenter.X;
                float dirY = Main.npc[(int)npc.ai[1]].position.Y + (float)(Main.npc[(int)npc.ai[1]].height / 2) - npcCenter.Y;
                npc.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                float dist = (length - npc.height) / length;
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
            Texture2D texture = mod.GetTexture("NPCs/Boss/SubspaceSerpentBodyFill");
            Vector2 origin = new Vector2(texture.Width * 0.5f, npc.height * 0.5f);
            float percentShield = (maxDPS - currentDPS) / maxDPS;
            NPC head = Main.npc[npc.realLife];
            SubspaceSerpentHead subHead = head.modNPC as SubspaceSerpentHead;
            bool phase2 = subHead.hasEnteredSecondPhase;
            if (phase2)
                percentShield = 0.3334f;
            if (percentShield > 0 || phase2)
            {
                Color color = new Color(phase2 ? 0 : 255, phase2 ? 255 : 0, 0);
                for (int i = 0; i < 2; i++)
                {
                    int direction = i * 2 - 1;
                    Vector2 toTheSide = new Vector2(6 * percentShield * direction, 0).RotatedBy(npc.rotation);
                    Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition + toTheSide, npc.frame, color * ((255f - npc.alpha) / 255f) * ((255f - npc.alpha) / 255f), npc.rotation, origin, 1f, SpriteEffects.None, 0);
                }
            }
            texture = Main.npcTexture[npc.type];
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, npc.frame, drawColor * ((255f - npc.alpha) / 255f), npc.rotation, origin, npc.scale, SpriteEffects.None, 0);
            return false;
        }
        int counter = 0;
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("NPCs/Boss/SubspaceSerpentBodyGlow");
            Vector2 origin = new Vector2(texture.Width * 0.5f, npc.height * 0.5f);
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, npc.frame, Color.White * ((255f - npc.alpha) / 255f), npc.rotation, origin, npc.scale, SpriteEffects.None, 0);
            counter++;
            if (counter > 12)
                counter = 0;
            for (int j = 0; j < 2; j++)
            {
                float bonusAlphaMult = 1 - 1 * (counter / 12f);
                float dir = j * 2 - 1;
                Vector2 offset = new Vector2(counter * 0.8f * dir, 0).RotatedBy(npc.rotation);
                Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition + offset, npc.frame, new Color(100, 100, 100, 0) * bonusAlphaMult * ((255f - npc.alpha) / 255f), npc.rotation, origin, 1.00f, SpriteEffects.None, 0.0f);
            }
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;   //this make that the npc does not have a health bar
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            DPSregenRate += 0.15f * numPlayers;
            base.ScaleExpertStats(numPlayers, bossLifeScale);
        }
        public override bool CheckActive()
        {
            return false;
        }
        public override void PostAI()
        {
            NPC head = Main.npc[npc.realLife];
            SubspaceSerpentHead subHead = head.modNPC as SubspaceSerpentHead;
            bool phase2 = subHead.hasEnteredSecondPhase;
            float mult = 0.6f;
            if (Main.expertMode)
                mult = 0.65f;
            bool wantsPhase2 = (npc.life < npc.lifeMax * mult && !phase2);
            maxDPS = 250;
            if (wantsPhase2)
                maxDPS = 100;
            if (phase2)
                maxDPS = 350;
            Lighting.AddLight(npc.Center, (255 - npc.alpha) * 2.5f / 255f, (255 - npc.alpha) * 1.6f / 255f, (255 - npc.alpha) * 2.4f / 255f);
            npc.timeLeft = 100000;

            if (currentDPS == -1)
                currentDPS = maxDPS;
            if (currentDPS < maxDPS)
            {
                if(!wantsPhase2)
                    currentDPS += (maxDPS / 60) * DPSregenRate;
                if (phase2 || wantsPhase2)
                {
                    currentDPS += (maxDPS / 180) * DPSregenRate * 0.9f;
                }
            }
            else
            {
                currentDPS = maxDPS;
            }
            if (Main.netMode != 1)
            {
                npc.netUpdate = true;
            }
        }
    }
}