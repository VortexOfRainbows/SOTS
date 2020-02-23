using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss
{[AutoloadBossHead]
	public class PharaohsCurse : ModNPC
	{
		float ai1 = 0;
		float ai2 = 0;
		float ai3 = 0;
		int animationType = 0;
		int frame = 1;
		int currentFrame;
		int initiate = -1;
		int despawn = 0;
		int finishTeleport = -1;
		int direction2 = -1;
		int teleportAttackType = 0;
		bool inBlock = false;
		int expertScale = 1;
		float teleportX;
		float teleportY;
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(ai1);
			writer.Write(ai2);
			writer.Write(ai3);
			writer.Write(animationType);
			writer.Write(frame);
			writer.Write(currentFrame);
			writer.Write(initiate);
			writer.Write(finishTeleport);
			writer.Write(direction2);
			writer.Write(teleportAttackType);
			writer.Write(inBlock);
			writer.Write(expertScale);
			writer.Write(teleportX);
			writer.Write(teleportY);
			writer.Write(npc.scale);
			writer.Write(npc.dontTakeDamage);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			ai1 = reader.ReadSingle();
			ai2 = reader.ReadSingle();
			ai3 = reader.ReadSingle();
			animationType = reader.ReadInt32();
			frame = reader.ReadInt32();
			currentFrame = reader.ReadInt32();
			initiate = reader.ReadInt32();
			finishTeleport = reader.ReadInt32();
			direction2 = reader.ReadInt32();
			teleportAttackType = reader.ReadInt32();
			inBlock = reader.ReadBoolean();
			expertScale = reader.ReadInt32();
			teleportX = reader.ReadSingle();
			teleportY = reader.ReadSingle();
			npc.scale = reader.ReadSingle();
			npc.dontTakeDamage = reader.ReadBoolean();
		}
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Pharaoh's Curse");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 14; 
			npc.lifeMax = 6000;
            npc.damage = 45;   
            npc.defense = 5; 
            npc.knockBackResist = 0f;
            npc.width = 70;
            npc.height = 76; 
            Main.npcFrameCount[npc.type] = 48;  
            npc.npcSlots = 20f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = false;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCDeath6;
            music = MusicID.Sandstorm;
			musicPriority = MusicPriority.BossMedium;
            npc.buffImmune[24] = true;
            npc.buffImmune[39] = true;
            npc.buffImmune[44] = true;
            npc.buffImmune[69] = true;
            npc.buffImmune[70] = true;
            npc.buffImmune[153] = true;
			bossBag = mod.ItemType("CurseBag");
           // npc.netUpdate = true;
            npc.netAlways = true;
		}
		public override void BossLoot(ref string name, ref int potionType)
		{ 
			if(!SOTSWorld.downedCurse)
			{
				Main.NewText("The pyramid's curse weakens", 155, 115, 0);
			}
			SOTSWorld.downedCurse = true;
			potionType = ItemID.HealingPotion;
		
			if(Main.expertMode)
			{ 
				npc.DropBossBags();
			} 
			else 
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CursedMatter"),Main.rand.Next(12, 25)); 
			}
		}
		public override void FindFrame(int frameHeight) 
		{
			frame = frameHeight;
			if(animationType == 0)
			{
				if (ai1 >= 3.5f) 
				{
					ai1 -= 3.5f;
					npc.frame.Y += frame;
					if(npc.frame.Y == 9 * frame && Main.rand.Next(3) != 0)
					{
						npc.frame.Y = 0;
					}
					if(npc.frame.Y >= 11 * frame)
					{
						npc.frame.Y = 0;
					}
				}
			}
			else
			{
				if (ai1 >= 1.5f || (Main.expertMode && ai1 >= 0.75f)) 
				{
					if(!Main.expertMode)
					{
					ai1 -= 1.5f;
					}
					else
					{
					ai1 -= 0.75f;
					}
					if(npc.frame.Y >= 11 * frame && animationType == 1)
					{
						npc.frame.Y += frame;
						if(npc.frame.Y >= 47 * frame)
						{
							npc.frame.Y = 47 * frame;
							teleportFinish(teleportX, teleportY);
						}
					}
					else if(animationType == 1)
					{
						npc.frame.Y = 11 * frame;
					}
					
					if(animationType == 2)
					{
						npc.frame.Y -= frame;
						if(npc.frame.Y <= 11 * frame)
						{
							ai3++;
							npc.frame.Y = 0;
							animationType = 0;
							finishTeleport = 0;
						}
					}
				}
			}
		}
		public void teleportTo(float x, float y)
		{
			if(animationType == 0)
			{
				npc.aiStyle = 0;
				npc.velocity.X *= 0;
				npc.velocity.Y *= 0;
				animationType = 1;
				teleportX = x;
				teleportY = y;
			}
		}
		public void teleportFinish(float x, float y)
		{
			if(animationType == 1)
			{
				npc.aiStyle = 0;
				npc.velocity.X *= 0;
				npc.velocity.Y *= 0;
				animationType = 2;
				npc.position.X = x - npc.width/2;
				npc.position.Y = y - npc.height/2;
			}
		}
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)((npc.lifeMax * bossLifeScale * 0.75f));  
            npc.damage = (int)(npc.damage * 0.8f); 
        }
		public override bool PreAI()
		{
			npc.netUpdate = true;
			inBlock = false;
			int x = (int)(npc.Center.X / 16);
			int y =	(int)(npc.Center.Y / 16);
			if(Main.tile[x, y].active() && Main.tileSolid[Main.tile[x, y].type] == true && Main.tileTable[Main.tile[x, y].type] == false)
			{
				inBlock = true;
			}
			if(Main.expertMode)
			{
				expertScale = 2;
			}
			if(initiate == -1)
			{
				
				npc.aiStyle = 0; 
				npc.scale = .28f;
				npc.width = (int)(70 * npc.scale);
				npc.height = (int)(76 * npc.scale);
				npc.velocity.X = 0;
				npc.velocity.Y = 0;
				npc.dontTakeDamage = true;
				initiate = 1;
			}
			ai2++;
			if(animationType == 0 && ai2 >= 360)
			{
				if(initiate == 1)
				{
					npc.aiStyle = 14; 
					initiate = 2;
				}
				int num1 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 70, 50, mod.DustType("CurseDust"));
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity.X = npc.velocity.X;
				Main.dust[num1].velocity.Y = -5;
				npc.dontTakeDamage = false;
			}
			ai1++;
			if(npc.velocity.X > 1.5)
			{
				npc.spriteDirection = 1;
			}
			if(npc.velocity.X < -1.5)
			{
				npc.spriteDirection = -1;
			}
			return true;
		}
		public override void AI()
		{
			int damage = npc.damage / 2;
			if (Main.expertMode) 
			{
				damage = (int)(damage / Main.expertDamage);
			}
			
			if(ai2 % 5 == 0 && ai2 <= 360)
			{
				npc.scale += 0.01f;
					
				npc.width = (int)(70 * npc.scale);
				npc.height = (int)(76 * npc.scale);
				
				for(int i = 0; i < 10; i++)
				{
					int num1 = Dust.NewDust(new Vector2(npc.position.X - 32, npc.position.Y - 32), npc.width + 64, npc.height + 64, mod.DustType("CurseDust"));
					Main.dust[num1].noGravity = true;
					float dusDisX = Main.dust[num1].position.X + 3 - npc.Center.X;
					float dusDisY = Main.dust[num1].position.Y + 3 - npc.Center.Y;
					//double dis = Math.Sqrt((double)(dusDisX * dusDisX + dusDisY * dusDisY))
						  
					dusDisX *= -0.03f;
					dusDisY *= -0.03f;
				   
					Main.dust[num1].velocity.X = dusDisX;
					Main.dust[num1].velocity.Y = dusDisY;
				}
			}
            npc.TargetClosest(false);
			Player player = Main.player[npc.target];
					  
			
			
			if(ai2 >= 400 && animationType != 1 && animationType != 2)
			ai3++;
			
			if(ai3 == 90)
			{
				npc.aiStyle = 14; 
				teleportAttackType = 0;
				finishTeleport = -1;
				if(Main.netMode != 1)
				{
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, mod.ProjectileType("CurseSpike"), damage, 0, Main.myPlayer, 90, 270);
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, mod.ProjectileType("CurseSpike"), damage, 0, Main.myPlayer, 180, 330);
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, mod.ProjectileType("CurseSpike"), damage, 0, Main.myPlayer, 270, 390);
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, mod.ProjectileType("CurseSpike"), damage, 0, Main.myPlayer, 360, 450);
				}
			}
			
			int direction = -1;
			
			
			if(Main.rand.Next(2) == 0)
			{
				direction = 1;
			}
			
			
			if(ai3 == 600)
			{
				teleportTo(player.Center.X + direction * (Main.rand.Next(200)), player.Center.Y - 100);
			}
			
		
			
			if(ai3 == 900 && !NPC.AnyNPCs(mod.NPCType("CurseFragment")))
			{
				if(Main.netMode != 1)
				{
					int npc1 = NPC.NewNPC((int)npc.Center.X + 40, (int)npc.Center.Y, mod.NPCType("CurseFragment"));
					Main.npc[npc1].netUpdate = true;
					npc1 = NPC.NewNPC((int)npc.Center.X - 40, (int)npc.Center.Y, mod.NPCType("CurseFragment"));
					Main.npc[npc1].netUpdate = true;
					if(Main.expertMode)
					{
						npc1 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y - 40, mod.NPCType("CurseFragment"));
						Main.npc[npc1].netUpdate = true;
					}
				}
				Main.PlaySound(SoundID.NPCDeath6, (int)(npc.Center.X), (int)(npc.Center.Y));
			}
			
			if(ai3 == 1100)
			{
				teleportTo(player.Center.X, player.Center.Y - 400);
			}
			if(ai3 == 1150)
			{
				teleportTo(player.Center.X, player.Center.Y - 400);
			}
			if(ai3 == 1200)
			{
				teleportTo(player.Center.X, player.Center.Y - 400);
			}
			if(ai3 == 1250)
			{
				teleportTo(player.Center.X, player.Center.Y);
				teleportAttackType = 2;
			}
			if(ai3 >= 1300)
			{
				ai3 = 0;
			}
			
			if(finishTeleport >= 0)
			{
				finishTeleport++;
			}
			if(finishTeleport >= 30 && teleportAttackType == 0)
			{
				if(finishTeleport == 30)
				{
					if(Main.rand.Next(2) == 0)
					{
						direction2 = 1;
					}
					else
					{
						direction2 = -1;
					}
					if(inBlock)
					{
						for(int i = 0; i < 5; i++)
						{
							int goreIndex = Gore.NewGore(new Vector2(npc.Center.X, npc.Center.Y), default(Vector2), Main.rand.Next(61,64), 1f);	
							Main.gore[goreIndex].scale = 1.75f;
						}
						for(int i = 0; i < 6 + (expertScale / 2); i++)
						{
							if(Main.netMode != 1)
							{
								Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-100,101), npc.Center.Y + Main.rand.Next(-20,21), 0, 7 + Main.rand.Next(-2,3), mod.ProjectileType("PyramidCollapse"), (int)(damage * 1.5f), 0, Main.myPlayer);
							}
						}
						Main.PlaySound(SoundID.Item14, (int)(npc.Center.X), (int)(npc.Center.Y));
					}
				}
				if(finishTeleport >= 90)
				{
					teleportAttackType = 1;
					finishTeleport = -1;
					npc.aiStyle = 14;
					Main.PlaySound(SoundID.NPCDeath6, (int)(npc.Center.X), (int)(npc.Center.Y));
				}
				if(!inBlock)
				{
					if(direction2 == 1 && finishTeleport % 4 == 0)
					{
						if(Main.netMode != 1)
						{
							int lineProj = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 7 * direction, 0, mod.ProjectileType("CurseBall"), damage, 0, 0);
							Main.projectile[lineProj].timeLeft = 2700;
							NetMessage.SendData(27, -1, -1, null, lineProj);
						}
					}
						
					if(direction2 == -1 && finishTeleport % 4 == 0)
					{
						if(Main.netMode != 1)
						{
							int lineProj = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 7 * direction, mod.ProjectileType("CurseBall"), damage, 0, 0);
							Main.projectile[lineProj].timeLeft = 2700;
							NetMessage.SendData(27, -1, -1, null, lineProj);
						}
					}
				}
			}
			if(finishTeleport >= 10 && teleportAttackType == 1)
			{
				if(Main.rand.Next(2) == 0)
				{
					direction2 = 1;
				}
				else
				{
					direction2 = -1;
				}
				if(inBlock)
				{
					for(int i = 0; i < 5; i++)
					{
					int goreIndex = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y), default(Vector2), Main.rand.Next(61,64), 1f);	
					Main.gore[goreIndex].scale = 0.75f;
					}
					for(int i = 0; i < 6 + (expertScale / 2); i++)
					{	
						if(Main.netMode != 1)
						{
							Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-100,101), npc.Center.Y + Main.rand.Next(-20,21), 0, 7 + Main.rand.Next(-2,3), mod.ProjectileType("PyramidCollapse"), (int)(damage * 1.5f), 0, Main.myPlayer);
						}
					}
					Main.PlaySound(SoundID.Item14, (int)(npc.Center.X), (int)(npc.Center.Y));
				}
				else
				{
					for(int i = 0; i < 6 + (expertScale / 2); i++)
					{
						if(Main.netMode != 1)
						{
							Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-100,101), npc.Center.Y + Main.rand.Next(-20,21), 0, 7 + Main.rand.Next(-2,3), mod.ProjectileType("CurseBall"), (int)(damage * 1.25f), 0, Main.myPlayer);
						}
					}
					Main.PlaySound(SoundID.Item14, (int)(npc.Center.X), (int)(npc.Center.Y));
				}
				teleportAttackType = 1;
				finishTeleport = -1;
				npc.aiStyle = 14;
			}
			if(player.dead || !player.GetModPlayer<SOTSPlayer>().PyramidBiome)
			{
				despawn++;
			}
			else if(despawn > 200)
			{
				despawn--;
			}
			if(despawn >= 360)
			{
				npc.active = false;
			}
		}
	}
}