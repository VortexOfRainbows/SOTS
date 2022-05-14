using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.NPCs.Boss.Polaris
{   
    public class BulletSnakeWing : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bullet Snake");
		}
        public override void SetDefaults()
        {
            NPC.width = 58; 
            NPC.height = 76; 
            NPC.damage = 60;
            NPC.defense = 40;
            NPC.lifeMax = 20000;  
            NPC.knockBackResist = 0.0f;
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
            NPC.damage = (int)(npc.damage * 0.75f);
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
            if (npc.ai[1] < (double)Main.npc.Length)
            {
                NPC lastNpc = Main.npc[(int)npc.ai[1]];
                Vector2 npcCenter = npc.Center;
                Vector2 dir = lastNpc.Center - npcCenter;
                npc.rotation = dir.ToRotation() + 1.57f;
                float length = dir.Length();
                float height = lastNpc.height - 8;
                float dist = (length - height) / length;
                Vector2 Pos = dir * dist;
                npc.velocity = Vector2.Zero;
                velocity = Pos;
                npc.position = npc.position + Pos;
                if (dir.X > 0)
                    npc.direction = 1;
                else
                    npc.direction = -1;
            }
            npc.spriteDirection = npc.direction;
            return false;
        }
        Vector2 velocity = Vector2.Zero;
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Npc[npc.type].Value;
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
            for (int i = 0; i < 5; i++)
            {
                float mult = 0.2f * i;
                int num1 = Dust.NewDust(npc.Center - new Vector2(5) + new Vector2(-14 * npc.spriteDirection, 32).RotatedBy(npc.rotation) + velocity * mult, 0, 0, mod.DustType("CopyDust4"));
                Dust dust = Main.dust[num1];
                dust.velocity *= 1.0f;
                dust.velocity += new Vector2(0, 15).RotatedBy(npc.rotation + MathHelper.ToRadians(Main.rand.NextFloat(-10, 10)));
                dust.noGravity = true;
                dust.scale += 0.25f;
                dust.color = new Color(255, 70, 70, 100);
                dust.fadeIn = 0.1f;
                dust.scale *= 1.5f;
            }
            Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.9f / 255f, (255 - npc.alpha) * 0.1f / 255f, (255 - npc.alpha) * 0.3f / 255f);
			npc.timeLeft = 100;
		}
    }
}