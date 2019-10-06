using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.NPCs.Boss
{[AutoloadBossHead]
    public class SubspaceSerpentHead : ModNPC
    {	float ai1 = 240;
		float ai2 = 0;
		int despawn = 0;
		float directX = 0;
		float directY = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Subspace Serpent");
		}
        public override void SetDefaults()
        {
           
            npc.lifeMax = 120000;      
            npc.damage = 100;
            npc.defense = 0;    
            npc.knockBackResist = 0f;
            npc.width = 40;
            npc.height = 40;
            npc.boss = true;
            npc.lavaImmune = true;      
            npc.noGravity = true;         
            npc.noTileCollide = true;  
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath32;
            npc.value = 10000;
            npc.npcSlots = 25;
            npc.netAlways = true;
			music = MusicID.Boss2;
            npc.buffImmune[69] = true;
            npc.buffImmune[70] = true;
			npc.aiStyle = 6;
        }
        public override bool PreAI()
        {
            if (Main.netMode != 1)
            {
                if (npc.ai[0] == 0)
                {
                    npc.realLife = npc.whoAmI;
                    int latestNPC = npc.whoAmI;
					
					/*
                    latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FrostHydra_WingBody"), npc.whoAmI, 0, latestNPC);
                    Main.npc[(int)latestNPC].realLife = npc.whoAmI;
                    Main.npc[(int)latestNPC].ai[3] = npc.whoAmI;
					*/
                    int randomWormLength = 50;
                    for (int i = 0; i < randomWormLength; ++i)
                    {
                        latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SubspaceSerpentBody"), npc.whoAmI, 0, latestNPC);
                        Main.npc[(int)latestNPC].realLife = npc.whoAmI;
                        Main.npc[(int)latestNPC].ai[3] = npc.whoAmI;
                    }
                    latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SubspaceSerpentTail"), npc.whoAmI, 0, latestNPC);
                    Main.npc[(int)latestNPC].realLife = npc.whoAmI;
                    Main.npc[(int)latestNPC].ai[3] = npc.whoAmI;
 
                    // We're setting npc.ai[0] to 1, so that this 'if' is not triggered again.
                    npc.ai[0] = 1;
                    npc.netUpdate = true;
                }
            }
 
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
            float speed = 17.5f;
            // acceleration is exactly what it sounds like. The speed at which this NPC accelerates.
            float acceleration = 0.2f;
 
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
                npc.TargetClosest(false);
                if (directY > speed)
                    directY = speed;
                if (Math.Abs(directX) + Math.Abs(directY) < speed * 0.4)
                {
                    if (directX < 0.0)
                        directX = directX - acceleration * 1.1f;
                    else
                        directX = directX + acceleration * 1.1f;
                }
                else if (directY == speed)
                {
                    if (directX < dirX)
                        directX = directX + acceleration;
                    else if (directX > dirX)
                        directX = directX - acceleration;
                }
                else if (directY > 4.0)
                {
                    if (directX < 0.0)
                        directX = directX + acceleration * 0.9f;
                    else
                        directX = directX - acceleration * 0.9f;
                }
            }
			
            // Else we want to play some audio (soundDelay) and move towards our target.
            
                if (npc.soundDelay == 0)
                {
                    float num1 = length / 40f;
                    if (num1 < 10.0)
                        num1 = 10f;
                    if (num1 > 20.0)
                        num1 = 20f;
                    npc.soundDelay = (int)num1;
                   // Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 1);
                }
                float absDirX = Math.Abs(dirX);
                float absDirY = Math.Abs(dirY);
                float newSpeed = speed / length;
                dirX = dirX * newSpeed;
                dirY = dirY * newSpeed;
                if (directX > 0.0 && dirX > 0.0 || directX < 0.0 && dirX < 0.0 || (directY > 0.0 && dirY > 0.0 || directY < 0.0 && dirY < 0.0))
                {
                    if (directX < dirX)
                        directX = directX + acceleration;
                    else if (directX > dirX)
                        directX = directX - acceleration;
                    if (directY < dirY)
                        directY = directY + acceleration;
                    else if (directY > dirY)
                        directY = directY - acceleration;
                    if (Math.Abs(dirY) < speed * 0.2 && (directX > 0.0 && dirX < 0.0 || directX < 0.0 && dirX > 0.0))
                    {
                        if (directY > 0.0)
                            directY = directY + acceleration * 2f;
                        else
                            directY = directY - acceleration * 2f;
                    }
                    if (Math.Abs(dirX) < speed * 0.2 && (directY > 0.0 && dirY < 0.0 || directY < 0.0 && dirY > 0.0))
                    {
                        if (directX > 0.0)
                            directX = directX + acceleration * 2f;
                        else
                            directX = directX - acceleration * 2f;
                    }
                }
                else if (absDirX > absDirY)
                {
                    if (directX < dirX)
                        directX = directX + acceleration * 1.1f;
                    else if (directX > dirX)
                        directX = directX - acceleration * 1.1f;
                    if (Math.Abs(directX) + Math.Abs(directY) < speed * 0.5)
                    {
                        if (directY > 0.0)
                            directY = directY + acceleration;
                        else
                            directY = directY - acceleration;
                    }
                }
                else
                {
                    if (directY < dirY)
                        directY = directY + acceleration * 1.1f;
                    else if (directY > dirY)
                        directY = directY - acceleration * 1.1f;
                    if (Math.Abs(directX) + Math.Abs(directY) < speed * 0.5)
                    {
                        if (directX > 0.0)
                            directX = directX + acceleration;
                        else
                            directX = directX - acceleration;
                    }
                }
            
            // Set the correct rotation for this NPC.
            npc.rotation = (float)Math.Atan2(directY, directX) + 1.57f;
           
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
            if ((directX > 0.0 && npc.oldVelocity.X < 0.0 || directX < 0.0 && npc.oldVelocity.X > 0.0 || (directY > 0.0 && npc.oldVelocity.Y < 0.0 || directY < 0.0 && npc.oldVelocity.Y > 0.0)) && !npc.justHit)
                npc.netUpdate = true;
 
            return true;
        }
 
        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Main.npcTexture[npc.type];
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, new Rectangle?(), drawColor, npc.rotation, origin, npc.scale, SpriteEffects.None, 0);
            return false;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1f;  
            return null;
        }
		public override void NPCLoot()
		{
			
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * bossLifeScale * 0.75f);  //boss life scale in expertmode
        }
		float rotate = 0;
		float goToX = 0;
		float goToY = 0;
		int phase = 0;
		float areaX;
		float areaY;
		float areaX2;
		float areaY2;
		float keepRotate;
		public override void AI()
		{
			Player player =	Main.player[npc.target];
			npc.TargetClosest(false);
			int expertScale = 1;
			if(Main.expertMode)
			{
				expertScale = 2;
			}
			
			
			ai1++;
			rotate += 1;
			
			if(ai1 >= 720 && ai1 <= 1800)
			{
				Vector2 SpinTo = new Vector2(640, 0).RotatedBy(MathHelper.ToRadians(rotate * 2.25f));
				
				
				goToX = player.Center.X + SpinTo.X - npc.Center.X;
				goToY = player.Center.Y + SpinTo.Y - npc.Center.Y;
				
				
			
				float distance = (float)System.Math.Sqrt((double)(goToX * goToX + goToY * goToY));
				if(distance > 48)
				{
					distance = 5.5f / distance;
									  
					goToX *= distance * 5;
					goToY *= distance * 5;
					
					directX = goToX;
					directY = goToY;
					
				}
				else
				{
					npc.position.X = player.Center.X + SpinTo.X - npc.height/2;
					npc.position.Y = player.Center.Y + SpinTo.Y - npc.width/2;
					
					distance = 5f / distance;
									  
					goToX *= distance * 5;
					goToY *= distance * 5;
					directX = 0;
					directY = 0;
					npc.rotation = (float)Math.Atan2(goToY, goToX) + 1.57f;
					if(ai1 % 120 == 0)
					{
						int rand1 = Main.rand.Next(8);

						int rand2 = Main.rand.Next(8);
						while(rand2 == rand1)
						{
							rand2 = Main.rand.Next(8);
						}
						
						int rand3 = Main.rand.Next(8);
						while(rand3 == rand2 || rand3 == rand1)
						{
							rand3 = Main.rand.Next(8);
						}
						
						int rand4 = Main.rand.Next(8);
						while(rand4 == rand3 || rand4 == rand2 || rand4 == rand1)
						{
							rand4 = Main.rand.Next(8);
						}
						
						if(Main.expertMode)
						{
							Laser(rand4, 45);
						}
						
						Laser(rand3, 42);
						
						if(npc.life < (int)(npc.lifeMax * 0.5f))
						Laser(rand2, 42);
						
						if(npc.life < (int)(npc.lifeMax * 0.25f))
						Laser(rand1, 42);
					}
				}
			}
			if(ai1 == 1805)
			{
				int rand1 = Main.rand.Next(4);
				
				if(rand1 == 0)
				{
					areaX = -900; // <--
					areaY = -400;
					areaX2 = 2400;
					areaY2 = -400;
				}
				if(rand1 == 1)
				{
					areaX = 800;
					areaY = -700;  // ^
					areaX2 = 800;
					areaY2 = 2400;
				}
				if(rand1 == 2)
				{
					areaX = 900; // -->
					areaY = 400;
					areaX2 = -2400;
					areaY2 = 400;
				}
				if(rand1 == 3)
				{
					areaX = -800;
					areaY = 700; // \/
					areaX2 = -800;
					areaY2 = -2400;
				}
			}
			if(ai1 > 1805 && ai1 <= 1810)
			{
				goToX = player.Center.X + areaX2 - npc.Center.X;
				goToY = player.Center.Y + areaY2 - npc.Center.Y;
				
				
			
				float distance = (float)System.Math.Sqrt((double)(goToX * goToX + goToY * goToY));
				if(distance > 48)
				{
					ai1--;
					distance = 8.5f / distance;
									  
					goToX *= distance * 5;
					goToY *= distance * 5;
					
					directX = goToX;
					directY = goToY;
				}
			}
			if(ai1 > 1810 && ai1 <= 1815)
			{
				goToX = player.Center.X + areaX - npc.Center.X;
				goToY = player.Center.Y + areaY - npc.Center.Y;
				
				
			
				float distance = (float)System.Math.Sqrt((double)(goToX * goToX + goToY * goToY));
				if(distance > 48)
				{
					ai1--;
					distance = 8.5f / distance;
									  
					goToX *= distance * 5;
					goToY *= distance * 5;
					
					directX = goToX;
					directY = goToY;
				}
				npc.rotation = (float)Math.Atan2(goToY, goToX) + 1.57f;
				keepRotate = npc.rotation;
				
			}
			if(ai1 >= 1820 && ai1 <= 2300)
			{
				npc.rotation = keepRotate;
				directX = 0;
				directY = 0;
				
				if(ai1 == 1822)
				{
					LaserWall(60);
				}
				if(ai1 % 100 == 0)
				{
					LaserWave(40);
				}
			}
			if(ai1 == 2305)
			{
				int rand1 = Main.rand.Next(4);
				
				if(rand1 == 0)
				{
					areaX = -900; // <--
					areaY = -400;
					areaX2 = 2400;
					areaY2 = -400;
				}
				if(rand1 == 1)
				{
					areaX = 800;
					areaY = -700;  // ^
					areaX2 = 800;
					areaY2 = 2400;
				}
				if(rand1 == 2)
				{
					areaX = 900; // -->
					areaY = 400;
					areaX2 = -2400;
					areaY2 = 400;
				}
				if(rand1 == 3)
				{
					areaX = -800;
					areaY = 700; // \/
					areaX2 = -800;
					areaY2 = -2400;
				}
			}
			if(ai1 > 2305 && ai1 <= 2310)
			{
				goToX = player.Center.X + areaX2 - npc.Center.X;
				goToY = player.Center.Y + areaY2 - npc.Center.Y;
				
				
			
				float distance = (float)System.Math.Sqrt((double)(goToX * goToX + goToY * goToY));
				if(distance > 48)
				{
					ai1--;
					distance = 8.5f / distance;
									  
					goToX *= distance * 5;
					goToY *= distance * 5;
					
					directX = goToX;
					directY = goToY;
				}
			}
			if(ai1 > 2310 && ai1 <= 2315)
			{
				goToX = player.Center.X + areaX - npc.Center.X;
				goToY = player.Center.Y + areaY - npc.Center.Y;
				
				
			
				float distance = (float)System.Math.Sqrt((double)(goToX * goToX + goToY * goToY));
				if(distance > 48)
				{
					ai1--;
					distance = 8.5f / distance;
									  
					goToX *= distance * 5;
					goToY *= distance * 5;
					
					directX = goToX;
					directY = goToY;
				}
				npc.rotation = (float)Math.Atan2(goToY, goToX) + 1.57f;
				keepRotate = npc.rotation;
				
			}
			if(ai1 >= 2320 && ai1 <= 3175)
			{
				npc.rotation = keepRotate;
				directX = 0;
				directY = 0;
			}
			if(ai1 >= 2400 && ai1 <= 3175)
			{
				
				if(ai1 == 2401)
				{
					LaserWall(60);
				}
				if(ai1 % 200 == 0)
				{
					LaserWarning(30);
				}
				if(ai1 % 200 == 100 || ai1 % 200 == 110 || ai1 % 200 == 120 || ai1 % 200 == 130 || ai1 % 200 == 140 || ai1 % 200 == 150)
				{
					LaserRapid(44);
					if(ai1 % 200 == 150) LaserReset();
				}
			}
			if(ai1 >= 3180) ai1 = 0;
			
			if(Main.player[npc.target].dead)
			{
			 despawn++;
			}
			if(despawn >= 720)
			{
			npc.active = false;
			}
			npc.timeLeft = 10000;
		}
		int slither = 1;
		public void LaserReset()
		{
			//reset npc.knockBackResist = 0.0f; to prepare for next warning
			for(int j = 0; j < 200; j++)
			{
				NPC npc2 = Main.npc[j];
				if(npc2.type == mod.NPCType("SubspaceSerpentBody") && npc2.active)
				{
					if(npc2.knockBackResist == 0.1f)
					{
						npc2.knockBackResist = 0.0f;
					}
				}
			}
		}
		public void LaserRapid(int damage)
		{
			//using npc.knockBackResist = 0.1f; to detect segments that will fire
			int direction = -1;
			if(areaX == 900) // ^
			direction = 1;
			if(areaX == -900) // \/
			direction = 2;
			if(areaY == -700) // <--
			direction = 3;
			if(areaY == 700) //  -->
			direction = 4;	
			
			for(int j = 0; j < 200; j++)
			{
				NPC npc2 = Main.npc[j];
				if(npc2.type == mod.NPCType("SubspaceSerpentBody") && npc2.active)
				{
					if(npc2.knockBackResist == 0.1f)
					{
						float posX = npc2.Center.X;
						float posY = npc2.Center.Y;
						if(direction == 1)
						{
						Vector2 properAngle = new Vector2(0, -24);
						Projectile.NewProjectile(posX, posY, properAngle.X, properAngle.Y, mod.ProjectileType("GreenShockBlast"), damage, 0, 0);
						}
						if(direction == 2)
						{
						Vector2 properAngle = new Vector2(0, 24);
						Projectile.NewProjectile(posX, posY, properAngle.X, properAngle.Y, mod.ProjectileType("GreenShockBlast"), damage, 0, 0);
						}
						if(direction == 3)
						{
						Vector2 properAngle = new Vector2(-24, 0);
						Projectile.NewProjectile(posX, posY, properAngle.X, properAngle.Y, mod.ProjectileType("GreenShockBlast"), damage, 0, 0);
						}
						if(direction == 4)
						{
						Vector2 properAngle = new Vector2(24, 0);
						Projectile.NewProjectile(posX, posY, properAngle.X, properAngle.Y, mod.ProjectileType("GreenShockBlast"), damage, 0, 0);
						}
					
					}
				}
			}
		}
		public void LaserWarning(int damage)
		{
			//using npc.knockBackResist = 0.1f; to prepare for rapid
			int direction = -1;
			if(areaX == 900) // ^
			direction = 1;
			if(areaX == -900) // \/
			direction = 2;
			if(areaY == -700) // <--
			direction = 3;
			if(areaY == 700) //  -->
			direction = 4;	
			
			for(int j = 0; j < 200; j++)
			{
				NPC npc2 = Main.npc[j];
				if(npc2.type == mod.NPCType("SubspaceSerpentBody") && npc2.active)
				{
					if((Main.rand.Next(4) == 0 && !Main.expertMode) || (Main.rand.Next(3) == 0 && Main.expertMode))
					{
					float posX = npc2.Center.X;
					float posY = npc2.Center.Y;
						if(direction == 1)
						{
						Vector2 properAngle = new Vector2(0, -8);
						Projectile.NewProjectile(posX, posY, properAngle.X, properAngle.Y, mod.ProjectileType("GreenCellBlast"), damage, 0, 0);
						}
						if(direction == 2)
						{
						Vector2 properAngle = new Vector2(0, 8);
						Projectile.NewProjectile(posX, posY, properAngle.X, properAngle.Y, mod.ProjectileType("GreenCellBlast"), damage, 0, 0);
						}
						if(direction == 3)
						{
						Vector2 properAngle = new Vector2(-8, 0);
						Projectile.NewProjectile(posX, posY, properAngle.X, properAngle.Y, mod.ProjectileType("GreenCellBlast"), damage, 0, 0);
						}
						if(direction == 4)
						{
						Vector2 properAngle = new Vector2(8, 0);
						Projectile.NewProjectile(posX, posY, properAngle.X, properAngle.Y, mod.ProjectileType("GreenCellBlast"), damage, 0, 0);
						}
						
						
						npc2.knockBackResist = 0.1f;
					}
				}
			}
		}
		public void LaserWave(int damage)
		{
			Player player =	Main.player[npc.target];
			for(int j = 0; j < 200; j++)
			{
				NPC npc2 = Main.npc[j];
				if(npc2.type == mod.NPCType("SubspaceSerpentBody") && npc2.active)
				{
					float posX = npc2.Center.X;
					float posY = npc2.Center.Y;
					float angleX = npc2.Center.X - player.Center.X;
					float angleY = npc2.Center.Y - player.Center.Y;
					Vector2 properAngle = new Vector2(-4, 0).RotatedBy(Math.Atan2(angleY, angleX));
					Projectile.NewProjectile(posX, posY, properAngle.X, properAngle.Y, mod.ProjectileType("GreenWaveBlast"), damage, 0, 0);
				}
			}
		}
		public void LaserWall(int damage)
		{
			for(int i = 0; i < 2; i++)
			{
				float posX = npc.Center.X;
				float posY = npc.Center.Y;
				if(i == 1)
				{
					for(int j = 0; j < 200; j++)
					{
						NPC npc2 = Main.npc[j];
						if(npc2.type == mod.NPCType("SubspaceSerpentTail") && npc2.active)
						{
							posX = npc2.Center.X;
							posY = npc2.Center.Y;
						}
					}
				}
				Vector2 properAngle = new Vector2(21, 0).RotatedBy(MathHelper.ToRadians(45));
				Projectile.NewProjectile(posX, posY, properAngle.X, properAngle.Y, mod.ProjectileType("EnergySerpent"), damage, 0, 0);
				properAngle = new Vector2(-21, 0).RotatedBy(MathHelper.ToRadians(45));
				Projectile.NewProjectile(posX, posY, properAngle.X, properAngle.Y, mod.ProjectileType("EnergySerpent"), damage, 0, 0);
				properAngle = new Vector2(0, 21).RotatedBy(MathHelper.ToRadians(45));
				Projectile.NewProjectile(posX, posY, properAngle.X, properAngle.Y, mod.ProjectileType("EnergySerpent"), damage, 0, 0);
				properAngle = new Vector2(0, -21).RotatedBy(MathHelper.ToRadians(45));
				Projectile.NewProjectile(posX, posY, properAngle.X, properAngle.Y, mod.ProjectileType("EnergySerpent"), damage, 0, 0);
			}
		}
		public void Laser(int area, int damage)
		{
			Player player =	Main.player[npc.target];
			float locationX = 0;
			float locationY = 0;
			if(area == 0)
			{
				locationX = -324;
				locationY = -324;
			}
			if(area == 1)
			{
				locationX = 0;
				locationY = -324;
			}
			if(area == 2)
			{
				locationX = 324;
				locationY = -324;
			}
			if(area == 3)
			{
				locationX = 324;
				locationY = 0;
			}
			if(area == 4)
			{
				locationX = 324;
				locationY = 324;
			}
			if(area == 5)
			{
				locationX = 0;
				locationY = 324;
			}
			if(area == 6)
			{
				locationX = -324;
				locationY = 324;
			}
			if(area == 7)
			{
				locationX = -324;
				locationY = 0;
			}
			
			Main.PlaySound(SoundID.Item92, (int)(player.Center.X + locationX), (int)(player.Center.Y + locationY));
			if(Main.rand.Next(2) == 0)
			{
				Projectile.NewProjectile(player.Center.X + locationX, player.Center.Y + locationY, 0, 0, mod.ProjectileType("plusLaser"), damage, 0, 0);
			}
			else
			{
				Projectile.NewProjectile(player.Center.X + locationX, player.Center.Y + locationY, 0, 0, mod.ProjectileType("XLaser"), damage, 0, 0);
			}
		}
		public override void PostAI()
		{
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 2.5f / 255f, (255 - npc.alpha) * 1.6f / 255f, (255 - npc.alpha) * 2.4f / 255f);
			if(slither > 0)
			{
				slither += 2;
				npc.velocity = new Vector2(directX, directY).RotatedBy(MathHelper.ToRadians(slither - 30));
			}
			if(slither > 60)
			{
				slither = -1;
			}
			if(slither < 0)
			{
				slither -= 2;
				npc.velocity = new Vector2(directX, directY).RotatedBy(MathHelper.ToRadians(slither + 30));
			}
			if(slither < -60)
			{
				slither = 1;
			}
			
		}
		
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(ai1);
			writer.Write(ai2);
			writer.Write(directX);
			writer.Write(directY);
			writer.Write(rotate);
			writer.Write(goToX);
			writer.Write(goToY);
			writer.Write(phase);
			writer.Write(slither);
			writer.Write(areaX);
			writer.Write(areaX2);
			writer.Write(areaY);
			writer.Write(areaY2);
			writer.Write(keepRotate);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			ai1 = reader.ReadSingle();
			ai2 = reader.ReadSingle();
			directX = reader.ReadSingle();
			directY = reader.ReadSingle();
			rotate = reader.ReadSingle();
			goToX = reader.ReadSingle();
			goToY = reader.ReadSingle();
			phase = reader.ReadInt32();
			slither = reader.ReadInt32();
			areaX = reader.ReadSingle();
			areaX2 = reader.ReadSingle();
			areaY = reader.ReadSingle();
			areaY2 = reader.ReadSingle();
			keepRotate = reader.ReadSingle();
		}
	}
}