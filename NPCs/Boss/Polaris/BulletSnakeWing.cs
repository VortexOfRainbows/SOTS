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
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            NPCID.Sets.DebuffImmunitySets.Add(Type, new Terraria.DataStructures.NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[]
                {
                    BuffID.Poisoned,
                    BuffID.Frostburn,
                    BuffID.Ichor,
                    BuffID.OnFire
                }
            });
        }
        public override void SetDefaults()
        {
            NPC.width = 58; 
            NPC.height = 76; 
            NPC.damage = 60;
            NPC.defense = 40;
            NPC.lifeMax = 20000;  
            NPC.knockBackResist = 0.0f;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
            NPC.noGravity = true;
            NPC.dontCountMe = true;
            NPC.value = 0;
        }
        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.damage = (int)(NPC.damage * 0.75f);
        }
        public override bool PreAI()
        {
            if (NPC.ai[3] > 0)
                NPC.realLife = (int)NPC.ai[3];
            if (NPC.target < 0 || NPC.target == byte.MaxValue || Main.player[NPC.target].dead)
                NPC.TargetClosest(true);
            if (Main.player[NPC.target].dead && NPC.timeLeft > 300)
                NPC.timeLeft = 300;
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
                NPC lastNpc = Main.npc[(int)NPC.ai[1]];
                Vector2 npcCenter = NPC.Center;
                Vector2 dir = lastNpc.Center - npcCenter;
                NPC.rotation = dir.ToRotation() + 1.57f;
                float length = dir.Length();
                float height = lastNpc.height - 8;
                float dist = (length - height) / length;
                Vector2 Pos = dir * dist;
                NPC.velocity = Vector2.Zero;
                velocity = Pos;
                NPC.position = NPC.position + Pos;
                if (dir.X > 0)
                    NPC.direction = 1;
                else
                    NPC.direction = -1;
            }
            NPC.spriteDirection = NPC.direction;
            return false;
        }
        Vector2 velocity = Vector2.Zero;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            spriteBatch.Draw(texture, NPC.Center - screenPos, null, Color.White, NPC.rotation, origin, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : 0, 0);
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
                int num1 = Dust.NewDust(NPC.Center - new Vector2(5) + new Vector2(-14 * NPC.spriteDirection, 32).RotatedBy(NPC.rotation) + velocity * mult, 0, 0, ModContent.DustType<Dusts.CopyDust4>());
                Dust dust = Main.dust[num1];
                dust.velocity *= 1.0f;
                dust.velocity += new Vector2(0, 15).RotatedBy(NPC.rotation + MathHelper.ToRadians(Main.rand.NextFloat(-10, 10)));
                dust.noGravity = true;
                dust.scale += 0.25f;
                dust.color = new Color(255, 70, 70, 100);
                dust.fadeIn = 0.1f;
                dust.scale *= 1.5f;
            }
            Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 0.9f / 255f, (255 - NPC.alpha) * 0.1f / 255f, (255 - NPC.alpha) * 0.3f / 255f);
			NPC.timeLeft = 100;
		}
    }
}