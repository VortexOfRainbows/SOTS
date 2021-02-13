using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss
{	[AutoloadBossHead]
	public class PutridPinkyPhase2 : ModNPC
	{
		private int expertModifier = 1;
		private float attackPhase {
			get => npc.ai[0];
			set => npc.ai[0] = value;
		}

		private float attackTimer {
			get => npc.ai[1];
			set => npc.ai[1] = value;
		}

		private float rotationSpeed {
			get => npc.ai[2];
			set => npc.ai[2] = value;
		}

		private float rotationDistance {
			get => npc.ai[3];
			set => npc.ai[3] = value;
		}

		private float fireToX = 0;
		private float fireToY = 0;
		private float followPlayer = 0;
		private float eyeReset = 0;
		
		private int storeDamage = 0;
		private int exponentialMod = 0;
		private int rotateDir = 1;
		int despawn = 0;
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(fireToX);
			writer.Write(fireToY);
			writer.Write(followPlayer);
			writer.Write(eyeReset);
			writer.Write(rotateDir);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			fireToX = reader.ReadSingle();
			fireToY = reader.ReadSingle();
			followPlayer = reader.ReadSingle();
			eyeReset = reader.ReadSingle();
			rotateDir = reader.ReadInt32();
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putrid Pinky");
		}
		public override void SetDefaults()
		{
            npc.aiStyle = -1;   
			npc.lifeMax = 5500;
            npc.damage = 40; 
            npc.defense = 0;   
            npc.knockBackResist = 0f;
            npc.width = 50;
            npc.height = 120;
            Main.npcFrameCount[npc.type] = 1;   
            npc.value = 150000;
            npc.npcSlots = 10f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath5;
            npc.buffImmune[20] = true;
			npc.alpha = 60;
            music = MusicID.Boss3;
			bossBag = mod.ItemType("PinkyBag");
		}
		const int alphaMin = 60;
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        { 
			for(int i = 0; i < Main.npc.Length; i++)
			{
				if(Main.npc[i].type == mod.NPCType("PutridHook") && Main.npc[i].active && (int)Main.npc[i].localAI[0] == npc.whoAmI)
				{
					Draw(Main.npc[i].Center);
				}
			}
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				if (Main.projectile[i].type == mod.ProjectileType("RecollectHook") && Main.projectile[i].active && (int)Main.projectile[i].ai[0] == npc.whoAmI)
				{
					Draw(Main.projectile[i].Center);
				}
			}
			return true;
        }
		public void Draw(Vector2 to, bool gore = false)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Boss/PutridVine");
			Vector2 position = npc.Center + new Vector2(0, 3.5f);
			Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
			float height = (float)texture.Height;
			Vector2 betweenPos = to - position;
			float rotation = betweenPos.ToRotation() - 1.57f;
			bool flag = true;
			if (float.IsNaN(position.X) && float.IsNaN(position.Y))
				flag = false;
			if (float.IsNaN(betweenPos.X) && float.IsNaN(betweenPos.Y))
				flag = false;
			bool flag2 = false;
			while (flag)
			{
				if ((double)betweenPos.Length() - texture.Height < 2.0)
				{
					flag = false;
				}
				else
				{
					float length = height;
					if (flag2)
					{
						if ((double)betweenPos.Length() - height < height + 2.0)
						{
							length = betweenPos.Length() - height + 2.0f;
						}
						Vector2 vector2_1 = betweenPos;
						vector2_1.Normalize();
						position += vector2_1 * height;
						betweenPos = to - position;
					}
					else
					{
						Vector2 vector2_1 = betweenPos;
						vector2_1.Normalize();
						position += vector2_1 * height * 0.5f;
						betweenPos = to - position;
					}
					if(!gore)
					{
						Color color2 = Lighting.GetColor((int)position.X / 16, (int)(position.Y / 16.0));
						color2 = npc.GetAlpha(color2);
						Main.spriteBatch.Draw(texture, position - Main.screenPosition, new Rectangle(0, 0, texture.Width, (int)length), color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);
					}
					else
                    {
						if(Main.rand.Next(5) >= 2)
						{
							Gore.NewGore(position, Vector2.Zero, mod.GetGoreSlot("Gores/PutridVineGore"), 1f);
						}
                    }
					flag2 = true;
				}
			}
		}
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * bossLifeScale * 0.7f) + 1;  
            npc.damage = (int)(npc.damage * 0.8f);  
        }
		
		public override bool PreAI()
		{
			if(Main.expertMode)
			{
				expertModifier = 2;
			}
			return true;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Boss/PutridPinkyEye");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawPos = npc.Center - Main.screenPosition;
			lightColor = lightColor * ((195 - npc.alpha + 60) / 195f);
			spriteBatch.Draw(texture, drawPos + new Vector2(0, 3.5f), null, lightColor, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
			DrawEye(spriteBatch, lightColor);
			base.PostDraw(spriteBatch, lightColor);
        }
        public void DrawEye(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Boss/PutridPinkyPupil");
			Vector2 drawOrigin = new Vector2(texture.Width/2, texture.Height/2);
			Vector2 drawPos = npc.Center - Main.screenPosition;
			
			float shootToX = fireToX - npc.Center.X;
			float shootToY = fireToY - npc.Center.Y + 3.5f;
			float distance = (float)Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

			distance = eyeReset * 2f * (npc.scale) / distance;
			shootToX *= distance * 2;
			shootToY *= distance * 2;
			
			drawPos.X += shootToX;
			drawPos.Y += 3.5f + shootToY;
			if(npc.scale == 1)
				spriteBatch.Draw(texture, drawPos, null, drawColor, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
		}
		private void DustCircle(float x, float y, float j, float dist, float scaleMod)
		{
			for(float i = 0; i < 360; i += j)
			{
				Vector2 circularLocation = new Vector2(-dist, 0).RotatedBy(MathHelper.ToRadians(i));
				
				int num1 = Dust.NewDust(new Vector2(x - 4, y - 4), 4, 4, 72);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity = circularLocation * 0.08f;
				//Main.dust[num1].alpha = 0;
				Main.dust[num1].scale *= 2.5f;
			}
		}
		public override void AI()
		{
			npc.TargetClosest(true);
			Player player  = Main.player[npc.target];
			float shootToX = player.Center.X - npc.Center.X;
			float shootToY = player.Center.Y - npc.Center.Y;
			float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

			distance = 1f / distance;
				  
			shootToX *= distance * 5;
			shootToY *= distance * 5;
			Vector2 toPlayer = new Vector2(shootToX, shootToY);
			
			if(attackPhase == 0)
			{
				storeDamage = npc.damage;
				eyeReset = 1;
				attackPhase = 1;
				attackTimer = 1200;
				followPlayer = 1;
				rotateDir = Main.rand.Next(2) == 1 ? -1 : 1;
				rotationSpeed = 0.4f * rotateDir;
				InitiateHooks();
				return;
			}
			if(followPlayer == 1)
			{
				fireToX = player.Center.X;
				fireToY = player.Center.Y;
			}
			if(attackPhase == 1)
			{
				if(rotationDistance < 119)
                {
					rotationDistance += 1f;
				}
				else if (rotationDistance > 121)
				{
					rotationDistance -= 1f;
				}
				else
                {
					rotationDistance = 120;
                }
				rotationSpeed = rotationSpeed * rotateDir > 0.4f ? rotationSpeed * 0.99f : rotationSpeed * 1.01f;
				eyeReset = eyeReset < 1 ? eyeReset + 0.08f : 1;
				attackTimer--;
				if(attackTimer == 900 || attackTimer == 630 || attackTimer == 360 || attackTimer == 840 || attackTimer == 570 || attackTimer == 300 || attackTimer == 780 || attackTimer == 510 || attackTimer == 240)
				{
					followPlayer = 0;
					DustCircle(npc.Center.X, npc.Center.Y, 10, 128, 2);
				}
				if(attackTimer == 870 || attackTimer == 600 || attackTimer == 330 || attackTimer == 810 || attackTimer == 540 || attackTimer == 270 || attackTimer == 750 || attackTimer == 480 || attackTimer == 210)
				{
					LaunchLaser(npc.Center, new Vector2(fireToX, fireToY));
					followPlayer = 1;
				}
				if(attackTimer == 0) // next pattern
				{
					followPlayer = 1;
					attackPhase = 2;
					attackTimer = 1200;
					exponentialMod = 12;
					storeDamage = npc.damage;
					npc.damage = 0;
				}
			}
			if(attackPhase == 2)
			{
				attackTimer--;
				if(attackTimer >= 1000)
				{
					npc.alpha += 3;
					rotationSpeed *= 1.012f;
					if (rotationDistance < 299)
					{
						rotationDistance += 1.5f;
					}
					else if (rotationDistance > 301)
					{
						rotationDistance -= 1.5f;
					}
					else
					{
						rotationDistance = 300;
					}
					npc.velocity *= 0.92f;
					npc.velocity += toPlayer * 0.22f;
					if(npc.alpha > 255)
					{
						npc.alpha = 255;
					}
				}
				else if(attackTimer > 950)
				{
					npc.velocity *= 0.5f;
					rotationSpeed *= 0.959f;
					npc.alpha -= 4;
					if(npc.alpha < alphaMin)
					{
						npc.alpha = alphaMin;
					}
				}
				else if(attackTimer >= 940)
				{
					npc.alpha = alphaMin;
					rotationDistance += exponentialMod;
					exponentialMod--;
				}
				else if(attackTimer > 0)
				{
					rotationDistance += exponentialMod;
					rotationDistance = rotationDistance < 0 ? 0 : rotationDistance;
					exponentialMod--; 
					npc.alpha = alphaMin;
					rotateDir = Main.rand.Next(2) == 1 ? -1 : 1;
				}
				if(rotationDistance == 0 && attackTimer > 0)
				{	
					npc.damage = storeDamage;
					rotationSpeed = 7f * rotateDir;
					attackTimer = -1;
					rotationDistance = 1;
					BurstAttack(1, 5f);
					BurstSpiral();
				}
				if(attackTimer == -10)
				{
					rotationDistance = 4;
					BurstAttack(1, 6.5f);
				}
				if(attackTimer == -20)
				{
					rotationDistance = 4;
					attackPhase = 3;
					attackTimer = 1800;
					BurstAttack(1, 8f);
					exponentialMod = -20;
				}
			}
			else
            {
				npc.velocity *= 0.98f;
				Vector2 vectorToPlayer = player.Center - npc.Center;
				bool loS = Collision.CanHitLine(player.position, player.width, player.height, npc.position, npc.width, npc.height);
				if (vectorToPlayer.Length() > 560 || !loS)
				{
					npc.velocity *= 0.9775f;
					float speedMult = vectorToPlayer.Length() * 0.01f;
					if(!loS)
						speedMult += 44;
					npc.velocity += toPlayer.SafeNormalize(Vector2.Zero) * (0.0125f * speedMult);
				}
				double distanceTB = 1600;
				if(npc.alpha == alphaMin && attackTimer > 300)
				{
					for (int i = 0; i < Main.npc.Length; i++) //find first enemy
					{
						NPC hook = Main.npc[i];
						if (Main.npc[i].type == mod.NPCType("HookTurret") && Main.npc[i].active && (int)Main.npc[i].localAI[0] == npc.whoAmI)
						{
							float disX = hook.Center.X - npc.Center.X;
							float disY = hook.Center.Y - npc.Center.Y;
							double dis = Math.Sqrt(disX * disX + disY * disY);
							if (dis > distanceTB || (Main.rand.NextBool(Main.expertMode ? 1200 + (npc.life / 2) : 1800 + (npc.life / 2)) && npc.life < npc.lifeMax / 2))
							{
								Recollect(Main.npc[i]);
							}
						}
					}
				}
			}
			if(attackPhase == 3)
			{
				if (rotationDistance < 119)
				{
					rotationDistance += 1.5f;
				}
				else if (rotationDistance > 121)
				{
					rotationDistance -= 1.5f;
				}
				else
				{
					rotationDistance = 120;
				}
				rotationSpeed = rotationSpeed * rotateDir > 0.7f ? rotationSpeed * 0.99f : rotationSpeed * 1.01f;
				eyeReset = eyeReset < 1 ? eyeReset + 0.08f : 1;
				attackTimer--;
				if(attackTimer == 1500)
				{
					int total = 0;
					int npcIndex = -1;
					int npcIndex1 = -1;
					int npcIndex2 = -1;
					for (int j = 0; j < 3; j++)
					{
						double distanceTB = 1600;
						for (int i = 0; i < Main.npc.Length; i++) //find first enemy
						{
							NPC hook = Main.npc[i];
							if (Main.npc[i].type == mod.NPCType("HookTurret") && Main.npc[i].active && (int)Main.npc[i].localAI[0] == npc.whoAmI)
							{
								if(j == 0)
									total++;
								if (npcIndex != i && npcIndex1 != i && npcIndex2 != i)
								{
									float disX = hook.Center.X - npc.Center.X;
									float disY = hook.Center.Y - npc.Center.Y;
									double dis = Math.Sqrt(disX * disX + disY * disY);
									if (dis < distanceTB && j == 0)
									{
										distanceTB = dis;
										npcIndex = i;
									}
									if (dis < distanceTB && j == 1)
									{
										distanceTB = dis;
										npcIndex1 = i;
									}
									if (dis < distanceTB && j == 2)
									{
										distanceTB = dis;
										npcIndex2 = i;
									}
								}
							}
						}
					}
					if(npcIndex != -1 && total > 0)
                    {
						Recollect(Main.npc[npcIndex]);
					}
					if (npcIndex1 != -1 && (total >= 4 || Main.expertMode))
					{
						Recollect(Main.npc[npcIndex1]);
					}
					if (npcIndex2 != -1 && (total >= 8 || (total >= 5 && Main.expertMode)))
					{
						Recollect(Main.npc[npcIndex2]);
					}
				}
				if(attackTimer <= 1100)
				{
					if(exponentialMod > -11)
					{
						if(exponentialMod == -5)
						{
							rotateDir *= -1;
							rotationSpeed = Main.rand.Next(30,61) * 0.1f * rotateDir;
						}
						rotationDistance += exponentialMod;
						exponentialMod--;
					}
				}
				if(attackTimer == 990 || attackTimer == 960 || attackTimer == 930 || attackTimer == 690 || attackTimer == 660 || attackTimer == 630 || attackTimer == 390 || attackTimer == 360 || attackTimer == 330)
				{
					for(int i = 0; i < 200; i++)
					{
						if(Main.npc[i].type == mod.NPCType("PutridHook") && Main.npc[i].active && (int)Main.npc[i].localAI[0] == npc.whoAmI)
						{
							DustCircle(Main.npc[i].Center.X, Main.npc[i].Center.Y, 20, 64, 1);
						}
					}
				}
				if(attackTimer == 900 || attackTimer == 600 || attackTimer == 300)
				{
					rotationSpeed = 0;
					exponentialMod = 10;
					BurstAttack(2, 14f);
					BurstAttack(2, 10f);
				}
				if(attackTimer == 200)
				{
					attackTimer = 1200;
					attackPhase = 1;
				}
			}
			npc.dontTakeDamage = false;
			if(npc.alpha > alphaMin + 30)
			{
				npc.dontTakeDamage = true;
			}
			for(int i = 0; i < 200; i++)
			{
				if(Main.npc[i].type == mod.NPCType("PutridHook") && Main.npc[i].active)
				{
					Main.npc[i].ai[0] = player.Center.X;
					Main.npc[i].ai[1] = player.Center.Y;
					if(attackPhase == 2)
					{
						Main.npc[i].ai[0] = npc.Center.X;
						Main.npc[i].ai[1] = npc.Center.Y + 3.5f;
					}
					if(attackPhase == 3)
					{
						float aimToX = Main.npc[i].Center.X - npc.Center.X;
						float aimToY = Main.npc[i].Center.Y - (npc.Center.Y + 3.5f);
						Vector2 aimTo = new Vector2(100, 0).RotatedBy(Math.Atan2(aimToY, aimToX));
						aimToX = aimTo.X + Main.npc[i].Center.X;
						aimToY = aimTo.Y + Main.npc[i].Center.Y;
						
						Main.npc[i].ai[0] = aimToX;
						Main.npc[i].ai[1] = aimToY;
					}
					Main.npc[i].alpha = npc.alpha;
				}
			}
			if(Main.player[npc.target].dead)
			{
				despawn++;
			}
			if(despawn >= 720)
			{
				npc.active = false;
			}
			
			if(Main.netMode != 1)
			npc.netUpdate = true;
		}
		private void BurstSpiral()
		{
			for(int i = 0; i < 200; i++)
			{
				if(Main.npc[i].type == mod.NPCType("HookTurret") && Main.npc[i].active)
				{
					if(Main.npc[i].ai[2] == -1)
					{
						Main.npc[i].ai[3] = Main.rand.Next(76, 125);
						//Main.npc[i].netUpdate = true;
					}
				}
			}
		}
		public void Recollect(NPC hook)
        {
			if(Main.netMode != NetmodeID.MultiplayerClient && hook.localAI[2] != -1)
				Projectile.NewProjectile(npc.Center, new Vector2(4, 0), ModContent.ProjectileType<RecollectHook>(), 0, 0, Main.myPlayer, npc.whoAmI, hook.whoAmI);
			hook.localAI[2] = -1;
			hook.netUpdate = true;
        }
		private void BurstAttack(int type, float speed)
		{
			for(int i = 0; i < 200; i++)
			{
				if(Main.npc[i].type == mod.NPCType("PutridHook") && Main.npc[i].active && (int)Main.npc[i].localAI[0] == npc.whoAmI)
				{
					float aimToX = npc.Center.X - Main.npc[i].Center.X;
					float aimToY = npc.Center.Y + 3.5f - Main.npc[i].Center.Y;
					Vector2 aimTo = new Vector2(speed, 0).RotatedBy(Math.Atan2(aimToY, aimToX) + MathHelper.ToRadians(180));
					
					int damage = npc.damage / 2;
					if (Main.expertMode) 
					{
						damage = (int)(damage / Main.expertDamage);
					}
			
					if(Main.netMode != 1 && type == 1)
						Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, aimTo.X, aimTo.Y, mod.ProjectileType("PinkBullet"), damage, 0, Main.myPlayer);
				
					damage = (npc.damage + 20) / 2;
					if (Main.expertMode) 
					{
						damage = (int)(damage / Main.expertDamage);
					}
					
					if(Main.netMode != 1 && type == 2)
					{
						Vector2 aimTo2 = new Vector2(aimTo.X, aimTo.Y).RotatedBy(MathHelper.ToRadians(15));
						Vector2 aimTo3 = new Vector2(aimTo.X, aimTo.Y).RotatedBy(MathHelper.ToRadians(-15));
						Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, aimTo.X, aimTo.Y, mod.ProjectileType("PinkTracer"), damage, 0, Main.myPlayer);
						Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, aimTo2.X, aimTo2.Y, mod.ProjectileType("PinkTracer"), damage, 0, Main.myPlayer);
						Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, aimTo3.X, aimTo3.Y, mod.ProjectileType("PinkTracer"), damage, 0, Main.myPlayer);
						if(Main.expertMode)
						{
							Vector2 aimTo4 = new Vector2(aimTo.X, aimTo.Y).RotatedBy(MathHelper.ToRadians(7.5f));
							Vector2 aimTo5 = new Vector2(aimTo.X, aimTo.Y).RotatedBy(MathHelper.ToRadians(-7.5f));
							Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, aimTo4.X, aimTo4.Y, mod.ProjectileType("PinkTracer"), damage, 0, Main.myPlayer);
							Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, aimTo5.X, aimTo5.Y, mod.ProjectileType("PinkTracer"), damage, 0, Main.myPlayer);
						}
					
					}
				}
			}
			return;
		}
		private void LaunchLaser(Vector2 fromArea, Vector2 toArea)
		{
			int damage = npc.damage / 2;
			if (Main.expertMode) 
			{
				damage = (int)(damage / Main.expertDamage);
			}
			Vector2 direction = fromArea - toArea;
			direction.Normalize();
			direction *= 1500;
			if(Main.netMode != 1)
				Projectile.NewProjectile(fromArea.X, fromArea.Y, 0, 0, mod.ProjectileType("PinkLaser"), damage, 0, Main.myPlayer, toArea.X - direction.X, toArea.Y - direction.Y);
			//NetMessage.SendData(27, -1, -1, null, Probe);
			eyeReset = -0.8f;
			npc.velocity += direction.SafeNormalize(Vector2.Zero) * 3.5f;
			Main.PlaySound(SoundID.Item94, (int)(fromArea.X), (int)(fromArea.Y));
		}
		private void InitiateHooks()
		{
			Player player  = Main.player[npc.target];
			if(Main.netMode != 1)
			{
				for(int i = 0; i < 12; i++)
				{
					int npcProj = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("PutridHook"), 0, player.Center.X, player.Center.Y, i * 30);
					Main.npc[npcProj].localAI[0] = npc.whoAmI;
					Main.npc[npcProj].netUpdate = true;
				}
			}
		}
		public override void HitEffect(int hitDirection, double damage) 
		{
			if (npc.life > 0) 
			{
				for (int i = 0; i < 10; i++) 
				{
					Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 72, hitDirection * 3, 0, 100, new Color(100, 100, 100, 100), 1f);
					dust.noGravity = true;
				}
				return;
			}
			else
			{
				for (int i = 0; i < 35; i++)
				{
					Dust dust = Dust.NewDustDirect(npc.position - new Vector2(5), npc.width, npc.height, 72, hitDirection * 3, 0, 100, new Color(100, 100, 100, 100), 1f);
					dust.noGravity = true;
					dust.scale *= 2;
					dust.velocity *= 2;
				}
				Gore.NewGore(npc.position + new Vector2(0, 24), npc.velocity, mod.GetGoreSlot("Gores/ppGore_1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ppGore_2"), 1f);
				Gore.NewGore(npc.position + new Vector2(24, 0), npc.velocity, mod.GetGoreSlot("Gores/ppGore_3"), 1f);
				for (int i = 0; i < Main.npc.Length; i++)
				{
					if (Main.npc[i].type == mod.NPCType("PutridHook") && Main.npc[i].active && (int)Main.npc[i].localAI[0] == npc.whoAmI)
					{
						Draw(Main.npc[i].Center, true);
					}
				}
				for (int i = 0; i < Main.projectile.Length; i++)
				{
					if (Main.projectile[i].type == mod.ProjectileType("RecollectHook") && Main.projectile[i].active && (int)Main.projectile[i].ai[0] == npc.whoAmI)
					{
						Draw(Main.projectile[i].Center, true);
					}
				}
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
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Wormwood"), Main.rand.Next(20,50)); 
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.PinkGel, Main.rand.Next(20,50)); 
			}
		}
	}
}