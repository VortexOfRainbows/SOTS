using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs
{
	public class Libra : ModNPC
	{	int restrictor = 0;
		int readyForSac = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Libra");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 0;  //5 is the flying AI
            npc.lifeMax = 1000;   //boss life
            npc.damage = 0;  //boss damage
            npc.defense = 99999;    //boss defense
            npc.knockBackResist = 0f;
            npc.width = 38;
            npc.height = 34;
            animationType = NPCID.SkeletronHead;   //this boss will behavior like the DemonEye
            Main.npcFrameCount[npc.type] = 1;    //boss frame/animation/
            npc.npcSlots = 1f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath7;
            npc.netAlways = true;
		}
		
		
		public override void AI()
		{	
		Player player = Main.player[npc.target];
		for (int k = 0; k < 400; k++)
				{
				
						if (Main.item[k].active)
						{
								if(npc.Center.X + 16 > Main.item[k].Center.X && npc.Center.X - 16 < Main.item[k].Center.X && npc.Center.Y + 16 > Main.item[k].Center.Y && npc.Center.Y - 16 < Main.item[k].Center.Y)
								{
									if(Main.item[k].type == mod.ItemType("FrightBlood"))
									{
										Main.NewText("This one was a bit harder!", 255, 255, 255);
					
									
										Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (mod.ItemType("EMaterial")), 1);
										npc.timeLeft = 1;
										npc.timeLeft--;
										npc.life = 0;
										npc.active = false;
										
									}
									if(Main.item[k].type == mod.ItemType("Enigma"))
									{
										if(player.name == "E" || player.name == "Ellith")
										{
										//Main.NewText("Trying out Endre this time huh?", 255, 255, 255);
										}
										else
										{
										Main.NewText("There is no way in hell that you found this out legit...", 255, 255, 255);
										}
									
										Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (mod.ItemType("SummonMaterial")), 1);
										npc.timeLeft = 1;
										npc.timeLeft--;
										npc.life = 0;
										npc.active = false;
										
									}
								}
						
						
					}
				}
		
		
		
		
		Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 38, 34, 160);
		Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 38, 34, 160);
		Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 38, 34, 160);
		npc.lifeMax = 1000; 
		npc.life = 1000;
		npc.position.Y += Main.rand.Next(-1, 2);
		npc.position.X += Main.rand.Next(-1, 2);
		npc.ai[0]++;
		if(npc.ai[0] == 25)
		{
					Main.NewText("What is it that you require?", 255, 255, 255);
		}
		if(npc.ai[0] == 380)
		{
					Main.NewText("Special items? Secrets? Well, you made it this far...", 255, 255, 255);
		}
		if(npc.ai[0] == 990)
		{
					Main.NewText("How did you discover me? Pure chance? Luck?", 255, 255, 255);
		}
		if(npc.ai[0] == 1260)
		{
					Main.NewText("Whatever the reason...", 255, 255, 255);
		}
		if(npc.ai[0] == 1520)
		{
					Main.NewText("I hope you know that I'm not giving you any hints...", 255, 255, 255);
					readyForSac = 1;
		}
		if(npc.ai[0] == 1920)
		{
					Main.NewText("...", 255, 255, 255);
					readyForSac = 1;
		}
		if(npc.ai[0] == 3600)
		{
					Main.NewText("Well... actually...", 255, 255, 255);
					
		}
		if(npc.ai[0] == 3900)
		{
					Main.NewText("I can tell you one thing...", 255, 255, 255);
					
		}
		if(npc.ai[0] == 4200)
		{
					Main.NewText("You should try naming your characters different names...", 255, 255, 255);
					
		}
		if(npc.ai[0] == 5320)
		{
					Main.NewText("Some names start you off with special items...", 255, 255, 255);
					
		}
		if(npc.ai[0] == 6000)
		{
					Main.NewText("...", 255, 255, 255);
					
		}
		if(npc.ai[0] == 9600)
		{
					Main.NewText("...", 255, 255, 255);
					
		}
		if(npc.ai[0] == 12400)
		{
					Main.NewText("...", 255, 255, 255);
					
		}
		if(npc.ai[0] == 13000)
		{
					Main.NewText("Fine... I'll tell you another thing...", 255, 255, 255);
					
		}
		if(npc.ai[0] == 13720)
		{
					Main.NewText("The letters at the end of some items...", 255, 255, 255);
					
		}
		if(npc.ai[0] == 15900)
		{
			npc.ai[0] = 19900;
					//Main.NewText("They spell something...", 255, 255, 255);
					
		}
		if(npc.ai[0] == 17800)
		{
				//	Main.NewText("Voxels...", 255, 255, 255);
					
		}
		if(npc.ai[0] == 19000)
		{
					//Main.NewText("...", 255, 255, 255);
					
		}
		if(npc.ai[0] == 19600)
		{
					//Main.NewText("... well...", 255, 255, 255);
					
		}
		if(npc.ai[0] == 19960)
		{
					Main.NewText("... inputing those letters will start you with special items...", 255, 255, 255);
					
		}
		if(npc.ai[0] == 21000)
		{
					Main.NewText("Some other names do that too", 255, 255, 255);
					
		}
		if(npc.ai[0] == 21320)
		{
					Main.NewText("...", 255, 255, 255);
					
		}
		if(npc.ai[0] == 21920)
		{
					Main.NewText("...", 255, 255, 255);
					
		}
		if(npc.ai[0] == 23000)
		{
					Main.NewText("Not all letters starting items...", 255, 255, 255);
					
		}
		if(npc.ai[0] == 24500)
		{
					//			Main.NewText("I'm not gonna tell you them you know. There is only 5 letters after all", 255, 255, 255);

		}
		if(npc.ai[0] == 26000)
		{
					Main.NewText("I really don't get why you've been standing here for this long...", 255, 255, 255);
					
		}
		if(npc.ai[0] == 26900)
		{
					Main.NewText("If you want to craft one of the Holy Relics...", 255, 255, 255);
					
		}
		if(npc.ai[0] == 29000)
		{
					Main.NewText("You should've attacked me with the required weapons by now.", 255, 255, 255);
					
		}
		if(npc.ai[0] == 29600)
		{
					Main.NewText("Really, if there is a random letter at the end of a tooltip, you can assume that it relates to me.", 255, 255, 255);
					
		}
		if(npc.ai[0] == 31800)
		{
					Main.NewText("If you want to find out how to get the other materials...", 255, 255, 255);
					
		}
		if(npc.ai[0] == 32300)
		{
					Main.NewText("The materials that aren't named after the letters.", 255, 255, 255);
					
		}
		if(npc.ai[0] == 33000)
		{
					Main.NewText("You should input the names of who's Holy Relic you want... The Holy relics do have letters at the end of them too... right?", 255, 255, 255);
					
		}
		if(npc.ai[0] == 34000)
		{
					Main.NewText("Thanks for taking your time to look at my chat text though!", 255, 255, 255);
		}
		if(npc.ai[0] == 34300)
		{
					Main.NewText("Most people don't have patience for that.", 255, 255, 255);
		}
		if(npc.ai[0] == 34600)
		{
					Main.NewText(":)", 255, 255, 255);
					npc.timeLeft = 1;
					npc.timeLeft--;
					npc.life = 0;
					Main.NewText("Thank you!", 255, 255, 255);
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (mod.ItemType("TimeMaterial")), 1);
					
		}
		}
	
	}
}





















