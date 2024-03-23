using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Linq;
using SOTS.Items.Pyramid.PyramidWalls;
using SOTS.Items.Pyramid;
using Terraria;
using SOTS.Items.Gems;
using Terraria.ID;

namespace SOTS
{
	public class SOTSWall : GlobalWall
	{
		public static int[] unsafePyramidWall;
		public static void LoadArrays() //called in SOTS.Load()
        {
            unsafePyramidWall = new int[] { WallType<UnsafePyramidWallWall>(), WallType<UnsafeCursedTumorWallWall>(), WallType<UnsafePyramidBrickWallWall>(), WallType<UnsafeOvergrownPyramidWallWall>(), WallType<UnsafeAcediaWallWall>() }; //Unsafe wall items
        }
        public override bool CanExplode(int i, int j, int type)
        {
            if (unsafePyramidWall.Contains(type) || Framing.GetTileSafely(i, j).TileType == TileType<SOTSGemLockTiles>())
                return false;
            return base.CanExplode(i, j, type);
        }
        public override void KillWall(int i, int j, int type, ref bool fail)
        {
            if (unsafePyramidWall.Contains(type))
                fail = !SOTSWorld.downedCurse;
            if (Framing.GetTileSafely(i, j).TileType == TileType<SOTSGemLockTiles>())
                fail = true;
        }
    }
}