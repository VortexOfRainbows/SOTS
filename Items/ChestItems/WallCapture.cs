using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.ChestItems
{
	public class WallCapture : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wall Capture");
			Tooltip.SetDefault("Development tool, NOT MEANT FOR GAMEPLAY\nHas many errors that I'm too lazy to fix currently\nDon't bother using this item");
			/*
				"\nCaptures a structure and exports code to generate it to the client log" +
				"\nClick to select points, click the same point twice to reset the structure" +
				"\nThe third click finishes the capture\nCan only capture top left to bottom right" +
				"\nGemspark blocks can be used to indicate for generating anchor, without them, the default anchor is the top left" +
				"\nSome multiblocks, like ambience objects, pots, trees, cacti, and more, will have to be inputted manually to properly spawn" +
				"\nSimply use a block as an indicator for these situations, then fill out the block placing method afterwards" +
				"\nLiquids will only capture in air blocks; no liquid filled blocks, though you can change that manually" +
				"\nRight clicking pastes the structure with the anchor on your cursor" +
				"\nMake sure to test the structure out before copying the export, also make sure all doors are closed" +
				"\nModded blocks need their IDs determined by reference, using mod.TileType(\" \") or mod.TileType<>, so change the int IDs when using the array" +
				"\nBest used with simple structures");
			*/
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 44;
			item.useTime = 12;
			item.useAnimation = 12;
			item.useStyle = 3;
			item.value = 0;
			item.rare = 9;
			item.UseSound = SoundID.Item1;
		}
		List<double> tiles = new List<double>(); //I store extra data as decimals because I'm lazy
		readonly int[] gemspark = {153,154,155,156,157,158,159,160,161,162,163,164,165,166};
		Vector2 point1 = new Vector2(-1, 0);
		Vector2 point2 = new Vector2(-1, 0);
		Vector2 anchor = new Vector2(0, 0);
		int[,] _structure;
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override void HoldItem(Player player)
		{
			player.rulerGrid = true;
			if (!point1.Equals(new Vector2(-1, 0)))
			{
				Projectile.NewProjectile(point1.X * 16 + 8, point1.Y * 16 + 8, 0, 0, mod.ProjectileType("WorldgenCapture_Highlight"), 0, 0, 0, point2.X, point2.Y); //yet again, I store it as a projectile because of laziness, player value is 0 to prevent multiplayer spawns
			}
		}
		public void PasteStructure(int[,] structure)
		{
			Vector2 mousePos = Main.MouseWorld;
			Vector2 tileLocation = mousePos / 16f;
			int PosX = (int)tileLocation.X - (int)anchor.X;	//spawnX and spawnY is where you want the anchor to be when this generates\n";
			int PosY = (int)tileLocation.Y - (int)anchor.Y;
			//i = vertical, j = horizontal\n";
			for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)	//This code is run multiple times to make sure gravity based objects and platforms are placed correctly, because the code runs the array top to bottom, it also helps with multi-blocks in general, reduce this number down if no multiblocks, like candles, are atop of tables or platforms";
			{
				for (int i = 0; i < structure.GetLength(0); i++)
				{
					for (int j = structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + i;
						int l = PosY + j;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							int w = structure[i, j];
							double specialType = tiles[w]; //WWWW.XYZZZZ
							tile.wall = (ushort)specialType;
						}
					}
				}
			}
		}
		public void FindOrigin(Tile tile, int pointX, int pointY) //offsets multiblocks so they anchor properly
		{
			TileObjectData tileData = TileObjectData.GetTileData(tile);
			/* if (tileData != null)
			{
				int width = tileData.Width;
				int height = tileData.Height;
				int style = TileObjectData.GetTileStyle(tile);
				Point16 offset = tileData.Origin;
				int correctX = pointX + offset.X;
				int correctY = pointY + offset.Y;
				if (tile.type == TileID.FishingCrate)
				{
					correctX--;
				}
				if (tile.type == TileID.ClosedDoor)
				{
					correctY++;
				}
				if (tile.type == TileID.TallGateClosed || tile.type == TileID.TallGateOpen)
				{
					correctY--;
				}
				int distX = width + pointX;
				int distY = height + pointY;
				for (int j = pointY; j < distY; j++)
				{
					for (int i = pointX; i < distX; i++)
					{
						Tile checkingTile = Main.tile[i + (int)point1.X, j + (int)point1.Y];
						double specialType = tile.type + (tile.slope() * 0.01) + (tile.halfBrick() ? 0.1 : 0); //This allows tile type to be stored as W in WWWW.XYZZZZ, slope to be stored as X in WWWW.XYZZZZ, and half brick as Y in WWWW.XYZZZZ;

						specialType += style * 0.01 * 0.0001; //this allows tile styles to be stored as Z in WWWW.XYZZZZ;
						if (i >= _structure.GetLength(0) || j >= _structure.GetLength(1) || checkingTile.type != tile.type)
						{
							Main.NewText("Points Reset", 150, 255, 255);
							point1 = new Vector2(-1, 0);
							point2 = new Vector2(-1, 0);
							tiles = new List<double>();
							return;
						}
						if (i == correctX && j == correctY)
						{
							_structure[i, j] = tiles.IndexOf(specialType);
						}
						else
						{
							if (tiles.IndexOf(-1) < 0)
							{
								tiles.Add(-1);
							}
							_structure[i, j] = tiles.IndexOf(-1);
						}
					}
				}
			} */
		}
		bool complete = false;
		public override bool UseItem(Player player)
		{
			#region tile finder
			if(complete)
			{
				if (player.altFunctionUse == 2)
				{
					PasteStructure(_structure);
					Main.NewText("Pasted", 150, 255, 255);
					return true;
				}
				Main.NewText("Points Reset", 150, 255, 255);
				point1 = new Vector2(-1, 0);
				point2 = new Vector2(-1, 0);
				tiles = new List<double>();
				complete = false;
				return true;
			}
			if (player.altFunctionUse == 2)
			{
				Main.NewText("A structure must be copied before pasting", 255, 0, 0);
				return true;
			}
			_structure = null;
			anchor = new Vector2(0, 0);
			Vector2 mousePos = Main.MouseWorld;
			Vector2 tileLocation = mousePos / 16f;
			tileLocation.X = (int)tileLocation.X;
			tileLocation.Y = (int)tileLocation.Y;
			if (tileLocation.Equals(point1) || tileLocation.Equals(point2))
			{
				point1 = new Vector2(-1, 0);
				point2 = new Vector2(-1, 0);
				tiles = new List<double>();
				Main.NewText("Points Reset", 150, 255, 255);
				return true;
			}
			if (point1.Equals(new Vector2(-1, 0)))
			{
				point1 = tileLocation;
				Main.NewText("Point 1 Selected", 150, 255, 255);
				return true;
			}
			if (point2.Equals(new Vector2(-1, 0)))
			{
				point2 = tileLocation;
				if (point1.X > point2.X || point1.Y > point2.Y)
				{
					Main.NewText("Invalid Point 2 Location", 150, 255, 255);
					Main.NewText("Points Reset", 150, 255, 255);
					point1 = new Vector2(-1, 0);
					point2 = new Vector2(-1, 0);
					tiles = new List<double>();
					return true;
				}
				Main.NewText("Point 2 Selected", 150, 255, 255);
				Main.NewText("Reminder: Make sure that multiblocks have all their tiles included", 255, 0, 0);
				return true;
			}
			#endregion

			#region array maker
			int differenceX = 1 + Math.Abs((int)(point1.X - point2.X));
			int differenceY = 1 + Math.Abs((int)(point1.Y - point2.Y));
			_structure = new int[differenceX, differenceY]; //the ACTUAL array itself, this code could definetly be simplified given I made this first, but I didn't realize I'd need it until very late in, and I'm too lazy to simplify the code
			for (int j = 0; j < differenceY; j++) //set every element to an arbitrary value first, this is to make sure the multiblock checking code doesn't override itself
			{
				for (int i = 0; i < differenceX; i++)
				{
					_structure[i, j] = -69;
				}
			}
			for (int j = 0; j < differenceY; j++)
			{
				for (int i = 0; i < differenceX; i++)
				{
					Tile tile = Main.tile[i + (int)point1.X, j + (int)point1.Y];
					if (gemspark.Contains<int>(tile.wall) && anchor.X == 0 && anchor.Y == 0) //sets the anchor to a gemspark block if currently unset
					{
						anchor.X = i;
						anchor.Y = j;
					}
					double specialType = tile.wall;

					/*
					TileObjectData tileData = TileObjectData.GetTileData(tile);
					int style = 0;
					if (tileData != null)
					{
						//Main.NewText("Passed TileDATA", 150, 255, 255);
						style = TileObjectData.GetTileStyle(tile);
						//Main.NewText("Style After TileData: " + style, 0, 255, 0);
					}
					*/
					//specialType += style * 0.01 * 0.0001; //this allows tile styles to be stored as Z in WWWW.XYZZZZ;
					
					/*//Main.NewText("Style Special: " + specialType, 0, 255, 0);
					if (!tile.active())
					{
						specialType = -1; //turns non-active blocks to air
						specialType -= (tile.liquid * 0.0001) + (tile.liquidType() * 0.1); //store liquid amount as X in W.YXXX, liquid type as Y in W.YXXX
					}*/

					if (tiles.IndexOf(specialType) < 0)
					{
						tiles.Add(specialType);
					}
					_structure[i, j] = tiles.IndexOf(specialType);

					/*
					if ((!Main.tileSolid[tile.type] || tileData != null) && tile.active())
					{
						FindOrigin(tile, i, j);
					}
					*/
				}
			}
			#endregion
			string arrayExport = "";
			for (int j = 0; j < _structure.GetLength(1); j++)
			{
				if (j > 0) arrayExport += ",\n";
				arrayExport += "				{";
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					if (i > 0) arrayExport += ",";
					arrayExport += _structure[i, j];
				}
				arrayExport += "}";
			}
			string finalExport = "int[,] _structure = {\n" + arrayExport + "\n			};\n";
            #region final export
            //finalExport += arrayExport;
			finalExport += "int PosX = spawnX -" + (int)anchor.X + ";	//spawnX and spawnY is where you want the anchor to be when this generates\n";
			finalExport += "int PosY = spawnY -" + (int)anchor.Y + ";\n";
			finalExport += "//i = vertical, j = horizontal\n";
			finalExport += "	for (int i = 0; i < _structure.GetLength(0); i++)\n";
			finalExport += "	{\n";
			finalExport += "		for (int j = _structure.GetLength(1) - 1; j >= 0; j--)\n";
			finalExport += "		{\n";
			finalExport += "			int k = PosX + j;\n";
			finalExport += "			int l = PosY + i;\n";
			finalExport += "			if (WorldGen.InWorld(k, l, 30))\n";
			finalExport += "			{\n";
			finalExport += "				Tile tile = Framing.GetTileSafely(k, l);\n";
			finalExport += "				switch (_structure[i, j])\n";
			finalExport += "				{\n";
			for(int i = 0; i < tiles.Count(); i++)
			{
				finalExport += "					case " + i + ":\n";
				double specialType = tiles[i]; //W.YXXX
				finalExport += "							tile.wall = " + specialType + ";\n";
				finalExport += "					break;\n";
			}
			finalExport += "				}\n";
			finalExport += "			}\n";
			finalExport += "		}\n";
			finalExport += "	}\n";
            #endregion 
            #region reset and export
            Main.NewText("Complete", 0, 255, 0);
			mod.Logger.Info("The following is the structure array and its necessary parts\n" + finalExport);
			mod.Logger.InfoFormat("Array Log from SOTS: {0}", mod.Name);
			complete = true;
            #endregion
            return true;
		}
	}
}