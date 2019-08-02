using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Items.Secrets.IceCream
{
	public class IceCreamOreTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			minPick = 80; 
			dustType = 72;
			drop = mod.ItemType("IceCreamOre");
			AddMapEntry(new Color(255, 192, 203));
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 2.5f;
			g = 1.9f;
			b = 2.1f;
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			if(Main.rand.Next(180) == 0)
			{
				int cloneDirection = Main.rand.Next(4);
			if(cloneDirection == 0 && Main.tile[i + 1, j].type != mod.TileType("IceCreamBrickTile") && Main.tile[i + 1, j].type != mod.TileType("IceCreamDoorClosed") && Main.tile[i + 1, j].type != mod.TileType("IceCreamDoorOpen") && Main.tile[i + 1, j].type != mod.TileType("IceCreamPlatformTile"))
			{
			WorldGen.PlaceTile(i + 1, j, mod.TileType("IceCreamOreTile"));
			Main.tile[i + 1, j].type = (ushort)mod.TileType("IceCreamOreTile");
			}
			if(cloneDirection == 1 && Main.tile[i - 1, j].type != mod.TileType("IceCreamBrickTile") && Main.tile[i - 1, j].type != mod.TileType("IceCreamDoorClosed") && Main.tile[i - 1, j].type != mod.TileType("IceCreamDoorOpen") && Main.tile[i - 1, j].type != mod.TileType("IceCreamPlatformTile"))
			{
			WorldGen.PlaceTile(i - 1, j, mod.TileType("IceCreamOreTile"));
			Main.tile[i - 1, j].type = (ushort)mod.TileType("IceCreamOreTile");
			}
			if(cloneDirection == 2 && Main.tile[i, j + 1].type != mod.TileType("IceCreamBrickTile") && Main.tile[i, j + 1].type != mod.TileType("IceCreamDoorClosed") && Main.tile[i, j + 1].type != mod.TileType("IceCreamDoorOpen") && Main.tile[i, j + 1].type != mod.TileType("IceCreamPlatformTile"))
			{
			WorldGen.PlaceTile(i, j + 1, mod.TileType("IceCreamOreTile"));
			Main.tile[i, j + 1].type = (ushort)mod.TileType("IceCreamOreTile");
			}
			if(cloneDirection == 3 && Main.tile[i, j - 1].type != mod.TileType("IceCreamBrickTile") && Main.tile[i, j - 1].type != mod.TileType("IceCreamDoorClosed") && Main.tile[i, j - 1].type != mod.TileType("IceCreamDoorOpen") && Main.tile[i, j - 1].type != mod.TileType("IceCreamPlatformTile"))
			{
			WorldGen.PlaceTile(i, j - 1, mod.TileType("IceCreamOreTile"));
			Main.tile[i, j - 1].type = (ushort)mod.TileType("IceCreamOreTile");
			}
			
			}
			return true;
		}
		public override void RandomUpdate(int i, int j)
		{
			for(int somethingidk = 0; somethingidk < 3; somethingidk++)
			{
			int cloneDirection = Main.rand.Next(4);
			if(cloneDirection == 0 && Main.tile[i + 1, j].type != mod.TileType("IceCreamBrickTile") && Main.tile[i + 1, j].type != mod.TileType("IceCreamDoorClosed") && Main.tile[i + 1, j].type != mod.TileType("IceCreamDoorOpen") && Main.tile[i + 1, j].type != mod.TileType("IceCreamPlatformTile"))
			{
			WorldGen.PlaceTile(i + 1, j, mod.TileType("IceCreamOreTile"));
			Main.tile[i + 1, j].type = (ushort)mod.TileType("IceCreamOreTile");
			}
			if(cloneDirection == 1 && Main.tile[i - 1, j].type != mod.TileType("IceCreamBrickTile") && Main.tile[i - 1, j].type != mod.TileType("IceCreamDoorClosed") && Main.tile[i - 1, j].type != mod.TileType("IceCreamDoorOpen") && Main.tile[i - 1, j].type != mod.TileType("IceCreamPlatformTile"))
			{
			WorldGen.PlaceTile(i - 1, j, mod.TileType("IceCreamOreTile"));
			Main.tile[i - 1, j].type = (ushort)mod.TileType("IceCreamOreTile");
			}
			if(cloneDirection == 2 && Main.tile[i, j + 1].type != mod.TileType("IceCreamBrickTile") && Main.tile[i, j + 1].type != mod.TileType("IceCreamDoorClosed") && Main.tile[i, j + 1].type != mod.TileType("IceCreamDoorOpen") && Main.tile[i, j + 1].type != mod.TileType("IceCreamPlatformTile"))
			{
			WorldGen.PlaceTile(i, j + 1, mod.TileType("IceCreamOreTile"));
			Main.tile[i, j + 1].type = (ushort)mod.TileType("IceCreamOreTile");
			}
			if(cloneDirection == 3 && Main.tile[i, j - 1].type != mod.TileType("IceCreamBrickTile") && Main.tile[i, j - 1].type != mod.TileType("IceCreamDoorClosed") && Main.tile[i, j - 1].type != mod.TileType("IceCreamDoorOpen") && Main.tile[i, j - 1].type != mod.TileType("IceCreamPlatformTile"))
			{
			WorldGen.PlaceTile(i, j - 1, mod.TileType("IceCreamOreTile"));
			Main.tile[i, j - 1].type = (ushort)mod.TileType("IceCreamOreTile");
			}
			}
	
		}	
	}
}