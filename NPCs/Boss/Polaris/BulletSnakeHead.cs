using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.NPCs.Boss.Polaris
{
    public class BulletSnakeHead : ModNPC
    {
		int despawn = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bullet Snake");
		}
        public override void SetDefaults()
        {
            NPC.aiStyle =0;
            NPC.lifeMax = 5500;      
            NPC.damage = 150;  
            NPC.defense = 0;      
            NPC.knockBackResist = 0f;
            NPC.width = 62; 
            NPC.height = 74;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.value = 0;
            NPC.npcSlots = 0;
            NPC.netAlways = true;
            NPC.buffImmune[BuffID.Frostburn] = true;
            NPC.buffImmune[BuffID.Ichor] = true;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.ai[1] = -1;
        }
        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }
        public override bool PreKill()
        {
            return false;
        }
        public void DoWormStuff()
        {
            float speed = 17.5f;
            float acceleration = 0.2f;
            Vector2 npcCenter = NPC.Center;
            Vector2 targetPos = Main.player[NPC.target].Center;
            float targetRoundedPosX = targetPos.X;
            float targetRoundedPosY = targetPos.Y;
            float dirX = targetRoundedPosX - npcCenter.X;
            float dirY = targetRoundedPosY - npcCenter.Y;
            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
            float absDirX = Math.Abs(dirX);
            float absDirY = Math.Abs(dirY);
            float newSpeed = speed / length;
            dirX = dirX * newSpeed;
            dirY = dirY * newSpeed;
            if (NPC.velocity.X > 0.0 && dirX > 0.0 || NPC.velocity.X < 0.0 && dirX < 0.0 || (NPC.velocity.Y > 0.0 && dirY > 0.0 || NPC.velocity.Y < 0.0 && dirY < 0.0))
            {
                if (NPC.velocity.X < dirX)
                    NPC.velocity.X = NPC.velocity.X + acceleration;
                else if (NPC.velocity.X > dirX)
                    NPC.velocity.X = NPC.velocity.X - acceleration;
                if (NPC.velocity.Y < dirY)
                    NPC.velocity.Y = NPC.velocity.Y + acceleration;
                else if (NPC.velocity.Y > dirY)
                    NPC.velocity.Y = NPC.velocity.Y - acceleration;
                if (Math.Abs(dirY) < speed * 0.2 && (NPC.velocity.X > 0.0 && dirX < 0.0 || NPC.velocity.X < 0.0 && dirX > 0.0))
                {
                    if (NPC.velocity.Y > 0.0)
                        NPC.velocity.Y = NPC.velocity.Y + acceleration * 2f;
                    else
                        NPC.velocity.Y = NPC.velocity.Y - acceleration * 2f;
                }
                if (Math.Abs(dirX) < speed * 0.2 && (NPC.velocity.Y > 0.0 && dirY < 0.0 || NPC.velocity.Y < 0.0 && dirY > 0.0))
                {
                    if (NPC.velocity.X > 0.0)
                        NPC.velocity.X = NPC.velocity.X + acceleration * 2f;
                    else
                        NPC.velocity.X = NPC.velocity.X - acceleration * 2f;
                }
            }
            else if (absDirX > absDirY)
            {
                if (NPC.velocity.X < dirX)
                    NPC.velocity.X = NPC.velocity.X + acceleration * 1.1f;
                else if (NPC.velocity.X > dirX)
                    NPC.velocity.X = NPC.velocity.X - acceleration * 1.1f;
                if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < speed * 0.5)
                {
                    if (NPC.velocity.Y > 0.0)
                        NPC.velocity.Y = NPC.velocity.Y + acceleration;
                    else
                        NPC.velocity.Y = NPC.velocity.Y - acceleration;
                }
            }
            else
            {
                if (NPC.velocity.Y < dirY)
                    NPC.velocity.Y = NPC.velocity.Y + acceleration * 1.1f;
                else if (NPC.velocity.Y > dirY)
                    NPC.velocity.Y = NPC.velocity.Y - acceleration * 1.1f;
                if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < speed * 0.5)
                {
                    if (NPC.velocity.X > 0.0)
                        NPC.velocity.X = NPC.velocity.X + acceleration;
                    else
                        NPC.velocity.X = NPC.velocity.X - acceleration;
                }
            }
            if (NPC.localAI[0] != 1)
                NPC.netUpdate = true;
            NPC.localAI[0] = 1f;
            if ((NPC.velocity.X > 0.0 && NPC.oldVelocity.X < 0.0 || NPC.velocity.X < 0.0 && NPC.oldVelocity.X > 0.0 || (NPC.velocity.Y > 0.0 && NPC.oldVelocity.Y < 0.0 || NPC.velocity.Y < 0.0 && NPC.oldVelocity.Y > 0.0)) && !NPC.justHit)
                NPC.netUpdate = true;
        }
        public override bool PreAI()
        {
            if (Main.netMode != 1)
            {
                if (NPC.ai[0] == 0)
                {
                    NPC.realLife = NPC.whoAmI;
                    int latestNPC = NPC.whoAmI;
 
                    latestNPC = NPC.NewNPC(NPC.GetSource_FromThis("SOTS:WormEnemy"), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<BulletSnakeWing>(), NPC.whoAmI, 0, latestNPC, 0, NPC.whoAmI);
                    Main.npc[latestNPC].realLife = NPC.whoAmI;
                    NPC.ai[1] = latestNPC;
                    for (int i = 0; i < 6; ++i)
                    {
                        latestNPC = NPC.NewNPC(NPC.GetSource_FromThis("SOTS:WormEnemy"), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<BulletSnakeBody>(), NPC.whoAmI, 0, latestNPC, 0, NPC.whoAmI);
                        Main.npc[latestNPC].realLife = NPC.whoAmI;
                    }
                    latestNPC = NPC.NewNPC(NPC.GetSource_FromThis("SOTS:WormEnemy"), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<BulletSnakeEnd>(), NPC.whoAmI, 0, latestNPC, 0, NPC.whoAmI);
                    Main.npc[latestNPC].realLife = NPC.whoAmI;
                    NPC.ai[0] = 1;
                    NPC.netUpdate = true;
                }
            }
            NPC.TargetClosest(false);
            DoWormStuff();
            if(NPC.ai[1] != -1)
            {
                NPC wings = Main.npc[(int)NPC.ai[1]];
                Vector2 toWings = wings.Center - NPC.Center;
                if (NPC.velocity.X > 0)
                    NPC.direction = 1;
                else
                    NPC.direction = -1;
                NPC.spriteDirection = NPC.direction;
                NPC.rotation = toWings.ToRotation() - (float)Math.PI / 2f;
            }
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            spriteBatch.Draw(texture, NPC.Center - screenPos, null, Color.White, NPC.rotation, origin, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : 0, 0);
            return false;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1f;   //this make the NPC Health Bar biger
            return null;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.75f * bossLifeScale);  //boss life scale in expertmode
            NPC.damage = (int)(NPC.damage * 0.75f);  //boss damage increase in expermode
        }
        public override void PostAI()
		{
			if(!NPC.AnyNPCs(ModContent.NPCType<Polaris>()))
			{
				//npc.velocity *= 0.9f;
			}
			Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 0.9f / 255f, (255 - NPC.alpha) * 0.1f / 255f, (255 - NPC.alpha) * 0.3f / 255f);
			if(Main.player[NPC.target].dead)
			    despawn++;
			if(despawn >= 600)
			    NPC.active = false;
			NPC.timeLeft = 100;
		}
    }
}