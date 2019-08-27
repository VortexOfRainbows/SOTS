using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class PyramidWallTile : ModWall
	{
		
		public override void SetDefaults()
		{
			Main.wallLargeFrames[Type] = (byte) 1;
			Main.wallHouse[Type] = false;
			dustType = 32;
			drop = ItemID.SandstoneBrickWall;
			AddMapEntry(new Color(100, 85, 52));
		}
		public override bool CanExplode(int i, int j) {
			return false;
		}
		public override void KillWall(int i, int j, ref bool fail) 
		{
			if(SOTSWorld.downedCurse)
			fail = false;
		
			fail = true;
		}
	}
}