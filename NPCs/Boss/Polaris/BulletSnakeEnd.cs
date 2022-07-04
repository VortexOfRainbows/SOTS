using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Permafrost;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.NPCs.Boss.Polaris
{   
    public class BulletSnakeEnd : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bullet Snake");
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
            NPC.width = 38;
            NPC.height = 48;
            NPC.damage = 40;
            NPC.defense = 20;
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
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
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
                Vector2 npcCenter = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                float dirX = lastNpc.position.X + (float)(lastNpc.width / 2) - npcCenter.X;
                float dirY = lastNpc.position.Y + (float)(lastNpc.height / 2) - npcCenter.Y;
                NPC.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                float height = lastNpc.height - 2;
                float dist = (length - height) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;
 
                NPC.velocity = Vector2.Zero;
                NPC.position.X = NPC.position.X + posX;
                NPC.position.Y = NPC.position.Y + posY;
                velocity = new Vector2(posX, posY);
                if (dirX > 0)
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
            if (Main.netMode != 1)
            {
                int damage = NPC.GetBaseDamage();
                NPC.ai[0]++;
                if (NPC.ai[0] >= 20)
                {
                    NPC.ai[0] = 0;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, 4).RotatedBy(NPC.rotation), ModContent.ProjectileType<PolarBullet>(), damage, 0, Main.myPlayer, 0f, 0f);
                }
                /* for (int i = 0; i < 3; i++)
                {
                    float mult = 0.33f * i;
                    int num1 = Dust.NewDust(npc.Center - new Vector2(5) + new Vector2(0, 20).RotatedBy(npc.rotation) + mult * velocity, 0, 0, mod.DustType("CopyDust4"));
                    Dust dust = Main.dust[num1];
                    dust.velocity *= 1.0f;
                    dust.velocity += new Vector2(0, 12).RotatedBy(npc.rotation + MathHelper.ToRadians(Main.rand.NextFloat(-15, 15)));
                    dust.noGravity = true;
                    dust.scale += 0.25f;
                    dust.color = new Color(255, 70, 70, 100);
                    dust.fadeIn = 0.1f;
                    dust.scale *= 1.25f;
                }*/
            }
            Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 0.9f / 255f, (255 - NPC.alpha) * 0.1f / 255f, (255 - NPC.alpha) * 0.3f / 255f);
			NPC.timeLeft = 100;
		}
    }
}