using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Secrets.IceCream
{
	public class IceCreamBrickWallTile : ModWall
	{
		
		public override void SetDefaults()
		{
			Main.wallLargeFrames[Type] = (byte) 1;
			Main.wallHouse[Type] = true;
			dustType = 72;
			drop = mod.ItemType("IceCreamBrickWall");
			AddMapEntry(new Color(219, 112, 147));
		}
	}
}