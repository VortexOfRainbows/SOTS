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
            npc.aiStyle = 0;
            npc.lifeMax = 5500;      
            npc.damage = 150;  
            npc.defense = 0;      
            npc.knockBackResist = 0f;
            npc.width = 62; 
            npc.height = 74;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.value = 0;
            npc.npcSlots = 0;
            npc.netAlways = true;
            npc.buffImmune[BuffID.Frostburn] = true;
            npc.buffImmune[BuffID.Ichor] = true;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.ai[1] = -1;
        }
        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }
        public override bool PreNPCLoot()
        {
            return false;
        }
        public void DoWormStuff()
        {
            float speed = 17.5f;
            float acceleration = 0.2f;
            Vector2 npcCenter = npc.Center;
            Vector2 targetPos = Main.player[npc.target].Center;
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
            if (npc.velocity.X > 0.0 && dirX > 0.0 || npc.velocity.X < 0.0 && dirX < 0.0 || (npc.velocity.Y > 0.0 && dirY > 0.0 || npc.velocity.Y < 0.0 && dirY < 0.0))
            {
                if (npc.velocity.X < dirX)
                    npc.velocity.X = npc.velocity.X + acceleration;
                else if (npc.velocity.X > dirX)
                    npc.velocity.X = npc.velocity.X - acceleration;
                if (npc.velocity.Y < dirY)
                    npc.velocity.Y = npc.velocity.Y + acceleration;
                else if (npc.velocity.Y > dirY)
                    npc.velocity.Y = npc.velocity.Y - acceleration;
                if (Math.Abs(dirY) < speed * 0.2 && (npc.velocity.X > 0.0 && dirX < 0.0 || npc.velocity.X < 0.0 && dirX > 0.0))
                {
                    if (npc.velocity.Y > 0.0)
                        npc.velocity.Y = npc.velocity.Y + acceleration * 2f;
                    else
                        npc.velocity.Y = npc.velocity.Y - acceleration * 2f;
                }
                if (Math.Abs(dirX) < speed * 0.2 && (npc.velocity.Y > 0.0 && dirY < 0.0 || npc.velocity.Y < 0.0 && dirY > 0.0))
                {
                    if (npc.velocity.X > 0.0)
                        npc.velocity.X = npc.velocity.X + acceleration * 2f;
                    else
                        npc.velocity.X = npc.velocity.X - acceleration * 2f;
                }
            }
            else if (absDirX > absDirY)
            {
                if (npc.velocity.X < dirX)
                    npc.velocity.X = npc.velocity.X + acceleration * 1.1f;
                else if (npc.velocity.X > dirX)
                    npc.velocity.X = npc.velocity.X - acceleration * 1.1f;
                if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed * 0.5)
                {
                    if (npc.velocity.Y > 0.0)
                        npc.velocity.Y = npc.velocity.Y + acceleration;
                    else
                        npc.velocity.Y = npc.velocity.Y - acceleration;
                }
            }
            else
            {
                if (npc.velocity.Y < dirY)
                    npc.velocity.Y = npc.velocity.Y + acceleration * 1.1f;
                else if (npc.velocity.Y > dirY)
                    npc.velocity.Y = npc.velocity.Y - acceleration * 1.1f;
                if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed * 0.5)
                {
                    if (npc.velocity.X > 0.0)
                        npc.velocity.X = npc.velocity.X + acceleration;
                    else
                        npc.velocity.X = npc.velocity.X - acceleration;
                }
            }
            if (npc.localAI[0] != 1)
                npc.netUpdate = true;
            npc.localAI[0] = 1f;
            if ((npc.velocity.X > 0.0 && npc.oldVelocity.X < 0.0 || npc.velocity.X < 0.0 && npc.oldVelocity.X > 0.0 || (npc.velocity.Y > 0.0 && npc.oldVelocity.Y < 0.0 || npc.velocity.Y < 0.0 && npc.oldVelocity.Y > 0.0)) && !npc.justHit)
                npc.netUpdate = true;
        }
        public override bool PreAI()
        {
            if (Main.netMode != 1)
            {
                if (npc.ai[0] == 0)
                {
                    npc.realLife = npc.whoAmI;
                    int latestNPC = npc.whoAmI;
 
                    latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<BulletSnakeWing>(), npc.whoAmI, 0, latestNPC, 0, npc.whoAmI);
                    Main.npc[latestNPC].realLife = npc.whoAmI;
                    npc.ai[1] = latestNPC;
                    for (int i = 0; i < 6; ++i)
                    {
                        latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<BulletSnakeBody>(), npc.whoAmI, 0, latestNPC, 0, npc.whoAmI);
                        Main.npc[latestNPC].realLife = npc.whoAmI;
                    }
                    latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<BulletSnakeEnd>(), npc.whoAmI, 0, latestNPC, 0, npc.whoAmI);
                    Main.npc[latestNPC].realLife = npc.whoAmI;
                    npc.ai[0] = 1;
                    npc.netUpdate = true;
                }
            }
            npc.TargetClosest(false);
            DoWormStuff();
            if(npc.ai[1] != -1)
            {
                NPC wings = Main.npc[(int)npc.ai[1]];
                Vector2 toWings = wings.Center - npc.Center;
                if (npc.velocity.X > 0)
                    npc.direction = 1;
                else
                    npc.direction = -1;
                npc.spriteDirection = npc.direction;
                npc.rotation = toWings.ToRotation() - (float)Math.PI / 2f;
            }
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
            scale = 1f;   //this make the NPC Health Bar biger
            return null;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.75f * bossLifeScale);  //boss life scale in expertmode
            npc.damage = (int)(npc.damage * 0.75f);  //boss damage increase in expermode
        }
        public override void PostAI()
		{
			if(!NPC.AnyNPCs(ModContent.NPCType<Polaris>()))
			{
				//npc.velocity *= 0.9f;
			}
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.9f / 255f, (255 - npc.alpha) * 0.1f / 255f, (255 - npc.alpha) * 0.3f / 255f);
			if(Main.player[npc.target].dead)
			    despawn++;
			if(despawn >= 600)
			    npc.active = false;
			npc.timeLeft = 100;
		}
    }
}