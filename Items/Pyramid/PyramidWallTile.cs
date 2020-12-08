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
			drop = mod.ItemType("PyramidWall");
			AddMapEntry(new Color(100, 85, 52));
		}
		public override bool CanExplode(int i, int j) {
			return false;
		}
		public override void KillWall(int i, int j, ref bool fail) 
		{
			fail = true;
			
			if(SOTSWorld.downedCurse)
			fail = false;
		
		}
	}
	public class OvergrownPyramidWallWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = false;
			dustType = DustID.Grass;
			drop = mod.ItemType("OvergrownPyramidWall");
			AddMapEntry(new Color(20, 90, 60));
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void KillWall(int i, int j, ref bool fail)
		{
			fail = true;
			if (SOTSWorld.downedCurse && (NPC.downedMechBoss1 || NPC.downedMechBoss2 || NPC.downedMechBoss3))
				fail = false;
		}
	}
}