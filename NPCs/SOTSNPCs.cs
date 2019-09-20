using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

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
				}
				
				if (npc.type == NPCID.SkeletronHead)  
				{
					if (Main.rand.Next(20) == 0) 
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BulletShark"), 1); 
				}
				if (npc.type == NPCID.GoblinPeon || npc.type == NPCID.GoblinArcher || npc.type == NPCID.GoblinWarrior || npc.type == NPCID.GoblinSorcerer)  
				{
					if (Main.rand.Next(2) == 0) 
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GoblinRockBar"), Main.rand.Next(2) + 1); 
					}
				}
				if (npc.type == NPCID.Plantera)  
				{
					if (Main.rand.Next(24) == 0) 
					{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ChlorophyteCrossFive"), 1); 
					}
					
				}
				if (npc.type == NPCID.UndeadMiner)  
				{
					if (Main.rand.Next(7) == 0) 
					{          
						 Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ManicMiner"), 1); 
					}
				}
				if (npc.type == NPCID.BlueSlime || npc.type == NPCID.Crab)  
				{
					if (Main.rand.Next(180) == 0) 
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FireSpitter"), 1); 
					}
				}
			if (npc.type == mod.NPCType("PutridPinkyPhase2"))
				{
					if (Main.rand.Next(25) == 0) 
					{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MeguminHat"), 1); 
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MeguminShirt"), 1); 
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MeguminLeggings"), 1); 
					}
				}
			if (npc.type == 64 && Main.rand.Next(70) == 0)
				{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PinkJellyfishStaff"), 1); 
				}
			if (npc.type == 140 && Main.rand.Next(150) == 0) //Possessed armor
				{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PossessedHelmet"), 1); 
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PossessedChainmail"), 1); 
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PossessedGreaves"), 1); 
				}
			if ((npc.type == 63 || npc.type == 103) && Main.rand.Next(70) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BlueJellyfishStaff"), 1); 
				}
			}
		}
		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns) 
		{
			if(player.GetModPlayer<SOTSPlayer>().PyramidBiome)
			{
				//if(npc.type == mod.NPCType("Snake") || npc.type == mod.NPCType("SnakePot") || npc.type == mod.NPCType("LostSoul"))
				
					maxSpawns = (int)(maxSpawns * 2.5f);
					spawnRate = 50;
			}
		}
		public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo) 
		{
			if(spawnInfo.player.GetModPlayer<SOTSPlayer>().PyramidBiome && spawnInfo.spawnTileType == (ushort)mod.TileType("PyramidSlabTile"))
			{
				/*
				pool.Add(mod.NPCType("SnakePot"),0.5f);
				
				pool.Add(mod.NPCType("Snake"),1f);
				
				pool.Add(mod.NPCType("LostSoul"),0.7f);
				
				pool.Add(mod.NPCType("PyramidTreasureSlime"),0.4f);
				*/
				if(Main.hardMode)
				{
					pool.Add(NPCID.Mummy,0.5f);
				
				}
			}
			
		}
	}
}

 
    


