using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Earth;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.NPCs.Constructs
{
    public class EarthenConstruct : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Earthen Construct");
		}
        public override void SetDefaults()
        {
            NPC.lifeMax = 225;      
            NPC.damage = 16;   
            NPC.defense = 12;   
            NPC.knockBackResist = 0f;
            NPC.width = 58; 
            NPC.height = 58; 
            NPC.lavaImmune = true;
            NPC.noGravity = true;         
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.value = 5050;
            NPC.npcSlots = 3f;
            NPC.behindTiles = true;
			NPC.aiStyle =-1;
            NPC.rarity = 5;
        }
        public void DoSound()
        {
            int minTilePosX = (int)(NPC.position.X / 16.0) - 1;
            int maxTilePosX = (int)((NPC.position.X + NPC.width) / 16.0) + 2;
            int minTilePosY = (int)(NPC.position.Y / 16.0) - 1;
            int maxTilePosY = (int)((NPC.position.Y + NPC.height) / 16.0) + 2;
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
                    if (Main.tile[i, j] != null && (Main.tile[i, j].HasUnactuatedTile && (Main.tileSolid[(int)Main.tile[i, j ].TileType] || Main.tileSolidTop[(int)Main.tile[i, j ].TileType] && (int)Main.tile[i, j].TileFrameY == 0) || (int)Main.tile[i, j].liquid > 64))
                    {
                        Vector2 vector2;
                        vector2.X = (float)(i * 16);
                        vector2.Y = (float)(j * 16);
                        if (NPC.position.X + NPC.width > vector2.X && NPC.position.X < vector2.X + 16.0 && (NPC.position.Y + NPC.height > (double)vector2.Y && NPC.position.Y < vector2.Y + 16.0))
                        {
                            if (Main.rand.Next(100) == 0 && Main.tile[i, j].HasUnactuatedTile)
                                WorldGen.KillTile(i, j, true, true, false);
                        }
                    }
                }
            }

            Vector2 npcCenter = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
            float targetXPos = Main.player[NPC.target].position.X + (Main.player[NPC.target].width / 2);
            float targetYPos = Main.player[NPC.target].position.Y + (Main.player[NPC.target].height / 2);

            float targetRoundedPosX = (float)((int)(targetXPos / 16.0) * 16);
            float targetRoundedPosY = (float)((int)(targetYPos / 16.0) * 16);
            npcCenter.X = (float)((int)(npcCenter.X / 16.0) * 16);
            npcCenter.Y = (float)((int)(npcCenter.Y / 16.0) * 16);
            float dirX = targetRoundedPosX - npcCenter.X;
            float dirY = targetRoundedPosY - npcCenter.Y;

            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);

            if (NPC.soundDelay == 0)
            {
                float num1 = length / 40f;
                if (num1 < 15.0)
                    num1 = 15f;
                if (num1 > 25.0)
                    num1 = 25f;
                NPC.soundDelay = (int)num1;
                SoundEngine.PlaySound(SoundLoader.customSoundType, (int)NPC.Center.X, (int)NPC.Center.Y, Mod.GetSoundSlot(SoundType.Custom, "Sounds/Enemies/EarthenElementalDig"), 1.0f, 0f);
            }
        }
        public void DoWormAI()
        {
            int minTilePosX = (int)(NPC.position.X / 16.0) - 1;
            int maxTilePosX = (int)((NPC.position.X + NPC.width) / 16.0) + 2;
            int minTilePosY = (int)(NPC.position.Y / 16.0) - 1;
            int maxTilePosY = (int)((NPC.position.Y + NPC.height) / 16.0) + 2;
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
                    if (Main.tile[i, j] != null && (Main.tile[i, j].HasUnactuatedTile && (Main.tileSolid[(int)Main.tile[i, j ].TileType] || Main.tileSolidTop[(int)Main.tile[i, j ].TileType] && (int)Main.tile[i, j].TileFrameY == 0) || (int)Main.tile[i, j].liquid > 64))
                    {
                        Vector2 vector2;
                        vector2.X = (float)(i * 16);
                        vector2.Y = (float)(j * 16);
                        if (NPC.position.X + NPC.width > vector2.X && NPC.position.X < vector2.X + 16.0 && (NPC.position.Y + NPC.height > (double)vector2.Y && NPC.position.Y < vector2.Y + 16.0))
                        {
                            collision = true;
                            if (Main.rand.Next(100) == 0 && Main.tile[i, j].HasUnactuatedTile)
                                WorldGen.KillTile(i, j, true, true, false);
                        }
                    }
                }
            }
            // If there is no collision with tiles, we check if the distance between this NPC and its target is too large, so that we can still trigger 'collision'.
            if (!collision)
            {
                Rectangle rectangle1 = new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height);
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

            Vector2 npcCenter = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
            float targetXPos = Main.player[NPC.target].position.X + (Main.player[NPC.target].width / 2);
            float targetYPos = Main.player[NPC.target].position.Y + (Main.player[NPC.target].height / 2);

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
                NPC.TargetClosest(true);
                NPC.velocity.Y = NPC.velocity.Y + 0.11f;
                if (NPC.velocity.Y > speed)
                    NPC.velocity.Y = speed;
                if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < speed * 0.4)
                {
                    if (NPC.velocity.X < 0.0)
                        NPC.velocity.X = NPC.velocity.X - acceleration * 1.1f;
                    else
                        NPC.velocity.X = NPC.velocity.X + acceleration * 1.1f;
                }
                else if (NPC.velocity.Y == speed)
                {
                    if (NPC.velocity.X < dirX)
                        NPC.velocity.X = NPC.velocity.X + acceleration;
                    else if (NPC.velocity.X > dirX)
                        NPC.velocity.X = NPC.velocity.X - acceleration;
                }
                else if (NPC.velocity.Y > 4.0)
                {
                    if (NPC.velocity.X < 0.0)
                        NPC.velocity.X = NPC.velocity.X + acceleration * 0.9f;
                    else
                        NPC.velocity.X = NPC.velocity.X - acceleration * 0.9f;
                }
            }
            // Else we want to play some audio (soundDelay) and move towards our target.
            else
            {
                if (NPC.soundDelay == 0)
                {
                    float num1 = length / 40f;
                    if (num1 < 15.0)
                        num1 = 15f;
                    if (num1 > 25.0)
                        num1 = 25f;
                    NPC.soundDelay = (int)num1;
                    SoundEngine.PlaySound(SoundLoader.customSoundType, (int)NPC.Center.X, (int)NPC.Center.Y, Mod.GetSoundSlot(SoundType.Custom, "Sounds/Enemies/EarthenElementalDig"), 1.0f, 0f);
                }
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
            }
            // Set the correct rotation for this NPC.
            NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + 1.57f;

            // Some netupdate stuff (multiplayer compatibility).
            if (collision)
            {
                if (NPC.localAI[0] != 1)
                    NPC.netUpdate = true;
                NPC.localAI[0] = 1f;
            }
            else
            {
                if (NPC.localAI[0] != 0.0)
                    NPC.netUpdate = true;
                NPC.localAI[0] = 0.0f;
            }
            if ((NPC.velocity.X > 0.0 && NPC.oldVelocity.X < 0.0 || NPC.velocity.X < 0.0 && NPC.oldVelocity.X > 0.0 || (NPC.velocity.Y > 0.0 && NPC.oldVelocity.Y < 0.0 || NPC.velocity.Y < 0.0 && NPC.oldVelocity.Y > 0.0)) && !NPC.justHit)
                NPC.netUpdate = true;
        }
        bool worm = true;
        int[] segments = {-1,-1,-1,-1};
        public override bool PreAI()
        {
            NPC.TargetClosest(true);
            if (Main.player[NPC.target].dead || Vector2.Distance(Main.player[NPC.target].Center, NPC.Center) > 4800)
            {
                NPC.active = false;
                return false;
            }
            if (Main.netMode != 1)
            {
                if (NPC.ai[0] == 0)
                {
                    NPC.realLife = NPC.whoAmI;
                    int latestNPC = NPC.whoAmI;
                    int WormLength = 4;
                    for (int i = 0; i < WormLength; i++)
                    {
                        latestNPC = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<EarthenConstructTail>(), NPC.whoAmI, 0, latestNPC);
                        Main.npc[latestNPC].realLife = NPC.whoAmI;
                        Main.npc[latestNPC].ai[3] = NPC.whoAmI;
                        Main.npc[latestNPC].ai[2] = i + 1;
                        segments[i] = latestNPC;
                    }

                    // We're setting npc.ai[0] to 1, so that this 'if' is not triggered again.
                    NPC.ai[0] = 1;
                }
                NPC.netUpdate = true;
            }
            if (Main.player[NPC.target].dead)
            {
                NPC.velocity.Y = 4;
            }
            else
            {
                if (worm) DoWormAI();
                if (!worm) DoSound();
            }
            NPC.rotation = NPC.velocity.ToRotation() + 1.57f;

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
            if (!NPC.active)
                return;
            Player player = Main.player[NPC.target];
            float distToPlayer = (player.Center - NPC.Center).Length();
            aiCounter++;
            eternalCounter++;
            int i = (int)NPC.Center.X / 16;
            int j = (int)NPC.Center.Y / 16;
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
                if (!tile.HasTile && extraCounter < 256)
                {
                    extraCounter += 4;
                }
                goTo.Y += 256 + extraCounter;
                Vector2 circularLocation = new Vector2(256, 0).RotatedBy(MathHelper.ToRadians(eternalCounter));
                goTo.X += circularLocation.X;
                Vector2 velo = goTo - NPC.Center;
                velo = velo.SafeNormalize(Vector2.Zero);
                if (12 >= (goTo - NPC.Center).Length())
                {
                    velo *= (goTo - NPC.Center).Length();
                }
                else
                {
                    velo *= 12;
                }
                NPC.velocity = velo;
                if(aiCounter >= 196)
                {
                    aiCounter = 0;
                    extraCounter = 0;
                    NPC.velocity *= 0;
                    aiMode = 2;
                }
            }
            if(aiMode == 2)
            {
                Tile tile = Framing.GetTileSafely(i, j);
                NPC.velocity.Y = -6f;
                bool lineOfSight = Collision.CanHitLine(NPC.position, NPC.width, NPC.height, NPC.position + new Vector2(0, -128), NPC.width, NPC.height);
                if (((!tile.HasTile || !Main.tileSolid[tile.TileType]) && lineOfSight) || NPC.Center.Y < player.Center.Y)
                {
                    aiCounter = 0;
                    aiMode = 3;
                    NPC.velocity.Y = 0;
                    extraCounter = 0;
                }
            }
            if(aiMode >= 3 && aiMode <= 5)
            {
                NPC.velocity.X = 0;
                NPC.velocity.Y = -1;
                if (aiCounter >= 30)
                {
                    LaunchBeams();
                    aiCounter = 0;
                    aiMode++;
                    NPC.position.Y += 30;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int x = 0; x < 4; x++)
                        {
                            if (segments[x] != -1)
                            {
                                NPC segment = Main.npc[segments[x]];
                                if (segment.active && segment.type == ModContent.NPCType<EarthenConstructTail>() && (int)segment.ai[1] == NPC.whoAmI)
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
            if (Main.netMode != NetmodeID.MultiplayerClient && eternalCounter % 30 == 0)
            {
                NPC.netUpdate = true;
            }
            NPC.spriteDirection = NPC.velocity.X > 0 ? 1 : -1;
            doAIExtras();
        }
        public void LaunchBeams()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient && !Main.player[NPC.target].dead)
            {
                int damage = NPC.damage / 2;
                if (Main.expertMode)
                {
                    damage = (int)(damage / Main.expertDamage);
                }
                int count = 3;
                if (Main.expertMode)
                {
                    count += 1;
                }
                for(int i = 0; i < count; i++)
                {
                    Vector2 circularLocation = new Vector2(0, -4).RotatedByRandom(MathHelper.ToRadians(17 + count));
                    Projectile.NewProjectile(NPC.Center.X, NPC.Center.Y, circularLocation.X, circularLocation.Y, ModContent.ProjectileType<EarthenShot>(), damage, 0, Main.myPlayer, 0, Main.player[NPC.target].Center.Y - 32);
                }
            }
            SoundEngine.PlaySound(SoundID.Item92, (int)(NPC.Center.X), (int)(NPC.Center.Y));
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/Constructs/EarthenConstructHead").Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Texture2D texture2 = Mod.Assets.Request<Texture2D>("NPCs/Constructs/EarthenConstruct").Value;
            Texture2D texture3 = Mod.Assets.Request<Texture2D>("NPCs/Constructs/EarthenConstructHeadGlow").Value;
            Vector2 origin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
            Main.spriteBatch.Draw(texture2, NPC.Center - Main.screenPosition, null, drawColor, NPC.rotation - MathHelper.ToRadians(NPC.localAI[1]), origin2, NPC.scale + 0.04f, NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            Main.spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, null, drawColor, NPC.rotation - MathHelper.ToRadians(90), origin, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            int i = (int)NPC.Center.X / 16;
            int j = (int)NPC.Center.Y / 16;
            if (!SOTSWorldgenHelper.Full(i - 1, j - 1, 3, 3))
                Main.spriteBatch.Draw(texture3, NPC.Center - Main.screenPosition, null, Color.White, NPC.rotation - MathHelper.ToRadians(90), origin, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
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
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 82, 2.5f * (float)hitDirection, -2.5f, 0, default(Color), 0.7f);
            }
            for (int i = 0; i < 30; i++)
            {
                int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<BigEarthDust>());
                Main.dust[dust].velocity *= 5f;
            }
            Gore.NewGore(NPC.position, NPC.velocity, Mod.GetGoreSlot("Gores/EarthenConstructGore1"), 1f);
            Gore.NewGore(NPC.position, NPC.velocity, Mod.GetGoreSlot("Gores/EarthenConstructGore2"), 1f);
            Gore.NewGore(NPC.position, NPC.velocity, Mod.GetGoreSlot("Gores/EarthenConstructGore3"), 1f);
            Gore.NewGore(NPC.position, NPC.velocity, Mod.GetGoreSlot("Gores/EarthenConstructGore4"), 1f);
            Gore.NewGore(NPC.position, NPC.velocity, Mod.GetGoreSlot("Gores/EarthenConstructGore5"), 1f);
            for (int i = 0; i < 9; i++)
                Gore.NewGore(NPC.position, NPC.velocity, Main.rand.Next(61, 64), 1f);
            gennedGore = true;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
            {
                if (!gennedGore) genGore(hitDirection);
            }
        }
        public override void NPCLoot()
        {
            if (!gennedGore && Main.netMode != NetmodeID.Server) genGore(0);

            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                int type = ModContent.NPCType<EarthenSpirit>();
                int j = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, type, 0, 0, 0);
                Main.npc[j].velocity.Y = -10f;
                Main.npc[j].netUpdate = true;

                int n = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, type, 0, 0, 1, j);
                Main.npc[n].velocity = new Vector2(1, -9f);
                Main.npc[n].netUpdate = true;

                n = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, type, 0, 0, 2, j);
                Main.npc[n].velocity = new Vector2(-1, -9f);
                Main.npc[n].netUpdate = true;

                n = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, type, 0, 0, 3, j);
                Main.npc[n].velocity = new Vector2(2, -8f);
                Main.npc[n].netUpdate = true;

                n = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, type, 0, 0, 4, j);
                Main.npc[n].velocity = new Vector2(-2, -8f);
                Main.npc[n].netUpdate = true;
            }
            Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<FragmentOfEarth>(), Main.rand.Next(4) + 4);
        }
        public void doAIExtras()
        {
            NPC.localAI[1] += (float)Math.Sqrt(NPC.velocity.Length()) * -NPC.spriteDirection;
			NPC.timeLeft = 100;
		}
    }
}