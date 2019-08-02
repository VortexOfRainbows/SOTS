using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss
{[AutoloadBossHead]
	public class Antilion : ModNPC
	{	
int angle = 0;
int npcai2 = 0;
int firedelay = 0;
int despawn = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Antimaterial Antlion");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 6;  //6 is the worm AI
            npc.lifeMax = 17500;   //boss life
            npc.damage = 100;  //boss damage
            npc.defense = 17;    //boss defense
            npc.knockBackResist = 0f;
            npc.width = 108;
            npc.height = 186;
            Main.npcFrameCount[npc.type] = 4;    //boss frame/animation
            animationType = NPCID.SkeletronHead;
            npc.behindTiles = true;          
            npc.value = 40000;
            npc.npcSlots = 1f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath5;
            music = MusicID.Boss2;
            npc.netAlways = true;
			
			bossBag = mod.ItemType("AntilionLootBag");
		}
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.75f);  //boss life scale in expertmode
            npc.damage = (int)(npc.damage * 1.25f);  //boss damage increase in expermode
        }
		
		public override void AI()
		{
			
			if(!Main.player[npc.target].ZoneDesert)
			{
				
					npc.dontTakeDamage = true;
			}
			else
			{
				
					npc.dontTakeDamage = false;
			}
			
					if(Main.rand.Next(2) == 0)
					{
						angle = Main.rand.Next(-8, -1);
					}
					else{
							
						angle = Main.rand.Next(2, 9);
					}
			firedelay++;
			npc.ai[0]++;
			if(npc.ai[0] <= 1300 && firedelay >= 180)
			{
				if(Main.rand.Next(2) == 0)
				{
			Projectile.NewProjectile(npc.Center.X, (npc.Center.Y), 10, 10, 258, (int)(npc.damage * .3f), 1, 0);
			Projectile.NewProjectile(npc.Center.X, (npc.Center.Y), 10, -10, 258, (int)(npc.damage * .3f), 1, 0);
			Projectile.NewProjectile(npc.Center.X, (npc.Center.Y), -10, -10, 258, (int)(npc.damage * .3f), 1, 0);
			Projectile.NewProjectile(npc.Center.X, (npc.Center.Y), -10, 10, 258, (int)(npc.damage * .3f), 1, 0);
				}else
				{
			Projectile.NewProjectile(npc.Center.X, (npc.Center.Y), 10, 0, 358, (int)(npc.damage * .3f), 1, 0);
			Projectile.NewProjectile(npc.Center.X, (npc.Center.Y), 0, -10, 258, (int)(npc.damage * .3f), 1, 0);
			Projectile.NewProjectile(npc.Center.X, (npc.Center.Y), -10, 0, 258, (int)(npc.damage * .3f), 1, 0);
			Projectile.NewProjectile(npc.Center.X, (npc.Center.Y), -0, 10, 258, (int)(npc.damage * .3f), 1, 0);
				}
				
					firedelay = 0;
				
			}
			
			if(npc.ai[0] == 650 || npc.ai[0] == 700 || npc.ai[0] == 800 ||  npc.ai[0] == 900 ||  npc.ai[0] == 1000)
			{
				
		NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.Antlion);
				
			}
			if(npc.ai[0] == 900)
			{
				if(Main.rand.Next(2) == 0)
				{
		NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y,  509);
		NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y,  509);
		NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y,  509);
		NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y,  509);
				}
				else{
					
		NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y,  508);
		NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y,  508);
		NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y,  530);
		NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y,  530);
				}
				
			}
			if(npc.ai[0] >= 1300)
			{
				npc.aiStyle = 57;
				npc.rotation = 0;
				npc.velocity.X = 0;
			   npc.timeLeft = 10000;
				if(npc.ai[0] >= 1500)
				{
					npc.dontTakeDamage = true;
					npc.aiStyle = 0;
			   npc.timeLeft = 10000;
					
					
			if(Main.expertMode)
			{
				int CragSelection = Main.rand.Next(10);
				
				if(CragSelection == 0 || CragSelection == 3 || CragSelection == 4|| CragSelection == 5 || CragSelection == 6 || CragSelection == 7)
				Projectile.NewProjectile(npc.Center.X, (npc.Center.Y - 38), angle, Main.rand.Next(-17, -4), mod.ProjectileType("SandCrag"), (int)(npc.damage * .2f), 1, 0);
				
				if(CragSelection == 1 || CragSelection == 8 || CragSelection == 9)
					Projectile.NewProjectile(npc.Center.X, (npc.Center.Y - 38), angle, Main.rand.Next(-17, -4), mod.ProjectileType("ObsidianCrag"), (int)(npc.damage * .2f), 1, 0);
					
				if(CragSelection == 2)
					Projectile.NewProjectile(npc.Center.X, (npc.Center.Y - 38), angle, Main.rand.Next(-17, -4), mod.ProjectileType("HellCrag"), (int)(npc.damage * .2f), 1, 0);
			}
			else
			{
				Projectile.NewProjectile(npc.Center.X, (npc.Center.Y - 38), angle, Main.rand.Next(-17, -4), mod.ProjectileType("SandCrag"), (int)(npc.damage * .2f), 1, 0);
			}
			
			
				}
				
			}
			if(Main.expertMode)
			{
				
			if(npc.ai[0] >= 2000 )
			{
					npc.dontTakeDamage = false;
				 npc.aiStyle = 6;
				npc.ai[0] = 0;
				npcai2++;
			}
			}
			else
				{
				if(npc.ai[0] >= 1650 )
			{
					npc.dontTakeDamage = false;
				 npc.aiStyle = 6;
				npc.ai[0] = 0;
				npcai2++;
			}
			
			}
				
						

			if(npcai2 == 2)
			{
				npcai2 = 0;
		NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y,  509);
		NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y,  509);
		NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y,  509);
		NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y,  509);
		NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y,  509);
		NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y,  509);
				
			}
			
		   
			if(Main.player[npc.target].dead)
			{
			 despawn++;
			}
			if(despawn >= 720)
			{
			npc.active = false;
			}
		   




		}
		public override void BossLoot(ref string name, ref int potionType)
		{ 
		SOTSWorld.downedAntilion = true;
		potionType = ItemID.GreaterHealingPotion;
	
		if(Main.expertMode)
		
		{ 
npc.DropBossBags();
		} 
		else 
			{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AntimaterialMandible"),Main.rand.Next(2, 4));
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BrassBar"),Main.rand.Next(1, 8));
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SteelBar"),Main.rand.Next(1, 8));
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SoulofNight,Main.rand.Next(1, 5));
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SoulofLight,Main.rand.Next(1, 5));
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.CopperBar,Main.rand.Next(0, 3));
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.TinBar,Main.rand.Next(0, 3));
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.LeadBar,Main.rand.Next(0, 3));
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.IronBar,Main.rand.Next(0, 3));
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SilverBar,Main.rand.Next(0, 3));
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GoldBar,Main.rand.Next(0, 3));
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.TungstenBar,Main.rand.Next(0, 3));
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.PlatinumBar,Main.rand.Next(0, 3));
		
		
			}
	}
	}

	
}