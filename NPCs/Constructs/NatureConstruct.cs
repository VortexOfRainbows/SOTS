using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Constructs
{
	public class NatureConstruct : ModNPC
		{
			int ai1;
			int initiateSpeed = 1;
			int ai2 = 30;
			int num1;
			float dir;
			bool canSpell = true;
			private float delay = 360;
			public override void SendExtraAI(BinaryWriter writer) 
			{
				writer.Write(delay);
				writer.Write(dir);
				writer.Write(canSpell);
			}
			public override void ReceiveExtraAI(BinaryReader reader)
			{	
				delay = reader.ReadSingle();
				dir = reader.ReadSingle();
				canSpell = reader.ReadBoolean();
			}
			public override void SetStaticDefaults()
			{
				
				DisplayName.SetDefault("Nature Construct");
			}
			public override void SetDefaults()
			{
				//npc.CloneDefaults(NPCID.BlackSlime);
				npc.aiStyle = 0;
				npc.lifeMax = 175;  
				npc.damage = 30; 
				npc.defense = 7;  
				npc.knockBackResist = 0.1f;
				npc.width = 120;
				npc.height = 70;
				Main.npcFrameCount[npc.type] = 3;  
				npc.value = 3330;
				npc.npcSlots = 3f;
				npc.boss = false;
				npc.lavaImmune = false;
				npc.noGravity = false;
				npc.noTileCollide = false;
				npc.netAlways = true;
				npc.HitSound = SoundID.NPCHit4;
				npc.DeathSound = SoundID.NPCDeath14;
			}
			public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
			{
				Player player = Main.player[npc.target];
				Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Constructs/NatureConstructHead");
				Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
				Vector2 drawPos = npc.Center - Main.screenPosition + new Vector2(0f, npc.gfxOffY);
				if(npc.frame.Y == 70) //frame 2
				drawPos.Y -= 4;
					
				if(npc.frame.Y == 140) //frame 3
				drawPos.Y -= 2;

				spriteBatch.Draw(texture, drawPos, null, drawColor, dir, drawOrigin, npc.scale + 0.04f, SpriteEffects.None, 0f);
			}
			int spellAmt = 0;
			public void InitiateSpell()
			{
				canSpell = false;
			}
			public void SpellLaunch()
			{
				int damage = npc.damage / 2;
				if (Main.expertMode) 
				{
					damage = (int)(damage / Main.expertDamage);
				}
				if(spellAmt == 1)
				{
					if(Main.netMode != 1)
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -7,  mod.ProjectileType("NatureBolt"), damage, 0, Main.myPlayer, 40, npc.target);
				}
				if(spellAmt == 2)
				{
					if(Main.netMode != 1)
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 1, -6,  mod.ProjectileType("NatureBolt"), damage, 0, Main.myPlayer, 60, npc.target);
				}
				if(spellAmt == 3)
				{
					if(Main.netMode != 1)
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -1, -6,  mod.ProjectileType("NatureBolt"), damage, 0, Main.myPlayer, 80, npc.target);
				}
				if(spellAmt == 4)
				{
					if(Main.netMode != 1)
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -2, -5,  mod.ProjectileType("NatureBolt"), damage, 0, Main.myPlayer, 100, npc.target);
				}
				if(spellAmt == 5)
				{
					if(Main.netMode != 1)
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 2, -5,  mod.ProjectileType("NatureBolt"), damage, 0, Main.myPlayer, 120, npc.target);
				
					canSpell = true;
					npc.netUpdate = true;
				}
				Main.PlaySound(SoundID.Item92, (int)(npc.Center.X), (int)(npc.Center.Y));
			}
			public override void HitEffect(int hitDirection, double damage)
			{
				ai2 = 0;
				
				if (npc.life <= 0)
				{
					for (int k = 0; k < 20; k++)
					{
						Dust.NewDust(npc.position, npc.width, npc.height, 82, 2.5f * (float)hitDirection, -2.5f, 0, default(Color), 0.7f);
					}
					for(int i = 0; i < 30; i ++)
					{
					int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, mod.DustType("BigNatureDust"));
					Main.dust[dust].velocity *= 5f;
					}
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/NatureConstructGore1"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/NatureConstructGore2"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/NatureConstructGore3"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/NatureConstructGore4"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/NatureConstructGore5"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/NatureConstructGore6"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/NatureConstructGore7"), 1f);
					for(int i = 0; i < 9; i++)
					Gore.NewGore(npc.position, npc.velocity, Main.rand.Next(61,64), 1f);	
				}
			}
			public override void FindFrame(int frameHeight) 
			{
				if (ai1 > 10f) 
				{
					ai1 = 0;
					npc.frame.Y = (npc.frame.Y + frameHeight);
					if(npc.frame.Y >= 150)
					{
						npc.frame.Y = 0;
					}
				}
			}
			public override bool PreAI()
			{
				//npc.ai[2] = 1;
				Player player = Main.player[npc.target];
				if(canSpell)
				dir = (float)Math.Atan2(player.Center.Y - npc.Center.Y, player.Center.X - npc.Center.X);
				ai1++;
				return true;
			}
			public override void AI()
			{
				delay--;
				if(dir < 0)
				{
					dir = MathHelper.ToRadians(360) + dir;
				}
				while(dir > MathHelper.ToRadians(360))
				{
					dir -= MathHelper.ToRadians(360);
				}
				if(delay <= (Main.expertMode ? 1 : 0) && canSpell)
				{
					delay = 240 + Main.rand.Next(180);
					npc.netUpdate = true;
					InitiateSpell();
				}
				if(!canSpell)
				{
					if(dir != MathHelper.ToRadians(270))
					{
						if(dir > MathHelper.ToRadians(275) || dir < MathHelper.ToRadians(90))
						{
							dir -= MathHelper.ToRadians(3);
						}
						else if(dir < MathHelper.ToRadians(265))
						{
							dir += MathHelper.ToRadians(3);
						}
						else while(spellAmt < 5)
						{
							dir = MathHelper.ToRadians(270);
							spellAmt++;
							SpellLaunch();
						}
						spellAmt = 0;
					}
				}
				
				Player player = Main.player[npc.target];
				if(npc.velocity.X == 0 && npc.velocity.Y == 0)
				{
					npc.aiStyle = 3;
						aiType = 73;
					initiateSpeed = -1;
				}
				else if (npc.velocity.Y == 0 && ai2 == 0)
				{
					ai2 = 1;
				}
				if(ai2 >= 1)
				{
					ai2++;
				}
					if(initiateSpeed == -1 && ai2 >= 5)
					{
						if(ai2 >= 30)
						ai2 = 30;
					
						if(player.Center.X > npc.Center.X + 12)
						{
							npc.velocity.X = 1 * npc.scale * (float)(ai2/30f);
							npc.spriteDirection = 1;
						}
						if(player.Center.X < npc.Center.X - 12)
						{
							npc.velocity.X = -1 * npc.scale * (float)(ai2/30f);
							npc.spriteDirection = -1;
						}
					}
			}
			public override float SpawnChance(NPCSpawnInfo spawnInfo)
			{
				Player player = spawnInfo.player;
				bool ZoneForest = !player.GetModPlayer<SOTSPlayer>().PyramidBiome && !player.ZoneDesert && !player.ZoneCorrupt && !player.ZoneDungeon && !player.ZoneDungeon && !player.ZoneHoly && !player.ZoneMeteor && !player.ZoneJungle && !player.ZoneSnow && !player.ZoneCrimson && !player.ZoneGlowshroom && !player.ZoneUndergroundDesert && (player.ZoneDirtLayerHeight || player.ZoneOverworldHeight) && !player.ZoneBeach;
				if(ZoneForest)
				{
					return (SpawnCondition.Overworld.Chance * 0.0125f);
				}
				return (SpawnCondition.SurfaceJungle.Chance * 0.025f) + (SpawnCondition.UndergroundJungle.Chance * 0.0075f);
			}
			public override void NPCLoot()
			{
				int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("NatureSpirit"));	
				Main.npc[n].velocity.Y = -10f;
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("FragmentOfNature"), Main.rand.Next(4) + 4);	
			}	
		}
}