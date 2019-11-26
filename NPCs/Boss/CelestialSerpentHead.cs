using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.NPCs.Boss
{[AutoloadBossHead]
    public class CelestialSerpentHead : ModNPC
    {	float ai1 = 0;
		float ai2 = 0;
		int despawn = 0;
		float directX = 0;
		float directY = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Celestial Serpent");
		}
        public override void SetDefaults()
        {
           
            npc.lifeMax = 110000;      
            npc.damage = 100;
            npc.defense = 0;    
            npc.knockBackResist = 0f;
            npc.width = 50;
            npc.height = 50;
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
			bossBag = mod.ItemType("CelestialBag");
        }
		public override void BossLoot(ref string name, ref int potionType)
		{ 
			Player player = Main.player[npc.target];
			SOTSWorld.downedCelestial = true;
			potionType = ItemID.GreaterHealingPotion;
			npc.position = player.position;
			if(Main.expertMode)
			
			{ 
			npc.DropBossBags();
			} 
			else 
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StarShard"), Main.rand.Next(16, 25)); 
				if(Main.rand.Next(20) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StrangeFruit"), 1); 
			}
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
                    int randomWormLength = 40;
                    for (int i = 0; i < randomWormLength; ++i)
                    {
                        latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("CelestialSerpentBody"), npc.whoAmI, 0, latestNPC);
                        Main.npc[(int)latestNPC].realLife = npc.whoAmI;
                        Main.npc[(int)latestNPC].ai[3] = npc.whoAmI;
                    }
                    latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("CelestialSerpentTail"), npc.whoAmI, 0, latestNPC);
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
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * bossLifeScale * 0.75f);  //boss life scale in expertmode
        }
		float rotate = 0;
		float goToX = 0;
		float goToY = 0;
		int phase = 0;
		public override void AI()
		{
			Player player =	Main.player[npc.target];
			npc.TargetClosest(false);
			int expertScale = 1;
			if(Main.expertMode)
			{
				expertScale = 2;
			}
			if(phase == 0 && npc.life < (int)(npc.lifeMax * 0.55f))
			{
				phase = 1;
				UnstableSerpent(player.Center.X - 1800, player.Center.Y, 40);
				UnstableSerpent(player.Center.X + 1800, player.Center.Y, 40);
				UnstableSerpent(player.Center.X, player.Center.Y + 1800, 40);
				UnstableSerpent(player.Center.X, player.Center.Y - 1800, 40);
				
				NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("ChaosFlame"));
				Main.PlaySound(SoundID.Item119, (int)(npc.Center.X), (int)(npc.Center.Y));
			}
			if(phase == 1 && npc.life < (int)(npc.lifeMax * 0.15f))
			{
				phase = 2;
				NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("ChaosFlame"));
				NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("ChaosFlame"));
				Main.PlaySound(SoundID.Item119, (int)(npc.Center.X), (int)(npc.Center.Y));
			}
			
			
			
			ai1++;
			rotate += 1;
			
			if(ai1 >= 720 && ai1 <= 1440)
			{
				Vector2 SpinTo = new Vector2(480, 0).RotatedBy(MathHelper.ToRadians(rotate * 1.75f));
				
				
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
				}
			}
			if(ai1 > 1440 && ai1 < 1600)
			{
				Vector2 SpinTo = new Vector2(24, 0).RotatedBy(Math.Atan2(goToY, goToX));
				directX = SpinTo.X;
				directY = SpinTo.Y;
			}
			if(ai1 == 1600)
			{
				Vector2 SpinTo = new Vector2(2800, 0).RotatedBy(Math.Atan2(goToY, goToX) + MathHelper.ToRadians(180));
				
				for(int i = 0; i < 2 * expertScale; i++)
				{
					Vector2 SpinToRand = new Vector2(2000, 0).RotatedBy(Math.Atan2(goToY, goToX) + MathHelper.ToRadians(Main.rand.Next(-60,61)));
					UnstableSerpent(SpinToRand.X + player.Center.X, SpinToRand.Y + player.Center.Y, 40);
				}
				npc.position.X = player.Center.X + SpinTo.X - npc.height/2;
				npc.position.Y = player.Center.Y + SpinTo.Y - npc.width/2;
			}
			if(ai1 == 1720)
			{
				float shootToX = player.Center.X - npc.Center.X;
				float shootToY = player.Center.Y - npc.Center.Y;
				for(int i = 0; i < 270; i += 90)
				{
					Vector2 SpinTo = new Vector2(5, 0).RotatedBy(Math.Atan2(shootToY, shootToX) + MathHelper.ToRadians(i));
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, SpinTo.X, SpinTo.Y, mod.ProjectileType("StarCluster"), 36, 0, 0);
				}
			}
			if(ai1 == 1900)
			{
				float shootToX = player.Center.X - npc.Center.X;
				float shootToY = player.Center.Y - npc.Center.Y;
				for(int i = 0; i < 270; i += 90)
				{
					Vector2 SpinTo = new Vector2(9, 0).RotatedBy(Math.Atan2(shootToY, shootToX) + MathHelper.ToRadians(i));
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, SpinTo.X, SpinTo.Y, mod.ProjectileType("StarCluster"), 36, 0, 0);
				}
			}
			if(ai1 == 2080)
			{
				float shootToX = player.Center.X - npc.Center.X;
				float shootToY = player.Center.Y - npc.Center.Y;
				for(int i = 0; i < 270; i += 90)
				{
					Vector2 SpinTo = new Vector2(13, 0).RotatedBy(Math.Atan2(shootToY, shootToX) + MathHelper.ToRadians(i));
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, SpinTo.X, SpinTo.Y, mod.ProjectileType("StarCluster"), 36, 0, 0);
				}
				if(phase == 0)
				ai1 = 0;
			}
			
			if(ai1 == 2200 && phase == 1)
			{
				UnstableSerpent(player.Center.X - 1800, player.Center.Y, 40);
				UnstableSerpent(player.Center.X + 1800, player.Center.Y, 40);
				UnstableSerpent(player.Center.X, player.Center.Y + 1800, 40);
				UnstableSerpent(player.Center.X, player.Center.Y - 1800, 40);
			}
			if(ai1 == 2320)
			{
				float shootToX = player.Center.X - npc.Center.X;
				float shootToY = player.Center.Y - npc.Center.Y;
				for(int i = 0; i < 270; i += 90)
				{
					Vector2 SpinTo = new Vector2(17, 0).RotatedBy(Math.Atan2(shootToY, shootToX) + MathHelper.ToRadians(i));
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, SpinTo.X, SpinTo.Y, mod.ProjectileType("StarCluster"), 36, 0, 0);
				}
				//if(phase == 1)
				NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("ChaosFlame"));
				ai1 = 0;
			}
			
			if(!Main.expertMode)
			{
				if(phase == 2 && ai1 % 464 == 0)
				{
					UnstableSerpent(player.Center.X - 1800, player.Center.Y, 40);
				}
				if(phase == 2 && ai1 % 464 == 116)
				{
					UnstableSerpent(player.Center.X + 1800, player.Center.Y, 40);
				}
			}
			
			if(Main.expertMode)
			{
				if(phase == 2 && ai1 % 464 == 0)
				{
					UnstableSerpent(player.Center.X - 1800, player.Center.Y, 40);
				}
				if(phase == 2 && ai1 % 464 == 116)
				{
					UnstableSerpent(player.Center.X, player.Center.Y - 1800, 40);
				}
				if(phase == 2 && ai1 % 464 == 232)
				{
					UnstableSerpent(player.Center.X + 1800, player.Center.Y, 40);
				}
				if(phase == 2 && ai1 % 464 == 348)
				{
					UnstableSerpent(player.Center.X, player.Center.Y + 1800, 40);
				}
				if(phase == 2 && ai1 % 464 == 0 && Main.rand.Next(8) == 0)
				NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("ChaosFlame"));
			}
			
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
		public void UnstableSerpent(float x, float y, int damage)
		{
			Player player =	Main.player[npc.target];
			float angX = player.Center.X - x;
			float angY = player.Center.Y - y;
			Vector2 properAngle = new Vector2(21, 0).RotatedBy(MathHelper.ToRadians(45) + Math.Atan2(angY, angX));
            Projectile.NewProjectile(x, y, properAngle.X, properAngle.Y, mod.ProjectileType("UnstableSerpent"), damage, 0, 0);
			Main.PlaySound(SoundID.Item119, (int)(x), (int)(y));
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
		}
	}
}