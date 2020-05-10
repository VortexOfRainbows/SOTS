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
	public class WorldgenPaste : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Worldgen Paste");
			Tooltip.SetDefault("Development tool, NOT MEANT FOR GAMEPLAY");
		}
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 34;
			item.useTime = 12;
			item.useAnimation = 12;
			item.useStyle = 3;
			item.value = 0;
			item.rare = 9;
			item.UseSound = SoundID.Item1;
		}
		public override void HoldItem(Player player)
		{
			player.rulerGrid = true;
		}
		public void PasteStructure()
		{
			Vector2 mousePos = Main.MouseWorld;
			Vector2 tileLocation = mousePos / 16f;
			int spawnX = (int)tileLocation.X;
			int spawnY = (int)tileLocation.Y;
			int[,] _structure = {
				{0,0,0,1,1,0,0,0,1,1,1,1,1},
				{2,3,2,1,1,2,3,2,1,1,1,1,1},
				{1,3,1,1,1,1,3,1,1,1,1,4,1},
				{1,3,1,1,1,1,3,1,1,1,1,6,1},
				{1,3,1,1,1,1,3,1,1,5,1,1,1},
				{0,0,0,6,6,0,0,0,0,0,0,0,0},
				{1,0,1,1,1,1,1,1,1,7,7,0,1},
				{0,0,1,1,1,1,1,1,1,6,6,0,0},
				{1,1,1,1,4,7,1,1,1,1,1,1,1},
				{1,1,1,1,1,1,1,1,1,1,1,1,1},
				{1,8,1,1,9,1,10,1,1,1,1,8,1},
				{0,0,0,0,0,0,0,0,0,0,0,0,0}
			};
			int PosX = spawnX - 0;  //spawnX and spawnY is where you want the anchor to be when this generates
			int PosY = spawnY - 0;
			//i = vertical, j = horizontal
			for (int confirmPlatforms = 0; confirmPlatforms < 3; confirmPlatforms++)    //This code is run twice to make sure gravity based objects and platforms are placed correctly, because the code runs the array top to bottom, it also helps with multi-blocks in general
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
									tile.type = 30;
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
									if (confirmPlatforms == 1)
									{
										tile.active(false);
										tile.slope(0);
										tile.halfBrick(false);
										WorldGen.PlaceTile(k, l, 91, true, true, -1, 0);
									}
									break;
								case 3:
									tile.active(true);
									tile.type = 124;
									tile.slope(0);
									tile.halfBrick(false);
									break;
								case 4:
									if (confirmPlatforms == 0)
										tile.active(false);
									WorldGen.PlaceTile(k, l, 33, true, true, -1, 0);
									tile.slope(0);
									tile.halfBrick(false);
									break;
								case 5:
									if (confirmPlatforms == 1)
									{
										tile.active(false);
										tile.slope(0);
										tile.halfBrick(false);
										WorldGen.PlaceTile(k, l, 376, true, true, -1, 0);
									}
									break;
								case 6:
									if (confirmPlatforms == 0)
										tile.active(false);
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 0);
									tile.slope(0);
									tile.halfBrick(false);
									break;
								case 7:
									if (confirmPlatforms == 0)
										tile.active(false);
									WorldGen.PlaceTile(k, l, 50, true, true, -1, 0);
									tile.slope(0);
									tile.halfBrick(false);
									break;
								case 8:
									if (confirmPlatforms == 1)
									{
										tile.active(false);
										tile.slope(0);
										tile.halfBrick(false);
										WorldGen.PlaceTile(k, l, 10, true, true, -1, 0);
									}
									break;
								case 9:
									if (confirmPlatforms == 1)
									{
										tile.active(false);
										tile.slope(0);
										tile.halfBrick(false);
										WorldGen.PlaceTile(k, l, 14, true, true, -1, 0);
									}
									break;
								case 10:
									if (confirmPlatforms == 1)
									{
										tile.active(false);
										tile.slope(0);
										tile.halfBrick(false);
										WorldGen.PlaceTile(k, l, 15, true, true, -1, 0);
									}
									break;
							}
						}
					}
				}
			}
		}
		public override bool UseItem(Player player)
		{
			PasteStructure();
            return true;
		}
	}
}