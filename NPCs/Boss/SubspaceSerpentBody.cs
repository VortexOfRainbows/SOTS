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
            npc.width = 36;
            npc.height = 40;
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
        }
        float currentDPS = -1;
        float DPSregenRate = 0.1f;
        float maxDPS = 250;
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
                // We're getting the center of this NPC.
                Vector2 npcCenter = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                // Then using that center, we calculate the direction towards the 'parent NPC' of this NPC.
                float dirX = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - npcCenter.X;
                float dirY = Main.npc[(int)npc.ai[1]].position.Y + (float)(Main.npc[(int)npc.ai[1]].height / 2) - npcCenter.Y;
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

        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Main.npcTexture[npc.type];
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, new Rectangle?(), drawColor, npc.rotation, origin, npc.scale, SpriteEffects.None, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("NPCs/Boss/DPSBarrier2");
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, new Rectangle?(), drawColor * (0.5f + (0.25f * (maxDPS - currentDPS) / maxDPS)), 0, origin, (maxDPS - currentDPS) / maxDPS, SpriteEffects.None, 0);
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