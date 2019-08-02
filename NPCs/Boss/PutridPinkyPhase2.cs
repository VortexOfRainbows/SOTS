using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss
{	[AutoloadBossHead]
	public class PutridPinkyPhase2 : ModNPC
	{
		int initiateMovement = 0;
		int despawn = 0;
		int initiateHooks = -1;
		int eyeID = -1;
		int phase = 1;
		int newRotation = 0;
		int initiatePinky = -1;
		int expertModifier = 1;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Putrid Pinky");
		}
		public override void SetDefaults()
		{
			
            //npc.aiStyle = 14;   
			npc.lifeMax = 7000;
            npc.damage = 30;   
            npc.defense = 0;   
            npc.knockBackResist = 0f;
            npc.width = 116;
            npc.height = 116;
            Main.npcFrameCount[npc.type] = 1;   
            npc.value = 150000;
            npc.npcSlots = 1f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath5;
            npc.buffImmune[24] = true;
            music = MusicID.Boss3;
            npc.netAlways = true;
			bossBag = mod.ItemType("PinkyBag");
			
			//bossBag = mod.ItemType("BossBagBloodLord");
		}
		public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/PutridVine");  
                     
			for(int i = 0; i < 1000; i++)
			{
				if(Main.projectile[i].type == mod.ProjectileType("PutridHook") && Main.projectile[i].active)
				{
					Vector2 position = npc.Center;
					Vector2 mountedCenter = Main.projectile[i].Center;
					Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
					Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
					float num1 = (float)texture.Height;
					Vector2 vector2_4 = mountedCenter - position;
					float rotation = (float)Math.Atan2((double)vector2_4.Y, (double)vector2_4.X) - 1.57f;
					bool flag = true;
					if (float.IsNaN(position.X) && float.IsNaN(position.Y))
						flag = false;
					if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
						flag = false;
					while (flag)
					{
						if ((double)vector2_4.Length() < (double)num1 + 1.0)
						{
							flag = false;
						}
						else
						{
							Vector2 vector2_1 = vector2_4;
							vector2_1.Normalize();
							position += vector2_1 * num1;
							vector2_4 = mountedCenter - position;
							Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)position.X / 16, (int)((double)position.Y / 16.0));
							color2 = npc.GetAlpha(color2);
							Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);
						}
					}
				}
			}
			return true;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * bossLifeScale * 0.8f);  //boss life scale in expertmode
            npc.damage = (int)(npc.damage * 1.2f);  //boss damage increase in expermode
        }
		
		public override bool PreAI()
		{
			if(Main.expertMode)
			{
				expertModifier = 2;
			}
            initiateMovement++;
			if(initiateMovement >= 360)
			{
				npc.aiStyle = 14;   
			}
			
			Player player  = Main.player[npc.target];
			
			if(initiateHooks == -1)
			{
				initiateHooks = 0;
				int Max = 0;
					for(int i = Main.rand.Next(-30,31); i < 720; i += Main.rand.Next(30,61))
					{
						Max++;
						if(Max > 12)
						{
							break;
						}
						Vector2 circularVelocity = new Vector2(-6, 0).RotatedBy(MathHelper.ToRadians(i));
						Projectile.NewProjectile((int)npc.Center.X, (int)npc.Center.Y, circularVelocity.X, circularVelocity.Y, mod.ProjectileType("PutridHook"), 20, 1, 0);
					}
			}
			return true;
		}
		public override void PostAI()
		{
			eyeID = -1;
			Player player  = Main.player[npc.target];
			for(int i = 0; i < 1000; i++)
			{
				Projectile eye = Main.projectile[i];
				if(eye.type == mod.ProjectileType("PutridPinkyEye") && eye.active)
				{
					eyeID = i;
					break;
				}
			}
			
					float shootToX = player.Center.X - npc.Center.X;
					float shootToY = player.Center.Y - npc.Center.Y;
					float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

					distance = 1.1f / distance;
						  
					shootToX *= distance * 5;
					shootToY *= distance * 5;
				
			if(eyeID != -1)
			{
				Projectile eye = Main.projectile[eyeID];
				eye.position.X = npc.Center.X - 4;
				eye.position.Y = npc.Center.Y - 4;
				eye.position.X += shootToX;
				eye.position.Y += shootToY * 2f;
			}
			else
			{
				Projectile.NewProjectile((int)npc.Center.X, (int)npc.Center.Y, 0, 0, mod.ProjectileType("PutridPinkyEye"), 0, 0, 0);
			}
		}
		public override void AI()
		{
			Player player  = Main.player[npc.target];
			npc.TargetClosest(false);
			
					float shootToX = player.Center.X - npc.Center.X;
					float shootToY = player.Center.Y - npc.Center.Y;
					float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

					distance = 1.1f / distance;
						  
					shootToX *= distance * 5;
					shootToY *= distance * 5;
					
			if((npc.life <= (int)(npc.lifeMax * .7f) || (npc.life <= (int)(npc.lifeMax * .75f) && Main.expertMode)) && phase == 1)
			{
				phase = 2;
			}
			if((npc.life <= (int)(npc.lifeMax * .3f) || (npc.life <= (int)(npc.lifeMax * .4f) && Main.expertMode)) && phase == 4)
			{
				phase = 5;
			}
			if(phase == 1)
			{
				npc.ai[0]++;
			}
			if(phase == 4)
			{
				npc.ai[0]++;
			}
			if(phase == 6)
			{
				npc.ai[0]++;
			}
			if(phase == 7)
			{
				npc.ai[0]++;
			}
		
		
			if(Main.player[npc.target].dead)
			{
				despawn++;
			}
			if(despawn >= 720)
			{
				npc.active = false;
			}
			
			if(phase == 1)
			{
				if(npc.ai[0] == 120)
					{
						int npcProj = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
			
						Main.PlaySound(SoundID.Item21, (int)(npc.Center.X), (int)(npc.Center.Y));
					}
					if(npc.ai[0] == 130)
					{
						int npcProj = NPC.NewNPC((int)npc.Center.X + (int)(3 * shootToY), (int)npc.Center.Y - (int)(3 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
			
						npcProj = NPC.NewNPC((int)npc.Center.X - (int)(3 * shootToY), (int)npc.Center.Y + (int)(3 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
						Main.PlaySound(SoundID.Item21, (int)(npc.Center.X), (int)(npc.Center.Y));
					}
					if(npc.ai[0] == 140)
					{
						int npcProj = NPC.NewNPC((int)npc.Center.X + (int)(6 * shootToY), (int)npc.Center.Y - (int)(6 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
			
						npcProj = NPC.NewNPC((int)npc.Center.X - (int)(6 * shootToY), (int)npc.Center.Y + (int)(6 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
						
						npcProj = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
						Main.PlaySound(SoundID.Item21, (int)(npc.Center.X), (int)(npc.Center.Y));
					}
					if(npc.ai[0] >= 150)
					{
						npc.ai[0] = 10;
					}
			}
			int Num2 = 0;
			if(phase == 2)
			{
				newRotation += 1;
				int Num = 0;
				for(int i = 0; i < 1000; i++)
				{
					Projectile hook = Main.projectile[i];
					if(hook.type == mod.ProjectileType("PutridHook") && hook.active)
					{
						float moveToX = hook.Center.X - npc.Center.X;
						float moveToY = hook.Center.Y - npc.Center.Y;
						float distanceTo = (float)System.Math.Sqrt((double)(moveToX * moveToX + moveToY * moveToY));
						Num++;
						if(distanceTo >= 90)
						{
							distanceTo = 2.5f / distanceTo;
								  
							moveToX *= distanceTo * 5;
							moveToY *= distanceTo * 5;
							hook.velocity.X = -moveToX;
							hook.velocity.Y = -moveToY;
							hook.knockBack = 0;
						}
						else
						{
							Num2++;
							Vector2 rotationalPosition = new Vector2(-80, 0).RotatedBy(MathHelper.ToRadians((Num * 30) + newRotation)); 
							moveToX = hook.Center.X - npc.Center.X + rotationalPosition.X;
							moveToY = hook.Center.Y - npc.Center.Y + rotationalPosition.Y;
							distanceTo = (float)System.Math.Sqrt((double)(moveToX * moveToX + moveToY * moveToY));
							distanceTo = 1f / distanceTo;
								  
							moveToX *= distanceTo * 5;
							moveToY *= distanceTo * 5;
							hook.velocity.X = -moveToX;
							hook.velocity.Y = -moveToY;
							hook.rotation = MathHelper.ToRadians(Main.rand.Next(360));
						}
					}
				}
			}
			if(Num2 >= 12)
			{
				phase = 3;
			}
			if(phase == 3)
			{
				newRotation += 1;
				int Num = 0;
				for(int i = 0; i < 1000; i++)
				{
					Projectile hook = Main.projectile[i];
					if(hook.type == mod.ProjectileType("PutridHook") && hook.active)
					{
							Num++;
							Vector2 rotationalPosition = new Vector2(-80, 0).RotatedBy(MathHelper.ToRadians((Num * 30) + newRotation)); 
								
							hook.position.X = npc.Center.X + rotationalPosition.X - (hook.width/2);
							hook.position.Y = npc.Center.Y + rotationalPosition.Y - (hook.width/2);
								
							hook.rotation = MathHelper.ToRadians(Main.rand.Next(360));
							hook.velocity.X = 0;
							hook.velocity.Y = 0;
					}
				}
				if(initiatePinky == -1)
				{
					NPC.NewNPC((int)npc.Center.X + 200, (int)npc.Center.Y + 200, mod.NPCType("PinkyFlyer"));
					NPC.NewNPC((int)npc.Center.X + 200, (int)npc.Center.Y - 200, mod.NPCType("PinkyFlyer"));	
					NPC.NewNPC((int)npc.Center.X - 200, (int)npc.Center.Y + 200, mod.NPCType("PinkyFlyer"));	
					NPC.NewNPC((int)npc.Center.X - 200, (int)npc.Center.Y - 200, mod.NPCType("PinkyFlyer"));	
					initiatePinky = 0;
				}
				if(!NPC.AnyNPCs(mod.NPCType("PinkyFlyer")))
				{
					phase = 4;
					for(int i = 0; i < 1000; i++)
					{
						Projectile hook = Main.projectile[i];
						if(hook.type == mod.ProjectileType("PutridHook") && hook.active)
						{
								hook.knockBack = 2;
								while(hook.velocity.X == 0 && hook.velocity.Y == 0)
								{
									hook.velocity.X = Main.rand.Next(-9,10);
									hook.velocity.Y = Main.rand.Next(-9,10);
								}
						}
					}
				}
			}
			if(phase == 4)
			{
				npc.aiStyle = 44;
					if(npc.ai[0] == 40)
					{
						int npcProj = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX * 2;
						Main.npc[npcProj].velocity.Y = shootToY * 2;
			
						Main.PlaySound(SoundID.Item21, (int)(npc.Center.X), (int)(npc.Center.Y));
					}
					if(npc.ai[0] == 50)
					{
						int npcProj = NPC.NewNPC((int)npc.Center.X + (int)(3 * shootToY), (int)npc.Center.Y - (int)(3 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX * 2;
						Main.npc[npcProj].velocity.Y = shootToY * 2;
			
						npcProj = NPC.NewNPC((int)npc.Center.X - (int)(3 * shootToY), (int)npc.Center.Y + (int)(3 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX * 2;
						Main.npc[npcProj].velocity.Y = shootToY * 2;
						Main.PlaySound(SoundID.Item21, (int)(npc.Center.X), (int)(npc.Center.Y));
					}
					if(npc.ai[0] == 60)
					{
						int npcProj = NPC.NewNPC((int)npc.Center.X + (int)(6 * shootToY), (int)npc.Center.Y - (int)(6 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX * 2;
						Main.npc[npcProj].velocity.Y = shootToY * 2;
			
						npcProj = NPC.NewNPC((int)npc.Center.X - (int)(6 * shootToY), (int)npc.Center.Y + (int)(6 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX * 2;
						Main.npc[npcProj].velocity.Y = shootToY * 2;
						
						Main.PlaySound(SoundID.Item21, (int)(npc.Center.X), (int)(npc.Center.Y));
					}
					if(npc.ai[0] == 70)
					{
						int npcProj = NPC.NewNPC((int)npc.Center.X + (int)(9 * shootToY), (int)npc.Center.Y - (int)(9 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX * 2;
						Main.npc[npcProj].velocity.Y = shootToY * 2;
			
						npcProj = NPC.NewNPC((int)npc.Center.X - (int)(9 * shootToY), (int)npc.Center.Y + (int)(9 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX * 2;
						Main.npc[npcProj].velocity.Y = shootToY * 2;
						
						Main.PlaySound(SoundID.Item21, (int)(npc.Center.X), (int)(npc.Center.Y));
					}
					if(npc.ai[0] == 80)
					{
						int npcProj = NPC.NewNPC((int)npc.Center.X + (int)(12 * shootToY), (int)npc.Center.Y - (int)(12 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX * 2;
						Main.npc[npcProj].velocity.Y = shootToY * 2;
			
						npcProj = NPC.NewNPC((int)npc.Center.X - (int)(12 * shootToY), (int)npc.Center.Y + (int)(12 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX * 2;
						Main.npc[npcProj].velocity.Y = shootToY * 2;
						
						Main.PlaySound(SoundID.Item21, (int)(npc.Center.X), (int)(npc.Center.Y));
					}
					if(npc.ai[0] == 90)
					{
						for(int i = 30; i < 330; i += 20/expertModifier)
						{
							Vector2 circularVelocity = new Vector2(-shootToX * 1.4f, -shootToY * 1.4f).RotatedBy(MathHelper.ToRadians(i));
							Projectile.NewProjectile((int)npc.Center.X, (int)npc.Center.Y, circularVelocity.X, circularVelocity.Y, mod.ProjectileType("PinkBullet"), 24, 1, 0);
						}
					}
					if(npc.ai[0] >= 120)
					{
						npc.ai[0] = -260;
					}
			}
			
			
			int Num3 = 0;
			if(phase == 5)
			{
				newRotation += 1;
				int Num = 0;
				for(int i = 0; i < 1000; i++)
				{
					Projectile hook = Main.projectile[i];
					if(hook.type == mod.ProjectileType("PutridHook") && hook.active)
					{
						float moveToX = hook.Center.X - npc.Center.X;
						float moveToY = hook.Center.Y - npc.Center.Y;
						float distanceTo = (float)System.Math.Sqrt((double)(moveToX * moveToX + moveToY * moveToY));
						Num++;
						if(distanceTo >= 90)
						{
							distanceTo = 2.5f / distanceTo;
								  
							moveToX *= distanceTo * 5;
							moveToY *= distanceTo * 5;
							hook.velocity.X = -moveToX;
							hook.velocity.Y = -moveToY;
							hook.knockBack = 0;
						}
						else
						{
							Num3++;
							Vector2 rotationalPosition = new Vector2(-80, 0).RotatedBy(MathHelper.ToRadians((Num * 30) + newRotation)); 
							moveToX = hook.Center.X - npc.Center.X + rotationalPosition.X;
							moveToY = hook.Center.Y - npc.Center.Y + rotationalPosition.Y;
							distanceTo = (float)System.Math.Sqrt((double)(moveToX * moveToX + moveToY * moveToY));
							distanceTo = 1f / distanceTo;
								  
							moveToX *= distanceTo * 5;
							moveToY *= distanceTo * 5;
							hook.velocity.X = -moveToX;
							hook.velocity.Y = -moveToY;
							hook.rotation = MathHelper.ToRadians(Main.rand.Next(360));
						}
					}
				}
			}
			if(Num3 >= 12)
			{
				phase = 6;
				initiatePinky = -1;
			}
			if(phase == 6)
			{
				npc.aiStyle = 0;  
				newRotation += 1;
				int Num = 0;
				for(int i = 0; i < 1000; i++)
				{
					Projectile hook = Main.projectile[i];
					if(hook.type == mod.ProjectileType("PutridHook") && hook.active)
					{
							Num++;
							Vector2 rotationalPosition = new Vector2(-80, 0).RotatedBy(MathHelper.ToRadians((Num * 30) + newRotation)); 
								
							hook.position.X = npc.Center.X + rotationalPosition.X - (hook.width/2);
							hook.position.Y = npc.Center.Y + rotationalPosition.Y - (hook.width/2);
								
							hook.rotation = MathHelper.ToRadians(Main.rand.Next(360));
							hook.velocity.X = 0;
							hook.velocity.Y = 0;
					}
				}
				if(initiatePinky == -1)
				{
					NPC.NewNPC((int)npc.Center.X + 200, (int)npc.Center.Y + 200, mod.NPCType("PinkyFlyer"));
					NPC.NewNPC((int)npc.Center.X + 200, (int)npc.Center.Y - 200, mod.NPCType("PinkyFlyer"));	
					NPC.NewNPC((int)npc.Center.X - 200, (int)npc.Center.Y + 200, mod.NPCType("PinkyFlyer"));	
					NPC.NewNPC((int)npc.Center.X - 200, (int)npc.Center.Y - 200, mod.NPCType("PinkyFlyer"));	
					initiatePinky = 0;
				}
				
				
				if(npc.ai[0] == 40)
					{
						int npcProj = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX * 2;
						Main.npc[npcProj].velocity.Y = shootToY * 2;
						
						Projectile.NewProjectile((int)npc.Center.X, (int)npc.Center.Y, shootToX * -2, shootToY * -2, mod.ProjectileType("PinkBullet"), 24, 1, 0);
			
						Main.PlaySound(SoundID.Item21, (int)(npc.Center.X), (int)(npc.Center.Y));
					}
					if(npc.ai[0] == 50)
					{
						int npcProj = NPC.NewNPC((int)npc.Center.X + (int)(3 * shootToY), (int)npc.Center.Y - (int)(3 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX * 2;
						Main.npc[npcProj].velocity.Y = shootToY * 2;
			
						npcProj = NPC.NewNPC((int)npc.Center.X - (int)(3 * shootToY), (int)npc.Center.Y + (int)(3 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX * 2;
						Main.npc[npcProj].velocity.Y = shootToY * 2;
						
						Projectile.NewProjectile((int)npc.Center.X + (int)(3 * shootToY), (int)npc.Center.Y - (int)(3 * shootToX), shootToX * -2, shootToY * -2, mod.ProjectileType("PinkBullet"), 24, 1, 0);
						
						Projectile.NewProjectile((int)npc.Center.X - (int)(3 * shootToY), (int)npc.Center.Y + (int)(3 * shootToX), shootToX * -2, shootToY * -2, mod.ProjectileType("PinkBullet"), 24, 1, 0);
						
						Main.PlaySound(SoundID.Item21, (int)(npc.Center.X), (int)(npc.Center.Y));
					}
					if(npc.ai[0] == 60)
					{
						int npcProj = NPC.NewNPC((int)npc.Center.X + (int)(6 * shootToY), (int)npc.Center.Y - (int)(6 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX * 2;
						Main.npc[npcProj].velocity.Y = shootToY * 2;
			
						npcProj = NPC.NewNPC((int)npc.Center.X - (int)(6 * shootToY), (int)npc.Center.Y + (int)(6 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX * 2;
						Main.npc[npcProj].velocity.Y = shootToY * 2;
						
						Projectile.NewProjectile((int)npc.Center.X + (int)(6 * shootToY), (int)npc.Center.Y - (int)(6 * shootToX), shootToX * -2, shootToY * -2, mod.ProjectileType("PinkBullet"), 24, 1, 0);
						
						Projectile.NewProjectile((int)npc.Center.X - (int)(6 * shootToY), (int)npc.Center.Y + (int)(6 * shootToX), shootToX * -2, shootToY * -2, mod.ProjectileType("PinkBullet"), 24, 1, 0);
						
						Main.PlaySound(SoundID.Item21, (int)(npc.Center.X), (int)(npc.Center.Y));
					}
					if(npc.ai[0] >= 90)
					{
						npc.ai[0] = 0;
					}
				
				
				if(!NPC.AnyNPCs(mod.NPCType("PinkyFlyer")))
				{
					phase = 7;
					for(int i = 0; i < 1000; i++)
					{
						Projectile hook = Main.projectile[i];
						if(hook.type == mod.ProjectileType("PutridHook") && hook.active)
						{
							hook.Kill();
						}
					}
					Main.PlaySound(2, (int)(npc.Center.X), (int)(npc.Center.Y), 15);
				}
			}
			if(phase == 7)
			{
				float hpMod = (npc.lifeMax / (npc.life + 2000));
				float randX = Main.rand.Next(-1,2) * hpMod;
				float randY = Main.rand.Next(-1,2) * hpMod;
				npc.position.X += randX;
				npc.position.Y += randY;
				npc.ai[0] += hpMod;
				if(npc.ai[0] >= 100 - ((expertModifier -1) * 40))
				{
					npc.ai[0] = 0;
					Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-58,59), npc.Center.Y + Main.rand.Next(-58,59), 0, 0, mod.ProjectileType("PinkExplosion"), 33, 1, 0);
				}
				if(npc.life > 100)
				{
					npc.life--;
				}
				
			}
		}
		public override void HitEffect(int hitDirection, double damage) 
		{
			if (npc.life > 0) {
				for (int i = 0; i < 3; i++) 
				{
					Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 72, hitDirection, -1f, 100, new Color(100, 100, 100, 100), 1f);
					dust.noGravity = true;
				}
				return;
			}
		}
		public override void BossLoot(ref string name, ref int potionType)
		{ 
			SOTSWorld.downedPinky = true;
			potionType = ItemID.HealingPotion;
		
			if(Main.expertMode)
			{ 
				npc.DropBossBags();
			} 
			else 
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("WormWoodCore"), 1); 
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.PinkGel, Main.rand.Next(20,50)); 
			}
		}
	}
}