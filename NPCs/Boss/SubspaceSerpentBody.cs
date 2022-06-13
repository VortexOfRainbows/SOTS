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
            NPC.width = 48;
            NPC.height = 34;
            NPC.damage = 70;
            NPC.defense = 100;
            NPC.lifeMax = 130000;
            NPC.knockBackResist = 0.0f;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
            NPC.noGravity = true;
            NPC.dontCountMe = true;
            NPC.value = 10000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath32;
            //Music = MusicID.Boss2;
            for (int i = 0; i < Main.maxBuffTypes; i++)
            {
                NPC.buffImmune[i] = true;
            }
            Main.npcFrameCount[NPC.type] = 8;
        }
        float currentDPS = -1;
        private float DPSregenRate = 0.1f;
        float maxDPS = 250;
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return !NPC.dontTakeDamage;
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
            writer.Write(NPC.knockBackResist);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.knockBackResist = reader.ReadSingle();
        }
        public override void FindFrame(int frameHeight)
        {
            NPC parent = Main.npc[(int)NPC.ai[1]];
            int frameHeight2 = frameHeight;
            if(parent.type == ModContent.NPCType<SubspaceSerpentHead>())
            {
                frameHeight2 = 44;
            }
            NPC.alpha = parent.alpha;
            NPC.dontTakeDamage = parent.dontTakeDamage;
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
            if (currentFrame > 7)
                currentFrame = 0;
            NPC.frame.Y = currentFrame * frameHeight;

        }
        public override bool PreAI()
        {
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
                }
                if (!NPC.active && Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, NPC.whoAmI, -1f, 0f, 0f, 0, 0, 0);
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
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/Boss/SubspaceSerpentBodyFill").Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, NPC.height * 0.5f);
            float percentShield = (maxDPS - currentDPS) / maxDPS;
            NPC head = Main.npc[NPC.realLife];
            SubspaceSerpentHead subHead = head.ModNPC as SubspaceSerpentHead;
            bool phase2 = subHead.hasEnteredSecondPhase;
            if (phase2)
                percentShield = 0.3334f;
            if (percentShield > 0 || phase2)
            {
                Color color = new Color(phase2 ? 0 : 255, phase2 ? 255 : 0, 0);
                for (int i = 0; i < 2; i++)
                {
                    int direction = i * 2 - 1;
                    Vector2 toTheSide = new Vector2(6 * percentShield * direction, 0).RotatedBy(NPC.rotation);
                    spriteBatch.Draw(texture, NPC.Center - screenPos + toTheSide, NPC.frame, color * ((255f - NPC.alpha) / 255f) * ((255f - NPC.alpha) / 255f), NPC.rotation, origin, 1f, SpriteEffects.None, 0);
                }
            }
            texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, drawColor * ((255f - NPC.alpha) / 255f), NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
            return false;
        }
        int counter = 0;
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/Boss/SubspaceSerpentBodyGlow").Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, NPC.height * 0.5f);
            spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, Color.White * ((255f - NPC.alpha) / 255f), NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
            counter++;
            if (counter > 12)
                counter = 0;
            for (int j = 0; j < 2; j++)
            {
                float bonusAlphaMult = 1 - 1 * (counter / 12f);
                float dir = j * 2 - 1;
                Vector2 offset = new Vector2(counter * 0.8f * dir, 0).RotatedBy(NPC.rotation);
                spriteBatch.Draw(texture, NPC.Center - screenPos + offset, NPC.frame, new Color(100, 100, 100, 0) * bonusAlphaMult * ((255f - NPC.alpha) / 255f), NPC.rotation, origin, 1.00f, SpriteEffects.None, 0.0f);
            }
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;   //this make that the npc does not have a health bar
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * bossLifeScale * 0.75f);  //boss life scale in expertmode
            DPSregenRate += 0.15f * numPlayers;
            base.ScaleExpertStats(numPlayers, bossLifeScale);
        }
        public override bool CheckActive()
        {
            return false;
        }
        public override void PostAI()
        {
            NPC head = Main.npc[NPC.realLife];
            SubspaceSerpentHead subHead = head.ModNPC as SubspaceSerpentHead;
            bool phase2 = subHead.hasEnteredSecondPhase;
            float mult = 0.6f;
            if (Main.expertMode)
                mult = 0.65f;
            bool wantsPhase2 = (NPC.life < NPC.lifeMax * mult && !phase2);
            maxDPS = 250;
            if (wantsPhase2)
                maxDPS = 100;
            if (phase2)
                maxDPS = 350;
            Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 2.5f / 255f, (255 - NPC.alpha) * 1.6f / 255f, (255 - NPC.alpha) * 2.4f / 255f);
            NPC.timeLeft = 100000;

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
                NPC.netUpdate = true;
            }
        }
    }
}