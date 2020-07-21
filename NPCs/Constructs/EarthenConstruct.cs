using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.NPCs.Constructs
{
    public class EarthenConstruct : ModNPC
    {
		int despawn = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Earthen Construct");
		}
        public override void SetDefaults()
        {
            npc.lifeMax = 300;      
            npc.damage = 24;   
            npc.defense = 0;   
            npc.knockBackResist = 0f;
            npc.width = 58; 
            npc.height = 58; 
            npc.lavaImmune = true;
            npc.noGravity = true;         
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.value = 3750;
            npc.npcSlots = 3f;
            npc.netAlways = true;
            npc.behindTiles = true;
			npc.aiStyle = 6;
        }
        public void DoSound()
        {
            int minTilePosX = (int)(npc.position.X / 16.0) - 1;
            int maxTilePosX = (int)((npc.position.X + npc.width) / 16.0) + 2;
            int minTilePosY = (int)(npc.position.Y / 16.0) - 1;
            int maxTilePosY = (int)((npc.position.Y + npc.height) / 16.0) + 2;
            if (minTilePosX < 0)
                minTilePosX = 0;
            if (maxTilePosX > Main.maxTilesX)
                maxTilePosX = Main.maxTilesX;
            if (minTilePosY < 0)
                minTilePosY = 0;
            if (maxTilePosY > Main.maxTilesY)
                maxTilePosY = Main.maxTilesY;

            // This is the initial check for collision with tiles.
            for (int i = minTilePosX; i < maxTilePosX; ++i)
            {
                for (int j = minTilePosY; j < maxTilePosY; ++j)
                {
                    if (Main.tile[i, j] != null && (Main.tile[i, j].nactive() && (Main.tileSolid[(int)Main.tile[i, j].type] || Main.tileSolidTop[(int)Main.tile[i, j].type] && (int)Main.tile[i, j].frameY == 0) || (int)Main.tile[i, j].liquid > 64))
                    {
                        Vector2 vector2;
                        vector2.X = (float)(i * 16);
                        vector2.Y = (float)(j * 16);
                        if (npc.position.X + npc.width > vector2.X && npc.position.X < vector2.X + 16.0 && (npc.position.Y + npc.height > (double)vector2.Y && npc.position.Y < vector2.Y + 16.0))
                        {
                            if (Main.rand.Next(100) == 0 && Main.tile[i, j].nactive())
                                WorldGen.KillTile(i, j, true, true, false);
                        }
                    }
                }
            }

            Vector2 npcCenter = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float targetXPos = Main.player[npc.target].position.X + (Main.player[npc.target].width / 2);
            float targetYPos = Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2);

            float targetRoundedPosX = (float)((int)(targetXPos / 16.0) * 16);
            float targetRoundedPosY = (float)((int)(targetYPos / 16.0) * 16);
            npcCenter.X = (float)((int)(npcCenter.X / 16.0) * 16);
            npcCenter.Y = (float)((int)(npcCenter.Y / 16.0) * 16);
            float dirX = targetRoundedPosX - npcCenter.X;
            float dirY = targetRoundedPosY - npcCenter.Y;

            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);

            if (npc.soundDelay == 0)
            {
                float num1 = length / 40f;
                if (num1 < 10.0)
                    num1 = 10f;
                if (num1 > 20.0)
                    num1 = 20f;
                npc.soundDelay = (int)num1;
                Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 1);
            }
        }
        public void DoWormAI()
        {
            int minTilePosX = (int)(npc.position.X / 16.0) - 1;
            int maxTilePosX = (int)((npc.position.X + npc.width) / 16.0) + 2;
            int minTilePosY = (int)(npc.position.Y / 16.0) - 1;
            int maxTilePosY = (int)((npc.position.Y + npc.height) / 16.0) + 2;
            if (minTilePosX < 0)
                minTilePosX = 0;
            if (maxTilePosX > Main.maxTilesX)
                maxTilePosX = Main.maxTilesX;
            if (minTilePosY < 0)
                minTilePosY = 0;
            if (maxTilePosY > Main.maxTilesY)
                maxTilePosY = Main.maxTilesY;

            bool collision = false;
            // This is the initial check for collision with tiles.
            for (int i = minTilePosX; i < maxTilePosX; ++i)
            {
                for (int j = minTilePosY; j < maxTilePosY; ++j)
                {
                    if (Main.tile[i, j] != null && (Main.tile[i, j].nactive() && (Main.tileSolid[(int)Main.tile[i, j].type] || Main.tileSolidTop[(int)Main.tile[i, j].type] && (int)Main.tile[i, j].frameY == 0) || (int)Main.tile[i, j].liquid > 64))
                    {
                        Vector2 vector2;
                        vector2.X = (float)(i * 16);
                        vector2.Y = (float)(j * 16);
                        if (npc.position.X + npc.width > vector2.X && npc.position.X < vector2.X + 16.0 && (npc.position.Y + npc.height > (double)vector2.Y && npc.position.Y < vector2.Y + 16.0))
                        {
                            collision = true;
                            if (Main.rand.Next(100) == 0 && Main.tile[i, j].nactive())
                                WorldGen.KillTile(i, j, true, true, false);
                        }
                    }
                }
            }
            // If there is no collision with tiles, we check if the distance between this NPC and its target is too large, so that we can still trigger 'collision'.
            if (!collision)
            {
                Rectangle rectangle1 = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
                int maxDistance = 1000;
                bool playerCollision = true;
                for (int index = 0; index < 255; ++index)
                {
                    if (Main.player[index].active)
                    {
                        Rectangle rectangle2 = new Rectangle((int)Main.player[index].position.X - maxDistance, (int)Main.player[index].position.Y - maxDistance, maxDistance * 2, maxDistance * 2);
                        if (rectangle1.Intersects(rectangle2))
                        {
                            playerCollision = false;
                            break;
                        }
                    }
                }
                if (playerCollision)
                    collision = true;
            }

            // speed determines the max speed at which this NPC can move.
            // Higher value = faster speed.
            float speed = 8f;
            // acceleration is exactly what it sounds like. The speed at which this NPC accelerates.
            float acceleration = 0.08f;

            Vector2 npcCenter = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float targetXPos = Main.player[npc.target].position.X + (Main.player[npc.target].width / 2);
            float targetYPos = Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2);

            float targetRoundedPosX = (float)((int)(targetXPos / 16.0) * 16);
            float targetRoundedPosY = (float)((int)(targetYPos / 16.0) * 16);
            npcCenter.X = (float)((int)(npcCenter.X / 16.0) * 16);
            npcCenter.Y = (float)((int)(npcCenter.Y / 16.0) * 16);
            float dirX = targetRoundedPosX - npcCenter.X;
            float dirY = targetRoundedPosY - npcCenter.Y;

            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
            // If we do not have any type of collision, we want the NPC to fall down and de-accelerate along the X axis.
            if (!collision)
            {
                npc.TargetClosest(true);
                npc.velocity.Y = npc.velocity.Y + 0.11f;
                if (npc.velocity.Y > speed)
                    npc.velocity.Y = speed;
                if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed * 0.4)
                {
                    if (npc.velocity.X < 0.0)
                        npc.velocity.X = npc.velocity.X - acceleration * 1.1f;
                    else
                        npc.velocity.X = npc.velocity.X + acceleration * 1.1f;
                }
                else if (npc.velocity.Y == speed)
                {
                    if (npc.velocity.X < dirX)
                        npc.velocity.X = npc.velocity.X + acceleration;
                    else if (npc.velocity.X > dirX)
                        npc.velocity.X = npc.velocity.X - acceleration;
                }
                else if (npc.velocity.Y > 4.0)
                {
                    if (npc.velocity.X < 0.0)
                        npc.velocity.X = npc.velocity.X + acceleration * 0.9f;
                    else
                        npc.velocity.X = npc.velocity.X - acceleration * 0.9f;
                }
            }
            // Else we want to play some audio (soundDelay) and move towards our target.
            else
            {
                if (npc.soundDelay == 0)
                {
                    float num1 = length / 40f;
                    if (num1 < 10.0)
                        num1 = 10f;
                    if (num1 > 20.0)
                        num1 = 20f;
                    npc.soundDelay = (int)num1;
                    Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 1);
                }
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
            }
            // Set the correct rotation for this NPC.
            npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;

            // Some netupdate stuff (multiplayer compatibility).
            if (collision)
            {
                if (npc.localAI[0] != 1)
                    npc.netUpdate = true;
                npc.localAI[0] = 1f;
            }
            else
            {
                if (npc.localAI[0] != 0.0)
                    npc.netUpdate = true;
                npc.localAI[0] = 0.0f;
            }
            if ((npc.velocity.X > 0.0 && npc.oldVelocity.X < 0.0 || npc.velocity.X < 0.0 && npc.oldVelocity.X > 0.0 || (npc.velocity.Y > 0.0 && npc.oldVelocity.Y < 0.0 || npc.velocity.Y < 0.0 && npc.oldVelocity.Y > 0.0)) && !npc.justHit)
                npc.netUpdate = true;
        }
        bool worm = true;
        int[] segments = {-1,-1,-1,-1};
        public override bool PreAI()
        {
            npc.TargetClosest(true);
            if (Main.netMode != 1)
            {
                if (npc.ai[0] == 0)
                {
                    npc.realLife = npc.whoAmI;
                    int latestNPC = npc.whoAmI;
                    int WormLength = 4;
                    for (int i = 1; i < WormLength + 1; ++i)
                    {
                        latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("EarthenConstructTail"), npc.whoAmI, 0, latestNPC);
                        Main.npc[(int)latestNPC].realLife = npc.whoAmI;
                        Main.npc[(int)latestNPC].ai[3] = npc.whoAmI;
                        Main.npc[(int)latestNPC].ai[2] = i;
                        segments[i - 1] = latestNPC;
                    }
 
                    // We're setting npc.ai[0] to 1, so that this 'if' is not triggered again.
                    npc.ai[0] = 1;
                }
                npc.netUpdate = true;
            }
            if (worm) DoWormAI();
            if (!worm) DoSound();
            npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;

            return false; //in order to make sure the vanilla worm ai doesn't crash the game
        }
        int aiMode = -1;
        float aiCounter = 0;
        int extraCounter = 0;
        int eternalCounter = 0;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(worm);
            writer.Write(aiMode);
            writer.Write(aiCounter);
            writer.Write(extraCounter);
            writer.Write(eternalCounter);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            worm = reader.ReadBoolean();
            aiMode = reader.ReadInt32();
            aiCounter = reader.ReadSingle();
            extraCounter = reader.ReadInt32();
            eternalCounter = reader.ReadInt32();
        }
        public override void PostAI()
        {
            Player player = Main.player[npc.target];
            float distToPlayer = (player.Center - npc.Center).Length();
            aiCounter++;
            eternalCounter++;
            int i = (int)npc.Center.X / 16;
            int j = (int)npc.Center.Y / 16;
            if (distToPlayer <= 256 && aiCounter >= 196 && aiMode == -1)
            {
                aiMode = 1;
                aiCounter = 0;
                worm = false;
            }
            if(aiMode == 1)
            {
                Tile tile = Framing.GetTileSafely(i, j - 1);
                Vector2 goTo = player.Center;
                if (!tile.active() && extraCounter < 256)
                {
                    extraCounter += 4;
                }
                goTo.Y += 256 + extraCounter;
                Vector2 circularLocation = new Vector2(256, 0).RotatedBy(MathHelper.ToRadians(eternalCounter));
                goTo.X += circularLocation.X;
                Vector2 velo = goTo - npc.Center;
                velo.Normalize();
                if (12 >= (goTo - npc.Center).Length())
                {
                    velo *= (goTo - npc.Center).Length();
                }
                else
                {
                    velo *= 12;
                }
                npc.velocity = velo;
                if(aiCounter >= 196)
                {
                    aiCounter = 0;
                    extraCounter = 0;
                    npc.velocity *= 0;
                    aiMode = 2;
                }
            }
            if(aiMode == 2)
            {
                Tile tile = Framing.GetTileSafely(i, j);
                npc.velocity.Y = -6f;
                bool lineOfSight = Collision.CanHitLine(npc.position, npc.width, npc.height, npc.position + new Vector2(0, -128), npc.width, npc.height);
                if (((!tile.active() || !Main.tileSolid[tile.type]) && lineOfSight) || npc.Center.Y < player.Center.Y)
                {
                    aiCounter = 0;
                    aiMode = 3;
                    npc.velocity.Y = 0;
                    extraCounter = 0;
                }
            }
            if(aiMode >= 3 && aiMode <= 5)
            {
                npc.velocity.X = 0;
                npc.velocity.Y = -1;
                if (aiCounter >= 30)
                {
                    LaunchBeams();
                    aiCounter = 0;
                    aiMode++;
                    npc.position.Y += 30;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int x = 0; x < 4; x++)
                        {
                            if (segments[x] != -1)
                            {
                                NPC segment = Main.npc[segments[x]];
                                if (segment.active && segment.type == mod.NPCType("EarthenConstructTail") && (int)segment.ai[1] == npc.whoAmI)
                                {
                                    segment.position.Y += 30;
                                    segment.netUpdate = true;
                                }
                            }
                        }
                    }
                }
            }
            if(aiMode == 6)
            {
                aiMode = -1;
                aiCounter = 0;
                worm = true;
            }
            doAIExtras();
        }
        public void LaunchBeams()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int damage = npc.damage / 2;
                if (Main.expertMode)
                {
                    damage = (int)(damage / Main.expertDamage);
                }
                int count = 3;
                if (Main.expertMode)
                {
                    count += 2;
                }
                for(int i = 0; i < count; i++)
                {
                    Vector2 circularLocation = new Vector2(0, -4).RotatedByRandom(MathHelper.ToRadians(17 + count));
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, circularLocation.X, circularLocation.Y, mod.ProjectileType("EarthenShot"), damage, 0, Main.myPlayer);
                }
            }
            Main.PlaySound(SoundID.Item92, (int)(npc.Center.X), (int)(npc.Center.Y));
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("NPCs/Constructs/EarthenConstructHead");
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Texture2D texture2 = mod.GetTexture("NPCs/Constructs/EarthenConstruct");
            Vector2 origin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
            Main.spriteBatch.Draw(texture2, npc.Center - Main.screenPosition, new Rectangle?(), drawColor, npc.rotation - MathHelper.ToRadians(npc.localAI[1]), origin2, npc.scale + 0.04f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, new Rectangle?(), drawColor, npc.rotation - MathHelper.ToRadians(90), origin, npc.scale, SpriteEffects.None, 0);
            return false;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1f;   //this make the NPC Health Bar biger
            return null;
        }
        bool gennedGore = false;
        public void genGore(int hitDirection)
        {
            for (int k = 0; k < 20; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, 82, 2.5f * (float)hitDirection, -2.5f, 0, default(Color), 0.7f);
            }
            for (int i = 0; i < 30; i++)
            {
                int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, mod.DustType("BigEarthDust"));
                Main.dust[dust].velocity *= 5f;
            }
            Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/EarthenConstructGore1"), 1f);
            Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/EarthenConstructGore2"), 1f);
            Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/EarthenConstructGore3"), 1f);
            Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/EarthenConstructGore4"), 1f);
            Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/EarthenConstructGore5"), 1f);

            for (int i = 0; i < 9; i++)
                Gore.NewGore(npc.position, npc.velocity, Main.rand.Next(61, 64), 1f);
            gennedGore = true;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                if (!gennedGore) genGore(hitDirection);
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Player player = spawnInfo.player;
            if(player.ZoneDesert || player.ZoneUndergroundDesert || (player.ZoneRockLayerHeight && !player.ZoneDungeon && !player.ZoneJungle && !player.ZoneSnow))
            {
                if(player.ZoneCorrupt || player.ZoneHoly)
                {
                    return 0.0025f;
                }
                if(player.ZoneRockLayerHeight)
                {
                    return 0.0075f;
                }    
                return 0.01f;
                
            }
            return 0;
        }
        public override void NPCLoot()
        {
            if (!gennedGore) genGore(0);

            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                int j = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("EarthenSpirit"), 0, 0, 0);
                Main.npc[j].velocity.Y = -10f;
                Main.npc[j].netUpdate = true;

                int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("EarthenSpirit"), 0, 0, 1, j);
                Main.npc[n].velocity = new Vector2(1, -9f);
                Main.npc[n].netUpdate = true;

                n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("EarthenSpirit"), 0, 0, 2, j);
                Main.npc[n].velocity = new Vector2(-1, -9f);
                Main.npc[n].netUpdate = true;

                n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("EarthenSpirit"), 0, 0, 3, j);
                Main.npc[n].velocity = new Vector2(2, -8f);
                Main.npc[n].netUpdate = true;

                n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("EarthenSpirit"), 0, 0, 4, j);
                Main.npc[n].velocity = new Vector2(-2, -8f);
                Main.npc[n].netUpdate = true;
            }
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FragmentOfEarth"), Main.rand.Next(4) + 4);
        }
        public void doAIExtras()
        {
            npc.localAI[1] += 3;
            if (Main.player[npc.target].dead)
			{
			    despawn++;
			}
			if(despawn >= 720)
			{
		    	npc.active = false;
			}
			npc.timeLeft = 100;
		}
    }
}