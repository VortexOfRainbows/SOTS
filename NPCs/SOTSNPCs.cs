using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.NPCs
{
    public class SOTSNPCs : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
			Player player = Main.player[Main.myPlayer];
			if(npc.target <= 255)
			{
			player = Main.player[npc.target];
			}
			else
			{
			player = Main.player[Main.myPlayer];
			}
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			
				
			if(npc.lifeMax > 5)
			{
				
				if(SOTSWorld.challengeIce)
				{
					if(Main.rand.Next(90) <= (int)(npc.lifeMax * 0.02) + 1)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("EternalFragment"), (int)(npc.lifeMax * 0.005) + 1 + Main.rand.Next(9));
					}
					if(Main.rand.Next(10) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("OverhealHeart"), 1);
					}
				}
			
				if(Main.rand.Next(750) == 0)
				{
						if(Main.rand.Next(2) == 0)
						{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SoulFragment"), 1);
						}
						else
						{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("OverhealHeart"), 1); 
						}
				}
				else if(npc.boss)
				{
					if(Main.rand.Next(250) == 0)
					{
						if(Main.rand.Next(2) == 0)
						{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SoulFragment"), 1);
						}
						else
						{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("OverhealHeart"), 1); 
						}
					}
				}
				if(modPlayer.ItemDivision && npc.lifeMax > 1 && npc.type != -2 && npc.type != -1 && npc.type != 81 && npc.type != 13 && npc.type != 14 && npc.type != 15)
				{
							int npcCheck = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, npc.type);	
							Main.npc[npcCheck].lifeMax = 1;
							Main.npc[npcCheck].life = 1;
							Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, mod.ProjectileType("Starsplosion"), (int)(npc.lifeMax * 0.15f) + 25, 0, Main.myPlayer, 0f, 0f);
				}
				if(npc.FindBuffIndex(mod.BuffType("OverhealHeart")) > -1)
				{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("OverhealHeart"), 1); 
					   
					
				}
				if(npc.FindBuffIndex(mod.BuffType("MargritToxin")) > -1)
				{
					   for(int i = 0; i < 9; i++)
						{
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.Next(-6,7), Main.rand.Next(-6,7), mod.ProjectileType("MargritToxin"), 33, 0, Main.myPlayer);
						}
							
				}
				if(npc.FindBuffIndex(mod.BuffType("DropAmmo")) > -1)
				{
					if(Main.rand.Next(5) == 0)
					{
					  Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.MusketBall, Main.rand.Next(9) + 1); 
					}
					if(Main.rand.Next(5) == 0)
					{
					  Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.WoodenArrow, Main.rand.Next(9) + 1); 
					}
							
				}
				 if (Main.rand.Next(100000) == 0) 
					{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ThundershockShortbow"), 1); 
					}
				 if (Main.rand.Next(100000) == 0) 
					{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PhantomicConductor"), 1); 
					}
				 if (Main.rand.Next(100000) == 0) 
					{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TheMelter"), 1); 
					}
				 if (Main.rand.Next(100000) == 0) 
					{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PrimeTalisman"), 1); 
					}
				 if (Main.rand.Next(100000) == 0) 
					{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MourningStar"), 1); 
					}
				 if (Main.rand.Next(100000) == 0) 
					{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DeoxysABall"), 1); 
					}
				 if (Main.rand.Next(100000) == 0) 
					{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Pulverizer"), 1); 
					}
				 if (Main.rand.Next(200000) == 0) 
					{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ZephyriousZepline"), 1); 
					}
				
				if (npc.type == NPCID.WallofFlesh)
					{
								Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.DemonHeart, 1); 
								Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HungryHunter"), 1); 
					}
					
			
			   
				if (npc.type == NPCID.KingSlime)  
				{
					if (Main.rand.Next(40) == 0) 
					{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("WaterMelon"), 1); 
					}
					if (Main.rand.Next(3) == 0) 
					{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SlimeStaff, 1); 
					}
				}
				if (npc.type == NPCID.WyvernHead)  
				{
					if (Main.rand.Next(20) == 0) 
					{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("WyvernSword"), 1); 
					}
					if (Main.rand.Next(4) == 0) 
					{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GiantHarpyFeather, 1); 
					}
					if (Main.rand.Next(36) == 0) 
					{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StormSerpantStaff"), 1); 
					}
				}
				if (npc.type == NPCID.EyeofCthulhu)  
				{
					if (Main.rand.Next(50) == 0) 
					{
						
						   Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("EoCPendant"), 1); 
						
					}
					if (Main.rand.Next(30) == 0)	
					{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DetachedEye"), 1);
					}
				}
				
				if (npc.type == NPCID.DemonEye)  
				{
					 if (Main.rand.Next(220) == 0)
					 {
						 Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DetachedEye"), 1);
					 }
				}
				if (npc.type == NPCID.EaterofWorldsHead)  
				{
					if (Main.rand.Next(500) == 0) 
					{
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PrimordialMismatch"), 1); 
						}
					}
				}
				if (npc.type == NPCID.BrainofCthulhu)  
				{
					if (Main.rand.Next(40) == 0) 
					{
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PrimordialMismatch"), 1); 
						}
					}
				}
				
				if (npc.type == NPCID.QueenBee)  
				{
					if (Main.rand.Next(40) == 0) 
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BeenadeLauncher"), 1);
					}
				}
				if (npc.type == NPCID.SkeletronHead)  
				{
					if (Main.rand.Next(20) == 0) 
					 Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ShadowSpitter"), 1); 
						
					if (Main.rand.Next(20) == 0) 
					 Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BulletShark"), 1); 
						
					
					   
						   
				}
				if (npc.type == NPCID.GoblinPeon || npc.type == NPCID.GoblinArcher || npc.type == NPCID.GoblinWarrior || npc.type == NPCID.GoblinSorcerer)  
				{
					if (Main.rand.Next(3) == 0) 
					{
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GoblinRockBar"), 1); 
						}
					}
					
					
					if (Main.rand.Next(100) == 0) 
					{
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TinyPlanet"), 1); 
						}
					}
					if (Main.rand.Next(100) == 0) 
					{
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BoosterCore"), 1); 
						}
					}
					if (Main.rand.Next(100) == 0) 
					{
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("RedRibbon"), 1); 
						}
					}
					if (Main.rand.Next(200) == 0) 
					{
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ShadowPistol"), 1); 
						}
					}
				}
				if (npc.type == NPCID.Spazmatism)  
				{
					if (Main.rand.Next(24) == 0) 
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("LusterBeam"), 1);
					}
				}if (npc.type == NPCID.Retinazer)  
				{
					if (Main.rand.Next(24) == 0) 
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MistBow"), 1);
					}
				}
				if (npc.type == NPCID.MartianSaucer)  
				{
					if (Main.rand.Next(49) == 0) 
					{
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MartianCore"), 1); 
						}
					}
				}
				if (npc.type == NPCID.TheDestroyer)  
				{
					if (Main.rand.Next(24) == 0) 
					{
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PurgeBringer"), 1); 
						}
					}
				}
				if (npc.type == NPCID.Plantera)  
				{
					if (Main.rand.Next(24) == 0) 
					{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ChlorophyteCrossFive"), 1); 
					}
					
				}
				if (npc.type == NPCID.Golem)  
				{
					if (Main.rand.Next(11) == 0) 
					{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("LihzahrdWrench"), 1); 
					}
					
				}
				if (npc.type == NPCID.UndeadMiner)  
				{
					if (Main.rand.Next(7) == 0) 
					{          
						 Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ManicMiner"), 1); 
					}
				}
				if (npc.type == NPCID.UndeadViking)  
				{
					if (Main.rand.Next(45) == 0) 
					{
					   
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BarbaricEssence"), 1); 
						
					}
					if (Main.rand.Next(35) == 0) 
					{
					   
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FrostburnBow"), 1); 
						
					}
				}
				if (npc.type == NPCID.BlueSlime || npc.type == NPCID.Crab)  
				{
					if (Main.rand.Next(150) == 0) 
					{
					   
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Waterweed"), 1); 
						
					}
					if (Main.rand.Next(180) == 0) 
					{
					   
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FireSpitter"), 1); 
						
					}
				}
				
				if (npc.type == NPCID.Snatcher || npc.type == NPCID.ManEater)  
				{
					if (Main.rand.Next(40) == 0) 
					{
					   
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Healherb"), 1); 
						
					}
				}
				
				if (npc.type == NPCID.Bunny || npc.type == NPCID.Squirrel)  
				{
					if (Main.rand.Next(40) == 0) 
					{
					   
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Bunnyflower"), 1); 
						
					}
				}
				if (npc.type == NPCID.AngryBones)  
				{
					if (Main.rand.Next(100) == 0) 
					{
					   
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Boneleaf"), 1); 
						
					}
				}
				if (npc.type == NPCID.Antlion || npc.type == NPCID.Vulture)  
				{
					if (Main.rand.Next(30) == 0) 
					{
					   
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Sandshrub"), 1); 
						
					}
				}
				if (npc.type == NPCID.Harpy)  
				{
					if (Main.rand.Next(30) == 0) 
					{
					   
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Skyroot"), 1); 
						
					}
				}
				
			if (npc.type == NPCID.Pixie)
				{
				   
					if (Main.rand.Next(120) == 0) 
					{
					   
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CrystalFin"), 1); 
						
					}
				}
			if (npc.type == NPCID.SandElemental)
				{
					if (Main.rand.Next(4) == 0) 
					{
					   
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ForbiddenForger"), 1); 
						
					}
				}
			if (npc.type == 243) //ice golem
				{
					if (Main.rand.Next(4) == 0) 
					{
					   
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FrostForger"), 1); 
						
					}
				}
			if (npc.type == mod.NPCType("PutridPinkyPhase2"))
				{
					if (Main.rand.Next(25) == 0) 
					{
					   
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MusketeerHat"), 1); 
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MusketeerShirt"), 1); 
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MusketeerLeggings"), 1); 
					}
				}
			if (npc.type == mod.NPCType("CrypticCarver2"))
				{
					if (Main.rand.Next(25) == 0) 
					{
					   
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DevilHelmet"), 1); 
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DevilLeggings"), 1); 
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DevilChestplate"), 1); 
					}
				}
			if (npc.type == mod.NPCType("EtherealEntity3"))
				{
					if (Main.rand.Next(25) == 0) 
					{
					   
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MeguminHat"), 1); 
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MeguminShirt"), 1); 
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MeguminLeggings"), 1); 
					}
				}
			if (npc.type == 64 && Main.rand.Next(120) == 0)
			{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PinkJellyfishStaff"), 1); 
			}
			if (npc.type == 140 && Main.rand.Next(150) == 0) //Possessed armor
			{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PossessedHelmet"), 1); 
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PossessedChainmail"), 1); 
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PossessedGreaves"), 1); 
			}
			if ((npc.type == 63 || npc.type == 103) && Main.rand.Next(120) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BlueJellyfishStaff"), 1); 
			}
			if (npc.type == NPCID.Demon || npc.type == NPCID.RedDevil || npc.type == NPCID.VoodooDemon || npc.type == mod.NPCType("ObsidianWormHead"))
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DemonBlood"), Main.rand.Next(19) + 1); 
				}
			}
		}
	}
}

 
    


