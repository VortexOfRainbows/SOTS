using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using SOTS.Items;
using SOTS.Items.Otherworld;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Items.Potions;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;

namespace SOTS
{
    public class SOTSWorld : ModWorld
	{	
		public static int planetarium = 0;
		public static int pyramidBiome = 0;
		public static int geodeBiome = 0;
		public static bool downedBoss2 = false;

		public static bool downedPinky = false;
		public static bool downedCurse = false;
		
		public static bool downedAmalgamation = false;
		public static bool downedCelestial = false;
		public static bool downedSubspace = false;
		public static bool downedAdvisor = false;

		#region unused
		public static bool downedEntity = false;
		public static bool downedAntilion = false;
		public static bool downedChess = false;
		
		public static bool challengeDecay = false;
		public static bool challengeLock = false;
		public static bool challengePermanence = false;
		
		public static bool challengeIce = false;
		public static bool challengeGlass = false;
		public static bool challengeIcarus = false;
        #endregion

        public override void Initialize()
		{
			downedPinky = false;
			downedAdvisor = false;
			downedCurse = false;
			downedEntity = false;
			downedBoss2 = false;
			downedAntilion = false;
			downedAmalgamation = false;
			downedChess = false;
			
			downedCelestial = false;
			downedSubspace = false;
			
			challengeDecay = false;
			challengeLock = false;
			challengePermanence = false;
			
			challengeIce = false;
			challengeGlass = false;
			challengeIcarus = false;
		}
		public override TagCompound Save() {
			var downed = new List<string>();
			if (downedPinky) {
				downed.Add("pinky");
			}
			if (downedAdvisor) {
				downed.Add("advisor");
			}
			if (downedCurse) {
				downed.Add("curse");
			}
			if (downedEntity) {
				downed.Add("entity");
			}
			if (downedAntilion) {
				downed.Add("antilion");
			}
			if (downedAmalgamation) {
				downed.Add("amalgamation");
			}
			if (downedChess) {
				downed.Add("chess");
			}
			if (downedCelestial) {
				downed.Add("celestial");
			}
			if (downedSubspace) {
				downed.Add("subspace");
			}
			if (downedBoss2)
			{
				downed.Add("boss2");
			}

			var challenge = new List<string>();
			if (challengeDecay) {
				challenge.Add("decay");
			}
			if (challengeLock) {
				challenge.Add("lock");
			}
			if (challengePermanence) {
				challenge.Add("permanence");
			}
			if (challengeIce) {
				challenge.Add("ice");
			}
			if (challengeGlass) {
				challenge.Add("glass");
			}
			if (challengeIcarus) {
				challenge.Add("icarus");
			}
			return new TagCompound {
				{"downed", downed},
				{"challenge", challenge},
			};
		}

		public override void Load(TagCompound tag) {
			var downed = tag.GetList<string>("downed");
			downedPinky = downed.Contains("pinky");
			downedAdvisor = downed.Contains("advisor");
			downedCurse = downed.Contains("curse");
			downedEntity = downed.Contains("entity");
			downedAntilion = downed.Contains("antilion");
			downedAmalgamation = downed.Contains("amalgamation");
			downedChess = downed.Contains("chess");
			downedCelestial = downed.Contains("celestial");
			downedSubspace = downed.Contains("subspace");
			downedBoss2 = downed.Contains("boss2");


			var challenge = tag.GetList<string>("challenge");
			challengeDecay = challenge.Contains("decay");
			challengeLock = challenge.Contains("lock");
			challengePermanence = challenge.Contains("permanence");
			challengeIce = challenge.Contains("ice");
			challengeGlass = challenge.Contains("glass");
			challengeIcarus = challenge.Contains("icarus");
		}

		public override void LoadLegacy(BinaryReader reader) {
			int loadVersion = reader.ReadInt32();
			if (loadVersion == 0) {
				BitsByte flags = reader.ReadByte();
				downedPinky = flags[0];
				downedAdvisor = flags[1];
				downedEntity = flags[2];
				downedAntilion = flags[3];
				downedAmalgamation = flags[4];
				downedChess = flags[5];
				downedCurse = flags[6];
				downedCelestial = flags[7];
				
				BitsByte flags3 = reader.ReadByte();
				downedSubspace = flags3[0];
				downedBoss2 = flags3[1];

				BitsByte flags2 = reader.ReadByte();
				challengeDecay = flags2[0];
				challengeLock = flags2[1];
				challengePermanence = flags2[2];
				challengeIce = flags2[3];
				challengeGlass = flags2[4];
				challengeIcarus = flags2[5];
			}
		}

		public override void NetSend(BinaryWriter writer) {
			BitsByte flags = new BitsByte();
			flags[0] = downedPinky;
			flags[1] = downedAdvisor;
			flags[2] = downedEntity;
			flags[3] = downedAntilion;
			flags[4] = downedAmalgamation;
			flags[5] = downedChess;
			flags[6] = downedCurse;
			flags[7] = downedCelestial;
			writer.Write(flags);
			
			BitsByte flags3 = new BitsByte();
			flags3[0] = downedSubspace;
			flags3[1] = downedBoss2;
			writer.Write(flags3);
			
			BitsByte flags2 = new BitsByte();
			flags2[0] = challengeDecay;
			flags2[1] = challengeLock;
			flags2[2] = challengePermanence;
			flags2[3] = challengeIce;
			flags2[4] = challengeGlass;
			flags2[5] = challengeIcarus;
			writer.Write(flags2);
		}
		public override void NetReceive(BinaryReader reader) {
			BitsByte flags = reader.ReadByte();
			downedPinky = flags[0];
			downedAdvisor = flags[1];
			downedEntity = flags[2];
			downedAntilion = flags[3];
			downedAmalgamation = flags[4];
			downedChess = flags[5];
			downedCurse = flags[6];
			downedCelestial = flags[7];
			
			BitsByte flags3 = reader.ReadByte();
			downedSubspace = flags3[0];
			downedBoss2 = flags3[1];

			BitsByte flags2 = reader.ReadByte();
			challengeDecay = flags2[0];
			challengeLock = flags2[1];
			challengePermanence = flags2[2];
			challengeIce = flags2[3];
			challengeGlass = flags2[4];
			challengeIcarus = flags2[5];
		}
		public override void PostUpdate()
		{
			if(NPC.downedBoss2 && !downedBoss2)
			{
				downedBoss2 = true;
				Main.NewText("The pyramid's curse weakens", 155, 115, 0);
			}
		}
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            int genIndexGems = tasks.FindIndex(genpass => genpass.Name.Equals("Random Gems"));
            int genIndexEnd = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));
            if (genIndex == -1)
            {
                return;
            }
			/*
            tasks.Insert(genIndex + 2, new PassLegacy("Geode Cavern", delegate (GenerationProgress progress)
            {
                progress.Message = "Molding Crystals";
                for (int i = 0; i < 3; i++)       //900 is how many biomes. the bigger is the number = less biomes
                {
                    int X = WorldGen.genRand.Next(300, Main.maxTilesX - 300);
                    int Y = WorldGen.genRand.Next((int)(Main.maxTilesY * 0.75f), (int)(Main.maxTilesY * 0.8f));
                    int TileType = mod.TileType("GeodeBlock");     //this is the tile u want to use for the biome , if u want to use a vanilla tile then its int TileType = 56; 56 is obsidian block

					
                    WorldGen.TileRunner(X, Y, (int)(Main.maxTilesY * 0.25f), 20, TileType, false, 0f, 0f, true, true);  
					
					for (int k = 0; k < (int)(Main.maxTilesY * 0.15f); k++)                    
					{
                        int Xo = X + Main.rand.Next(-(int)(Main.maxTilesY * 0.1f), (int)(Main.maxTilesY * 0.1f));
                        int Yo = Y + Main.rand.Next(-(int)(Main.maxTilesY * 0.1f), (int)(Main.maxTilesY * 0.1f));
                        if (Main.tile[Xo, Yo].type == mod.TileType("GeodeBlock"))   //this is the tile where the ore will spawn
                        {
 
                            {
                                WorldGen.TileRunner(Xo, Yo, (double)WorldGen.genRand.Next(17, 25), WorldGen.genRand.Next(5, 10), 56, false, 0f, 0f, false, true);
                            }
                        }
                    }
					for (int k = 0; k < (int)(Main.maxTilesY * 0.35f); k++)                     //750 is the ore spawn rate. the bigger is the number = more ore spawns
                    {
                        int Xo = X + Main.rand.Next(-(int)(Main.maxTilesY * 0.1f), (int)(Main.maxTilesY * 0.1f));
                        int Yo = Y + Main.rand.Next(-(int)(Main.maxTilesY * 0.1f), (int)(Main.maxTilesY * 0.1f));
                        if (Main.tile[Xo, Yo].type == 56)   //this is the tile where the ore will spawn
                        {
 
                            {
                                WorldGen.TileRunner(Xo, Yo, (double)WorldGen.genRand.Next(5, 7), WorldGen.genRand.Next(2, 7), 58, false, 0f, 0f, false, true);  
                            }
                        }
                    }
					for (int k = 0; k < (int)(Main.maxTilesY * 0.45f); k++)                     //750 is the ore spawn rate. the bigger is the number = more ore spawns
                    {
                        int Xo = X + Main.rand.Next(-(int)(Main.maxTilesY * 0.1f), (int)(Main.maxTilesY * 0.1f));
                        int Yo = Y + Main.rand.Next(-(int)(Main.maxTilesY * 0.1f), (int)(Main.maxTilesY * 0.1f));
					
                        if (Main.tile[Xo, Yo].type == 56)   //this is the tile where the ore will spawn
                        {
 
                            {
                                WorldGen.TileRunner(Xo, Yo, (double)WorldGen.genRand.Next(4, 6), WorldGen.genRand.Next(1, 7), 37, false, 0f, 0f, false, true);  
                            }
                        }
                    }
					
                }
			
            }));
			
			tasks.Insert(genIndexGems, new PassLegacy("Planetarium", delegate (GenerationProgress progress)
            {
				
                progress.Message = "Generating An Ancient Sky Artifact";
				
					int Xd = Main.maxTilesX - (Main.maxTilesX - 550);
					int Xp = Main.maxTilesX - 550;
					if(Main.rand.Next(2) == 0)
					{
					Xp = Main.maxTilesX - (Main.maxTilesX - 550);
					Xd = Main.maxTilesX - 550;
					}
					int Yp = 100;

					
					int xPositionLibra = (int)(Xd);
					int xPosition = (int)(Xp);
                    int yPosition = (int)(Yp);
					
			int bottomYPosition = yPosition;
			for(int ydown = bottomYPosition; ydown < Main.maxTilesY - 100; ydown++)
			{
				Tile tile = Framing.GetTileSafely(xPosition, ydown);
				if(tile.active() && ydown > Main.worldSurface * 0.4f)
				{
					bottomYPosition = ydown;
					break;
				}
			}
			int[,] _planetaryBaseStructure = {
				{0,0,0,0,0},
				{0,2,2,2,0},
				{0,2,5,2,0},
				{1,2,2,2,1},
				{1,1,1,1,1},
				{0,1,1,1,0},
				{0,0,1,0,0},
			};
					int Xq = xPosition;
					int Yq = bottomYPosition - 30;
					
					
				Yq -= (int)(.5f * _planetaryBaseStructure.GetLength(0));
				Xq -= (int)(.5f * _planetaryBaseStructure.GetLength(1));
					
				for (int y = 0; y < _planetaryBaseStructure.GetLength(0); y++) {
					for (int x = 0; x < _planetaryBaseStructure.GetLength(1); x++) {
						int k = Xq + x;
						int l = Yq + y;
						if (WorldGen.InWorld(k, l, 30)) {
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_planetaryBaseStructure[y, x]) {
								case 0:
									//tile.active(false);
									//tile2.active(false);
									break;
								case 1:
									tile.type = 189; //cloud
									tile.active(true);
									break;
								case 2:
									tile.type = 223; //titanium
									tile.active(true);
									break;
								case 3:
									tile.type = TileID.Cobweb; //grass
									tile.active(true);
									break;
								case 4:
									tile.type = 20; //acorn
									tile.active(true);
									break;
								case 5:
									tile.type = (ushort)mod.TileType("EmptyPlanetariumBlock");
									tile.active(true);
									break;
							}
						}
					}
				}
				for(int i = 0; i < 8; i++) //Cloud Spheres
				{
					float scaleDistance = i * 0.3f + 1;
					int randomX = Main.rand.Next((int)(-45 * i), (int)(45 * i + 1)) + xPosition;
					int randomY = yPosition + Main.rand.Next(-80, 51);
			
					int c1radius = (int)(i + 1) * 2;  
					
					for (int x = -c1radius; x <= c1radius; x++)
					{
						for (int y = -c1radius; y <= c1radius; y++)
						{
							int xPositionc1 = (int)(x + randomX);
							int yPositionc1 = (int)(y + randomY);
		 
							if (Math.Sqrt(x * x + y * y) <= c1radius + 0.5) 
							{
								
								WorldGen.PlaceTile(randomX, randomY, mod.TileType("EmptyPlanetariumBlock"));
								WorldGen.PlaceTile(xPositionc1, yPositionc1, 189);
								
								Tile tile = Framing.GetTileSafely(xPositionc1, yPositionc1);

								if(tile.type != mod.TileType("EmptyPlanetariumBlock"))
								tile.type = 189;
							
								tile.wall = (ushort)WallID.Cloud;
							}
						}
					}
					
					int totalPlatforms = 2 * c1radius + Main.rand.Next(15 * i + 15) + 12;
					if(Main.rand.Next(3) != 0)
					{
						for(int t = 0; t < totalPlatforms; t++)
						{
							int distanceFromCenter = randomX + t;
							int distanceFromCenter2 = randomX - t;
							
								
							Tile tile = Framing.GetTileSafely(distanceFromCenter, randomY);
							Tile tileUP = Framing.GetTileSafely(distanceFromCenter, randomY - 1);
							Tile tileDOWN = Framing.GetTileSafely(distanceFromCenter, randomY + 1);
							
							Tile tile2 = Framing.GetTileSafely(distanceFromCenter2, randomY);
							Tile tile2UP = Framing.GetTileSafely(distanceFromCenter2, randomY - 1);
							Tile tile2DOWN = Framing.GetTileSafely(distanceFromCenter2, randomY + 1);
								
							if(!tile.active() && !tileUP.active() && !tileDOWN.active())
							WorldGen.PlaceTile(distanceFromCenter, randomY, mod.TileType("CloudPlatformTile"));
							
							if(!tile2.active() && !tile2UP.active() && !tile2DOWN.active())
							WorldGen.PlaceTile(distanceFromCenter2, randomY, mod.TileType("CloudPlatformTile"));
							
						}
					}
				}
				
				
			
			
			
			for(int i = 0; i < 10; i++) //Grass islands
			{ 
				
				float scaleDistance = 1 + i * 1.3f;
					int newXpos = xPosition;
					int newYpos = yPosition;
					
						if(i % 2 == 0)
						{
						scaleDistance = 1 + i * 0.25f;
						newXpos = xPosition + 30 + Main.rand.Next((int)(23 * i + 12));
						}
						else
						{
						scaleDistance = 1 + (i - 1) * 0.25f;
						newXpos = xPosition - 30 - Main.rand.Next((int)(23 * i + 12));
						}
						
						newYpos = yPosition + Main.rand.Next(-75, 51);
					
					if(newXpos - 20 < 0)
					{
						newXpos = xPosition + 30 + Main.rand.Next((int)(23 * i + 12));
					}
					if(newXpos + 20 > Main.maxTilesX)
					{
						newXpos = xPosition - 30 - Main.rand.Next((int)(23 * i + 12));
					}
						int radius2 = 10;     //this is the explosion radius, the highter is the value the bigger is the explosion
 
				for (int x = -radius2; x <= radius2; x++)
				{
					for (int y = -radius2; y <= radius2; y++)
					{
						int xPosition3 = (int)(x + newXpos);
						int yPosition3 = (int)(y + newYpos);
	 
						if (Math.Sqrt(x * x + y * y) <= radius2 + 0.5)   //this make so the explosion radius is a circle
						{
							WorldGen.KillTile(xPosition3 , yPosition3 , false, false, false);  //this make the explosion destroy tiles  
							Tile tile = Framing.GetTileSafely(xPosition3, yPosition3);
							tile.wall = (ushort)WallID.Cloud;
						}
					}
				}
				
				int[,] _grassIsland = {
				{0,0,0,0,0,0,2,2,0},
				{0,0,0,0,0,2,2,2,0},
				{0,0,0,0,2,2,5,2,0}, 
				{0,0,2,2,2,2,2,2,0}, 
				{3,3,2,2,2,2,2,2,2}, 
				{1,1,1,1,1,1,1,1,2}, 
				{5,1,1,1,1,1,1,1,0}, 
				{1,1,1,1,1,1,1,0,0}, 
				{1,1,1,1,1,1,0,0,0}, 
				{1,1,1,1,0,0,0,0,0}, 
			};
					int Xs = newXpos;
					int Ys = newYpos;
					
					
				Ys -= (int)(.5f * _grassIsland.GetLength(0));
				
				int totalPlatforms = Main.rand.Next(8 * i + 10) + 24;
				if(Main.rand.Next(3) != 0)
					{
						for(int t = 0; t < totalPlatforms; t++)
						{
							int distanceFromCenter = Xs + t;
							int distanceFromCenter2 = Xs - t;
							
							Tile tile = Framing.GetTileSafely(distanceFromCenter, newYpos);
							Tile tileUP = Framing.GetTileSafely(distanceFromCenter, newYpos - 1);
							Tile tileDOWN = Framing.GetTileSafely(distanceFromCenter, newYpos + 1);
							
							Tile tile2 = Framing.GetTileSafely(distanceFromCenter2, newYpos);
							Tile tile2UP = Framing.GetTileSafely(distanceFromCenter2, newYpos - 1);
							Tile tile2DOWN = Framing.GetTileSafely(distanceFromCenter2, newYpos + 1);
								
							if(!tile.active() && !tileUP.active() && !tileDOWN.active())
							WorldGen.PlaceTile(distanceFromCenter, newYpos, mod.TileType("CloudPlatformTile"));
							
							if(!tile2.active() && !tile2UP.active() && !tile2DOWN.active())
							WorldGen.PlaceTile(distanceFromCenter2, newYpos, mod.TileType("CloudPlatformTile"));
							
						}
					}
					
				for (int y = 0; y < _grassIsland.GetLength(0); y++) {
					for (int x = 0; x < _grassIsland.GetLength(1); x++) {
						int k = Xs + x;
						int l = Ys + y;
						int k2 = Xs - x;
						int l2 = Ys - y;
						if (WorldGen.InWorld(k, l, 30)) {
							Tile tile = Framing.GetTileSafely(k, l);
							Tile tile2 = Framing.GetTileSafely(k2, l);
							switch (_grassIsland[y, x]) {
								case 0:
									//tile.active(false);
									//tile2.active(false);
									break;
								case 1:
									tile.type = 189; //cloud
									tile.active(true);
									tile2.type = 189;
									tile2.active(true);
									break;
								case 2:
									tile.type = 223; //titanium
									tile.active(true);
									tile2.type = 223;
									tile2.active(true);
									break;
								case 3:
									tile.type = 2; //grass
									tile.active(true);
									tile2.type = 2;
									tile2.active(true);
									break;
								case 4:
									tile.type = 20; //acorn
									tile.active(true);
									tile2.type = 20;
									tile2.active(true);
									break;
								case 5:
									tile.type = (ushort)mod.TileType("EmptyPlanetariumBlock");
									tile.active(true);
									tile2.type = (ushort)mod.TileType("EmptyPlanetariumBlock");
									tile2.active(true);
									break;
							}
						}
					}
				}
			}
			
			{ //Planetarium Temple
				int radius = 21;    
		 
					for (int x = -radius; x <= radius; x++)
					{
						for (int y = -radius; y <= radius; y++)
						{
							int xPosition2 = (int)(x + xPosition);
							int yPosition2 = (int)(y + yPosition);
		 
							if (Math.Sqrt(x * x + y * y) <= radius + 0.5) 
							{
								WorldGen.KillTile(xPosition2, yPosition2, false, false, false); 
								Tile tile = Framing.GetTileSafely(xPosition2, yPosition2);
								tile.wall = (ushort)WallID.Cloud;
							}
						}
					}
					
				int[,] _planetarium = {
				{1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, //22
				{1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, //21
				{1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, //20
				{1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0}, //19
				{2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0}, //18
				{3,3,3,3,3,3,3,3,3,3,2,2,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0}, //17
				{2,2,2,2,2,2,2,2,2,2,2,3,2,1,1,1,1,0,0,0,0,0,0,0,0,0,0}, //16
				{0,0,0,0,0,0,0,0,2,2,2,3,3,2,1,1,1,1,0,0,0,0,0,0,0,0,0}, //15
				{0,0,0,0,0,0,0,0,2,5,2,2,3,3,2,1,1,1,1,0,0,0,0,0,0,0,0}, //14
				{0,0,2,2,2,0,0,0,2,2,2,2,2,3,3,2,1,1,1,1,0,0,0,0,0,0,0}, //13
				{0,2,2,3,2,0,0,0,0,0,0,0,2,2,2,2,1,1,1,0,0,0,0,0,0,0,0}, //12
				{2,2,3,3,2,0,0,0,0,0,0,0,2,3,3,2,0,0,0,0,0,0,0,0,0,0,0}, //11
				{3,3,3,2,2,0,0,0,0,0,0,0,2,3,3,2,0,0,0,0,0,0,0,0,0,0,0}, //10
				{3,2,2,2,0,0,0,0,0,0,0,0,2,3,3,2,0,0,0,0,0,0,0,0,0,0,0}, //9
				{3,2,0,0,0,0,0,0,0,0,0,0,2,3,3,2,0,0,2,0,0,0,0,0,0,0,2}, //8
				{3,2,2,2,2,0,0,0,0,0,0,0,2,3,3,2,0,0,2,2,2,2,2,2,2,2,2}, //7
				{3,3,3,3,2,0,0,0,0,0,0,0,2,3,3,2,0,0,2,3,3,3,3,3,3,3,2}, //6
				{3,2,2,2,2,0,0,0,0,0,0,0,2,3,3,2,0,0,2,3,2,2,2,2,2,2,2}, //5
				{3,2,0,0,0,0,0,0,0,0,0,0,2,3,3,2,0,0,2,3,2,0,0,0,0,0,0}, //4
				{2,2,0,0,0,0,0,0,0,0,0,0,2,3,3,2,0,0,2,3,2,0,0,0,0,0,0}, //3
				{2,0,0,0,0,0,0,0,0,0,0,0,2,3,3,2,0,0,2,3,2,0,0,0,0,0,0}, //2
				{2,0,0,0,0,0,0,0,0,0,0,0,2,3,3,2,0,0,2,3,2,0,0,0,0,0,0}, //1
				{4,0,0,0,0,0,0,0,0,0,0,0,2,3,3,2,0,0,2,3,2,0,0,0,0,0,0}, //0
				{2,0,0,0,0,0,0,0,0,0,0,0,2,3,3,2,0,0,2,3,2,0,0,0,0,0,0}, //-1
				{2,0,0,0,0,0,0,0,0,0,0,0,2,3,3,2,0,0,2,3,2,0,0,0,0,0,0}, //-2
				{2,2,2,0,0,0,0,0,0,0,0,0,2,3,3,2,0,0,2,3,2,0,0,0,0,0,0}, //-3
				{3,2,2,0,0,0,0,0,0,0,0,0,2,3,3,2,0,0,2,3,2,0,0,0,0,0,0}, //-4
				{3,2,2,2,2,2,2,2,2,2,0,0,2,3,3,2,0,0,2,3,2,0,0,0,0,0,0}, //-5
				{3,3,3,3,3,3,3,3,3,2,0,0,2,2,2,2,0,0,2,3,2,0,0,0,0,0,0}, //-6
				{2,2,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,2,3,2,1,0,0,0,0,0}, //-7
				{1,1,1,1,1,1,1,1,2,2,0,0,0,0,0,0,0,0,2,3,2,1,1,0,0,0,0}, //-8
				{1,1,1,1,1,1,1,1,2,2,0,0,0,0,0,0,0,0,2,3,2,1,1,1,0,0,0}, //-9
				{1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,3,2,1,1,1,0,0,0}, //-10
				{1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,3,2,1,1,0,0,0,0}, //-11
				{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,1,1,0,0,0,0}, //-12
				{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0}, //-13
				{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0}, //-14
				{1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, //-15
				{1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, //-16
			};
			
				int Xr = xPosition;
				int Yr = yPosition;
				Yr -= (int)(.5f * _planetarium.GetLength(0));
				
					for (int y = 0; y < _planetarium.GetLength(0); y++) {
						for (int x = 0; x < _planetarium.GetLength(1); x++) {
							int k = Xr + x;
							int l = Yr + y;
							int k2 = Xr - x;
							int l2 = Yr - y;
							if (WorldGen.InWorld(k, l, 30)) {
								Tile tile = Framing.GetTileSafely(k, l);
								Tile tile2 = Framing.GetTileSafely(k2, l);
								switch (_planetarium[y, x]) {
									case 0:
										//tile.active(false);
										//tile2.active(false);
										break;
									case 1:
										tile.type = 189;
										tile.active(true);
										tile2.type = 189;
										tile2.active(true);
										break;
									case 2:
										tile.type = 223;
										tile.active(true);
										tile2.type = 223;
										tile2.active(true);
										break;
									case 3:
										tile.type = 177;
										tile.active(true);
										tile2.type = 177;
										tile2.active(true);
										break;
									case 4:
										tile.type = (ushort)mod.TileType("PlanetariumBlock");
										tile.active(true);
										tile2.type = (ushort)mod.TileType("PlanetariumBlock");
										tile2.active(true);
										break;
									case 5:
										tile.type = (ushort)mod.TileType("EmptyPlanetariumBlock");
										tile.active(true);
										tile2.type = (ushort)mod.TileType("EmptyPlanetariumBlock");
										tile2.active(true);
										break;
								}
							}
						}
					}
			}
						
						

			  
						
						
			
				
						int radius4 = 15;
					for (int x = -radius4; x <= radius4; x++)
            {
                for (int y = -radius4; y <= radius4; y++)
                {
                    int xPosition5 = (int)(x + xPositionLibra);
                    int yPosition5 = (int)(y + yPosition);
 
                    if (Math.Sqrt(x * x + y * y) <= radius4 + 0.5)   //this make so the explosion radius is a circle
                    {
                        WorldGen.KillTile(xPosition5 , yPosition5 , false, false, false);  //this make the explosion destroy tiles  
                    }
                }
            }
			
						WorldGen.PlaceTile(xPositionLibra, yPosition, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPositionLibra, yPosition +1, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPositionLibra, yPosition +2, mod.TileType("DevilTile"));
						
						
						WorldGen.PlaceTile(xPositionLibra + 1, yPosition, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPositionLibra - 1, yPosition, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPositionLibra + 2, yPosition, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPositionLibra - 2, yPosition, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPositionLibra + 3, yPosition, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPositionLibra - 3, yPosition, mod.TileType("DevilTile"));
						
						WorldGen.PlaceTile(xPositionLibra + 4, yPosition, TileID.ObsidianBrick);
						WorldGen.PlaceTile(xPositionLibra - 4, yPosition, TileID.ObsidianBrick);
						
						WorldGen.PlaceTile(xPositionLibra + 1, yPosition + 1, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPositionLibra - 1, yPosition + 1, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPositionLibra + 2, yPosition + 1, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPositionLibra - 2, yPosition + 1, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPositionLibra + 3, yPosition + 1, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPositionLibra - 3, yPosition + 1, mod.TileType("DevilTile"));
						
						WorldGen.PlaceTile(xPositionLibra + 4, yPosition + 1, TileID.ObsidianBrick);
						WorldGen.PlaceTile(xPositionLibra - 4, yPosition + 1, TileID.ObsidianBrick);
						
						WorldGen.PlaceTile(xPositionLibra + 1, yPosition + 2, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPositionLibra - 1, yPosition + 2, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPositionLibra + 2, yPosition + 2, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPositionLibra - 2, yPosition + 2, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPositionLibra + 3, yPosition + 2, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPositionLibra - 3, yPosition + 2, mod.TileType("DevilTile"));
						
						WorldGen.PlaceTile(xPositionLibra + 5, yPosition + 1, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPositionLibra - 5, yPosition + 1, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPositionLibra + 5, yPosition, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPositionLibra - 5, yPosition, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPositionLibra + 5, yPosition - 1, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPositionLibra - 5, yPosition - 1, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPositionLibra + 5, yPosition - 2, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPositionLibra - 5, yPosition - 2, mod.TileType("DevilTile"));	
							
						
						WorldGen.PlaceTile(xPositionLibra + 6, yPosition, TileID.ObsidianBrick);
						WorldGen.PlaceTile(xPositionLibra - 6, yPosition, TileID.ObsidianBrick);
						WorldGen.PlaceTile(xPositionLibra + 6, yPosition - 1, TileID.ObsidianBrick);
						WorldGen.PlaceTile(xPositionLibra - 6, yPosition - 1, TileID.ObsidianBrick);
						WorldGen.PlaceTile(xPositionLibra + 6, yPosition - 2, TileID.ObsidianBrick);
						WorldGen.PlaceTile(xPositionLibra - 6, yPosition - 2, TileID.ObsidianBrick);
						WorldGen.PlaceTile(xPositionLibra + 6, yPosition - 3, TileID.ObsidianBrick);
						WorldGen.PlaceTile(xPositionLibra - 6, yPosition - 3, TileID.ObsidianBrick);
						WorldGen.PlaceTile(xPositionLibra + 6, yPosition - 4, TileID.ObsidianBrick);
						WorldGen.PlaceTile(xPositionLibra - 6, yPosition - 4, TileID.ObsidianBrick);
						
						WorldGen.PlaceTile(xPositionLibra + 5, yPosition - 4, 341);
						WorldGen.PlaceTile(xPositionLibra - 5, yPosition - 4, 341);
						
						WorldGen.PlaceTile(xPositionLibra, yPosition - 1, mod.TileType("DevilAltarTile"));
			
				})); 
				*/
			tasks.Insert(genIndexGems, new PassLegacy("ModdedSOTSStructures", delegate (GenerationProgress progress)
			{
					progress.Message = "Generating Surface Structures";
						
						int iceY = -1;
						int iceX = -1;
						for(int xCheck = Main.rand.Next(Main.maxTilesX); xCheck != -1; xCheck = Main.rand.Next(Main.maxTilesX))
						{
							for(int ydown = 0; ydown != -1; ydown++)
							{
								Tile tile = Framing.GetTileSafely(xCheck, ydown);
								if(tile.active() && tile.type == TileID.SnowBlock)
								{
									iceY = ydown;
									break;
								}
								else if(tile.active())
								{
									break;
								}
							}
							if(iceY != -1)
							{
								iceX = xCheck;
								break;
							}
						 }
						 
					int radius9 = 12;
					for (int x = -radius9; x <= radius9; x++)
					{
						for (int y = -radius9; y <= radius9; y++)
						{
							int xPosition6 = iceX + x;
							int yPosition6 = iceY + -Math.Abs(y); 
		 
							if (Math.Sqrt(x * x + y * y) <= radius9 + 0.5)   
							{
								WorldGen.KillTile(xPosition6 , yPosition6 , false, false, false);
							}
						}
					}
					int[,] _iceArtifact = {
					{0,0,0,0,0,0,1,2,3,0,0,0,0,0,0},
					{0,0,0,0,0,1,2,4,2,3,0,0,0,0,0},
					{0,0,0,0,1,2,4,4,4,2,3,0,0,0,0},
					{0,0,0,1,2,4,4,4,4,4,2,3,0,0,0},
					{0,0,1,2,4,4,4,5,4,4,4,2,3,0,0},
					{0,1,2,4,4,4,5,0,5,4,4,4,2,3,0},
					{0,0,5,4,4,5,0,0,0,5,4,4,5,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,9,9,9,0,0,0,0,0,0},
					{0,0,0,0,0,0,9,6,9,0,0,0,0,0,0},
					{0,0,4,4,4,4,7,7,7,4,4,4,4,0,0},
					{0,0,0,5,5,4,7,7,7,4,5,5,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{4,4,4,4,4,4,5,5,5,4,4,4,4,4,4},
					{0,0,5,5,2,0,0,0,0,0,2,5,5,0,0},
					{0,0,0,4,4,4,5,5,5,4,4,4,0,0,0},
					{0,0,5,5,2,0,0,0,0,0,2,5,5,0,0},
					{0,0,0,4,4,4,5,5,5,4,4,4,0,0,0},
					{0,0,0,0,2,0,0,0,0,0,2,0,0,0,0},
					{0,0,0,0,2,0,0,0,0,0,2,0,0,0,0},
					{0,0,0,0,2,0,0,0,0,0,2,0,0,0,0},
					{0,0,2,2,2,2,0,0,0,2,2,2,2,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,2,2,2,2,2,2,2,2,2,2,2,0,0},
					{8,8,2,2,2,2,2,2,2,2,2,2,2,8,8}
				};	
					int[,] _iceArtifactWalls = {
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,3,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,3,1,3,0,0,0,0,0,0},
					{0,0,0,0,3,3,1,1,1,3,3,0,0,0,0},
					{0,0,0,0,3,2,1,1,1,2,3,0,0,0,0},
					{0,0,0,0,3,2,1,1,1,2,3,0,0,0,0},
					{0,0,0,0,3,2,1,1,1,2,3,0,0,0,0},
					{0,0,0,0,3,2,1,1,1,2,3,0,0,0,0},
					{0,0,0,0,3,2,1,1,1,2,3,0,0,0,0},
					{0,0,0,0,3,2,1,1,1,2,3,0,0,0,0},
					{0,0,0,0,0,3,1,1,1,3,0,0,0,0,0},
					{0,0,0,0,0,3,2,1,2,3,0,0,0,0,0},
					{0,0,0,0,0,3,2,1,2,3,0,0,0,0,0},
					{0,3,3,3,3,3,2,1,2,3,3,3,3,3,0},
					{0,0,0,0,0,3,3,3,3,3,0,0,0,0,0},
					{0,0,0,0,0,3,2,1,2,3,0,0,0,0,0},
					{0,0,0,0,0,2,3,3,3,2,0,0,0,0,0},
					{0,0,0,0,0,3,2,1,2,3,0,0,0,0,0},
					{0,0,0,0,0,2,3,3,3,2,0,0,0,0,0},
					{0,0,0,0,0,3,2,1,2,3,0,0,0,0,0},
					{0,0,0,0,0,2,2,1,2,2,0,0,0,0,0},
					{0,0,0,0,0,2,2,1,2,2,0,0,0,0,0},
					{0,0,0,0,0,2,2,1,2,2,0,0,0,0,0},
					{0,0,0,1,2,1,2,1,2,1,2,1,0,0,0},
					{0,0,0,1,2,1,2,1,2,1,2,1,0,0,0},
					{0,0,0,1,2,1,2,1,2,1,2,1,0,0,0},
					{0,0,0,1,2,1,2,1,2,1,2,1,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
				};
				
					int iceArtifactPositionX = iceX;
					int iceArtifactPositionY = iceY - _iceArtifact.GetLength(0);
					iceArtifactPositionX -= (int)(.5f * _iceArtifact.GetLength(1));
					//iceArtifactPositionY -= (int)(.5f * _iceArtifact.GetLength(0));
					for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
					{
						for (int y = 0; y < _iceArtifact.GetLength(0); y++) {
							for (int x = 0; x < _iceArtifact.GetLength(1); x++) {
								int k = iceArtifactPositionX + x;
								int l = iceArtifactPositionY + y;
								if (WorldGen.InWorld(k, l, 30)) {
									Tile tile = Framing.GetTileSafely(k, l);
									switch (_iceArtifact[y, x]) {
										case 0:
											tile.active(false);
											break;
										case 1:
											tile.type = 148; //snowbrick
											tile.slope(2);
											tile.active(true);
											break;
										case 2:
											tile.type = 148; //snowbrick
											tile.active(true);
											tile.slope(0);
											break;
										case 3:
											tile.type = 148; //snowbrick
											tile.active(true);
											tile.slope(1);
											break;
										case 4:
											tile.type = 321; //boreal
											tile.active(true);
											tile.slope(0);
											break;
										case 5:
											WorldGen.PlaceTile(k, l, TileID.Platforms, true, true, -1, 19); //boreal platform
											break;
										case 6:
											WorldGen.PlaceTile(k, l, (ushort)mod.TileType("FrostArtifactTile"));
											break;
										case 7:
											tile.type = (ushort)mod.TileType("HardIceBrickTile"); //ice
											tile.active(true);
											tile.slope(0);
											break;
										case 8:
											tile.type = TileID.SnowBlock;
											tile.active(true);
											tile.slope(0);
											break;
										case 9:
											break;
										
									}
								}
							}
						}
					}
					for (int y = 0; y < _iceArtifactWalls.GetLength(0); y++) {
						for (int x = 0; x < _iceArtifactWalls.GetLength(1); x++) {
							int k = iceArtifactPositionX + x;
							int l = iceArtifactPositionY + y;
							if (WorldGen.InWorld(k, l, 30)) {
								Tile tile = Framing.GetTileSafely(k, l);
								switch (_iceArtifactWalls[y, x]) {
									case 0:
										break;
									case 1:
										tile.wall = (ushort)WallID.SnowflakeWallpaper; //snowflake wall
										break;
									case 2:
										tile.wall = 31; //snowbrick wall
										break;
									case 3:
										tile.wall = 149; //boreal wall
										break;
								}
							}
						}
					}
					int radius6 = 10;
					for (int x = -radius6; x <= radius6; x++)
					{
						for (int y = -radius6; y <= radius6; y++)
						{
							int xPosition6 = iceX + x;
							int yPosition6 = iceY + Math.Abs(y); 
		 
							if (Math.Sqrt(x * x + y * y) <= radius6 + 0.5)   
							{
								WorldGen.PlaceTile(xPosition6, yPosition6, TileID.SnowBlock);
								Tile tile = Framing.GetTileSafely(xPosition6, yPosition6);
								tile.slope(0);
							}
						}
					}
					
						int spawnX = -1;
						int spawnY = -1;
						int randomOne = Main.rand.Next(2);
						if(randomOne == 0)
						{
							randomOne = -1;
						}
						for(int xCheck = (int)(Main.maxTilesX * 0.5f) + (randomOne * Main.rand.Next(15,41)); xCheck != -1; xCheck = (int)(Main.maxTilesX * 0.5f) + (randomOne * Main.rand.Next(15,41)))
						{
							for(int ydown = 0; ydown != -1; ydown++)
							{
								Tile tile = Framing.GetTileSafely(xCheck, ydown);
								if(tile.active() && Main.tileSolid[(int)tile.type]) //grass block
								{
									spawnY = ydown;
									break;
								}
							}
							if(spawnY != -1)
							{
								spawnX = xCheck;
								break;
							}
						 }
						 
						 
					int radius8 = 10;
					for (int x = -radius8; x <= radius8; x++)
					{
						for (int y = -radius8; y <= radius8; y++)
						{
							int xPosition6 = spawnX + x;
							int yPosition6 = spawnY + -Math.Abs(y); 
		 
							if (Math.Sqrt(x * x + y * y) <= radius8 + 0.5)   
							{
								WorldGen.KillTile(xPosition6 , yPosition6 , false, false, false);
							}
						}
					}
					
					int[,] _startingHouse = {
					{4,4,4,0,0,4,4,4,0,0,0,0,0},
					{8,1,8,0,0,8,1,8,0,0,0,0,0},
					{9,1,9,0,0,9,1,9,0,0,0,2,0},
					{9,1,9,9,9,9,1,9,0,9,9,5,0},
					{0,1,0,6,9,0,1,0,0,7,9,0,0},
					{4,4,4,5,5,4,4,4,4,4,4,4,4},
					{0,4,0,0,0,0,0,0,0,3,3,4,0},
					{4,4,0,0,0,0,0,0,0,5,5,4,4},
					{0,9,0,0,2,3,0,0,0,0,0,9,0},
					{0,9,0,9,9,9,9,0,0,0,0,9,0},
					{0,12,0,9,10,9,11,0,0,0,0,12,0},
					{4,4,4,4,4,4,4,4,4,4,4,4,4}
				};	
					int[,] _startingHouseWalls = {
					{0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,1,0},
					{0,1,1,1,1,1,1,1,1,1,1,1,0},
					{0,0,2,2,2,2,2,2,2,2,2,0,0},
					{0,0,2,2,2,2,2,2,2,2,2,0,0},
					{0,0,2,4,4,4,2,2,2,2,2,0,0},
					{0,0,2,4,4,4,2,2,2,2,2,0,0},
					{0,0,2,2,2,2,2,2,2,2,2,0,0},
					{0,0,3,3,3,3,3,3,3,3,3,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0}
				};
				
					int housePosX = spawnX;
					int housePosY = spawnY - _startingHouse.GetLength(0);
					housePosX -= (int)(.5f * _startingHouse.GetLength(1));
					
					for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
					{
						for (int y = 0; y < _startingHouse.GetLength(0); y++) {
							for (int x = 0; x < _startingHouse.GetLength(1); x++) {
								int k = housePosX + x;
								int l = housePosY + y + 1;
								if (WorldGen.InWorld(k, l, 30)) {
									Tile tile = Framing.GetTileSafely(k, l);
									switch (_startingHouse[y, x]) {
										case 0:
											tile.active(false);
											break;
										case 1:
											tile.type = TileID.WoodenBeam; //beam
											tile.active(true);
											break;
										case 2:
											tile.type = TileID.Candles;
											tile.active(true);
											break;
										case 3:
											tile.type = 50; //book
											tile.active(true);
											break;
										case 4:
											tile.type = TileID.WoodBlock; //wood
											tile.active(true);
											tile.slope(0);
											break;
										case 5:
											WorldGen.PlaceTile(k, l, TileID.Platforms, true, true, -1, 0); //platform
											break;
										case 6:
											WorldGen.PlaceTile(k, l, mod.TileType("StrangeChestTile"));
											break;
										case 7:
											WorldGen.PlaceTile(k, l, 376); //crate
											break;
										case 8:
											WorldGen.PlaceTile(k, l, TileID.Banners, true, true, -1, 0); //banner
											break;
										case 9:
											break;
										case 10:
											WorldGen.PlaceTile(k, l, TileID.Tables, true, true, -1, 0); //table
											break;
										case 11:
											WorldGen.PlaceTile(k, l, TileID.Chairs, true, true, -1, 0); //chair
											break;
										case 12:
											WorldGen.PlaceTile(k, l, TileID.ClosedDoor, true, true, -1, 0); //door
											break;
										
									}
								}
							}
						}
					}
					for (int y = 0; y < _startingHouseWalls.GetLength(0); y++) {
						for (int x = 0; x < _startingHouseWalls.GetLength(1); x++) {
							int k = housePosX + x;
							int l = housePosY + y + 1;
							if (WorldGen.InWorld(k, l, 30)) {
								Tile tile = Framing.GetTileSafely(k, l);
								switch (_startingHouseWalls[y, x]) {
									case 0:
										break;
									case 1:
										tile.wall = 106; //fence
										break;
									case 2:
										tile.wall = 4; //wooden
										break;
									case 3:
										tile.wall = 27; //planked
										break;
									case 4:
										tile.wall = WallID.Glass; //glass
										break;
								}
							}
						}
					}
					int radius7 = 10;
					for (int x = -radius7; x <= radius7; x++)
					{
						for (int y = -radius7; y <= radius7; y++)
						{
							int xPosition6 = spawnX + x;
							int yPosition6 = spawnY + Math.Abs(y); 
		 
							if (Math.Sqrt(x * x + y * y) <= radius7 + 0.5)   
							{
								Tile tile = Framing.GetTileSafely(xPosition6, yPosition6);
								Tile tile2 = Framing.GetTileSafely(xPosition6, yPosition6 - 1);
								if(!tile.active())
								{
								tile.type = TileID.Dirt;
								}
								if(!tile2.active() && tile.type == TileID.Dirt)
								{
								tile.type = 2;	
								}
								tile.active();
							}
						}
					}
						
				}));
			tasks.Insert(genIndexEnd + 2, new PassLegacy("genIndexModPlanetarium", delegate (GenerationProgress progress)
			{
				progress.Message = "Generating Sky Artifact";
				int dungeonSide = -1;
				if (Main.dungeonX > (int)(Main.maxTilesX / 2))
				{
					dungeonSide = 1;
				}
				// -1 = dungeon on left, 1 = dungeon on right
				int pyramidX = -1;
				int checks = 0;
				if (dungeonSide == -1)
				{
					int xCheck = Main.rand.Next(400, (int)(Main.maxTilesX / 2));
					for (; xCheck != -1; xCheck = Main.rand.Next(400, (int)(Main.maxTilesX / 2)))
					{
						pyramidX = xCheck;
						bool validLocation = false;
						for (int ydown = 0; ydown != -1; ydown++)
						{
							Tile tile = Framing.GetTileSafely(xCheck, ydown);
							if (tile.active() && (tile.type == TileID.SnowBlock || tile.type == TileID.IceBlock))
							{
								validLocation = true;
								break;
							}
							else if (tile.active() && Main.tileSolid[tile.type])
							{
								break;
							}
						}
						checks++;
						if (validLocation || checks >= 300)
						{
							bool force = false;
							if (checks >= 400)
							{
								force = true;
							}
							if (SOTSWorldgenHelper.GeneratePlanetariumFull(mod, pyramidX, 140, force))
							{
								return;
							}
						}
					}
				}
				if (dungeonSide == 1)
				{
					int xCheck = Main.rand.Next((int)(Main.maxTilesX / 2), Main.maxTilesX - 400);
					for (; xCheck != -1; xCheck = Main.rand.Next((int)(Main.maxTilesX / 2), Main.maxTilesX - 400))
					{
						pyramidX = xCheck;
						bool validLocation = false;
						for (int ydown = 0; ydown != -1; ydown++)
						{
							Tile tile = Framing.GetTileSafely(xCheck, ydown);
							if (tile.active() && (tile.type == TileID.SnowBlock || tile.type == TileID.IceBlock))
							{
								validLocation = true;
								break;
							}
							else if(tile.active() && Main.tileSolid[tile.type])
                            {
								break;
                            }
						}
						checks++;
						if (validLocation || checks >= 300)
						{
							bool force = false;
							if (checks >= 400)
							{
								force = true;
							}
							if (SOTSWorldgenHelper.GeneratePlanetariumFull(mod, pyramidX, 140, force))
							{
								return;
							}
						}
					}
				}
			}));
			tasks.Insert(genIndexEnd + 3, new PassLegacy("genIndexModPyramid", delegate (GenerationProgress progress)
			{
				progress.Message = "Generating A Pyramid";
				///Finding the dungeon
				int dungeonSide = -1;
				if (Main.dungeonX > (int)(Main.maxTilesX / 2))
				{
					dungeonSide = 1;
				}
				// -1 = dungeon on left, 1 = dungeon on right
				int pyramidY = -1;
				int pyramidX = -1;
				int checks = 0;
				if(dungeonSide == -1)
				{
					int xCheck = Main.rand.Next((int)(Main.maxTilesX / 2), Main.maxTilesX - 500);
					for (; xCheck != -1; xCheck = Main.rand.Next((int)(Main.maxTilesX / 2), Main.maxTilesX -500))
					{
						for(int ydown = 0; ydown != -1; ydown++)
						{
							Tile tile = Framing.GetTileSafely(xCheck, ydown);
							if (tile.active() && (tile.type == TileID.Sand || checks >= 1000))
							{
								if ((!WorldGen.UndergroundDesertLocation.Contains(new Point(xCheck, ydown + 60)) && !WorldGen.UndergroundDesertLocation.Contains(new Point(xCheck, ydown + 120))) || checks > 200)
								{
									pyramidY = ydown;
								}
								break;
							}
							else if (tile.active())
							{
								break;
							}
						}
						if(pyramidY != -1)
						{
							pyramidX = xCheck;
							break;
						}
						checks++;
					}
				 }
				if(dungeonSide == 1)
				{
					int xCheck = Main.rand.Next(500, (int)(Main.maxTilesX / 2));
					for (; xCheck != -1; xCheck = Main.rand.Next(500, (int)(Main.maxTilesX / 2)))
					{
						for(int ydown = 0; ydown != -1; ydown++)
						{
							Tile tile = Framing.GetTileSafely(xCheck, ydown);
							if (tile.active() && tile.type == TileID.Sand)
							{
								if ((!WorldGen.UndergroundDesertLocation.Contains(new Point(xCheck, ydown + 60)) && !WorldGen.UndergroundDesertLocation.Contains(new Point(xCheck, ydown + 120))) || checks > 200)
								{
									pyramidY = ydown;
								}
								break;
							}
							else if (tile.active())
							{
								break;
							}
						}
						if(pyramidY != -1)
						{
							pyramidX = xCheck;
							break;
						}
						checks++;
					}
				}
				pyramidY -= 15;
				int direction = Main.rand.Next(2);
				int finalDirection = direction;
				int nextAmount = Main.rand.Next(6,16);
				int size = 300;
				int endingTileX = -1;
				int endingTileY = -1;
				int initialPath = 1;
				
				if(Main.maxTilesX > 4000)
				size = 220;
				
				if(Main.maxTilesX > 6000)
				size = 260;
				
				if(Main.maxTilesX > 8000)
				size = 300;

				if(direction == 0)
				{
					direction = -1;
				}
					
				for(int pyramidLevel = 0; pyramidLevel < size; pyramidLevel++)
				{							
					for(int h = -pyramidLevel; h <= pyramidLevel; h++)
					{
						Tile tile = Framing.GetTileSafely(pyramidX + h, pyramidY + pyramidLevel);
						if(tile.type != TileID.BlueDungeonBrick && tile.type != TileID.GreenDungeonBrick && tile.type != TileID.PinkDungeonBrick && tile.wall != 7 && tile.wall != 8 && tile.wall != 9 && tile.wall != 94 && tile.wall != 95 && tile.wall != 96 && tile.wall != 97 && tile.wall != 98 && tile.wall != 99) //check for not dungeon!
						{
							tile.type = (ushort)mod.TileType("PyramidSlabTile");
							tile.slope(0);
							tile.halfBrick(false);
							tile.liquidType(0);
							tile.liquid = 0;
							tile.active(true);
						}
					}
					for(int h = -pyramidLevel + 1; h <= pyramidLevel - 1; h++)
					{
						Tile tile = Framing.GetTileSafely(pyramidX + h, pyramidY + pyramidLevel);
						if(tile.type != TileID.BlueDungeonBrick && tile.type != TileID.GreenDungeonBrick && tile.type != TileID.PinkDungeonBrick && tile.wall != 7 && tile.wall != 8 && tile.wall != 9 && tile.wall != 94 && tile.wall != 95 && tile.wall != 96 && tile.wall != 97 && tile.wall != 98 && tile.wall != 99) //check for not dungeon!
						{
							tile.wall = (ushort)mod.WallType("PyramidWallTile");
						}
					}
					if(pyramidLevel >= 10 && pyramidLevel <= 15)
					{
						if(direction == -1)
						{
							for(int g = -pyramidLevel; g <= pyramidLevel -10; g++)
							{
								Tile tile = Framing.GetTileSafely(pyramidX + g, pyramidY + pyramidLevel);
								if(tile.type == (ushort)mod.TileType("PyramidSlabTile"))
								tile.active(false);
							}
						}
						if(direction == 1)
						{
							for(int g = pyramidLevel; g >= -pyramidLevel + 10; g--)
							{
								Tile tile = Framing.GetTileSafely(pyramidX + g, pyramidY + pyramidLevel);
								if(tile.type == (ushort)mod.TileType("PyramidSlabTile"))
								tile.active(false);
							}
						}
					}
					if(pyramidLevel >= 15 && initialPath == 1)
					{
						if(15 + nextAmount <= pyramidLevel)
						{
							initialPath = -1;
						}
						else
						{
							if(direction == -1)
							{
								for(int g = pyramidLevel - 16; g <= pyramidLevel - 10; g++)
								{
									Tile tile = Framing.GetTileSafely(pyramidX + g, pyramidY + pyramidLevel);
									if(tile.type == (ushort)mod.TileType("PyramidSlabTile"))
									tile.active(false);
								
								}
								endingTileX = pyramidX + (pyramidLevel - 13);
							}
							if(direction == 1)
							{
								for(int g = -pyramidLevel + 16; g >= -pyramidLevel + 10; g--)
								{
									Tile tile = Framing.GetTileSafely(pyramidX + g, pyramidY + pyramidLevel);
									if(tile.type == (ushort)mod.TileType("PyramidSlabTile"))
									tile.active(false);
								}
								endingTileX = pyramidX + (-pyramidLevel + 13);
							}
						}
						endingTileY = pyramidY + pyramidLevel;
					}
				}
				for(int totalAmount = 0; totalAmount < size; totalAmount += nextAmount)
				{
					direction *= -1;
					nextAmount = Main.rand.Next(6,31);
					if(totalAmount > size - 230)
					{
						if(endingTileX > pyramidX && endingTileX < pyramidX + 85)
						{
							direction = 1;
							finalDirection = -1;
						}
						if(endingTileX < pyramidX && endingTileX > pyramidX - 85)
						{
							direction = -1;
							finalDirection = 1;
						}
					}
					for(int g = nextAmount; g > 0; g--)
					{
						if(direction == -1)
						{
							endingTileX--;
							for(int h = 3; h >= -3; h--)
							{
								Tile tile = Framing.GetTileSafely(endingTileX + h, endingTileY);
								if(tile.type == (ushort)mod.TileType("PyramidSlabTile"))
									tile.active(false);
							
							}
							endingTileY++;
						}
						if(direction == 1)
						{
							endingTileX++;
							for(int h = 3; h >= -3; h--)
							{
								Tile tile = Framing.GetTileSafely(endingTileX + h, endingTileY);
								if(tile.type == (ushort)mod.TileType("PyramidSlabTile"))
									tile.active(false);
							
							}
							endingTileY++;
						}
					}
				}

				//creates cooridors
				int overgrownX = -1;
				int overgrownY = -1;
				int counterL = 0;
				int counterR = 0;
				int counterSpike = 0;
				bool continueCooridor = true;
				for(int fx = -size; fx < size; fx++)
				{
					int findTileX = (fx * -finalDirection) + pyramidX;
					for (int y1 = 2; y1 >= -2; y1--) //top cooridor
					{
						int higherUpY = pyramidY + 30;
						Tile selectTile = Framing.GetTileSafely(findTileX, higherUpY + y1);
						Tile selectTileLeft = Framing.GetTileSafely(findTileX - 1, higherUpY + y1);
						Tile selectTileLeft2 = Framing.GetTileSafely(findTileX - 2, higherUpY + y1);
						Tile selectTileRight = Framing.GetTileSafely(findTileX + 1, higherUpY + y1);
						Tile selectTileRight2 = Framing.GetTileSafely(findTileX + 2, higherUpY + y1);
						if(selectTile.type == (ushort)mod.TileType("PyramidSlabTile") && selectTileLeft.type == (ushort)mod.TileType("PyramidSlabTile") && selectTileLeft2.type == (ushort)mod.TileType("PyramidSlabTile") && selectTileRight.type == (ushort)mod.TileType("PyramidSlabTile") && selectTileRight2.type == (ushort)mod.TileType("PyramidSlabTile"))
						{
							selectTile.active(false);
						}
					}
					int counterInvalid = 0;
					for (int y1 = 2; y1 >= -2; y1--) //zeppelin room and burial room cooridor
					{
						int higherUpY = pyramidY + (size - 40);
						if (!continueCooridor)
                        {
							if(overgrownX == -1 && overgrownY == -1)
							{
								overgrownX = findTileX - (26 * finalDirection);
								overgrownY = higherUpY;
							}
							break;
						}
						Tile selectTile = Framing.GetTileSafely(findTileX, higherUpY + y1);
						Tile selectTileLeft = Framing.GetTileSafely(findTileX - 1, higherUpY + y1);
						Tile selectTileLeft2 = Framing.GetTileSafely(findTileX - 2, higherUpY + y1);
						Tile selectTileRight = Framing.GetTileSafely(findTileX + 1, higherUpY + y1);
						Tile selectTileRight2 = Framing.GetTileSafely(findTileX + 2, higherUpY + y1);
						if (!selectTile.active() && !selectTileLeft.active() && !selectTileRight.active() && selectTile.wall == (ushort)mod.WallType("PyramidWallTile") && selectTileLeft.wall == (ushort)mod.WallType("PyramidWallTile") && selectTileRight.wall == (ushort)mod.WallType("PyramidWallTile")) //end the cooridor upon reaching the stairwell
						{
							counterInvalid++;
							if (counterInvalid >= 5)
								continueCooridor = false;
						}
						if (selectTile.type == (ushort)mod.TileType("PyramidSlabTile") && selectTileLeft.type == (ushort)mod.TileType("PyramidSlabTile") && selectTileLeft2.type == (ushort)mod.TileType("PyramidSlabTile") && selectTileRight.type == (ushort)mod.TileType("PyramidSlabTile") && selectTileRight2.type == (ushort)mod.TileType("PyramidSlabTile"))
						{
							selectTile.active(false);
						}
					}
					for(int y1 = 2; y1 >= -2; y1--) //sandslabe cooridor for zepline room
					{
						int higherUpY = pyramidY + (size - 12);
						Tile selectTile = Framing.GetTileSafely(findTileX, higherUpY + y1);
						if(selectTile.type == (ushort)mod.TileType("PyramidSlabTile") && selectTile.active() == true)
						{
							selectTile.type = 274;
						}
					}
					for(int findTileY = pyramidY + (size - 55); findTileY > pyramidY + 10; findTileY--)
					{
						int max = Math.Abs((int)((findTileY - pyramidY) * 0.8f));
						int min = Math.Abs((int)((findTileY - pyramidY) * 0.5f));
						Tile tile = Framing.GetTileSafely(findTileX, findTileY);
						Tile tileLeft = Framing.GetTileSafely(findTileX - 1, findTileY);
						Tile tileRight = Framing.GetTileSafely(findTileX + 1, findTileY);
						if(tile.type == (ushort)mod.TileType("PyramidSlabTile"))
						{
							if(!(findTileY > pyramidY + (size - 70) && finalDirection == 1) && tileLeft.active() && tile.active() && !tileRight.active() && tileRight.wall == (ushort)mod.WallType("PyramidWallTile"))
							{
								//generate cooridor to the left
								counterL++;
								int randDistance = Main.rand.Next(min, max);
								if(Main.rand.Next(3) == 0 && counterL >= 40)
								{
									int coorXPos = findTileX + 2;
									for(int dis = randDistance; dis > 0; dis--)
									{
										for(int y1 = 2; y1 >= -2; y1--)
										{
											Tile selectTile = Framing.GetTileSafely(coorXPos, findTileY + y1);
											Tile selectTileLeft = Framing.GetTileSafely(coorXPos - 1, findTileY + y1);
											Tile selectTileLeft2 = Framing.GetTileSafely(coorXPos - 2, findTileY + y1);
											if(selectTile.type == (ushort)mod.TileType("PyramidSlabTile") && selectTileLeft.type == (ushort)mod.TileType("PyramidSlabTile") && selectTileLeft2.type == (ushort)mod.TileType("PyramidSlabTile"))
											{
												selectTile.active(false);
											}
										}
										coorXPos--;
										counterL = 0;
									}
								}
							}
							if (!(findTileY > pyramidY + (size - 70) && finalDirection == -1) && tileRight.active() && tile.active() && !tileLeft.active() && tileLeft.wall == (ushort)mod.WallType("PyramidWallTile"))
							{
								//generate cooridor to the right
								int randDistance = Main.rand.Next(min,max);
								counterR++;
								if(Main.rand.Next(3) == 0 && counterR >= 40)
								{
									int coorXPos = findTileX - 2;
									for(int dis = randDistance; dis > 0; dis--)
									{
										for(int y1 = 2; y1 >= -2; y1--)
										{
											Tile selectTile = Framing.GetTileSafely(coorXPos, findTileY + y1);
											Tile selectTileRight = Framing.GetTileSafely(coorXPos + 1, findTileY + y1);
											Tile selectTileRight2 = Framing.GetTileSafely(coorXPos + 2, findTileY + y1);
											if(selectTile.type == (ushort)mod.TileType("PyramidSlabTile") && selectTileRight.type == (ushort)mod.TileType("PyramidSlabTile") && selectTileRight2.type == (ushort)mod.TileType("PyramidSlabTile"))
											{
												selectTile.active(false);
											}
										}
										coorXPos++;
										counterR = 0;
									}
								}
							}
							Tile tileUp = Framing.GetTileSafely(findTileX, findTileY - 1);
							Tile tileDown = Framing.GetTileSafely(findTileX, findTileY + 1);
							if (tile.type == (ushort)mod.TileType("PyramidSlabTile") && tile.active() && tileLeft.type == (ushort)mod.TileType("PyramidSlabTile") && tileLeft.active() && tileRight.type == (ushort)mod.TileType("PyramidSlabTile") && tileRight.active() && (!tileUp.active() || !tileDown.active()))
							{
								counterSpike++;
								if(counterSpike >= 40)
								{
									counterSpike = 0;
									for(int sizeSpike = 0; sizeSpike < Main.rand.Next(4,20); sizeSpike++)
									{
											Tile tileSpikeR = Framing.GetTileSafely(findTileX + sizeSpike, findTileY);
											Tile tileSpikeRU = Framing.GetTileSafely(findTileX + sizeSpike, findTileY - 1);
											Tile tileSpikeRD = Framing.GetTileSafely(findTileX + sizeSpike, findTileY + 1);
											Tile tileSpikeL = Framing.GetTileSafely(findTileX - sizeSpike, findTileY);
											Tile tileSpikeLU = Framing.GetTileSafely(findTileX - sizeSpike, findTileY - 1);
											Tile tileSpikeLD = Framing.GetTileSafely(findTileX - sizeSpike, findTileY + 1);
											
										if(tileSpikeR.active() && tileSpikeR.type == (ushort)mod.TileType("PyramidSlabTile") && tileSpikeL.active() && tileSpikeL.type == (ushort)mod.TileType("PyramidSlabTile"))
										{
											if(tileSpikeLU.active() == false && tileSpikeLD.active() == true)
											{
												tileSpikeL.type = 232; //wooden spike
											}
											if(tileSpikeRU.active() == false && tileSpikeRD.active() == true)
											{
												tileSpikeR.type = 232; //wooden spike
											}
											if(tileSpikeLU.active() == true && tileSpikeLD.active() == false)
											{
												tileSpikeL.type = 232; //wooden spike
											}
											if(tileSpikeRU.active() == true && tileSpikeRD.active() == false)
											{
												tileSpikeR.type = 232; //wooden spike
											}
										}
										else
										{
											break;
										}
									}
								}
							}
						}
					}
				}
				
				
				int[,] _bossRoom = {
					
					{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
					{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
					{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
					{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
					{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
					{1,1,1,1,1,1,1,1,1,1,1,1,1,1,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
					{1,1,1,1,1,1,1,1,1,1,1,1,1,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,2,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,1,1,1,1,1,1,1,1,1,1,1,1,1},
					{1,1,1,1,1,1,1,1,1,1,1,1,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,1,1,1,1,1,1,1,1,1,1,1,1},
					{1,1,1,1,1,1,1,1,1,1,1,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,1,1,1,1,1,1,1,1,1,1,1},
					{1,1,1,1,1,1,1,1,1,1,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,2,2,2,2,2,2,0,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,1,1,1,1,1,1,1,1,1,1},
					{1,1,1,1,1,1,1,1,1,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,1,1,1,1,1,1,1,1,1},
					{1,1,1,1,1,1,1,1,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,0,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,1,1,1,1,1,1,1,1},
					{1,1,1,1,1,1,1,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,0,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,1,1,1,1,1,1,1},
					{1,1,1,1,1,1,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,1,1,1,1,1,1},
					{1,1,1,1,1,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0,2,2,2,2,2,2,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,1,1,1,1,1},
					{1,1,1,1,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,1,1,1,1},
					{1,1,1,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,1,1,1},
					{1,1,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,2,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,1,1},
					{1,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,1},
					{8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,4,4,0,0,0,0,0,0,0,0,4,4,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,4,4,4,4,0,0,0,3,0,0,4,4,4,4,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				};	
				int bossPosX = pyramidX + ((size - 115) * finalDirection);
				int bossPosY = pyramidY + (size - 50);
				bossPosY -= (int)(.5f * _bossRoom.GetLength(0));
				bossPosX -= (int)(.5f * _bossRoom.GetLength(1));
				
				for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
				{
					for (int y = 0; y < _bossRoom.GetLength(0); y++) {
						for (int x = 0; x < _bossRoom.GetLength(1); x++) {
							int k = bossPosX + x;
							int l = bossPosY + y;
							if (WorldGen.InWorld(k, l, 30)) {
								Tile tile = Framing.GetTileSafely(k, l);
								switch (_bossRoom[y, x]) {
									case 0:
											if(confirmPlatforms == 0)
											tile.active(false);
										break;
									case 1:
											tile.type = (ushort)mod.TileType("PyramidSlabTile");
											tile.active(true);
											tile.slope(0);
											tile.halfBrick(false);
										break;
									case 2:
											tile.active(false);
											tile.wall = 10; //gold brick wall
										break;
									case 3:
											if(confirmPlatforms == 1)
											WorldGen.PlaceTile(k, l, (ushort)mod.TileType("SarcophagusTile")); //sarcophagus
										break;
									case 4:
											tile.type = 332; //gold coin
											tile.active(true);
										break;
									case 5:
											tile.active(false);
											tile.type = 332; //gold coin
											if(Main.rand.Next(3) == 0)
											{
											tile.active(true);
											}
											
										break;
									case 6:
										break;
									case 7:
										tile.type = (ushort)mod.TileType("ZeplineLureTile");
										tile.slope(0);
										tile.halfBrick(false);
										tile.active(true);
										break;
									case 8:
											tile.type = (ushort)mod.TileType("PyramidSlabTile");
											tile.active(true);
											tile.slope(3);
										break;
									case 9:
											tile.type = (ushort)mod.TileType("PyramidSlabTile");
											tile.active(true);
											tile.slope(4);
										break;
								}
							}
						}
					}
				}


				bool buildSword = false;
				bool buildPick = false;
				while (!buildPick)
				{
					int findTileY = Main.rand.Next(pyramidY + 40, pyramidY + (size - 70));
					int width = findTileY - pyramidY;
					int findTileX = pyramidX + Main.rand.Next(-width, width + 1);
					int structureWidth = 19;
					int structureHeight = 14;
					int structureRect = structureHeight * structureWidth;
					int count = 0;
					for(int i = 0; i < structureWidth; i++)
					{
						for (int j = 0; j < structureHeight; j++)
						{
							Tile tile = Framing.GetTileSafely(findTileX + i, findTileY + j);
							if(tile.active() && tile.wall == (ushort)mod.WallType("PyramidWallTile") && tile.type == (ushort)mod.TileType("PyramidSlabTile"))
							{
								count++;
							}
						}
					}
					if(count == structureRect)
					{
						buildPick = true;
						GenerateShrineRoom(findTileX, findTileY, 0);
					}
				}
				while (!buildSword)
				{
					int findTileY = Main.rand.Next(pyramidY + 40, pyramidY + (size - 70));
					int width = findTileY - pyramidY;
					int findTileX = pyramidX + Main.rand.Next(-width, width + 1);
					int structureWidth = 19;
					int structureHeight = 14;
					int structureRect = structureHeight * structureWidth;
					int count = 0;
					for (int i = 0; i < structureWidth; i++)
					{
						for (int j = 0; j < structureHeight; j++)
						{
							Tile tile = Framing.GetTileSafely(findTileX + i, findTileY + j);
							if (tile.active() && tile.wall == (ushort)mod.WallType("PyramidWallTile") && tile.type == (ushort)mod.TileType("PyramidSlabTile"))
							{
								count++;
							}
						}
					}
					if (count == structureRect)
					{
						buildSword = true;
						GenerateShrineRoom(findTileX, findTileY, 1);
					}
				}
				float counterRoom = 0;
				for (int findTileY = pyramidY + (size - 70); findTileY > pyramidY + 25; findTileY--)
				{
					counterRoom += 4.25f;
					int width = findTileY - pyramidY;
					if(counterRoom >= 20)
					{
						while (counterRoom > 0)
						{
							int findTileX = pyramidX + Main.rand.Next(-width, width + 1);
							Tile tile = Framing.GetTileSafely(findTileX, findTileY);
							if (tile.active() && tile.wall == (ushort)mod.WallType("PyramidWallTile") && tile.type == (ushort)mod.TileType("PyramidSlabTile"))
							{
								counterRoom--;
								bool canBeLeft = false;
								bool canBeRight = false;
								bool canBeUp = false;
								bool canBeDown = false;
								int tilesLeft = 0;
								int tilesRight = 0;
								int tilesUp = 0;
								int tilesDown = 0;
								int squareCount = 0;
								for (int checkLeft = 0; checkLeft < 80; checkLeft++)
								{
									Tile tileCheck = Framing.GetTileSafely(findTileX - checkLeft, findTileY);
									if (tileCheck.active() == false && tileCheck.wall == (ushort)mod.WallType("PyramidWallTile"))
									{
										tilesLeft = checkLeft;
										break;
									}
									if (tileCheck.active() == true && tileCheck.wall == (ushort)mod.WallType("PyramidWallTile") && tileCheck.type == 151)
									{
										tilesLeft = -1;
										break;
									}
								}
								for (int checkRight = 0; checkRight < 80; checkRight++)
								{
									Tile tileCheck = Framing.GetTileSafely(findTileX + checkRight, findTileY);
									if (tileCheck.active() == false && tileCheck.wall == (ushort)mod.WallType("PyramidWallTile"))
									{
										tilesRight = checkRight;
										break;
									}
									if (tileCheck.active() == true && tileCheck.wall == (ushort)mod.WallType("PyramidWallTile") && tileCheck.type == 151)
									{
										tilesRight = -1;
										break;
									}
								}
								for (int checkUp = 0; checkUp < 40; checkUp++)
								{
									Tile tileCheck = Framing.GetTileSafely(findTileX, findTileY - checkUp);
									if (tileCheck.active() == false && tileCheck.wall == (ushort)mod.WallType("PyramidWallTile"))
									{
										tilesUp = checkUp;
										break;
									}
									if (tileCheck.active() == true && tileCheck.wall == (ushort)mod.WallType("PyramidWallTile") && tileCheck.type == 151)
									{
										tilesUp = -1;
										break;
									}
								}
								for (int checkDown = 0; checkDown < 80; checkDown++)
								{
									Tile tileCheck = Framing.GetTileSafely(findTileX, findTileY + checkDown);
									if (tileCheck.active() == false && tileCheck.wall == (ushort)mod.WallType("PyramidWallTile"))
									{
										tilesDown = checkDown;
										break;
									}
									if (tileCheck.active() == true && tileCheck.wall == (ushort)mod.WallType("PyramidWallTile") && tileCheck.type == 151)
									{
										tilesDown = -1;
										break;
									}
								}
								for (int checkSquareX = -12; checkSquareX <= 12; checkSquareX++)
								{
									for (int checkSquareY = -12; checkSquareY <= 12; checkSquareY++)
									{
										Tile tileCheck = Framing.GetTileSafely(findTileX + checkSquareX, findTileY + checkSquareY);
										if (tileCheck.type == (ushort)mod.TileType("PyramidSlabTile") && tileCheck.active() && tileCheck.wall == (ushort)mod.WallType("PyramidWallTile"))
										{
											squareCount++;

										}
									}
								}

								if (squareCount >= 625 && tilesDown >= 0 && tilesUp >= 0 && tilesLeft >= 0 && tilesRight >= 0)
								{
									if (tilesRight > tilesDown && tilesRight > tilesUp && tilesRight > tilesLeft)
									{
										canBeRight = true;
									}
									if (tilesLeft > tilesDown && tilesLeft > tilesUp && tilesLeft > tilesRight)
									{
										canBeLeft = true;
									}
									if (tilesDown > tilesUp && tilesDown > tilesLeft && tilesDown > tilesRight)
									{
										canBeDown = true;
									}
									if ((int)(tilesUp * 1.2f) > tilesRight && (int)(tilesUp * 1.2f) > tilesDown && (int)(tilesUp * 1.2f) > tilesLeft) //check this last because of the priority modifications
									{
										canBeUp = true;
									}
								}
								if (canBeRight)
								{
									for (int checkSquareX = -12; checkSquareX <= 12; checkSquareX++)
									{
										for (int checkSquareY = -12; checkSquareY <= 12; checkSquareY++)
										{
											Tile tileCheck = Framing.GetTileSafely(findTileX + checkSquareX, findTileY + checkSquareY);
											if (checkSquareX == -12 || checkSquareX == 12 || checkSquareY == -12 || checkSquareY == 12)
											{
												tileCheck.type = 151; //sandstoneBrick
											}
										}
									}
									for (int checkSquareX = -10; checkSquareX <= 10; checkSquareX++)
									{
										for (int checkSquareY = -10; checkSquareY <= 10; checkSquareY++)
										{
											Tile tileCheck = Framing.GetTileSafely(findTileX + checkSquareX, findTileY + checkSquareY);
											if (checkSquareX == -10 || checkSquareX == 10 || checkSquareY == -10 || checkSquareY == 10)
											{
												tileCheck.type = 151; //sandstoneBrick
											}
										}
									}
									GeneratePyramidRoom(findTileX, findTileY, 1);
									break;
								}
								if (canBeLeft)
								{
									for (int checkSquareX = -12; checkSquareX <= 12; checkSquareX++)
									{
										for (int checkSquareY = -12; checkSquareY <= 12; checkSquareY++)
										{
											Tile tileCheck = Framing.GetTileSafely(findTileX + checkSquareX, findTileY + checkSquareY);
											if (checkSquareX == -12 || checkSquareX == 12 || checkSquareY == -12 || checkSquareY == 12)
											{
												tileCheck.type = 151; //sandstoneBrick
											}
										}
									}
									for (int checkSquareX = -10; checkSquareX <= 10; checkSquareX++)
									{
										for (int checkSquareY = -10; checkSquareY <= 10; checkSquareY++)
										{
											Tile tileCheck = Framing.GetTileSafely(findTileX + checkSquareX, findTileY + checkSquareY);
											if (checkSquareX == -10 || checkSquareX == 10 || checkSquareY == -10 || checkSquareY == 10)
											{
												tileCheck.type = 151; //sandstoneBrick
											}
										}
									}
									GeneratePyramidRoom(findTileX, findTileY, 0);
									break;
								}
								if (canBeDown)
								{
									for (int checkSquareX = -12; checkSquareX <= 12; checkSquareX++)
									{
										for (int checkSquareY = -12; checkSquareY <= 12; checkSquareY++)
										{
											Tile tileCheck = Framing.GetTileSafely(findTileX + checkSquareX, findTileY + checkSquareY);
											if (checkSquareX == -12 || checkSquareX == 12 || checkSquareY == -12 || checkSquareY == 12)
											{
												tileCheck.type = 151; //sandstoneBrick
											}
										}
									}
									for (int checkSquareX = -10; checkSquareX <= 10; checkSquareX++)
									{
										for (int checkSquareY = -10; checkSquareY <= 10; checkSquareY++)
										{
											Tile tileCheck = Framing.GetTileSafely(findTileX + checkSquareX, findTileY + checkSquareY);
											if (checkSquareX == -10 || checkSquareX == 10 || checkSquareY == -10 || checkSquareY == 10)
											{
												tileCheck.type = 151; //sandstoneBrick
											}
										}
									}
									GeneratePyramidRoom(findTileX, findTileY, 3);
									break;
								}
								if (canBeUp)
								{
									for (int checkSquareX = -12; checkSquareX <= 12; checkSquareX++)
									{
										for (int checkSquareY = -12; checkSquareY <= 12; checkSquareY++)
										{
											Tile tileCheck = Framing.GetTileSafely(findTileX + checkSquareX, findTileY + checkSquareY);
											if (checkSquareX == -12 || checkSquareX == 12 || checkSquareY == -12 || checkSquareY == 12)
											{
												tileCheck.type = 151; //sandstoneBrick
											}
										}
									}
									for (int checkSquareX = -10; checkSquareX <= 10; checkSquareX++)
									{
										for (int checkSquareY = -10; checkSquareY <= 10; checkSquareY++)
										{
											Tile tileCheck = Framing.GetTileSafely(findTileX + checkSquareX, findTileY + checkSquareY);
											if (checkSquareX == -10 || checkSquareX == 10 || checkSquareY == -10 || checkSquareY == 10)
											{
												tileCheck.type = 151; //sandstoneBrick
											}
										}
									}
									GeneratePyramidRoom(findTileX, findTileY, 2);
									break;
								}
							}
						}
					}
				}
				int[,] _zepline = {
					{2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
					{3,3,3,3,3,3,3,3,3,3,3,2,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},	
					{2,2,2,2,2,2,2,2,2,2,3,2,3,3,3,2,2,2,2,2,2,2,2,2,3,3,3,3,3,3,3,3,3,2,2,2,2,2,2,2,2},	
					{0,0,0,0,0,0,0,3,3,2,3,2,3,3,3,3,3,2,2,2,2,2,2,2,2,2,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2},	
					{0,0,0,0,0,0,0,0,3,2,3,2,2,2,2,2,2,2,2,2,2,2,2,2,9,9,5,0,0,0,5,9,9,2,2,2,3,2,2,2,2},	
					{0,0,0,0,0,0,0,0,0,2,3,3,3,3,3,3,3,2,3,2,3,2,3,2,4,4,4,0,0,0,4,4,4,2,2,3,3,3,2,2,2},	
					{0,0,0,0,0,0,0,0,0,2,3,0,0,0,0,0,3,2,3,0,3,0,3,2,2,2,3,0,0,0,3,2,2,2,3,3,0,3,3,2,2},	
					{0,0,0,0,0,0,0,0,0,0,3,0,5,9,5,0,3,2,0,0,0,0,0,0,2,2,3,0,0,0,3,2,2,3,3,0,0,0,3,3,2},	
					{9,6,0,0,0,0,0,0,0,0,3,0,4,4,4,0,3,0,0,0,0,0,0,0,0,2,2,0,0,0,2,2,3,3,0,0,0,0,0,3,3},	
					{3,3,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
					{2,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,6,9,9,9,9,9,9,9,3},
					{9,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,3,9,9,9,9,9,3,3},	
					{3,3,3,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,3,9,9,9,3,3,2},	
					{7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,3,9,3,3,2,2},	
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,3,9,3,2,2,2},	
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,6,6,9,9,2,2,9,2,2,2,2},	
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,6,9,6,6,0,1,3,3,3,3,2,2,9,2,2,2,2},	
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,3,3,8,0,0,1,3,3,3,2,2,9,2,2,2,2},	
					{9,6,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,9,2,2,2,2},	
					{3,3,3,3,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,9,9,9,9,9,9,9,9,9,6,2,2,2,2,2,9,2,2,2,2},	
					{2,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,6,9,9,9,9,9,6,3,3,2,2,2,2,2,9,2,2,2,2},	
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,9,3,3,3,3,3,2,2,2,2,2,9,2,2,2,2},	
					{0,0,0,0,0,0,6,6,9,9,9,9,9,9,9,9,9,9,9,9,2,2,2,3,3,9,3,3,2,2,2,2,2,2,2,2,9,2,2,2,2},	
					{0,0,0,0,0,0,3,3,9,9,9,9,9,9,9,9,9,9,9,9,2,2,2,2,2,9,2,2,2,2,2,2,2,2,2,2,9,2,2,2,2},	
					{0,0,0,0,0,0,3,3,9,9,9,9,9,9,9,9,9,9,9,3,3,2,2,2,2,9,9,9,3,3,3,3,3,3,9,9,9,2,2,2,2},	
					{9,9,9,9,9,9,9,3,9,9,9,9,9,9,9,9,9,9,9,3,3,2,2,2,2,3,9,9,9,9,9,9,9,9,9,9,9,2,2,2,2},	
					{9,9,9,9,9,9,9,3,3,9,9,9,9,9,9,9,9,9,3,3,3,2,2,2,2,3,3,9,9,9,9,9,9,9,9,9,9,2,2,2,2},	
					{9,9,9,9,9,9,9,3,3,3,9,9,9,9,9,9,9,3,3,3,2,2,3,3,3,3,3,3,3,9,9,9,9,9,9,9,3,2,2,2,2},	
					{9,9,9,9,9,9,9,2,3,3,3,3,9,9,9,3,3,3,3,2,2,2,2,3,3,3,3,3,9,9,9,9,9,9,9,3,3,2,2,2,2},	
					{9,9,9,9,9,9,9,2,2,3,3,3,3,9,3,3,3,3,2,2,2,2,2,2,3,3,3,9,9,9,9,9,9,9,3,3,3,2,2,2,2},	
					{9,9,9,9,9,9,2,2,2,2,2,2,2,9,2,2,2,2,2,2,2,2,2,2,2,3,9,9,9,9,9,9,9,3,3,3,3,2,2,2,2},	
					{9,9,9,9,9,2,2,2,2,2,2,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,3,3,3,3,3,2,2,2,2},	
					{9,9,2,2,2,2,2,2,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,3,3,3,3,3,3,2,2,2,2},	
					{9,2,2,2,2,2,9,9,9,9,9,9,9,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},	
					{9,9,9,9,9,9,9,9,9,9,9,9,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},	
					{9,9,9,9,9,9,9,9,9,9,9,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},	
					{9,9,9,9,9,9,9,9,9,9,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},	
					{3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},	
					{2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},		
				};	
				int[,] _zeplineWalls = {
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0},
					{0,0,0,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0},
					{0,0,0,1,1,1,1,1,1,1,1,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,2,2,2,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,1,1,1,1,1,1,1,1,0,2,2,2,0,1,1,0,0,0,0,0,0,0,0,0,2,2,2,0,0,0,0,0,0,0,0,0,0,0},
					{2,2,2,0,1,1,1,1,1,1,1,0,2,2,2,0,1,1,0,0,0,0,0,0,0,0,0,2,2,2,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,2,0,1,1,1,1,1,1,1,0,2,2,2,0,1,1,0,0,0,0,0,0,0,0,0,2,2,2,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,2,0,1,1,1,1,1,1,1,0,2,2,2,0,1,1,0,0,0,0,0,0,0,0,0,2,2,2,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,2,0,1,1,1,1,1,1,1,0,2,2,2,0,1,1,0,0,0,0,0,0,0,0,0,2,2,2,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,2,0,1,1,1,1,1,1,1,0,2,2,2,0,1,1,0,0,0,0,0,0,0,0,0,2,2,2,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,2,0,1,1,1,1,1,1,1,0,2,2,2,0,1,1,0,0,0,0,0,0,0,0,0,2,2,2,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,2,0,1,1,1,1,1,1,1,0,2,2,2,0,1,1,1,0,0,0,0,0,0,0,0,2,2,2,0,0,0,0,0,0,0,0,0,0,0},
					{2,2,2,2,0,1,1,1,1,1,1,0,2,2,2,0,1,1,1,0,0,0,0,0,0,0,0,2,2,2,0,2,0,0,0,0,0,0,0,0,0},
					{0,0,0,2,0,1,1,1,1,1,1,0,2,2,2,0,1,1,1,0,0,0,0,0,0,0,0,2,2,2,0,2,0,0,0,0,0,0,0,0,0},
					{0,0,0,2,0,1,1,1,1,1,1,0,2,2,2,0,1,1,1,0,0,0,0,0,0,0,0,2,2,2,0,2,0,0,0,0,0,0,0,0,0},
					{0,0,0,2,0,1,1,1,1,1,1,0,2,2,2,0,1,1,1,0,0,0,0,0,0,0,0,2,2,2,0,2,0,0,0,0,0,0,0,0,0},
					{0,0,0,2,0,1,1,1,1,1,1,0,2,2,2,0,1,1,1,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,0,0,0,0,0,0,0},
					{0,0,0,2,0,1,1,1,1,1,1,0,2,2,2,0,1,1,1,0,0,0,0,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,2,0,1,1,1,1,1,1,0,2,2,2,0,1,1,1,0,0,0,0,2,0,0,0,2,2,2,0,0,0,0,0,0,0,0,0,0,0},
					{2,2,2,2,2,0,1,1,1,1,1,0,2,2,2,0,1,1,1,1,0,0,0,2,0,0,0,2,2,2,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,2,0,1,1,1,1,1,0,2,2,2,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,2,0,1,1,1,1,1,0,2,2,2,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,2,0,1,1,1,1,1,0,2,2,2,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
					{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				};
						
				int pyramidPosX = pyramidX;
				int pyramidPosY = pyramidY + (size - 40);
				pyramidPosY -= (int)(.5f * _zepline.GetLength(0)) - 10;
				
				for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
				{
					for (int y = 0; y < _zepline.GetLength(0); y++) {
						for (int x = 0; x < _zepline.GetLength(1); x++) {
							int k = pyramidPosX + x;
							int l = pyramidPosY + y;
							int k2 = pyramidPosX - x;
							if (WorldGen.InWorld(k, l, 30)) {
								Tile tile = Framing.GetTileSafely(k, l);
								Tile tile2 = Framing.GetTileSafely(k2, l);
								switch (_zepline[y, x]) {
									case 0:
										if(tile.type == (ushort)mod.TileType("PyramidSlabTile"))
											tile.active(false);
										
										if(tile2.type == (ushort)mod.TileType("PyramidSlabTile"))
											tile2.active(false);
										break;
									case 1:
										tile.type = 274; 
										tile.active(true);
										tile.slope(4);
										tile2.type = 274;  
										tile2.active(true);
										tile2.slope(3);
										break;
									case 2:
										tile.type = (ushort)mod.TileType("PyramidSlabTile");
										tile.active(true);
										tile.slope(0);
										tile.halfBrick(false);
										tile2.type = (ushort)mod.TileType("PyramidSlabTile");
										tile2.active(true);
										tile2.slope(0);
										tile2.halfBrick(false);
										break;
									case 3:
										tile.type = 274; //sandstone slab
										tile.active(true);
										tile.slope(0);
										tile.halfBrick(false);
										tile2.type = 274; 
										tile2.active(true);
										tile2.slope(0);
										tile2.halfBrick(false);
										break;
									case 4:
										tile.type = 326; //waterfall
										tile.active(true);
										tile.slope(0);
										tile.halfBrick(false);
										
										tile2.type = 326; //waterfall
										tile2.active(true);
										tile2.slope(0);
										tile2.halfBrick(false);
										break;
									case 5:
										tile.type = 326; //waterfall
										tile.active(true);
										tile.halfBrick(true);
										tile2.type = 326; //waterfall
										tile2.active(true);
										tile2.halfBrick(true);
										break;
									case 6:
										tile.type = 274;
										tile.active(true);
										tile.halfBrick(true);
										tile2.type = 274;
										tile2.active(true);
										tile2.halfBrick(true);
										break;
									case 7:
										tile.type = (ushort)mod.TileType("ZeplineLureTile");
										tile.slope(0);
										tile.halfBrick(false);
										tile.active(true);
										break;
									case 8:
										tile.type = 274; 
										tile.active(true);
										tile.slope(3);
										tile2.type = 274;  
										tile2.active(true);
										tile2.slope(4);
										break;
									case 9:
										tile.liquidType(0);
										tile.liquid = 255;
										tile.active(false);
										WorldGen.SquareTileFrame(k, l, false);
										
										tile2.liquidType(0);
										tile2.liquid = 255;
										tile2.active(false);
										WorldGen.SquareTileFrame(k2, l, false);
									break;
								}
							}
						}
					}
				}
				for (int y = 0; y < _zeplineWalls.GetLength(0); y++) {
					for (int x = 0; x < _zeplineWalls.GetLength(1); x++) {
						int k = pyramidPosX + x;
						int l = pyramidPosY + y;
						int k2 = pyramidPosX - x;
						if (WorldGen.InWorld(k, l, 30)) {
							Tile tile = Framing.GetTileSafely(k, l);
							Tile tile2 = Framing.GetTileSafely(k2, l);
							switch (_zeplineWalls[y, x]) {
								case 0:
									tile.wall = (ushort)mod.WallType("PyramidWallTile");  //sandbrick
									tile2.wall = (ushort)mod.WallType("PyramidWallTile");
									break;
								case 1:
									tile.wall = 226; //sandfall 
									tile2.wall = 226;
									break;
								case 2:
									tile.wall = 136; //waterfall
									tile2.wall = 136;
									break;
								case 3:
									break;
								case 4:
									break;
							}
						}
					}
				}

				//rebuild outside of pyramid
				for (int pyramidLevel = 0; pyramidLevel < size; pyramidLevel++)
				{
					for (int h = -pyramidLevel; h <= pyramidLevel; h++)
					{
						Tile tile = Framing.GetTileSafely(pyramidX + h, pyramidY + pyramidLevel);
						if(pyramidLevel >= 20 && h < -pyramidLevel + 2 && h > pyramidLevel - 2)
							if (tile.type != TileID.BlueDungeonBrick && tile.type != TileID.GreenDungeonBrick && tile.type != TileID.PinkDungeonBrick && tile.wall != 7 && tile.wall != 8 && tile.wall != 9 && tile.wall != 94 && tile.wall != 95 && tile.wall != 96 && tile.wall != 97 && tile.wall != 98 && tile.wall != 99) //check for not dungeon!
							{
								tile.type = (ushort)mod.TileType("PyramidSlabTile");
								tile.slope(0);
								tile.halfBrick(false);
								tile.liquidType(0);
								tile.liquid = 0;
								tile.active(true);
							}
					}
				}

				if(overgrownX != -1 && overgrownY != -1)
                {
					SOTSWorldgenHelper.GenerateAcediaRoom(overgrownX, overgrownY, mod, finalDirection);
                }

				for (int findTileY = pyramidY + (size - 30); findTileY > pyramidY + 30; findTileY--)
				{
					int width = findTileY - pyramidY;
					for(int t = 0; t < 6; t++)
					{
						int findTileX = pyramidX + Main.rand.Next(-width, width + 1);
						Tile tile = Framing.GetTileSafely(findTileX, findTileY);

						for (int built = 0; built < 2; built++)
						{
							int tileX = findTileX;
							if (tile.wall == (ushort)mod.WallType("PyramidWallTile") && (!tile.active() || Main.rand.Next(size / 3) == 0))
							{
								Tile tile2 = Framing.GetTileSafely(findTileX, findTileY - 3);
								Tile tile3 = Framing.GetTileSafely(findTileX, findTileY + 3);
								if (tile2.active() && tile2.type == (ushort)mod.TileType("PyramidSlabTile") && tile2.wall == (ushort)mod.WallType("PyramidWallTile"))
								{
									if (tile3.active() && tile3.type == (ushort)mod.TileType("PyramidSlabTile") && tile3.wall == (ushort)mod.WallType("PyramidWallTile"))
									{
										InfectionTester.Generate(new Vector2(findTileX * 16, findTileY * 16), mod);
										findTileX += Main.rand.Next(-20, 21);
									}
									else break;
								}
								else break;
							}
							else break;
						}
					}
				}

				for (int findTileX = 100; findTileX < Main.maxTilesX - 100; findTileX++)
				{
					for(int findTileY = Main.maxTilesY - 100; findTileY > 100; findTileY--)
					{
						Tile tile = Framing.GetTileSafely(findTileX, findTileY);
						Tile tileU = Framing.GetTileSafely(findTileX, findTileY -1);
						Tile tileLU = Framing.GetTileSafely(findTileX - 1, findTileY-1);
						Tile tileU2 = Framing.GetTileSafely(findTileX, findTileY -2);
						Tile tileLU2 = Framing.GetTileSafely(findTileX - 1, findTileY-2);
						Tile tileU3 = Framing.GetTileSafely(findTileX, findTileY -3);
						Tile tileLU3 = Framing.GetTileSafely(findTileX - 1, findTileY-3);
						if(tile.type == (ushort)mod.TileType("PyramidSlabTile") && !tileLU.active() && !tileLU2.active() && !tileU.active() && !tileU2.active())
						{
							if(Main.rand.Next(5) == 0)
							{
								WorldGen.PlaceTile(findTileX, findTileY - 1, 28, true, true, -1, 3); //pots
							}
							else if(Main.rand.Next(size / 2) == 0)
							{
								WorldGen.PlaceTile(findTileX, findTileY - 1, (ushort)mod.TileType("CrystalStatue")); //life crystal
							}
							else if(Main.rand.Next(size / 2) == 0)
							{
								WorldGen.PlaceTile(findTileX, findTileY - 1, (ushort)mod.TileType("ManaStatue")); //mana crystal
							}
							else if(Main.rand.Next(size / 3) == 0)
							{
								WorldGen.PlaceTile(findTileX, findTileY - 1, (ushort)mod.TileType("PyramidChestTile")); //Chests
							}
							else if(Main.rand.Next((int)(size / 3.5f)) == 0)
							{
								GenerateCrate(findTileX, findTileY - 1);
							}
							else if(!tileU3.active() && !tileLU3.active() && Main.rand.Next(size / 2) == 0)
							{
								WorldGen.PlaceTile(findTileX, findTileY - 1, TileID.Statues, true, true, -1, Main.rand.Next(71)); //random statue
							}
						}
						if(tile.wall == (ushort)mod.WallType("PyramidWallTile") && Main.rand.Next(500) == 0)
						{
							int radius7 = 3;
							for (int x = -radius7; x <= radius7; x++)
							{
								for (int y = -radius7; y <= radius7; y++)
								{
									int xPosition6 = findTileX + x;
									int yPosition6 = findTileY + y; 
					 
									if (Math.Sqrt(x * x + y * y) <= radius7 + 0.5)   
									{
										Tile tileRad = Framing.GetTileSafely(xPosition6, yPosition6);
										if(!tileRad.active())
										{
											tileRad.type = 51; //cobweb
											tileRad.active(true);
										}
									}
								}
							}
						}
					}
				}
			}));
			
		}
		public override void TileCountsAvailable(int[] tileCounts)
		{
			planetarium = tileCounts[mod.TileType("DullPlatingTile")] + tileCounts[mod.TileType("AvaritianPlatingTile")];  
			//geodeBiome = tileCounts[mod.TileType("GeodeBlock")];
			pyramidBiome = tileCounts[mod.TileType("SarcophagusTile")];  
		}
		public override void PostWorldGen()
		{
			int xPosition2 = Main.maxTilesX/2;
			int yPosition2 = Main.maxTilesY - 55;
            int radius = 6;    	
			for (int x = -radius; x <= radius; x++)
			{
				for (int y = -radius; y <= radius; y++)
				{
					int xPosition = (int)(x + xPosition2);
					int yPosition = (int)(y + yPosition2);
	 
					if (Math.Sqrt(x * x + y * y) <= radius + 0.5) 	
					{
						WorldGen.KillTile(xPosition, yPosition, false, false, false); 	
						WorldGen.PlaceTile(xPosition, yPosition, mod.TileType("IceCreamBrickTile"));
					}
				}
			}
            WorldGen.KillTile(xPosition2, yPosition2, false, false, false);	  
			WorldGen.PlaceTile(xPosition2, yPosition2, mod.TileType("IceCreamBottleTile"));

			// Iterate chests
			List<int> starItemPool2 = new List<int>() { ModContent.ItemType<SkywareBattery>(), ModContent.ItemType<Poyoyo>(), ModContent.ItemType<SupernovaHammer>(), ModContent.ItemType<StarshotCrossbow>(), ModContent.ItemType<LashesOfLightning>(), ModContent.ItemType<Starbelt>(), ModContent.ItemType<TwilightAssassinsCirclet>() };
			List<int> lightItemPool2 = new List<int>() { ModContent.ItemType<HardlightQuiver>(), ModContent.ItemType<CodeCorrupter>(), ModContent.ItemType<PlatformGenerator>(), ModContent.ItemType<Calculator>(), ModContent.ItemType<TwilightAssassinsLeggings>(), ModContent.ItemType<TwilightFishingPole>(), ModContent.ItemType<ChainedPlasma>() };
			List<int> fireItemPool2 = new List<int>() { ModContent.ItemType<BlinkPack>(), ModContent.ItemType<FlareDetonator>(), ModContent.ItemType<VibrancyModule>(), ModContent.ItemType<CataclysmMusketPouch>(), ModContent.ItemType<TerminatorAcorns>(), ModContent.ItemType<TwilightAssassinsChestplate>(), ModContent.ItemType<InfernoHook>() };

			List<int> starItemPool = new List<int>() { ModContent.ItemType<SkywareBattery>(), ModContent.ItemType<Poyoyo>(), ModContent.ItemType<SupernovaHammer>(), ModContent.ItemType<StarshotCrossbow>(),ModContent.ItemType<LashesOfLightning>(), ModContent.ItemType<Starbelt>(), ModContent.ItemType<TwilightAssassinsCirclet>() };
			List<int> lightItemPool = new List<int>() { ModContent.ItemType<HardlightQuiver>(), ModContent.ItemType<CodeCorrupter>(), ModContent.ItemType<PlatformGenerator>(), ModContent.ItemType<Calculator>(), ModContent.ItemType<TwilightAssassinsLeggings>(), ModContent.ItemType<TwilightFishingPole>(), ModContent.ItemType<ChainedPlasma>() };
			List<int> fireItemPool = new List<int>() { ModContent.ItemType<BlinkPack>(), ModContent.ItemType<FlareDetonator>(), ModContent.ItemType<VibrancyModule>(), ModContent.ItemType<CataclysmMusketPouch>(), ModContent.ItemType<TerminatorAcorns>(), ModContent.ItemType<TwilightAssassinsChestplate>(), ModContent.ItemType<InfernoHook>() };
			foreach (Chest chest in Main.chest.Where(c => c != null))
			{
				// Get a chest
				var tile = Main.tile[chest.x, chest.y]; // the chest tile 
				if (tile.type == mod.TileType("LockedStrangeChest") || tile.type == mod.TileType("LockedSkywareChest") || tile.type == mod.TileType("LockedMeteoriteChest"))
				{
					int type = tile.type == mod.TileType("LockedStrangeChest") ? 0 : tile.type == mod.TileType("LockedSkywareChest") ? 1 : 2;
					int slot = 39;
					for (int i = 0; i < 39; i++)
					{
						if (chest.item[i].type == 0 && i < slot)
						{
							slot = i;
						}
					}
					int firstType = 0;
					if (type == 0)
					{
						firstType = lightItemPool2[Main.rand.Next(lightItemPool2.Count)];
						if (lightItemPool.Count > 0)
						{
							int rand = Main.rand.Next(lightItemPool.Count);
							firstType = lightItemPool[rand];
							lightItemPool.RemoveAt(rand);
						}
					}
					else if (type == 1)
					{
						firstType = starItemPool2[Main.rand.Next(starItemPool2.Count)];
						if (starItemPool.Count > 0)
						{
							int rand = Main.rand.Next(starItemPool.Count);
							firstType = starItemPool[rand];
							starItemPool.RemoveAt(rand);
						}
					}
					else
					{
						firstType = fireItemPool2[Main.rand.Next(fireItemPool2.Count)];
						if (fireItemPool.Count > 0)
						{
							int rand = Main.rand.Next(fireItemPool.Count);
							firstType = fireItemPool[rand];
							fireItemPool.RemoveAt(rand);
						}
					}
					chest.item[slot].SetDefaults(firstType); //add primary item to chest loot
					slot++;

					if (!Main.rand.NextBool(3)) //Adds ores and shards to chest
					{
						int amt = Main.rand.Next(5, 9); //5 to 8
						int rand = Main.rand.Next(9);
						int secondType = ModContent.ItemType<HardlightAlloy>();
						if (type == 0 || rand == 0)
							secondType = ModContent.ItemType<HardlightAlloy>();
						if (type == 1 || rand == 1)
							secondType = ModContent.ItemType<StarlightAlloy>();
						if (type == 2 || rand == 2)
							secondType = ModContent.ItemType<OtherworldlyAlloy>();
						chest.item[slot].SetDefaults(secondType);
						chest.item[slot].stack = amt;
						slot++;
					}
					else
					{
						int secondType = ModContent.ItemType<TwilightShard>();
						int amt = Main.rand.Next(6, 10); //6 to 9
						chest.item[slot].SetDefaults(secondType);
						chest.item[slot].stack = amt;
						slot++;
					}

					if (!Main.rand.NextBool(3)) //Adds ammo or stars to chest
					{
						int amt = Main.rand.Next(66, 101); //66 to 100
						int[] ammoItems = new int[] { ItemID.JestersArrow, ItemID.HellfireArrow, ItemID.MeteorShot, ItemID.FallenStar, ModContent.ItemType<ExplosiveKnife>() };
						int rand = Main.rand.Next(ammoItems.Length);
						int thirdType = ammoItems[rand];
						if (thirdType == ItemID.FallenStar) //cut quantity if stars 13 - 20
						{
							amt /= 5;
						}
						chest.item[slot].SetDefaults(thirdType);
						chest.item[slot].stack = amt;
						slot++;
					}
					if(Main.rand.Next(5) <= 2) //adds healing
                    {
						int fourthType = ItemID.RestorationPotion;
						int amt = Main.rand.Next(10, 15); //10 to 14
						chest.item[slot].SetDefaults(fourthType);
						chest.item[slot].stack = amt;
						slot++;
					}
					if (!Main.rand.NextBool(5)) //adds first potions 80%
					{
						int amt = Main.rand.Next(2) + 1; //1 to 2
						int[] potions1 = new int[] { ModContent.ItemType<AssassinationPotion>(), ModContent.ItemType<BrittlePotion>(), ModContent.ItemType<RoughskinPotion>(), ModContent.ItemType<SoulAccessPotion>(), ModContent.ItemType<VibePotion>(), ItemID.LifeforcePotion, ItemID.HeartreachPotion, ItemID.ManaRegenerationPotion, ItemID.MagicPowerPotion, ItemID.AmmoReservationPotion, ItemID.InfernoPotion};
						int rand = Main.rand.Next(potions1.Length);
						int fifthType = potions1[rand];
						chest.item[slot].SetDefaults(fifthType);
						chest.item[slot].stack = amt;
						slot++;
					}
					if (Main.rand.NextBool(5)) //adds second potions 20%
					{
						int amt = 1;
						int[] potions2 = new int[] { ModContent.ItemType<BlightfulTonic>(), ModContent.ItemType<GlacialTonic>(), ModContent.ItemType<SeismicTonic>(), ModContent.ItemType<StarlightTonic>(), ModContent.ItemType<DoubleVisionPotion>() };
						int rand = Main.rand.Next(potions2.Length);
						int fifthType = potions2[rand];
						chest.item[slot].SetDefaults(fifthType);
						chest.item[slot].stack = amt;
						slot++;
					}

					if (!Main.rand.NextBool(3)) //Adds torches
					{
						int amt = Main.rand.Next(15, 30); //15 to 29
						int sixthType = ItemID.Torch;
						chest.item[slot].SetDefaults(sixthType);
						chest.item[slot].stack = amt;
						slot++;
					}
					if (Main.rand.NextBool(2))
					{
						chest.item[slot].SetDefaults(ItemID.GoldCoin);
						chest.item[slot].stack = Main.rand.Next(3) + 4; // 4 to 6
						slot++;
					}
					if (Main.rand.NextBool(5)) //20%
					{
						int amt = 1; 
						int seventhType = ModContent.ItemType<StrangeKey>();
						if (type == 0)
						{
							if (Main.rand.NextBool(7))
								seventhType = ModContent.ItemType<StrangeKey>();
							else if(Main.rand.NextBool(2))
								seventhType = ModContent.ItemType<SkywareKey>();
							else
								seventhType = ModContent.ItemType<MeteoriteKey>();

						}
						if (type == 1)
						{
							if (Main.rand.NextBool(7))
								seventhType = ModContent.ItemType<SkywareKey>();
							else if (Main.rand.NextBool(2))
								seventhType = ModContent.ItemType<StrangeKey>();
							else
								seventhType = ModContent.ItemType<MeteoriteKey>();
						}
						if (type == 2)
						{
							if (Main.rand.NextBool(7))
								seventhType = ModContent.ItemType<MeteoriteKey>();
							else if (Main.rand.NextBool(2))
								seventhType = ModContent.ItemType<SkywareKey>();
							else
								seventhType = ModContent.ItemType<StrangeKey>();
						}
						chest.item[slot].SetDefaults(seventhType);
						chest.item[slot].stack = amt;
						slot++;
					}
				}
				if (tile.type == mod.TileType("StrangeChestTile"))
				{
					int slot = 0;
					chest.item[slot].SetDefaults(mod.ItemType("WorldgenScanner"));
					slot++;
				}
				if (tile.type == mod.TileType("PyramidChestTile"))
				{
					int slot = 39;
					for(int i = 0; i < 39; i++)
					{
						if(chest.item[i].type == 0 && i < slot)
						{
							slot = i;
						}
					}
				
					int rand = Main.rand.Next(12);
					if(rand == 0)
					{
						chest.item[slot].SetDefaults(mod.ItemType("Aten"));
						slot++;
					}
					if(rand == 1)
					{
						chest.item[slot].SetDefaults(mod.ItemType("EmeraldBracelet"));
						slot++;
					}
					if(rand == 2)
					{
						chest.item[slot].SetDefaults(mod.ItemType("ImperialPike"));
						slot++;
					}
					if(rand == 3)
					{
						chest.item[slot].SetDefaults(mod.ItemType("PharaohsCane"));
						slot++;
					}
					if(rand == 4)
					{
						chest.item[slot].SetDefaults(mod.ItemType("PitatiLongbow"));
						slot++;
					}
					if(rand == 5)
					{
						chest.item[slot].SetDefaults(mod.ItemType("RoyalMagnum"));
						slot++;
					}
					if(rand == 6)
					{
						chest.item[slot].SetDefaults(mod.ItemType("SandstoneEdge"));
						slot++;
					}
					if(rand == 7)
					{
						chest.item[slot].SetDefaults(mod.ItemType("SandstoneWarhammer"));
						slot++;
					}
					if(rand == 8)
					{
						chest.item[slot].SetDefaults(mod.ItemType("ShiftingSands"));
						slot++;
					}
					if(rand == 9)
					{
						chest.item[slot].SetDefaults(mod.ItemType("SunlightAmulet"));
						slot++;
					}
					if(rand == 10)
					{
						chest.item[slot].SetDefaults(ItemID.FlyingCarpet);
						slot++;
					}
					if(rand == 11)
					{
						chest.item[slot].SetDefaults(ItemID.SandstorminaBottle);
						slot++;
					}
					
					int second = Main.rand.Next(10);
					if(second == 0)
					{
						chest.item[slot].SetDefaults(848);
						slot++;
						chest.item[slot].SetDefaults(866);
						slot++;
					}
					if(second == 1)
					{
						chest.item[slot].SetDefaults(mod.ItemType("AnubisHat"));
						slot++;
					}
					if(second > 1)
					{
						chest.item[slot].SetDefaults(mod.ItemType("JuryRiggedDrill"));
						chest.item[slot].stack = Main.rand.Next(35) + 11;
						slot++;
					}
					
					int third = Main.rand.Next(12);
					if(third == 0)
					{
						chest.item[slot].SetDefaults(ItemID.MiningPotion);
						chest.item[slot].stack = Main.rand.Next(2) + 1;
						slot++;
					}
					if(third == 1)
					{
						chest.item[slot].SetDefaults(ItemID.SpelunkerPotion);
						chest.item[slot].stack = Main.rand.Next(2) + 1;
						slot++;
					}
					if(third == 2)
					{
						chest.item[slot].SetDefaults(ItemID.BuilderPotion);
						chest.item[slot].stack = Main.rand.Next(2) + 1;
						slot++;
					}
					if(third == 3)
					{
						chest.item[slot].SetDefaults(ItemID.ShinePotion);
						chest.item[slot].stack = Main.rand.Next(2) + 1;
						slot++;
					}
					if(third == 4)
					{
						chest.item[slot].SetDefaults(ItemID.NightOwlPotion);
						chest.item[slot].stack = Main.rand.Next(2) + 1;
						slot++;
					}
					if(third == 5)
					{
						chest.item[slot].SetDefaults(ItemID.ArcheryPotion);
						chest.item[slot].stack = Main.rand.Next(2) + 1;
						slot++;
					}
					if(third == 6)
					{
						chest.item[slot].SetDefaults(ItemID.EndurancePotion);
						chest.item[slot].stack = Main.rand.Next(2) + 1;
						slot++;
					}
					if(third == 7)
					{
						chest.item[slot].SetDefaults(ItemID.SummoningPotion);
						chest.item[slot].stack = Main.rand.Next(2) + 1;
						slot++;
					}
					
					
					int fourth = Main.rand.Next(12);
					if(fourth == 0)
					{
						chest.item[slot].SetDefaults(ItemID.WrathPotion);
						chest.item[slot].stack = Main.rand.Next(2) + 1;
						slot++;
					}
					if(fourth == 1)
					{
						chest.item[slot].SetDefaults(ItemID.HeartreachPotion);
						chest.item[slot].stack = Main.rand.Next(2) + 1;
						slot++;
					}
					if(fourth == 2)
					{
						chest.item[slot].SetDefaults(ItemID.RagePotion);
						chest.item[slot].stack = Main.rand.Next(2) + 1;
						slot++;
					}
					if(fourth == 3)
					{
						chest.item[slot].SetDefaults(ItemID.TitanPotion);
						chest.item[slot].stack = Main.rand.Next(2) + 1;
						slot++;
					}
					if(fourth == 4)
					{
						chest.item[slot].SetDefaults(ItemID.TeleportationPotion);
						chest.item[slot].stack = Main.rand.Next(2) + 1;
						slot++;
					}
					
					int fifth = Main.rand.Next(8);
					if(fifth == 0)
					{
						chest.item[slot].SetDefaults(ItemID.GoldBar);
						chest.item[slot].stack = Main.rand.Next(9) + 5;
						slot++;
					}
					if(fifth == 1)
					{
						chest.item[slot].SetDefaults(ItemID.PlatinumBar);
						chest.item[slot].stack = Main.rand.Next(9) + 5;
						slot++;
					}
					if(fifth == 2)
					{
						chest.item[slot].SetDefaults(ItemID.CrimtaneBar);
						chest.item[slot].stack = Main.rand.Next(9) + 5;
						slot++;
					}
					if(fifth == 3)
					{
						chest.item[slot].SetDefaults(ItemID.DemoniteBar);
						chest.item[slot].stack = Main.rand.Next(9) + 5;
						slot++;
					}
					
					int thirdLast = Main.rand.Next(4);
					if(thirdLast == 0)
					{
						chest.item[slot].SetDefaults(mod.ItemType("ExplosiveKnife"));
						chest.item[slot].stack = Main.rand.Next(41) + 20;
						slot++;
					}
					if(thirdLast == 1)
					{
						chest.item[slot].SetDefaults(ItemID.HellfireArrow);
						chest.item[slot].stack = Main.rand.Next(41) + 20;
						slot++;
					}
					if(thirdLast == 2)
					{
						chest.item[slot].SetDefaults(ItemID.AngelStatue);
						slot++;
					}
					
					
					int secLast = Main.rand.Next(2);
					if(secLast == 0)
					{
						chest.item[slot].SetDefaults(ItemID.RecallPotion);
						chest.item[slot].stack = Main.rand.Next(2) + 2;
						slot++;
					}
					
					int last = Main.rand.Next(3);
					if(last == 0)
					{
						chest.item[slot].SetDefaults(ItemID.Torch);
						chest.item[slot].stack = Main.rand.Next(20) + 15;
						slot++;
					}
					if(last == 1)
					{
						chest.item[slot].SetDefaults(ItemID.GoldCoin);
						chest.item[slot].stack = Main.rand.Next(3) + 2;
						slot++;
					}
				}
				if (tile.type == TileID.Containers)
				{
					
					int slot = 39;
					for(int i = 0; i < 39; i++)
					{
						if(chest.item[i].type == 0 && i < slot)
						{
							slot = i;
						}
					}
					
					if(WorldGen.genRand.NextBool(45))
					{
						if(WorldGen.genRand.NextBool(2))
						{
							chest.item[slot].SetDefaults(mod.ItemType("ShieldofDesecar"));
						}
						else
						{
							chest.item[slot].SetDefaults(mod.ItemType("ShieldofStekpla"));
						}
						slot++;
					}
					if(WorldGen.genRand.NextBool(7) && chest.item[0].type == 975)
					{
						chest.item[slot].SetDefaults(mod.ItemType("SpikedClub"));
						slot++;
					}
					if(WorldGen.genRand.NextBool(5) && chest.item[0].type == 997)
					{
						//chest.item[slot].SetDefaults(mod.ItemType("CaveIn"));
						//slot++;
					}
					if(WorldGen.genRand.NextBool(7) && chest.item[0].type == 54)
					{
						chest.item[slot].SetDefaults(mod.ItemType("WingedKnife"));
						slot++;
					}
					if(WorldGen.genRand.NextBool(3) && chest.item[0].type == 906)
					{
						//chest.item[slot].SetDefaults(mod.ItemType("LavaPelter"));
						//slot++;
					}
					if(WorldGen.genRand.NextBool(2) && (chest.item[0].type == ItemID.Starfury || chest.item[0].type == ItemID.ShinyRedBalloon || chest.item[0].type == ItemID.LuckyHorseshoe))
					{
						chest.item[slot].SetDefaults(mod.ItemType("TinyPlanet"));
						slot++;
					}
					if(WorldGen.genRand.NextBool(6) && (chest.item[0].type == ItemID.IceBoomerang || chest.item[0].type == ItemID.IceBlade || chest.item[0].type == ItemID.IceSkates || chest.item[0].type == ItemID.BlizzardinaBottle || chest.item[0].type == ItemID.FlurryBoots))
					{
						//chest.item[slot].SetDefaults(mod.ItemType("FragmentOfPermafrost"));
						//chest.item[slot].stack = Main.rand.Next(3) + 5;
						//slot++;
					}
					if(WorldGen.genRand.NextBool(80))
					{
						chest.item[slot].SetDefaults(mod.ItemType("Grenadier"));
						slot++;
					}
				}
			}
		}	
		int variation = 0;
		public void GenerateCrate(int x, int y)
		{
			int rand = Main.rand.Next(4);
			if(rand != 3)
			{
				WorldGen.PlaceTile(x, y, 376, true, true, -1, rand);
			}
			else
			{
				WorldGen.PlaceTile(x + 1, y, mod.TileType("PyramidCrateTile"));
			}
		}
		public void GenerateShrineRoom(int x, int y, int type = 0)
		{ 
			int[,] _structure = {
				{0,0,0,0,0,0,0,1,1,12,1,1,0,0,0,0,0,0,0},
				{0,0,2,2,1,1,1,1,1,1,1,1,1,1,1,2,2,0,0},
				{0,0,2,1,1,1,1,1,3,4,3,1,1,1,1,1,2,0,0},
				{0,1,2,4,3,1,1,1,5,2,6,1,1,1,3,4,2,1,0},
				{1,1,5,2,6,1,1,1,1,1,1,1,1,1,5,2,6,1,1},
				{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
				{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
				{1,1,1,1,1,1,1,1,1,7,1,1,1,1,1,1,1,1,1},
				{8,8,8,8,8,8,8,4,13,13,13,4,8,8,8,8,8,8,8},
				{0,2,8,8,8,10,10,10,10,10,10,10,10,10,8,8,8,2,0},
				{0,2,2,8,8,9,4,4,4,4,4,4,4,11,8,8,2,2,0},
				{0,0,2,2,8,8,8,9,4,4,4,11,8,8,8,2,2,0,0},
				{0,0,0,2,2,8,8,8,8,8,8,8,8,8,2,2,0,0,0},
				{0,0,0,0,2,2,2,8,8,8,8,8,2,2,2,0,0,0,0}
			};
			int PosX = x;  //spawnX and spawnY is where you want the anchor to be when this generates
			int PosY = y;
			//i = vertical, j = horizontal
			for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 0:
									tile.active(true);
									tile.type = (ushort)mod.TileType("PyramidSlabTile");
									tile.slope(0);
									tile.halfBrick(false);
									break;
								case 1:
									if (confirmPlatforms == 0)
									{
										tile.active(false);
										tile.halfBrick(false);
										tile.slope(0);
									}
									break;
								case 2:
									tile.active(true);
									tile.type = 274;
									tile.slope(0);
									tile.halfBrick(false);
									break;
								case 3:
									tile.active(true);
									tile.type = 274;
									tile.slope(0);
									tile.halfBrick(true);
									break;
								case 4:
									if (confirmPlatforms == 0)
									{
										tile.active(false);
										tile.halfBrick(false);
										tile.slope(0);
										tile.liquid = 255;
										tile.liquidType(0);
									}
									break;
								case 5:
									tile.active(true);
									tile.type = 274;
									tile.slope(4);
									tile.halfBrick(false);
									break;
								case 6:
									tile.active(true);
									tile.type = 274;
									tile.slope(3);
									tile.halfBrick(false);
									break;
								case 7:
									if (confirmPlatforms == 1)
									{
										tile.active(false);
										tile.slope(0);
										tile.halfBrick(false);
										if(type == 0)
											WorldGen.PlaceTile(k, l, mod.TileType("EnchantedSwordShrineTile"), true, true, -1, 0);

										if (type == 1)
											WorldGen.PlaceTile(k, l, mod.TileType("EnchantedPickShrineTile"), true, true, -1, 0);
									}
									break;
								case 8:
									tile.active(true);
									tile.type = 2;
									tile.slope(0);
									tile.halfBrick(false);
									break;
								case 9:
									tile.active(true);
									tile.type = 2;
									tile.slope(1);
									tile.halfBrick(false);
									break;
								case 10:
									if (confirmPlatforms == 0)
									{
										tile.active(false);
										tile.halfBrick(false);
										tile.slope(0);
										tile.liquid = 254;
										tile.liquidType(0);
									}
									break;
								case 11:
									tile.active(true);
									tile.type = 2;
									tile.slope(2);
									tile.halfBrick(false);
									break;
								case 12:
									if (confirmPlatforms == 0)
									{
										tile.active(false);
										tile.halfBrick(false);
										tile.slope(0);
										for(int h = 1; h < 50; h++)
										{
											Tile tile2 = Framing.GetTileSafely(k, l - h);
											Tile tile3 = Framing.GetTileSafely(k, l - h - 1);
											Tile tile4 = Framing.GetTileSafely(k, l - h - 2);
											Tile tile5 = Framing.GetTileSafely(k, l - h - 3);
											if (tile2.type == mod.TileType("PyramidSlabTile") && tile2.active() && tile3.type == mod.TileType("PyramidSlabTile") && tile3.active() && tile4.type == mod.TileType("PyramidSlabTile") && tile4.active() && tile5.type == mod.TileType("PyramidSlabTile") && tile5.active())
											{
												if(Main.rand.Next(h) <= Main.rand.Next(50))
												{
													tile2.active(false);
													tile2.halfBrick(false);
													tile2.slope(0);
												}
											}
											else
											{
												break;
											}
										}
									}
									break;
								case 13:
									tile.active(true);
									tile.type = (ushort)mod.TileType("CursedHive");
									tile.slope(0);
									tile.halfBrick(false);
									break;
							}
						}
					}
				}
			}
		}
		public void GeneratePyramidRoom(int x, int y, int direction) 
		{
			//direction 0 = left, 1 = right, 2 = up, 3 = down
			if(direction == 0 || direction == 1 || direction == 2 || direction == 3)
			{
				Tile initialTile = Framing.GetTileSafely(x, y);
				variation = WorldGen.genRand.Next(12);
				if(direction == 0)
				{
					//tile.type = 200;
					if(Main.rand.Next(2) != 0)
					{
						for(int checkLeft = 0; checkLeft < 300; checkLeft++)
						{
							int check5 = 0;
							for(int h = 2; h >= -2; h--)
							{
								Tile checkTile = Framing.GetTileSafely(x - checkLeft, y + h);
								if(checkTile.active() == false || checkTile.wall != (ushort)mod.WallType("PyramidWallTile") || (checkTile.type != mod.TileType("PyramidSlabTile") && checkTile.type != TileID.SandStoneSlab && checkTile.type != TileID.SandstoneBrick))
								{
									check5++;
								}
								checkTile.active(false);
							}
							if(check5 >= 5)
							{
								break;
							}
						}
					}
					if(variation == 0)
					{
						int[,] _pyramidRoom = {
							{1,1,1,1,1,1,7,7,3,3,3,3,3,3,3,3,3},
							{1,7,7,7,7,7,7,7,0,0,4,8,8,3,3,3,3},
							{1,7,7,7,7,7,0,0,0,0,3,3,3,3,3,3,3},
							{1,7,7,0,0,0,0,0,0,0,0,0,4,8,8,3,3},
							{1,1,7,7,0,0,0,0,0,0,0,0,3,3,3,3,3},
							{2,2,2,2,2,2,2,2,2,0,0,0,2,2,2,2,2},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3},
							{2,2,2,2,2,2,2,2,2,2,2,2,3,3,3,3,3},
							{1,1,1,1,1,1,8,8,8,8,8,8,8,3,3,3,3},
							{1,8,8,8,8,8,8,8,8,8,8,8,8,8,3,3,3},
							{1,8,8,8,8,8,8,8,8,8,8,8,8,8,8,3,3},
							{1,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,3},
							{1,1,8,5,8,8,8,8,6,8,8,8,8,8,8,8,8}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 151; //sandstone brick
												tile.halfBrick(true);
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Statues, true, true, -1, Main.rand.Next(71)); //random statue
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 7:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
											case 8:
												tile.liquidType(0);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 1)
					{
						int[,] _pyramidRoom = {
							{1,1,1,1,1,7,0,0,0,0,0,7,1,1,1,1,1},
							{1,2,1,7,7,0,0,0,0,0,0,0,7,7,1,2,1},
							{1,1,7,0,0,0,0,0,0,0,0,0,0,0,7,1,1},
							{1,7,0,0,0,0,0,0,0,0,0,0,0,0,0,7,1},
							{1,7,0,0,0,0,0,7,0,7,0,0,0,0,0,7,1},
							{7,0,0,0,0,0,0,7,2,7,0,0,0,0,0,0,7},
							{0,0,0,0,0,0,2,7,7,7,2,0,0,0,0,0,0},
							{0,0,0,0,7,7,7,1,1,1,7,7,7,0,0,9,9},
							{0,0,0,0,0,2,7,1,0,1,7,2,0,0,0,6,9},
							{0,0,0,0,7,7,7,1,1,1,7,7,7,0,0,3,3},
							{0,0,0,0,0,0,2,7,7,7,2,0,0,0,0,0,3},
							{7,0,0,0,0,0,0,7,2,7,0,0,0,0,0,0,7},
							{1,7,0,0,0,0,0,7,0,7,0,0,0,0,0,7,1},
							{1,7,0,0,0,0,0,0,0,0,0,0,0,0,0,7,1},
							{1,1,7,0,0,0,0,0,0,0,0,0,0,0,7,1,1},
							{1,2,1,7,7,0,0,0,0,0,0,0,7,7,1,2,1},
							{1,1,1,1,1,7,0,0,0,0,0,7,1,1,1,1,1}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 151; //sandstone brick
												tile.halfBrick(true);
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Statues, true, true, -1, Main.rand.Next(71)); //random statue
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 7:
												tile.type = 232; //woodenspike
												tile.active(true);
												break;
											case 8:
												tile.liquidType(0);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 2)
					{
						int[,] _pyramidRoom = {
							{1,1,1,1,1,1,3,1,1,1,1,1,1,3,1,1,1},
							{1,0,0,0,0,0,3,0,0,0,0,3,3,3,3,3,0},
							{1,0,0,0,3,3,3,3,3,0,0,3,1,1,1,3,0},
							{1,0,0,0,3,1,1,1,3,0,0,0,1,1,1,0,0},
							{1,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,0},
							{0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1},
							{0,0,0,0,0,0,5,0,0,0,0,3,3,3,3,3,0},
							{0,0,0,1,1,1,1,1,1,0,0,0,3,0,0,0,0},
							{3,0,0,0,3,3,3,3,0,0,0,0,3,0,3,0,0},
							{3,0,0,0,0,0,3,0,0,3,8,8,8,8,3,0,0},
							{3,8,8,8,3,0,3,0,0,3,8,8,8,8,3,8,3},
							{3,8,8,8,3,8,8,8,8,3,8,8,8,8,3,8,3},
							{3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 151; //sandstone brick
												tile.halfBrick(true);
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												WorldGen.PlaceTile(k, l, 16); //anvil
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, 302); //glass kiln
												break;
											case 7:
												tile.type = 232; //woodenspike
												tile.active(true);
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(true);
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 3)
					{
						int[,] _pyramidRoom = {
							{0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,0},
							{0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
							{0,0,0,3,3,3,3,3,3,3,3,3,3,3,0,0,0},
							{0,0,0,0,2,2,2,2,2,2,2,2,2,0,0,0,0},
							{0,0,0,0,0,3,3,3,3,3,3,3,0,0,0,0,0},
							{0,0,0,0,0,0,2,2,2,2,2,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,3,3,3,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,7,0},
							{0,0,0,0,0,0,0,3,3,3,0,0,0,3,3,3,3},
							{0,0,0,0,0,0,2,2,2,2,2,0,0,0,2,2,3},
							{0,0,0,0,0,3,3,3,3,3,3,3,0,0,0,0,3},
							{0,0,0,0,2,2,2,2,2,2,2,2,2,0,0,0,0},
							{0,0,0,3,3,3,3,3,3,3,3,3,3,3,0,0,0},
							{0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
							{0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,0}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 151; //sandstone brick
												tile.halfBrick(true);
												tile.active(true);
												break;
											case 5:
												tile.active(true);
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, 16); //anvil
												break;
											case 7:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("CrystalStatue")); //heart crystal
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(true);
												break;
										}
									}
								}
							}
						}
					}
					if(variation == 4)
					{
						int[,] _pyramidRoom = {
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
							{1,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{1,3,5,0,5,0,0,0,0,0,0,0,0,0,0,0,0},
							{1,9,9,9,9,9,9,0,0,0,0,0,0,0,0,4,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4},
							{0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4},
							{0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2},
							{0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3},
							{0,0,0,0,0,0,0,0,4,0,3,3,3,3,3,3,3},
							{0,0,0,0,0,0,0,0,4,4,3,3,3,3,3,3,3},
							{0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2},
							{0,0,0,0,0,4,4,0,3,3,3,3,3,3,3,3,3},
							{0,0,0,0,4,4,4,4,3,3,3,3,3,3,3,3,3},
							{2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
							{3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
							{3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 332; //gold coin
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												GenerateCrate(k, l); //crates
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, 16); //anvil
												break;
											case 7:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("CrystalStatue")); //heart crystal
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Platforms, true, true, -1, 0); //platform //platform
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 5)
					{
						int[,] _pyramidRoom = {
							{3,3,3,3,3,2,3,3,3,3,3,2,3,3,3,3,3},
							{3,6,6,6,6,2,1,1,1,1,1,2,6,6,6,6,3},
							{3,0,6,0,6,6,6,6,6,6,6,6,6,0,6,0,3},
							{3,0,0,0,6,0,0,0,6,0,0,0,6,0,0,0,3},
							{3,0,0,0,0,0,0,0,6,0,0,0,0,0,0,0,3},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,7,0,0,0,0,0,0,0,0},
							{3,0,0,0,0,0,0,0,2,2,0,0,0,0,0,0,3},
							{3,0,0,0,6,0,0,0,2,0,0,0,6,0,0,0,3},
							{3,0,6,0,6,6,6,6,2,6,6,6,6,0,6,0,3},
							{3,6,6,6,6,2,1,1,1,1,1,2,6,6,6,6,3},
							{3,3,3,3,3,2,3,3,3,3,3,2,3,3,3,3,3}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 332; //gold coin
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												GenerateCrate(k, l); //crates
												break;
											case 6:
												tile.type = 232; //wooden spike
												tile.active(true);
												break;
											case 7:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Platforms, true, true, -1, 0); //platform //platform
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 6)
					{
						int[,] _pyramidRoom = {
							{3,3,3,3,3,0,0,0,0,0,0,0,3,3,3,3,3},
							{3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
							{3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
							{3,0,0,3,3,3,2,2,2,2,2,3,3,3,0,0,3},
							{3,0,0,3,0,0,0,0,0,0,0,0,0,3,0,0,3},
							{0,0,0,3,0,0,0,0,0,0,0,0,0,3,0,0,0},
							{0,0,0,2,0,0,3,3,3,3,3,0,0,2,0,0,0},
							{0,0,0,2,0,0,2,0,0,0,2,0,0,2,0,0,0},
							{0,0,0,2,0,0,2,0,0,0,2,0,0,2,0,0,0},
							{0,0,0,2,0,0,2,0,4,0,2,0,0,2,0,0,0},
							{0,0,0,2,0,0,3,3,3,3,3,0,0,2,0,0,0},
							{0,0,0,3,7,7,7,7,0,7,7,7,7,3,0,0,0},
							{3,0,0,3,7,7,7,7,7,7,7,7,7,3,0,0,3},
							{3,0,0,3,3,3,2,2,2,2,2,3,3,3,0,0,3},
							{3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
							{3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
							{3,3,3,3,3,0,0,0,0,0,0,0,3,3,3,3,3}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.active(false);
												WorldGen.PlaceTile(k, l, 219); //extractinator
												break;
											case 5:
												tile.type = 232; //spike
												tile.active(true);
												break;
											case 6:
												tile.active(false);
												GenerateCrate(k, l); //crates
												break;
											case 7:
												tile.type = 332; //gold coin
												tile.active(true);
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Platforms, true, true, -1, 0); //platform //platform
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 7)
					{
						int[,] _pyramidRoom = {
							{1,1,1,1,5,5,0,0,0,0,0,5,5,1,1,1,1},
							{1,1,5,5,5,0,0,0,0,0,0,0,5,5,5,1,1},
							{1,5,5,5,0,0,0,0,0,0,0,0,0,5,5,5,1},
							{5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5},
							{5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5},
							{0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,3,3,3,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,3,3,3,3,3,0,0,0,0,0,0},
							{0,0,0,0,0,3,3,3,3,3,3,3,8,8,8,8,8},
							{0,0,0,0,3,3,3,3,3,3,3,3,3,8,8,8,8},
							{0,0,0,3,3,3,3,3,3,3,3,3,3,3,8,8,8},
							{2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
							{3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
							{3,3,2,3,2,0,3,2,2,2,3,0,2,3,2,3,3},
							{3,3,2,3,0,0,3,2,0,2,3,0,0,3,2,3,3},
							{3,3,2,3,6,0,3,2,2,2,3,6,0,3,2,3,3},
							{3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
											case 5:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("CrystalStatue")); //heart crystal
												break;
											case 7:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(true);
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 8)
					{
						int[,] _pyramidRoom = {
							{5,5,5,2,1,3,0,0,0,0,0,3,1,2,5,5,5},
							{5,0,5,2,1,3,0,0,0,0,0,3,1,2,5,0,5},
							{5,5,5,2,2,3,0,0,0,0,0,3,2,2,5,5,5},
							{2,2,2,1,1,3,0,0,0,0,0,3,1,1,2,2,2},
							{1,1,2,1,3,0,0,0,0,0,0,0,3,1,2,1,1},
							{3,3,3,3,0,0,0,0,0,0,0,0,0,3,3,3,3},
							{0,0,0,0,0,0,0,5,5,5,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,5,1,1,1,5,0,0,0,0,0,0},
							{0,0,0,0,0,0,5,1,3,1,5,0,0,0,0,0,0},
							{0,0,0,0,0,0,5,1,1,1,5,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,5,5,5,0,0,0,0,7,0,0},
							{3,3,3,3,0,0,0,0,0,0,0,0,0,3,3,3,3},
							{1,1,2,1,3,0,0,0,0,0,0,0,3,1,2,0,0},
							{2,2,2,1,1,3,0,0,0,0,0,3,1,1,2,2,2},
							{5,5,5,2,2,3,0,0,0,0,0,3,2,2,5,5,5},
							{5,0,5,2,1,3,0,0,0,0,0,3,0,2,5,0,5},
							{5,5,5,2,1,3,0,0,0,0,0,3,0,2,5,5,5}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
											case 5:
												tile.type = 232; //wooden spike
												tile.active(true);
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("CrystalStatue")); //heart crystal
												break;
											case 7:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(true);
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 9)
					{
						int[,] _pyramidRoom = {
							{3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
							{3,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,3},
							{3,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,3},
							{3,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,3},
							{0,5,4,4,4,4,4,4,4,4,4,4,4,4,4,4,3},
							{0,0,5,4,4,4,4,4,4,4,4,4,4,4,4,4,3},
							{0,0,0,5,4,4,4,4,4,4,4,4,4,4,4,4,3},
							{0,0,0,0,5,4,4,4,4,4,4,4,4,4,4,4,3},
							{0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,3},
							{0,0,0,0,0,5,4,4,4,4,4,4,4,4,4,4,3},
							{0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,3},
							{0,0,0,0,0,0,5,4,2,2,2,2,2,2,2,2,2},
							{3,0,0,0,0,0,0,4,2,3,3,3,3,3,3,3,3},
							{2,2,0,0,0,0,0,4,2,0,0,0,0,0,0,0,0},
							{3,3,3,0,0,0,0,5,3,0,0,0,0,0,0,0,0},
							{2,2,2,2,0,0,0,0,3,0,0,0,0,0,0,0,0},
							{3,3,3,3,3,0,0,0,3,0,0,0,0,6,0,7,0}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 53; //sand
												tile.active(true);
												break;
											case 5:
												tile.type = 53; //sand
												tile.active(true);
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("CrystalStatue")); //heart crystal
												break;
											case 7:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(true);
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 10)
                    {
						int[,] _structure = {
							{0,0,1,0,0,0,0,0,0,1,0,0,0,0,1,0,0},
							{0,0,1,0,0,0,0,0,0,1,0,0,0,0,1,0,0},
							{0,0,1,0,0,0,0,0,0,1,0,0,0,0,1,0,0},
							{0,0,1,0,0,0,0,0,2,3,2,0,0,0,1,0,0},
							{0,0,1,0,0,0,0,0,0,2,0,0,0,0,1,0,0},
							{0,2,3,2,0,0,0,0,0,0,0,0,0,0,1,0,0},
							{0,0,2,0,0,0,0,0,4,0,0,0,0,2,3,2,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0},
							{0,0,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,7,0,0,0,0,0,0},
							{10,0,0,0,0,0,0,8,0,12,10,10,9,0,13,14,10},
							{10,10,0,11,0,13,13,14,14,10,10,10,14,14,14,10,10},
							{10,10,10,14,14,14,14,14,10,10,10,10,15,14,15,15,10},
							{10,15,15,15,14,15,15,15,15,15,15,15,15,15,15,15,10}
						};
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _structure.GetLength(1));
						PosY -= (int)(.5f * _structure.GetLength(0));
						//i = vertical, j = horizontal
						for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
						{
							for (int i = 0; i < _structure.GetLength(0); i++)
							{
								for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
								{
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30))
									{
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_structure[i, j])
										{
											case 0:
												if (confirmPlatforms == 0)
												{
													tile.active(false);
													tile.halfBrick(false);
													tile.slope(0);
												}
												break;
											case 1:
												tile.active(true);
												tile.type = 213;
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 2:
												tile.active(true);
												tile.type = 232;
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 3:
												tile.active(true);
												tile.type = (ushort)mod.TileType("PyramidSlabTile");
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 4:
												if (confirmPlatforms == 1)
												{
													tile.active(false);
													tile.slope(0);
													tile.halfBrick(false);
													WorldGen.PlaceTile(k, l, 241, true, true, -1, 8);
												}
												break;
											case 5:
												if (confirmPlatforms == 1)
												{
													tile.active(false);
													tile.slope(0);
													tile.halfBrick(false);
													WorldGen.PlaceTile(k, l, 241, true, true, -1, 1);
												}
												break;
											case 6:
												if (confirmPlatforms == 1)
												{
													tile.active(false);
													tile.slope(0);
													tile.halfBrick(false);
													WorldGen.PlaceTile(k, l, 241, true, true, -1, 0);
												}
												break;
											case 7:
												if (confirmPlatforms == 1)
												{
													tile.active(false);
													tile.slope(0);
													tile.halfBrick(false);
													WorldGen.PlaceTile(k, l,  (ushort)mod.TileType("PyramidChestTile"), true, true, -1, 0);
												}
												break;
											case 8:
												if (confirmPlatforms == 1)
												{
													tile.active(false);
													tile.slope(0);
													tile.halfBrick(false);
													WorldGen.PlaceTile(k, l, 85, true, true, -1, 2);
												}
												break;
											case 9:
												if (confirmPlatforms == 1)
												{
													tile.active(false);
													tile.slope(0);
													tile.halfBrick(false);
													WorldGen.PlaceTile(k, l, 85, true, true, -1, 8);
												}
												break;
											case 10:
												tile.active(true);
												tile.type = 2;
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 11:
												if (confirmPlatforms == 1)
												{
													tile.active(false);
													tile.slope(0);
													tile.halfBrick(false);
													WorldGen.PlaceTile(k, l, 85, true, true, -1, 1);
												}
												break;
											case 12:
												tile.active(true);
												tile.type = 2;
												tile.slope(0);
												tile.halfBrick(true);
												break;
											case 13:
												tile.active(true);
												tile.type = 1;
												tile.slope(0);
												tile.halfBrick(true);
												break;
											case 14:
												tile.active(true);
												tile.type = 1;
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 15:
												tile.active(true);
												tile.type = 0;
												tile.slope(0);
												tile.halfBrick(false);
												break;
										}
									}
								}
							}
						}
					}
					if (variation == 11)
					{
						int[,] _structure = {
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0},
							{0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1},
							{0,0,0,0,0,0,0,0,0,0,0,1,0,1,1,1,1},
							{0,0,0,0,2,0,0,2,2,2,0,0,0,2,2,2,1},
							{0,2,0,0,2,0,0,2,2,2,2,0,2,2,2,2,1},
							{2,2,0,0,2,2,0,2,2,2,2,2,2,2,2,2,1},
							{2,2,2,0,2,2,2,2,2,2,2,2,2,2,2,1,1},
							{2,2,2,0,2,2,2,2,2,2,2,2,2,3,2,0,0},
							{2,2,0,0,2,2,0,2,2,2,2,2,2,1,1,1,0},
							{2,2,0,0,2,2,0,2,2,1,1,0,1,1,1,1,1},
							{2,0,0,0,0,0,0,0,1,1,0,0,1,1,1,1,1},
							{0,0,0,0,0,0,0,0,1,1,1,0,1,1,1,1,1},
							{0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,1},
							{0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,1},
							{0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1}
						};
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _structure.GetLength(1));
						PosY -= (int)(.5f * _structure.GetLength(0));
						//i = vertical, j = horizontal
						for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
						{
							for (int i = 0; i < _structure.GetLength(0); i++)
							{
								for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
								{
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30))
									{
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_structure[i, j])
										{
											case 0:
												tile.active(true);
												tile.type = (ushort)mod.TileType("CursedHive");
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 1:
												tile.active(true);
												tile.type =  (ushort)mod.TileType("PyramidSlabTile");
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 2:
												if (confirmPlatforms == 0)
												{
													tile.active(false);
													tile.halfBrick(false);
													tile.slope(0);
												}
												break;
											case 3:
												if (confirmPlatforms == 1)
												{
													tile.active(false);
													tile.slope(0);
													tile.halfBrick(false);
													WorldGen.PlaceTile(k, l,  (ushort)mod.TileType("PyramidChestTile"), true, true, -1, 0);
												}
												break;
										}
									}
								}
							}
						}
					}
				}
				if(direction == 1)
				{
					//tile.type = 100;
					if(Main.rand.Next(2) != 0)
					{
						for(int checkRight = 0; checkRight < 300; checkRight++)
						{
							int check5 = 0;
							for(int h = 2; h >= -2; h--)
							{
								Tile checkTile = Framing.GetTileSafely(x + checkRight, y + h);
								if(checkTile.active() == false || checkTile.wall != (ushort)mod.WallType("PyramidWallTile") || (checkTile.type != mod.TileType("PyramidSlabTile") && checkTile.type != TileID.SandStoneSlab && checkTile.type != TileID.SandstoneBrick))
								{
									check5++;
								}
								checkTile.active(false);
							}
							if(check5 >= 5)
							{
								break;
							}
						}
					}
					if(variation == 0)
					{
						int[,] _pyramidRoom = {
							{3,1,1,1,1,1,1,1,1,2,3,3,3,3,3,3,3},
							{3,0,0,0,0,0,0,2,1,2,3,0,0,0,0,0,3},
							{3,0,0,0,0,0,0,2,1,2,3,0,9,9,0,0,3},
							{3,0,0,0,0,0,0,2,1,2,3,0,6,9,0,0,3},
							{3,0,0,0,0,0,0,0,1,2,2,2,2,2,2,2,2},
							{3,0,0,2,1,0,0,0,1,1,1,1,1,1,1,1,1},
							{3,0,0,2,1,0,0,0,0,0,0,0,3,0,0,0,0},
							{3,0,0,2,1,0,0,9,9,0,0,0,3,0,0,0,0},
							{3,0,0,2,1,4,0,9,9,0,0,0,3,0,0,0,0},
							{3,0,0,2,1,7,4,5,9,0,0,0,3,0,0,0,0},
							{3,0,0,2,1,2,2,2,2,2,0,0,3,0,0,0,0},
							{3,0,0,2,1,1,1,1,1,2,0,0,3,0,0,2,3},
							{2,0,0,2,2,2,2,2,1,2,0,0,3,0,0,2,3},
							{0,0,0,0,0,0,0,2,1,2,0,0,3,0,0,2,3},
							{0,4,4,4,4,0,0,2,1,2,0,0,0,0,0,2,3},
							{7,7,4,4,4,4,0,2,1,2,0,0,0,0,0,2,3},
							{7,7,7,7,7,7,7,2,1,2,0,0,0,0,0,2,3}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 331; //silver coin
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Statues, true, true, -1, Main.rand.Next(71)); //random statue
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 7:
												tile.type = 332; //gold coin
												tile.active(true);
												break;
											case 8:
												tile.liquidType(0);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 1)
					{
						int[,] _pyramidRoom = {
							{3,1,1,1,1,2,1,4,4,4,4,4,1,1,1,1,1},
							{3,3,4,4,4,2,1,1,1,1,1,4,4,1,1,1,1},
							{3,3,3,4,4,2,2,2,2,2,1,4,4,4,1,1,1},
							{3,3,3,3,4,4,4,4,0,0,1,0,4,0,0,1,1},
							{3,3,3,3,3,4,0,0,0,0,0,0,0,0,0,0,1},
							{3,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0},
							{3,3,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0},
							{3,3,3,3,3,3,3,3,0,0,0,0,0,0,0,0,0},
							{3,3,3,3,3,3,3,3,3,0,0,0,0,0,0,0,0},
							{3,3,3,3,3,3,3,3,3,3,0,0,0,0,0,0,0},
							{3,3,3,3,3,3,3,3,3,3,3,0,0,0,0,0,0},
							{3,3,3,3,3,3,3,3,3,3,3,3,0,0,0,0,0},
							{3,3,3,3,3,3,3,3,3,3,3,3,2,2,2,2,2},
							{3,0,0,0,3,3,3,3,3,3,3,3,2,1,1,1,1},
							{3,9,9,0,3,3,3,3,3,3,3,3,2,1,0,0,1},
							{3,6,9,0,3,3,3,3,3,3,3,3,2,1,0,0,1},
							{3,3,3,3,3,3,3,3,3,3,3,3,2,1,1,1,1}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Statues, true, true, -1, Main.rand.Next(71)); //random statue
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 7:
												tile.type = 332; //gold coin
												tile.active(true);
												break;
											case 8:
												tile.liquidType(0);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 2)
					{
						int[,] _pyramidRoom = {
							{2,1,1,1,2,2,2,2,2,2,2,2,2,2,2,1,1},
							{1,1,1,1,1,2,2,2,2,4,4,4,1,1,2,2,1},
							{1,0,0,0,1,1,2,2,2,4,4,4,4,1,2,2,2},
							{0,7,7,7,0,1,1,1,1,4,4,4,4,1,1,1,2},
							{7,7,7,7,7,1,1,1,1,4,4,0,0,0,0,1,1},
							{7,7,7,7,7,1,1,1,1,4,0,0,0,0,0,0,0},
							{2,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0},
							{2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0},
							{2,2,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{2,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{4,0,9,9,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,6,9,0,0,0,0,0,0,0,0,0,0,0,0,1},
							{0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,1,1},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1},
							{8,8,8,8,8,8,8,8,8,8,8,8,8,1,1,1,1},
							{8,8,8,8,8,8,8,8,8,8,8,8,1,1,1,1,2},
							{8,8,8,8,8,8,8,8,8,8,8,1,1,1,1,2,2}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Statues, true, true, -1, Main.rand.Next(71)); //random statue
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 7:
												tile.type = 332; //gold coin
												tile.active(true);
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 3)
					{
						int[,] _pyramidRoom = {
							{1,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{1,2,2,2,0,0,0,0,0,0,0,0,0,5,0,5,0},
							{1,1,1,1,1,0,0,0,0,0,0,2,1,1,1,1,1},
							{0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,1},
							{5,0,5,0,0,0,0,0,0,0,0,0,0,0,0,2,1},
							{1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,2,2},
							{1,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0},
							{1,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{1,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,3},
							{0,0,9,0,0,0,0,2,2,2,2,2,0,0,0,2,3},
							{2,2,2,2,2,2,8,8,8,2,8,8,8,8,8,2,3},
							{3,3,3,3,3,3,3,3,3,2,3,3,3,3,3,2,3}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												GenerateCrate(k, l); //crates
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 7:
												tile.type = 93; //tikitorch
												tile.active(true);
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(false);
												WorldGen.PlaceTile(k, l, 102, true, true, -1); //throne
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 4)
					{
						int[,] _pyramidRoom = {
							{3,3,3,2,3,0,0,0,0,0,0,0,0,0,0,0,3},
							{0,0,3,2,3,0,0,0,0,0,0,0,0,0,0,0,3},
							{0,0,3,2,3,0,0,0,0,0,3,3,3,3,3,3,3},
							{0,0,3,2,3,0,0,0,0,0,3,2,2,2,2,2,2},
							{0,0,3,2,3,6,0,0,0,0,0,2,0,0,0,0,0},
							{0,0,0,2,3,3,3,3,0,0,0,0,0,4,8,4,0},
							{0,0,0,2,2,2,2,0,0,0,0,0,0,3,3,3,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,3,3,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,2,2,3,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,2,3,0,7,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,2,3,3,3,3,3,0,0,0,0,0,0,0,0},
							{0,0,0,2,3,2,2,2,0,0,0,0,0,0,0,0,0},
							{0,0,0,2,3,2,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,3,2,0,0,0,0,0,0,0,0,0,0,0},
							{8,8,8,8,3,2,8,8,8,8,8,8,8,8,8,8,8}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 151; //sandstone brick
												tile.halfBrick(true);
												tile.active(true);
												break;
											case 5:
												tile.active(true);
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Statues, true, true, -1, Main.rand.Next(71)); //random statue
												break;
											case 7:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("CrystalStatue")); //heart crystal
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(true);
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 5)
					{
						int[,] _pyramidRoom = {
							{3,3,3,3,3,2,3,3,3,3,3,2,3,3,3,3,3},
							{3,6,6,6,6,2,1,1,1,1,1,2,6,6,6,6,3},
							{3,0,6,0,6,6,6,6,6,6,6,6,6,0,6,0,3},
							{3,0,0,0,6,0,0,0,6,0,0,0,6,0,0,0,3},
							{3,0,0,0,0,0,0,0,6,0,0,0,0,0,0,0,3},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,7,0,0,0,0,0,0,0,0,0},
							{3,0,0,0,0,0,0,2,2,0,0,0,0,0,0,0,3},
							{3,0,0,0,6,0,0,0,2,0,0,0,6,0,0,0,3},
							{3,0,6,0,6,6,6,6,2,6,6,6,6,0,6,0,3},
							{3,6,6,6,6,2,1,1,1,1,1,2,6,6,6,6,3},
							{3,3,3,3,3,2,3,3,3,3,3,2,3,3,3,3,3}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 332; //gold coin
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												GenerateCrate(k, l); //crates
												break;
											case 6:
												tile.type = 232; //wooden spike
												tile.active(true);
												break;
											case 7:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Platforms, true, true, -1, 0); //platform //platform
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 6)
					{
						int[,] _pyramidRoom = {
							{3,3,3,3,3,0,0,0,0,0,0,0,3,3,3,3,3},
							{3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
							{3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
							{3,0,0,3,3,3,2,2,2,2,2,3,3,3,0,0,3},
							{3,0,0,3,0,0,0,0,0,0,0,0,0,3,0,0,3},
							{0,0,0,3,0,0,0,0,0,0,0,0,0,3,0,0,0},
							{0,0,0,2,0,0,3,3,3,3,3,0,0,2,0,0,0},
							{0,0,0,2,0,0,2,0,0,0,2,0,0,2,0,0,0},
							{0,0,0,2,0,0,2,0,0,0,2,0,0,2,0,0,0},
							{0,0,0,2,0,0,2,0,4,0,2,0,0,2,0,0,0},
							{0,0,0,2,0,0,3,3,3,3,3,0,0,2,0,0,0},
							{0,0,0,3,7,7,7,7,0,7,7,7,7,3,0,0,0},
							{3,0,0,3,7,7,7,7,7,7,7,7,7,3,0,0,3},
							{3,0,0,3,3,3,2,2,2,2,2,3,3,3,0,0,3},
							{3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
							{3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
							{3,3,3,3,3,0,0,0,0,0,0,0,3,3,3,3,3}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.active(false);
												WorldGen.PlaceTile(k, l, 219); //extractinator
												break;
											case 5:
												tile.type = 232; //spike
												tile.active(true);
												break;
											case 6:
												tile.active(false);
												GenerateCrate(k, l); //crates
												break;
											case 7:
												tile.type = 332; //gold coin
												tile.active(true);
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Platforms, true, true, -1, 0); //platform //platform
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 7)
					{
						int[,] _pyramidRoom = {
							{3,3,3,3,3,3,3,3,2,3,3,3,3,3,7,3,3},
							{4,4,4,4,2,2,2,2,2,2,2,2,2,7,7,7,2},
							{4,4,4,0,0,3,3,3,2,3,3,3,7,3,3,3,3},
							{4,4,0,0,0,0,2,2,2,2,2,7,7,7,2,2,2},
							{4,0,0,0,0,0,0,3,2,3,7,3,3,3,3,3,3},
							{4,0,0,0,0,0,0,0,2,7,7,7,2,2,2,2,2},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,6,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{2,2,2,2,2,7,7,7,2,0,0,0,0,0,0,0,0},
							{3,3,3,3,3,3,7,3,2,3,0,0,0,0,0,0,0},
							{2,2,2,7,7,7,2,2,2,2,2,8,8,8,8,8,8},
							{3,3,3,3,7,3,3,3,2,3,3,3,8,8,8,8,8},
							{2,7,7,7,2,2,2,2,2,2,2,2,2,8,8,8,8},
							{3,3,7,3,3,3,3,3,2,3,3,3,3,3,3,3,3}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Statues, true, true, -1, Main.rand.Next(71)); //random statue
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 7:
												tile.type = 232; //spike
												tile.active(true);
												break;
											case 8:
												tile.liquidType(0);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(false);
												WorldGen.PlaceTile(k, l, 240, true, true, -1, Main.rand.Next(16, 18)); //hanging skeleton
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 8)
					{
						int[,] _pyramidRoom = {
							{4,4,4,4,4,0,0,0,4,4,7,7,7,7,7,7,7},
							{4,4,0,0,0,0,0,0,0,4,7,3,3,3,3,3,3},
							{4,0,0,9,0,0,0,0,0,0,0,0,7,7,7,7,7},
							{0,0,0,0,0,0,0,0,0,0,0,0,7,3,3,3,3},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,7},
							{6,0,0,0,0,0,0,0,0,0,0,0,0,0,7,0,0},
							{1,1,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{3,3,3,3,7,0,0,0,0,0,0,0,0,0,0,0,0},
							{7,7,7,7,7,0,0,0,0,0,0,0,0,0,0,0,0},
							{3,3,3,3,3,3,7,0,0,0,0,0,0,0,0,0,0},
							{7,7,7,7,7,7,7,0,0,0,0,0,0,0,0,0,0},
							{3,3,3,3,3,3,3,3,7,0,0,0,0,0,0,0,0},
							{7,7,7,7,7,7,7,7,7,0,0,0,0,0,0,0,4},
							{3,3,3,3,3,3,3,3,3,3,7,2,2,7,0,4,4},
							{7,7,7,7,7,7,7,7,7,7,7,2,2,7,7,7,7},
							{3,3,3,3,3,3,3,3,3,3,3,2,2,3,3,3,3}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												GenerateCrate(k, l); //crates
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 7:
												tile.type = 232; //wooden spike
												tile.active(true);
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(false);
												WorldGen.PlaceTile(k, l, 240, true, true, -1, Main.rand.Next(16, 18)); //hanging skeleton
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 9)
					{
						int[,] _pyramidRoom = {
							{3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
							{3,1,1,2,2,2,2,2,3,3,1,1,1,1,1,1,3},							
							{3,1,2,2,2,2,2,3,3,1,1,0,0,0,9,9,3},							
							{3,3,3,3,3,3,3,3,1,1,0,0,0,0,0,0,3},							
							{3,1,1,1,1,1,1,1,1,0,0,0,4,4,7,0,3},							
							{3,9,9,0,0,0,0,0,0,0,0,0,4,1,1,1,3},							
							{3,9,0,0,0,5,0,0,0,0,0,4,1,1,0,0,0},							
							{3,0,0,0,0,0,0,0,4,4,4,1,1,0,0,0,0},							
							{3,0,0,0,4,4,4,4,4,4,1,1,0,0,0,0,0},							
							{3,0,0,1,1,1,1,1,1,1,1,0,0,0,0,0,0},							
							{3,0,0,0,0,0,0,0,9,9,0,0,0,0,0,0,0},							
							{3,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,3},							
							{3,9,0,0,0,0,0,0,0,0,0,0,0,1,1,2,3},							
							{3,9,9,0,0,0,0,0,0,0,0,0,1,1,2,2,3},							
							{3,9,9,0,0,0,0,0,0,0,0,1,1,2,2,2,3},							
							{3,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,3},							
							{3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 332; //coins
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												WorldGen.PlaceTile(k, l, 240, true, true, -1, Main.rand.Next(16, 18)); //hanging skeleton
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("CrystalStatue")); //heart crystal
												break;
											case 7:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if (variation == 10)
					{
						int[,] _structure = {
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,1,0,0,0,0,2,2,0,0,0,0,0,0,0,0,0},
							{3,4,0,0,0,5,6,6,0,0,0,0,0,0,0,0,0},
							{7,7,0,0,5,6,8,0,0,0,0,0,0,0,0,0,0},
							{7,7,0,5,6,8,5,6,6,9,0,0,0,0,0,0,0},
							{7,7,5,6,8,5,6,6,6,6,9,0,0,4,0,0,0},
							{7,6,6,8,5,6,6,0,0,6,6,9,0,4,0,0,0},
							{6,6,6,2,2,2,2,10,0,6,6,6,6,7,7,0,0},
							{6,6,6,6,6,6,6,6,6,6,6,6,6,6,7,4,0},
							{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,7,7},
							{6,6,6,0,0,6,6,6,6,6,6,6,6,6,6,6,7},
							{6,6,6,11,0,6,6,6,6,6,6,6,6,6,6,6,6},
							{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6},
							{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6},
							{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6}
						};
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _structure.GetLength(1));
						PosY -= (int)(.5f * _structure.GetLength(0));
						//i = vertical, j = horizontal
						for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
						{
							for (int i = 0; i < _structure.GetLength(0); i++)
							{
								for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
								{
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30))
									{
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_structure[i, j])
										{
											case 0:
												if (confirmPlatforms == 0)
												{
													tile.active(false);
													tile.halfBrick(false);
													tile.slope(0);
												}
												break;
											case 1:
												tile.active(true);
												tile.type = 188;
												tile.slope(0);
												tile.halfBrick(true);
												break;
											case 2:
												tile.active(true);
												tile.type = 151;
												tile.slope(0);
												tile.halfBrick(true);
												break;
											case 3:
												tile.active(true);
												tile.type = 53;
												tile.slope(1);
												tile.halfBrick(false);
												break;
											case 4:
												tile.active(true);
												tile.type = 188;
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 5:
												tile.active(true);
												tile.type = 151;
												tile.slope(2);
												tile.halfBrick(false);
												break;
											case 6:
												tile.active(true);
												tile.type = 151;
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 7:
												tile.active(true);
												tile.type = 53;
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 8:
												tile.active(true);
												tile.type = 151;
												tile.slope(3);
												tile.halfBrick(false);
												break;
											case 9:
												tile.active(true);
												tile.type = 151;
												tile.slope(1);
												tile.halfBrick(false);
												break;
											case 10:
												if (confirmPlatforms == 1)
												{
													tile.active(false);
													tile.slope(0);
													tile.halfBrick(false);
													WorldGen.PlaceTile(k, l,  (ushort)mod.TileType("PyramidChestTile"), true, true, -1, 0);
												}
												break;
											case 11:
												if (confirmPlatforms == 1)
												{
													tile.active(false);
													tile.slope(0);
													tile.halfBrick(false);
													WorldGen.PlaceTile(k, l, (ushort)mod.TileType("CrystalStatue"), true, true, -1, 0);
												}
												break;
										}
									}
								}
							}
						}
					}
					if (variation == 11)
					{
						int[,] _structure = {
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0},
							{1,2,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
							{1,2,2,1,1,2,1,1,2,2,2,2,2,1,1,1,0},
							{1,1,2,2,1,2,1,2,2,1,1,1,1,1,1,0,0},
							{0,1,1,2,2,2,2,2,1,1,0,0,0,0,0,0,0},
							{0,0,0,2,0,0,2,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,2,2,2,3,0,2,2,0,0,0,0,4,0,0,5,5},
							{5,5,2,2,2,2,2,2,2,5,5,5,5,5,5,5,6},
							{6,6,6,2,6,6,6,2,2,2,2,6,7,7,7,7,6},
							{6,6,2,2,6,6,6,2,2,6,7,7,8,0,0,10,7},
							{6,6,2,6,6,6,6,6,2,7,7,11,0,9,0,11,7},
							{6,6,6,6,6,6,6,6,6,6,7,7,7,7,7,7,6}
						};
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _structure.GetLength(1));
						PosY -= (int)(.5f * _structure.GetLength(0));
						//i = vertical, j = horizontal
						for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
						{
							for (int i = 0; i < _structure.GetLength(0); i++)
							{
								for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
								{
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30))
									{
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_structure[i, j])
										{
											case 0:
												if (confirmPlatforms == 0)
												{
													tile.active(false);
													tile.halfBrick(false);
													tile.slope(0);
												}
												break;
											case 1:
												tile.active(true);
												tile.type = 192;
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 2:
												tile.active(true);
												tile.type = 191;
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 3:
												if (confirmPlatforms == 1)
												{
													tile.active(false);
													tile.slope(0);
													tile.halfBrick(false);
													WorldGen.PlaceTile(k, l,  (ushort)mod.TileType("PyramidChestTile"), true, true, -1, 0);
												}
												break;
											case 4:
												if (confirmPlatforms == 1)
												{
													tile.active(false);
													tile.slope(0);
													tile.halfBrick(false);
													WorldGen.PlaceTile(k, l, 86, true, true, -1, 0);
												}
												break;
											case 5:
												tile.active(true);
												tile.type = 2;
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 6:
												tile.active(true);
												tile.type = 0;
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 7:
												tile.active(true);
												tile.type = 1;
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 8:
												tile.active(true);
												tile.type = 1;
												tile.slope(3);
												tile.halfBrick(false);
												break;
											case 9:
												if (confirmPlatforms == 1)
												{
													tile.active(false);
													tile.slope(0);
													tile.halfBrick(false);
													WorldGen.PlaceTile(k, l, (ushort)mod.TileType("CrystalStatue"), true, true, -1, 0);
												}
												break;
											case 10:
												tile.active(true);
												tile.type = 1;
												tile.slope(4);
												tile.halfBrick(false);
												break;
											case 11:
												tile.active(true);
												tile.type = 1;
												tile.slope(0);
												tile.halfBrick(true);
												break;
										}
									}
								}
							}
						}
					}

				}
				if(direction == 2)
				{
					//tile.type = 150;
					if(Main.rand.Next(2) != 0)
					{
							
						for(int checkUp = 0; checkUp < 300; checkUp++)
						{
							int check5 = 0;
							for(int h = 2; h >= -2; h--)
							{
								Tile checkTile = Framing.GetTileSafely(x + h, y - checkUp);
								if(checkTile.active() == false || checkTile.wall != (ushort)mod.WallType("PyramidWallTile") || (checkTile.type != mod.TileType("PyramidSlabTile") && checkTile.type != TileID.SandStoneSlab && checkTile.type != TileID.SandstoneBrick))
								{
									check5++;
								}
								checkTile.active(false);
							}
							if(check5 >= 5)
							{
								break;
							}
						}
					}
					if(variation == 0)
					{
						int[,] _pyramidRoom = {
							{3,3,3,4,4,4,0,0,0,0,0,0,2,3,3,3,3},
							{2,2,2,2,4,0,0,0,0,0,0,0,2,4,4,4,3},
							{3,4,4,4,0,0,0,0,0,0,0,0,2,4,4,4,3},
							{3,4,4,4,0,0,2,2,2,2,2,2,2,0,4,4,3},
							{3,4,4,0,0,0,0,0,4,4,4,4,0,0,0,4,3},
							{3,4,4,0,0,0,0,0,0,0,0,0,0,0,0,4,3},
							{3,4,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2},
							{2,2,2,2,2,2,2,2,2,2,2,2,0,0,0,2,3},
							{4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,2,3},
							{4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,2,3},
							{3,3,3,3,3,2,0,0,0,0,0,0,0,0,0,2,3},
							{3,0,0,7,0,2,0,0,0,0,0,0,0,0,2,2,3},
							{3,7,7,7,0,2,0,0,0,0,0,0,0,0,0,0,3},
							{3,7,7,7,7,2,0,0,0,0,0,0,0,0,0,0,3},
							{3,7,7,7,7,2,0,0,0,0,0,0,0,9,9,0,3},
							{3,7,7,7,7,2,2,2,2,2,2,2,0,6,9,0,3},
							{3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Statues, true, true, -1, Main.rand.Next(71)); //random statue
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 7:
												tile.type = 332; //gold coin
												tile.active(true);
												break;
											case 8:
												tile.liquidType(0);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 1)
					{
						int[,] _pyramidRoom = {
							{2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,1,1},
							{2,1,1,0,0,1,0,0,0,0,0,0,0,0,0,0,1},
							{2,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0},
							{2,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0},
							{2,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0},
							{2,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0},
							{2,0,0,0,0,2,2,2,2,2,2,2,1,1,1,1,2},
							{2,0,0,0,0,0,2,2,2,2,2,2,0,0,0,0,2},
							{2,0,0,0,0,0,4,4,7,7,2,2,0,0,0,0,2},
							{2,0,0,0,0,0,0,0,4,7,2,2,0,0,0,0,2},
							{2,0,0,0,0,0,0,0,4,7,2,2,0,0,0,0,2},
							{2,0,0,0,0,0,0,0,0,7,2,2,0,0,0,4,2},
							{2,4,0,0,0,6,0,0,0,7,2,2,0,0,0,4,2},
							{2,4,4,0,0,9,9,0,0,7,2,2,0,0,4,7,2},
							{2,7,4,0,0,0,0,0,0,7,2,2,7,4,4,7,2},
							{2,7,7,7,7,7,7,7,7,7,2,2,7,7,7,7,2},
							{2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Statues, true, true, -1, Main.rand.Next(71)); //random statue
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 7:
												tile.type = 232; //wooden spike
												tile.active(true);
												break;
											case 8:
												tile.liquidType(0);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Platforms, true, true, -1, 0); //platform
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 2)
					{
						int[,] _pyramidRoom = {
							{3,3,3,3,2,4,0,0,0,0,0,4,2,3,3,3,3},
							{3,4,2,3,2,4,0,0,0,0,0,4,2,3,2,4,3},
							{4,4,2,3,2,4,0,0,0,0,0,4,2,3,2,4,4},
							{4,0,2,3,4,0,0,0,0,0,0,0,4,3,2,0,4},
							{4,0,2,3,4,0,0,0,0,0,0,0,4,3,2,0,4},
							{0,0,0,3,0,0,0,0,0,0,0,0,0,3,0,0,0},
							{0,0,0,3,0,0,0,0,0,0,0,0,0,3,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,9,0,0,0,0,0,0,0,0,0,0,0,0,9,0,0},
							{3,3,3,2,0,0,0,0,0,0,0,0,0,2,3,3,3},
							{3,2,0,0,0,0,0,0,0,0,0,0,0,0,0,2,3},
							{2,0,0,0,0,0,0,0,7,0,0,0,0,0,0,0,2},
							{7,0,0,7,0,0,7,3,7,3,7,0,0,7,0,0,7},
							{7,0,7,7,7,0,7,3,2,3,7,0,7,7,7,0,7},
							{7,7,2,7,2,7,7,3,2,3,7,7,2,7,2,7,7}
						};	
						
						int PosX = x - (int)(.5f * _pyramidRoom.GetLength(1));
						int PosY = y - (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Statues, true, true, -1, Main.rand.Next(71)); //random statue
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 7:
												tile.type = 232; //wooden spike
												tile.active(true);
												break;
											case 8:
												tile.liquidType(0);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("CrystalStatue")); //heart crystal
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 3)
					{
						int[,] _pyramidRoom = {
							{1,1,1,1,1,2,0,0,0,0,0,2,1,1,1,1,1},
							{1,1,1,4,4,2,0,0,0,0,0,2,0,4,1,1,1},
							{1,4,4,0,4,2,1,1,1,1,1,2,0,4,4,4,1},
							{1,4,0,0,4,4,4,0,0,0,0,0,4,4,4,4,1},
							{4,4,0,0,0,4,0,0,0,0,0,4,4,4,4,4,4},
							{4,0,0,0,0,4,0,0,0,0,0,4,4,4,4,4,4},
							{4,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4},
							{0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4},
							{4,0,2,0,0,0,0,0,0,0,0,0,0,4,2,4,4},
							{4,4,2,0,0,0,0,0,0,0,0,0,0,0,2,4,4},
							{4,4,2,0,0,0,0,0,0,0,0,0,0,0,2,4,4},
							{0,4,2,0,0,0,0,0,0,0,0,0,0,0,2,4,0},
							{0,0,2,8,8,8,8,8,8,8,8,8,8,8,2,0,7},
							{7,0,2,8,8,8,8,8,8,8,8,8,8,8,2,7,7},
							{7,7,2,8,8,8,2,8,8,8,2,8,8,8,2,7,7},
							{1,1,2,2,2,2,2,8,8,8,2,2,2,2,2,1,1}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												WorldGen.PlaceTile(k, l, 377); //sharpening station
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 7:
												tile.type = 332; //gold coin
												tile.active(true);
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Platforms, true, true, -1, 0); //platform //platform
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 4)
					{
						int[,] _pyramidRoom = {
							{4,4,4,4,0,0,0,0,0,0,0,0,0,4,4,4,4},
							{2,2,2,2,2,0,0,0,0,0,0,0,2,2,2,2,2},
							{4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4},
							{2,2,2,2,0,0,0,0,0,0,0,0,0,2,2,2,2},
							{4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4},
							{2,2,2,0,0,0,0,0,0,0,0,0,0,0,2,2,2},
							{8,5,0,0,0,0,0,0,0,0,0,0,0,0,0,5,8},
							{2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{2,2,0,0,0,0,0,0,6,0,0,0,0,0,0,2,2},
							{0,0,0,0,0,0,2,2,2,2,2,0,0,0,0,0,0},
							{2,2,2,0,0,0,2,3,2,3,2,0,0,0,2,2,2},
							{0,0,0,0,0,0,0,3,2,3,0,0,0,0,0,0,0},
							{2,2,2,2,0,0,0,3,2,3,0,0,0,2,2,2,2},
							{8,8,8,8,8,8,3,3,2,3,3,8,8,8,8,8,8}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
											case 5:
												tile.type = 151; //sandstone brick
												tile.halfBrick(true);
												tile.active(true);
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, 77); //hellforge
												break;
											case 7:
												tile.type = 332; //gold coin
												tile.active(true);
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Platforms, true, true, -1, 0); //platform //platform
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 5)
					{
						int[,] _pyramidRoom = {
							{3,3,3,3,0,0,0,0,0,0,0,0,0,3,3,3,3},
							{2,2,2,0,0,0,0,0,0,0,0,0,0,0,2,2,2},
							{3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
							{2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2},
							{5,5,5,0,0,0,0,0,0,0,0,0,0,0,5,5,5},
							{2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
							{5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5},
							{2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
							{5,6,0,0,0,0,0,0,0,0,0,0,0,0,6,0,5},
							{2,9,9,9,0,0,0,0,0,0,0,0,0,9,9,9,2},
							{5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5},
							{2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
							{5,5,5,0,0,0,0,0,0,0,0,0,0,0,5,5,5},
							{2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2},
							{3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
							{2,2,2,0,0,0,0,0,0,0,0,0,0,0,2,2,2},
							{3,3,3,3,0,0,0,0,0,0,0,0,0,3,3,3,3}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
											case 5:
												tile.type = 232; //spike
												tile.active(true);
												break;
											case 6:
												tile.active(false);
												GenerateCrate(k, l); //crates
												break;
											case 7:
												tile.type = 332; //gold coin
												tile.active(true);
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Platforms, true, true, -1, 0); //platform //platform
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 6)
					{
						int[,] _pyramidRoom = {
							{3,3,3,3,3,0,0,0,0,0,0,0,3,3,3,3,3},
							{3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
							{3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
							{3,0,0,3,3,3,2,2,2,2,2,3,3,3,0,0,3},
							{3,0,0,3,0,0,0,0,0,0,0,0,0,3,0,0,3},
							{0,0,0,3,0,0,0,0,0,0,0,0,0,3,0,0,0},
							{0,0,0,2,0,0,3,3,3,3,3,0,0,2,0,0,0},
							{0,0,0,2,0,0,2,0,0,0,2,0,0,2,0,0,0},
							{0,0,0,2,0,0,2,0,0,0,2,0,0,2,0,0,0},
							{0,0,0,2,0,0,2,0,4,0,2,0,0,2,0,0,0},
							{0,0,0,2,0,0,3,3,3,3,3,0,0,2,0,0,0},
							{0,0,0,3,7,7,7,7,0,7,7,7,7,3,0,0,0},
							{3,0,0,3,7,7,7,7,7,7,7,7,7,3,0,0,3},
							{3,0,0,3,3,3,2,2,2,2,2,3,3,3,0,0,3},
							{3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
							{3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
							{3,3,3,3,3,0,0,0,0,0,0,0,3,3,3,3,3}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.active(false);
												WorldGen.PlaceTile(k, l, 219); //extractinator
												break;
											case 5:
												tile.type = 232; //spike
												tile.active(true);
												break;
											case 6:
												tile.active(false);
												GenerateCrate(k, l); //crates
												break;
											case 7:
												tile.type = 332; //gold coin
												tile.active(true);
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Platforms, true, true, -1, 0); //platform //platform
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 7)
					{
						int[,] _pyramidRoom = {
							{3,3,3,3,3,3,0,0,0,0,0,3,3,3,3,3,3},
							{3,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,3},
							{3,0,0,0,0,0,0,0,0,0,0,2,3,3,3,2,3},
							{3,0,0,0,0,0,0,0,0,0,0,2,3,2,3,2,3},
							{3,0,0,0,0,0,0,0,4,0,0,2,3,2,3,2,3},
							{3,0,0,0,0,0,0,0,0,0,0,2,3,2,3,2,3},
							{3,6,0,0,5,0,0,0,0,0,0,2,3,2,3,2,3},
							{3,2,1,3,3,3,3,3,3,3,2,2,3,2,3,2,3},
							{3,2,1,3,2,2,2,3,2,3,2,3,3,2,3,2,3},
							{3,2,1,3,3,3,2,3,2,3,2,3,2,2,3,2,3},
							{3,2,1,1,1,3,2,3,2,3,2,3,2,3,3,2,3},
							{3,2,3,3,1,3,2,3,2,3,2,3,2,3,3,2,3},
							{3,2,2,2,1,3,2,3,2,3,2,3,2,3,3,2,3},
							{3,3,3,2,1,3,2,3,2,3,2,3,2,3,3,2,3},
							{3,3,3,2,1,3,3,3,2,3,2,3,2,3,3,2,3},
							{3,3,3,2,1,0,0,3,2,3,2,3,2,3,3,2,3},
							{3,3,3,2,1,7,0,3,2,3,2,3,2,2,2,2,3}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												if(confirmPlatforms == 1)
												WorldGen.PlaceTile(k, l, 240, true, true, -1, Main.rand.Next(16, 18)); //hanging skeleton
												break;
											case 5:
												tile.active(false);
												WorldGen.PlaceTile(k, l, 215); //campfire
												break;
											case 6:
												tile.active(false);
												GenerateCrate(k, l); //crates
												break;
											case 7:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Platforms, true, true, -1, 0); //platform //platform
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 8)
					{
						int[,] _pyramidRoom = {
							{7,7,7,7,7,2,0,0,0,0,0,2,7,7,7,7,7},
							{7,0,0,0,7,2,0,0,0,0,9,2,7,0,0,0,7},
							{0,0,0,0,7,2,0,0,0,0,0,2,7,0,0,0,0},
							{0,0,0,0,7,2,0,0,0,0,0,2,0,0,5,0,0},
							{0,0,0,0,7,2,9,0,0,0,0,2,0,0,0,0,0},
							{0,0,5,0,0,2,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7},
							{0,0,2,0,0,0,0,0,0,0,0,0,0,0,3,3,3},
							{0,0,2,0,0,0,0,0,7,0,0,0,0,0,0,0,3},
							{0,0,2,0,0,0,0,0,7,0,0,0,0,0,0,0,7},
							{0,0,2,0,0,0,0,0,7,0,0,0,0,0,0,0,7},
							{6,0,2,0,0,0,0,0,7,7,0,0,0,0,0,0,7},
							{9,9,2,0,0,0,0,7,3,7,0,0,0,0,0,7,7},
							{7,7,2,7,7,7,7,3,3,3,3,7,7,7,7,7,3}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												WorldGen.PlaceTile(k, l, 240, true, true, -1, Main.rand.Next(16, 18)); //hanging skeleton
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 7:
												tile.type = 232; //wooden spike
												tile.active(true);
												break;
											case 8:
												tile.liquidType(0);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Platforms, true, true, -1, 0); //platform //platform
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 9)
					{
						int[,] _pyramidRoom = {
							{3,3,3,3,3,2,0,0,0,0,0,2,3,3,3,3,3},
							{3,7,7,7,3,2,0,0,0,0,0,2,3,2,2,2,2},
							{3,7,0,7,3,2,0,0,0,0,0,2,3,2,0,0,0},
							{3,7,0,7,3,2,0,0,0,0,0,2,3,2,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,2,3,2,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,2,3,2,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,2,3,2,0,0,0},
							{0,0,2,2,2,2,2,2,2,2,2,2,3,2,0,0,0},
							{0,0,0,2,3,2,3,3,3,3,3,2,3,2,0,0,0},
							{0,0,0,2,3,2,3,7,7,7,3,2,3,0,0,0,0},
							{0,0,0,0,3,2,3,7,0,7,3,2,3,0,0,6,0},
							{3,0,0,0,0,2,3,7,0,7,3,2,0,0,0,9,3},
							{3,0,0,0,0,2,0,0,0,0,0,2,0,0,0,0,3},
							{2,7,0,0,0,0,0,0,0,0,0,0,0,0,0,7,2},
							{3,3,7,0,0,0,0,0,0,0,0,0,0,0,7,3,3},
							{3,0,3,7,0,0,0,0,0,0,0,0,0,7,3,2,3},
							{3,3,3,2,3,2,0,0,0,0,0,2,3,2,3,3,3}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												WorldGen.PlaceTile(k, l, 240, true, true, -1, Main.rand.Next(16, 18)); //hanging skeleton
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 7:
												tile.type = 232; //wooden spike
												tile.active(true);
												break;
											case 8:
												tile.liquidType(0);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Platforms, true, true, -1, 0); //platform //platform
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if (variation == 10) 
					{
						int[,] _structure = {
							{0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0},
							{0,2,2,2,0,0,1,1,1,1,1,0,0,2,2,2,0},
							{0,2,2,2,0,1,1,1,1,1,1,1,0,2,2,2,0},
							{0,0,0,2,0,1,1,1,1,1,1,1,0,2,2,2,0},
							{0,3,0,2,0,1,1,1,1,1,1,1,0,2,0,0,0},
							{0,3,0,0,0,1,1,1,1,1,1,1,0,0,0,3,0},
							{0,3,0,0,1,1,1,1,1,1,1,1,1,0,0,3,0},
							{0,3,0,0,1,1,1,1,1,1,1,1,1,0,0,3,0},
							{0,3,4,1,1,1,1,1,1,1,1,1,1,1,1,3,0},
							{0,1,4,1,1,1,1,1,1,1,1,1,1,1,1,3,0},
							{0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
							{0,1,5,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
							{0,1,7,7,1,1,1,1,1,1,1,1,1,1,1,1,0},
							{0,1,1,1,1,1,1,1,6,1,1,1,1,1,1,1,0},
							{0,0,1,3,1,1,0,0,0,0,0,1,1,3,1,0,0},
							{0,0,3,3,3,0,0,2,2,2,0,0,3,3,3,0,0},
							{0,0,0,3,0,0,0,0,0,0,0,0,0,3,0,0,0}
						};
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _structure.GetLength(1));
						PosY -= (int)(.5f * _structure.GetLength(0));
						//i = vertical, j = horizontal
						for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
						{
							for (int i = 0; i < _structure.GetLength(0); i++)
							{
								for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
								{
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30))
									{
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_structure[i, j])
										{
											case 0:
												tile.active(true);
												tile.type = 2;
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 1:
												if (confirmPlatforms == 0)
												{
													tile.active(false);
													tile.halfBrick(false);
													tile.slope(0);
												}
												break;
											case 2:
												tile.active(true);
												tile.type = 0;
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 3:
												tile.active(true);
												tile.type = 232;
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 4:
												tile.active(true);
												tile.type = 52;
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 5:
												if (confirmPlatforms == 1)
												{
													tile.active(false);
													tile.slope(0);
													tile.halfBrick(false);
													WorldGen.PlaceTile(k, l,  (ushort)mod.TileType("PyramidChestTile"), true, true, -1, 0);
												}
												break;
											case 6:
												if (confirmPlatforms == 1)
												{
													tile.active(false);
													tile.slope(0);
													tile.halfBrick(false);
													WorldGen.PlaceTile(k, l, 304, true, true, -1, 0);
												}
												break;
											case 7:
												if (confirmPlatforms == 0)
													tile.active(false);
												WorldGen.PlaceTile(k, l, 19, true, true, -1, 0);
												tile.slope(0);
												tile.halfBrick(false);
												break;
										}
									}
								}
							}
						}
					}
					if (variation == 11)
					{
						int[,] _structure = {
							{0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1},
							{0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{0,0,2,1,1,1,1,1,1,1,1,1,1,1,2,1,0},
							{0,2,2,1,1,1,1,1,1,1,1,1,1,1,2,1,0},
							{0,2,2,1,1,1,1,1,1,1,3,1,1,1,2,1,0},
							{0,2,2,1,1,1,1,1,1,4,4,4,4,1,2,2,0},
							{0,0,2,2,1,1,2,1,1,1,1,1,1,2,2,2,0},
							{0,0,0,2,1,1,2,1,1,1,1,1,0,2,2,2,0},
							{0,0,0,2,1,1,2,1,1,2,1,1,0,2,2,0,0},
							{0,0,0,0,2,1,2,1,2,2,1,0,0,0,2,0,0},
							{0,0,0,0,2,2,2,2,2,2,0,0,0,0,2,0,0},
							{0,0,0,0,0,2,2,2,2,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
						};
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _structure.GetLength(1));
						PosY -= (int)(.5f * _structure.GetLength(0));
						//i = vertical, j = horizontal
						for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
						{
							for (int i = 0; i < _structure.GetLength(0); i++)
							{
								for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
								{
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30))
									{
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_structure[i, j])
										{
											case 0:
												tile.active(true);
												tile.type =  (ushort)mod.TileType("CursedHive");
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 1:
												if (confirmPlatforms == 0)
												{
													tile.active(false);
													tile.halfBrick(false);
													tile.slope(0);
												}
												break;
											case 2:
												tile.active(true);
												tile.type = 232;
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 3:
												if (confirmPlatforms == 1)
												{
													tile.active(false);
													tile.slope(0);
													tile.halfBrick(false);
													WorldGen.PlaceTile(k, l,  (ushort)mod.TileType("PyramidChestTile"), true, true, -1, 0);
												}
												break;
											case 4:
												if (confirmPlatforms == 0)
													tile.active(false);
												WorldGen.PlaceTile(k, l, 19, true, true, -1, 0);
												tile.slope(0);
												tile.halfBrick(false);
												break;
										}
									}
								}
							}
						}
					}

				}
				if(direction == 3)
				{
					//tile.type = 50;
					if(Main.rand.Next(2) != 0)
					{
						for(int checkDown = 0; checkDown < 300; checkDown++)
						{
							int check5 = 0;
							for(int h = 2; h >= -2; h--)
							{
								Tile checkTile = Framing.GetTileSafely(x + h, y + checkDown);
								if(checkTile.active() == false || checkTile.wall != (ushort)mod.WallType("PyramidWallTile") || (checkTile.type != mod.TileType("PyramidSlabTile") && checkTile.type != TileID.SandStoneSlab && checkTile.type != TileID.SandstoneBrick))
								{
									check5++;
								}
								checkTile.active(false);
							}
							if(check5 >= 5)
							{
								break;
							}
						}
					}
					if(variation == 0)
					{
						int[,] _pyramidRoom = {
							{7,7,7,7,7,4,4,4,4,2,3,3,3,3,3,3,3},
							{3,3,3,3,7,4,4,4,4,2,3,2,2,2,2,2,3},
							{2,2,2,3,7,4,4,4,0,2,3,3,3,3,3,2,3},
							{4,2,3,3,2,7,4,4,0,2,2,2,2,2,2,2,3},
							{4,2,2,2,2,3,7,0,0,0,0,0,3,3,3,3,3},
							{4,4,3,3,3,3,7,0,0,0,9,9,3,4,4,4,4},
							{4,4,4,7,3,7,0,0,0,0,6,9,3,0,4,4,4},
							{4,4,7,2,3,7,0,0,3,3,3,3,3,0,7,7,7},
							{4,4,7,2,3,2,0,0,0,0,0,0,0,0,7,2,2},
							{4,7,3,3,3,3,3,0,0,0,0,0,0,0,7,2,3},
							{4,0,7,2,3,2,0,0,0,0,0,7,7,7,7,2,3},
							{0,0,7,2,3,0,0,0,0,0,0,7,2,2,2,2,3},
							{0,0,0,7,3,0,0,0,0,0,0,7,2,3,3,3,3},
							{7,0,0,0,0,0,0,0,0,0,0,7,2,3,3,3,3},
							{3,7,0,0,9,9,0,0,0,0,0,7,2,3,3,3,3},
							{3,2,7,0,9,9,0,0,0,0,0,7,2,3,3,3,3},
							{3,2,3,7,5,9,0,0,0,0,0,7,2,3,3,3,3}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Statues, true, true, -1, Main.rand.Next(71)); //random statue
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 7:
												tile.type = 232; //wooden spike
												tile.active(true);
												break;
											case 8:
												tile.liquidType(0);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 1)
					{
						int[,] _pyramidRoom = {
							{2,1,1,2,2,1,1,1,1,1,1,1,2,2,1,1,2},
							{1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1},
							{1,2,2,2,0,0,0,0,0,0,4,4,4,2,2,2,1},
							{2,2,2,0,0,0,0,0,0,0,0,4,4,4,2,2,2},
							{2,2,0,0,0,0,0,0,0,0,0,0,0,4,4,2,2},
							{3,4,0,0,0,0,0,0,0,0,0,0,0,0,4,4,3},
							{3,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
							{4,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0},
							{4,0,0,0,0,1,1,1,1,2,1,1,1,0,0,0,0},
							{0,0,0,0,0,0,0,7,1,1,1,1,1,1,0,0,8},
							{0,0,0,0,0,6,0,7,1,1,1,1,1,1,1,8,8},
							{9,9,9,9,9,1,1,1,1,1,1,1,1,1,1,1,8},
							{1,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1},
							{1,1,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1},
							{1,1,1,0,0,0,0,0,0,0,0,0,1,1,1,1,1},
							{1,0,1,1,0,0,0,0,0,0,0,0,1,1,1,1,1},
							{1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1,1}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Statues, true, true, -1, Main.rand.Next(71)); //random statue
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 7:
												tile.type = 232; //wooden spike
												tile.active(true);
												break;
											case 8:
												tile.type = 332; //gold coin
												tile.active(true);
												break;
											case 9:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Platforms, true, true, -1, 0); //platform
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 2)
					{
						int[,] _pyramidRoom = {
							{3,4,4,4,4,4,4,0,3,3,3,3,3,3,3,3,3},
							{3,4,4,4,4,0,0,0,0,2,2,3,2,2,2,2,2},
							{3,4,0,0,0,0,0,0,0,0,2,3,2,4,4,4,4},
							{3,0,0,0,0,0,0,0,0,0,2,3,2,0,4,4,4},
							{3,0,0,0,0,0,0,0,0,0,2,3,2,0,0,0,4},
							{3,0,0,0,0,0,0,0,0,0,0,3,2,0,0,0,0},
							{3,0,5,0,0,0,0,0,0,0,0,3,2,0,0,0,0},
							{3,3,3,3,3,3,9,9,9,9,9,3,0,0,0,0,0},
							{3,2,2,2,2,0,0,0,0,0,0,3,0,0,0,0,0},
							{3,7,7,7,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{3,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{3,7,0,0,0,0,0,0,0,0,0,0,0,0,8,0,0},
							{3,7,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3},
							{2,2,0,0,0,0,0,0,0,0,0,0,0,2,2,2,3},
							{3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,2,3},
							{2,2,2,2,0,0,0,0,0,0,0,2,2,2,2,2,2},
							{3,3,3,3,3,0,0,0,0,0,0,0,3,3,3,3,3}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												WorldGen.PlaceTile(k, l, 377); //sharpening station
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 7:
												tile.type = 232; //wooden spike
												tile.active(true);
												break;
											case 8:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("CrystalStatue")); //heart crystal
												break;
											case 9:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Platforms, true, true, -1, 0); //platform
												break;
										}
									}
								}
							}
						}
					}
					if(variation == 3)
					{
						int[,] _pyramidRoom = {
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,4,4,0,0,0,0,0,0,0,0,0,0,4,4,4,4},
							{1,4,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4},
							{1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4},
							{1,0,0,0,2,2,2,2,2,2,2,2,0,0,2,0,0},
							{1,0,0,0,0,0,0,0,4,4,2,2,0,0,2,0,0},
							{1,0,0,0,0,0,0,0,0,4,2,2,0,0,2,0,0},
							{1,0,0,0,0,0,0,0,0,0,2,2,0,0,2,0,0},
							{2,2,2,2,2,2,2,0,0,0,2,2,0,0,2,0,0},
							{1,4,4,0,0,0,0,0,0,0,2,2,4,4,2,6,0},
							{1,4,0,0,0,0,0,0,0,0,2,2,0,0,2,9,9},
							{1,0,0,0,0,0,0,0,0,0,2,2,0,0,2,0,0},
							{1,0,0,0,2,2,2,2,2,2,2,2,0,0,2,0,0},
							{1,0,0,0,0,0,0,0,0,0,0,2,8,8,2,0,0},
							{1,0,0,0,0,0,0,0,0,0,0,2,8,8,2,0,0},
							{1,0,0,0,0,0,0,0,0,0,0,2,8,8,2,0,0},
							{1,1,1,1,1,1,0,0,0,0,0,2,2,2,2,2,2}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												WorldGen.PlaceTile(k, l, 377); //sharpening station
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 7:
												tile.type = 332; //gold coin
												tile.active(true);
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Platforms, true, true, -1, 0); //platform //platform
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 4)
					{
						int[,] _pyramidRoom = {
							{0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1},
							{0,0,0,0,0,0,0,0,0,1,1,0,1,1,1,0,1},
							{0,6,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1},
							{3,3,3,3,3,0,0,0,0,0,0,1,0,1,1,1,1},
							{3,2,2,2,2,2,0,0,0,0,1,0,0,0,1,1,1},
							{3,3,2,1,1,1,1,0,0,0,0,1,0,1,0,0,0},
							{2,2,2,1,0,0,0,0,0,0,1,0,1,0,0,1,0},
							{0,1,2,1,0,0,0,0,0,0,0,0,0,0,0,1,0},
							{0,1,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,1,2,1,0,0,0,0,0,0,0,0,0,0,3,3,3},
							{0,1,1,1,0,0,0,0,0,0,0,0,0,2,2,2,2},
							{0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3},
							{0,0,0,0,0,0,0,0,0,0,0,0,3,2,2,2,2},
							{0,0,0,0,0,0,0,0,0,0,0,0,3,2,0,0,0},
							{0,0,9,0,0,0,0,0,0,0,0,0,3,2,0,0,0},
							{2,2,2,2,2,0,0,0,0,0,0,0,3,2,0,0,0},
							{3,3,3,3,0,0,0,0,0,0,0,0,3,2,0,6,0}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												WorldGen.PlaceTile(k, l, 377); //sharpening station
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 7:
												tile.type = 332; //gold coin
												tile.active(true);
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(false);
												WorldGen.PlaceTile(k, l, 354); //bewitching table
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 5)
					{
						int[,] _pyramidRoom = {
							{3,3,3,3,0,0,0,0,0,0,0,0,0,3,3,3,3},
							{2,2,2,0,0,0,0,0,0,0,0,0,0,0,2,2,2},
							{3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
							{2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2},
							{5,5,5,0,0,0,0,0,0,0,0,0,0,0,5,5,5},
							{2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
							{5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5},
							{2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
							{5,6,0,0,0,0,0,0,0,0,0,0,0,0,6,0,5},
							{2,9,9,9,0,0,0,0,0,0,0,0,0,9,9,9,2},
							{5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5},
							{2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
							{5,5,5,0,0,0,0,0,0,0,0,0,0,0,5,5,5},
							{2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2},
							{3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
							{2,2,2,0,0,0,0,0,0,0,0,0,0,0,2,2,2},
							{3,3,3,3,0,0,0,0,0,0,0,0,0,3,3,3,3}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
											case 5:
												tile.type = 232; //spike
												tile.active(true);
												break;
											case 6:
												tile.active(false);
												GenerateCrate(k, l); //crates
												break;
											case 7:
												tile.type = 332; //gold coin
												tile.active(true);
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Platforms, true, true, -1, 0); //platform //platform
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 6)
					{
						int[,] _pyramidRoom = {
							{3,3,3,3,3,0,0,0,0,0,0,0,3,3,3,3,3},
							{3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
							{3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
							{3,0,0,3,3,3,2,2,2,2,2,3,3,3,0,0,3},
							{3,0,0,3,0,0,0,0,0,0,0,0,0,3,0,0,3},
							{0,0,0,3,0,0,0,0,0,0,0,0,0,3,0,0,0},
							{0,0,0,2,0,0,3,3,3,3,3,0,0,2,0,0,0},
							{0,0,0,2,0,0,2,0,0,0,2,0,0,2,0,0,0},
							{0,0,0,2,0,0,2,0,0,0,2,0,0,2,0,0,0},
							{0,0,0,2,0,0,2,0,4,0,2,0,0,2,0,0,0},
							{0,0,0,2,0,0,3,3,3,3,3,0,0,2,0,0,0},
							{0,0,0,3,7,7,7,7,0,7,7,7,7,3,0,0,0},
							{3,0,0,3,7,7,7,7,7,7,7,7,7,3,0,0,3},
							{3,0,0,3,3,3,2,2,2,2,2,3,3,3,0,0,3},
							{3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
							{3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
							{3,3,3,3,3,0,0,0,0,0,0,0,3,3,3,3,3}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.active(false);
												WorldGen.PlaceTile(k, l, 219); //extractinator
												break;
											case 5:
												tile.type = 232; //spike
												tile.active(true);
												break;
											case 6:
												tile.active(false);
												GenerateCrate(k, l); //crates
												break;
											case 7:
												tile.type = 332; //gold coin
												tile.active(true);
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Platforms, true, true, -1, 0); //platform //platform
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 7)
					{
						int[,] _pyramidRoom = {
							{4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0},
							{2,2,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0},
							{4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,6,0},
							{2,2,2,0,0,0,0,0,0,0,0,0,4,4,2,2,2},
							{4,4,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4},
							{2,2,2,2,0,0,0,0,0,0,0,0,0,0,4,2,2},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4},
							{2,2,2,2,2,0,0,0,0,0,0,0,0,0,2,2,2},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4},
							{2,2,2,2,2,2,0,0,0,0,0,0,0,2,2,2,2},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4},
							{2,2,2,2,2,2,2,0,0,0,0,0,2,2,2,2,2},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2},
							{0,5,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												WorldGen.PlaceTile(k, l, 355); //alchemy table
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 7:
												tile.type = 332; //gold coin
												tile.active(true);
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Platforms, true, true, -1, 0); //platform //platform
												break;
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 8)
					{
						int[,] _pyramidRoom = {
							{4,3,3,3,0,0,0,0,3,0,0,0,0,3,3,3,4},
							{4,4,3,3,5,0,5,0,3,5,0,5,0,3,3,4,4},
							{4,4,4,3,3,3,3,3,3,3,3,3,3,3,4,4,4},
							{4,4,4,4,3,3,3,3,3,3,3,3,3,4,4,4,4},
							{4,4,4,0,0,3,3,3,3,3,3,3,0,0,4,4,4},
							{4,4,4,1,0,0,3,3,3,3,3,0,0,1,4,4,4},
							{4,4,0,1,1,0,0,3,3,3,0,0,1,1,0,4,4},
							{4,0,0,0,1,1,0,0,3,0,0,1,1,0,0,0,4},
							{4,0,1,0,0,1,1,0,0,0,1,1,0,0,1,0,4},
							{0,1,0,0,1,0,0,0,0,0,0,0,1,0,0,1,0},
							{0,0,0,0,1,0,1,0,1,0,1,0,1,0,0,0,0},
							{0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4},
							{3,3,3,0,0,0,0,0,0,0,0,0,0,0,3,3,3},
							{2,2,2,2,0,0,0,0,0,0,0,0,0,2,2,2,2},
							{3,3,3,3,3,0,0,0,0,0,0,0,3,3,3,3,3},
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												GenerateCrate(k, l); //crates
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 7:
												tile.type = 332; //gold coin
												tile.active(true);
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Platforms, true, true, -1, 0); //platform //platform
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 9)
					{
						int[,] _pyramidRoom = {
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1},
							{1,2,2,3,2,1,1,1,2,2,2,2,2,2,2,2,1},
							{1,2,3,3,2,0,0,0,0,0,0,0,0,4,4,2,1},
							{1,2,2,3,2,0,0,0,0,0,0,0,0,0,4,2,1},
							{1,2,2,2,2,5,0,0,0,0,0,0,0,0,0,2,1},
							{1,2,3,2,2,1,1,1,1,1,1,1,1,0,0,2,1},
							{1,2,3,3,2,2,2,2,2,2,2,2,0,0,0,2,1},
							{1,2,3,2,2,1,1,1,1,2,2,0,0,0,0,2,1},
							{1,2,2,2,2,2,2,2,2,2,0,0,0,0,0,2,1},
							{1,2,2,4,4,4,0,0,0,0,0,0,0,0,2,2,1},
							{1,2,2,4,0,0,0,0,0,0,0,0,0,0,2,2,1},
							{1,2,2,0,0,0,0,0,0,0,0,0,0,0,2,2,1},
							{1,2,2,1,6,0,0,0,0,0,0,6,0,1,2,2,1},
							{1,2,2,1,1,1,0,0,0,0,0,1,1,1,2,2,1},
							{1,2,2,2,2,2,0,0,0,0,0,2,2,2,2,2,1},
							{1,1,1,1,1,1,0,0,0,0,0,1,1,1,1,1,1}
						};	
						
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _pyramidRoom.GetLength(1));
						PosY -= (int)(.5f * _pyramidRoom.GetLength(0));
						//i = vertical, j = horizontal
						for(int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
						{
							for (int i = 0; i < _pyramidRoom.GetLength(0); i++) {
								for (int j = 0; j < _pyramidRoom.GetLength(1); j++) {
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30)) {
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_pyramidRoom[i, j]) {
											case 0:
												if(confirmPlatforms == 0)
												tile.active(false);
												break;
											case 1:
												tile.type = 274; //sandstoneslab
												tile.active(true);
												break;
											case 2:
												tile.type = (ushort)mod.TileType("PyramidSlabTile"); //pyramid slab
												tile.active(true);
												break;
											case 3:
												tile.type = 151; //sandstone brick
												tile.active(true);
												break;
											case 4:
												tile.type = 51; //cobweb
												tile.active(true);
												break;
											case 5:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("PyramidChestTile")); //chest
												break;
											case 6:
												tile.active(false);
												WorldGen.PlaceTile(k, l, (ushort)mod.TileType("CrystalStatue")); //chest
												break;
											case 7:
												tile.type = 332; //gold coin
												tile.active(true);
												break;
											case 8:
												tile.liquidType(1);
												tile.liquid = 255;
												
												if(confirmPlatforms == 0)
												tile.active(false);
											
												WorldGen.SquareTileFrame(k, l, false);
												break;
											case 9:
												tile.active(false);
												WorldGen.PlaceTile(k, l, TileID.Platforms, true, true, -1, 0); //platform //platform
												break;
											
										}
									}
								}
							}
						}
					}
					if(variation == 10)
                    {
						int[,] _structure = {
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0},
							{0,0,0,0,0,1,1,0,0,0,1,1,0,0,0,0,0},
							{0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,1,0,2,0,0,0,0,0,3,0,0,0},
							{0,0,0,0,0,1,1,1,1,1,1,1,0,3,0,0,0},
							{0,4,4,5,6,6,7,7,7,7,7,6,6,6,5,4,0},
							{0,8,5,5,5,7,7,7,7,7,7,7,5,5,5,9,0},
							{0,0,0,8,5,5,5,5,5,5,5,5,5,9,0,0,0},
							{0,3,0,0,0,8,5,5,5,5,5,9,0,0,0,0,0},
							{0,3,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0},
							{0,3,0,0,0,0,0,0,0,0,0,0,0,0,3,0,3},
							{6,6,0,3,0,0,0,0,0,0,0,0,3,0,3,6,6},
							{7,6,6,6,0,3,0,0,0,0,0,0,3,6,6,6,7},
							{7,7,7,6,6,3,0,0,0,0,0,0,6,6,7,7,7},
							{7,7,7,7,6,6,0,0,0,0,0,6,6,7,7,7,7}
						};
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _structure.GetLength(1));
						PosY -= (int)(.5f * _structure.GetLength(0));
						//i = vertical, j = horizontal
						for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
						{
							for (int i = 0; i < _structure.GetLength(0); i++)
							{
								for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
								{
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30))
									{
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_structure[i, j])
										{
											case 0:
												if (confirmPlatforms == 0)
												{
													tile.active(false);
													tile.halfBrick(false);
													tile.slope(0);
												}
												break;
											case 1:
												tile.active(true);
												tile.type =  (ushort)mod.TileType("PyramidSlabTile");
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 2:
												if (confirmPlatforms == 1)
												{
													tile.active(false);
													tile.slope(0);
													tile.halfBrick(false);
													WorldGen.PlaceTile(k, l,  (ushort)mod.TileType("PyramidChestTile"), true, true, -1, 0);
												}
												break;
											case 3:
												tile.active(true);
												tile.type = 170;
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 4:
												tile.active(true);
												tile.type = 189;
												tile.slope(0);
												tile.halfBrick(true);
												break;
											case 5:
												tile.active(true);
												tile.type = 189;
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 6:
												tile.active(true);
												tile.type = 2;
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 7:
												tile.active(true);
												tile.type = 0;
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 8:
												tile.active(true);
												tile.type = 189;
												tile.slope(4);
												tile.halfBrick(false);
												break;
											case 9:
												tile.active(true);
												tile.type = 189;
												tile.slope(3);
												tile.halfBrick(false);
												break;
										}
									}
								}
							}
						}
					}
					if(variation == 11)
                    {
						int[,] _structure = {
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,1,0,0,0,0,0,0,0,0,0,0,0,2,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,3,0,0,0,0,0,0,0,0,0,0,4,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0},
							{0,0,0,0,0,0,0,0,5,0,0,0,0,0,7,8,9},
							{10,0,0,11,12,12,0,13,13,13,0,0,6,0,14,8,9},
							{10,10,11,15,0,0,0,0,0,0,0,0,8,16,0,8,9},
							{10,12,15,11,12,17,0,0,0,0,0,0,8,8,0,8,9},
							{12,15,0,12,12,12,0,0,0,0,0,0,8,8,0,8,8},
							{12,0,0,12,10,12,0,0,0,0,0,9,8,8,0,0,8},
							{12,18,0,12,20,10,0,0,0,0,0,9,20,8,19,0,8},
							{12,12,12,12,10,10,0,0,0,0,0,9,8,8,8,8,8}
						};
						int PosX = x;
						int PosY = y;
						PosX -= (int)(.5f * _structure.GetLength(1));
						PosY -= (int)(.5f * _structure.GetLength(0));
						//i = vertical, j = horizontal
						for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
						{
							for (int i = 0; i < _structure.GetLength(0); i++)
							{
								for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
								{
									int k = PosX + j;
									int l = PosY + i;
									if (WorldGen.InWorld(k, l, 30))
									{
										Tile tile = Framing.GetTileSafely(k, l);
										switch (_structure[i, j])
										{
											case 0:
												if (confirmPlatforms == 0)
												{
													tile.active(false);
													tile.halfBrick(false);
													tile.slope(0);
												}
												break;
											case 1:
												if (confirmPlatforms == 1)
												{
													tile.active(false);
													tile.slope(0);
													tile.halfBrick(false);
													WorldGen.PlaceTile(k, l, 240, true, true, -1, 12);
												}
												break;
											case 2:
												if (confirmPlatforms == 1)
												{
													tile.active(false);
													tile.slope(0);
													tile.halfBrick(false);
													WorldGen.PlaceTile(k, l, 240, true, true, -1, 13);
												}
												break;
											case 3:
												if (confirmPlatforms == 1)
												{
													tile.active(false);
													tile.slope(0);
													tile.halfBrick(false);
													WorldGen.PlaceTile(k, l, 242, true, true, -1, 13);
												}
												break;
											case 4:
												if (confirmPlatforms == 1)
												{
													tile.active(false);
													tile.slope(0);
													tile.halfBrick(false);
													WorldGen.PlaceTile(k, l, 242, true, true, -1, 8);
												}
												break;
											case 5:
												if (confirmPlatforms == 1)
												{
													tile.active(false);
													tile.slope(0);
													tile.halfBrick(false);
													WorldGen.PlaceTile(k, l, 243, true, true, -1, 0);
												}
												break;
											case 6:
												tile.active(true);
												tile.type = 25;
												tile.slope(0);
												tile.halfBrick(true);
												break;
											case 7:
												tile.active(true);
												tile.type = 25;
												tile.slope(2);
												tile.halfBrick(false);
												break;
											case 8:
												tile.active(true);
												tile.type = 25;
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 9:
												tile.active(true);
												tile.type = 23;
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 10:
												tile.active(true);
												tile.type = 199;
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 11:
												tile.active(true);
												tile.type = 203;
												tile.slope(2);
												tile.halfBrick(false);
												break;
											case 12:
												tile.active(true);
												tile.type = 203;
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 13:
												if (confirmPlatforms == 0)
													tile.active(false);
												WorldGen.PlaceTile(k, l, 19, true, true, -1, 0);
												tile.slope(0);
												tile.halfBrick(false);
												break;
											case 14:
												tile.active(true);
												tile.type = 25;
												tile.slope(4);
												tile.halfBrick(false);
												break;
											case 15:
												tile.active(true);
												tile.type = 203;
												tile.slope(3);
												tile.halfBrick(false);
												break;
											case 16:
												tile.active(true);
												tile.type = 25;
												tile.slope(1);
												tile.halfBrick(false);
												break;
											case 17:
												tile.active(true);
												tile.type = 203;
												tile.slope(1);
												tile.halfBrick(false);
												break;
											case 18:
												if (confirmPlatforms == 1)
												{
													tile.active(false);
													tile.slope(0);
													tile.halfBrick(false);
													WorldGen.PlaceTile(k, l, (ushort)mod.TileType("CrystalStatue"), true, true, -1, 0);
												}
												break;
											case 19:
												if (confirmPlatforms == 1)
												{
													tile.active(false);
													tile.slope(0);
													tile.halfBrick(false);
													WorldGen.PlaceTile(k, l, (ushort)mod.TileType("ManaStatue"), true, true, -1, 0);
												}
												break;
											case 20:
												tile.active(true);
												tile.type = 0;
												tile.slope(0);
												tile.halfBrick(false);
												break;
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}
}