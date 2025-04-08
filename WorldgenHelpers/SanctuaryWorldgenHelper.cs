using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using System;
using SOTS.Items.Invidia;
using System.Linq;
using SOTS.Items.Invidia.MoonShard;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SOTS.Items.Gems;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Furniture.Evostone;
using Microsoft.CodeAnalysis;
using SOTS.Items.Furniture.Earthen;
using Terraria.WorldBuilding;

namespace SOTS.WorldgenHelpers
{
    public static class SanctuaryWorldgenHelper
    {
        public static Rectangle Rectangle = SetRect();
        private static ushort PillarWall;
        private static ushort EvostoneWall;
        private static ushort RuneWall;
        private static ushort EvostoneBrick;
        private static ushort Evostone;
        private static ushort OvergrownEvostone;
        private static ushort OvergrownEvostoneBrick;
        private static ushort Rune;
        private static ushort RuneBrick;
        private static ushort EvostoneTable;
        private static ushort EvostoneChair;
        private static ushort EvostonePlatform;
        private static ushort DarkShingles;
        private static ushort InvidiaPlating;
        public static FastNoiseLite genNoise = null;
        public static FastNoiseLite genNoise2 = null;
        public static bool IsGenerating = false;
        public static void InitTypes()
        {
            EvostoneBrick = (ushort)ModContent.TileType<EvostoneBrickTile>();
            RuneWall = (ushort)ModContent.WallType<EvostoneRuneBrickWallTile>();
            EvostoneWall = (ushort)ModContent.WallType<EvostoneBrickWallTile>();
            PillarWall = (ushort)ModContent.WallType<EvostoneGrandPillarWall>();
            Evostone = (ushort)ModContent.TileType<EvostoneTile>();
            OvergrownEvostone = (ushort)ModContent.TileType<OvergrownEvostoneTile>();
            OvergrownEvostoneBrick = (ushort)ModContent.TileType<OvergrownEvostoneBrickTile>();
            Rune = (ushort)ModContent.TileType<RunicEvostoneTile>();
            RuneBrick = (ushort)ModContent.TileType<RunicEvostoneBrickTile>();
            EvostoneTable = (ushort)ModContent.TileType<EvostoneTableTile>();
            EvostoneChair = (ushort)ModContent.TileType<EvostoneChairTile>();
            EvostonePlatform = (ushort)ModContent.TileType<EvostonePlatformTile>();
            DarkShingles = (ushort)ModContent.TileType<DarkShinglesTile>();
            InvidiaPlating = (ushort)ModContent.TileType<InvidiaPlatingTile>();
            if(SpawnPos <= 0)
                SpawnPos = DetermineSpawnLocation();
            Rectangle = SetRect();
        }
        public static int DetermineSpawnLocation()
        {
            // In infernum mod, the jungle is always on the right, forcing the sanctuary to the brimstone crags, where it probably will not conflict with infernum
            // Spooky mod spawns the eye valley on the jungle-side of the world, meaning this should also fix spooky mod compatability
            if (SOTS.SpookyMod != null || SOTS.InfernumMod != null)
                if(GenVars.JungleX > Main.maxTilesX / 2) //Jungle is on the right
                    return Main.maxTilesX * 1 / 8;
            return Main.maxTilesX * 7 / 8;
        }
        public static void GenerateNewEmeraldGemStructure(int x, int y)
        {
            int PosX = x - 23; //spawnX and spawnY is where you want the anchor to be when this generates
            int PosY = y - 29;
            int[,] _structure = new int[,] {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,2,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,2,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,2,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
                {0,0,0,0,0,0,1,1,1,1,1,1,1,1,3,3,3,3,1,1,1,1,1,2,1,1,1,1,1,3,3,3,3,1,1,1,1,1,1,1,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,1,1,1,1,1,1,1,3,3,3,3,1,1,1,1,1,2,1,1,1,1,1,3,3,3,3,1,1,1,1,1,1,1,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,1,1,1,1,1,1,1,3,3,3,3,1,1,1,1,1,2,1,1,1,1,1,3,3,3,3,1,1,1,1,1,1,1,0,0,0,0,0,0,0},
                {0,0,0,0,1,1,1,1,1,1,1,1,1,1,3,3,3,3,1,1,1,1,1,2,1,1,1,1,1,3,3,3,3,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
                {0,0,0,0,1,1,1,1,1,1,1,1,1,1,3,3,3,3,1,1,1,1,1,2,1,1,1,1,1,3,3,3,3,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
                {0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
                {0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
                {0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
                {0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
                {0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
                {0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
                {0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0}
            };
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
                            case 1:
                                tile.WallType = (ushort)ModContent.WallType<EvostoneBrickWallTile>();
                                break;
                            case 2:
                                tile.WallType = 156;
                                break;
                            case 3:
                                tile.WallType = 91;
                                break;
                        }
                    }
                }
            }
            _structure = new int[,] {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 2, 2, 1, 1, 1, 2, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 5, 6, 7,-1,-1,-1,-1,-1, 5, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 3, 0, 5, 5, 6,-1,-1,-1,-1,-1, 9, 5, 5, 0, 4, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 1, 1, 2, 2, 0, 5, 5,-1,-1,-1, 8,-1,-1, 6, 5, 5, 0, 2, 2, 1, 1, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 3, 0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 5, 5, 6,-1,-1,-1,-1,-1, 6, 5, 5, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0, 4, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 5, 5, 6, 6,10,-1,-1, 6, 6, 5, 5, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 6,-1, 6, 6, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 4, 0, 0, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 6, 6, 6, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 0, 0, 3, 0, 0, 0, 0, 0},
                { 0, 0, 3, 0, 1, 2, 2, 2, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 6, 6, 6, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 2, 2, 2, 1, 0, 4, 0, 0},
                { 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 5, 5, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 5, 5, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0},
                { 0, 0,11, 2, 5, 5, 5, 5, 5, 5, 5, 5, 7,12, 6, 6, 6, 7,12,13, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,12,13, 6, 6, 5, 5, 5, 5, 5, 5, 5, 5, 2,14, 0, 0},
                { 0, 0, 0, 0, 5, 5, 5, 5, 5, 5, 5, 5,-1,-1,-1, 6,12,-1,-1,-1,-1,-1, 6, 6, 7,12,13, 6, 6,-1,-1,-1,-1,12, 6, 5, 5, 5, 5, 5, 5, 5, 5, 0, 0, 0, 0},
                { 3, 0, 1, 2, 5, 5, 5, 5, 5, 5, 5, 5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 6,-1,-1,-1,-1,-1,-1,-1, 5, 5, 5, 5, 5, 5, 5, 5, 2, 1, 0, 4},
                { 2, 2, 2, 2, 2, 2, 5, 5,17,17,17,17,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,17,17,17,17, 5, 5, 2, 2, 2, 2, 2, 2},
                {11, 5, 5, 5, 5, 5, 5, 5,21,21,21,21,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,21,21,21,21, 5, 5, 5, 5, 5, 5, 5,14},
                { 0, 5, 5, 5, 5, 5, 5, 5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 5, 5, 5, 5, 5, 5, 5, 0},
                { 0, 5, 5, 5, 5, 5, 5, 5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 5, 5, 5, 5, 5, 5, 5, 0},
                { 0, 5, 5, 5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,15,-1,-1,-1,-1,-1,-1,-1,22,-1,-1,-1,-1,-1,-1,16,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 5, 5, 5, 0},
                { 0, 5, 5, 5,-1,-1,-1,-1,-1,-1,-1,23,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,25,-1,-1,-1,-1,-1,-1,-1, 5, 5, 5, 0},
                { 0, 5, 5, 5,19,19,-1,-1,-1,-1,23,-1,24,24,24, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5,24,24,24,-1,25,-1,-1,-1,-1,19,19, 5, 5, 5, 0},
                { 0, 5, 5, 5,18,19,-1,-1,-1,23,-1,-1,-1,-1,24, 5, 5, 5,27,-1,-1,29, 5, 5, 5,27,-1,-1,29, 5, 5, 5,24,-1,-1,-1,-1,25,-1,-1,-1,18,19, 5, 5, 5, 0},
                { 0, 5, 5, 5,24,24,24,21,21,21,21,24,24,24,24, 5, 5, 5,-1,28,-1,-1, 5, 5, 5,-1,30,-1,-1, 5, 5, 5,24,24,24,24,21,21,21,21,24,24,24, 5, 5, 5, 0},
                { 0, 5, 5, 5, 5, 5,24,-1,-1,-1,-1,24, 5, 5, 5, 5, 5, 5, 5,31,31, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5,24,-1,-1,-1,-1,24, 5, 5, 5, 5, 5, 0},
                { 0, 5, 5, 5, 5, 5,24,-4,-2,-2,-3,24, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5,24,-4,-2,-2,-3,24, 5, 5, 5, 5, 5, 0}
            };
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
                                case -4:
                                case -3:
                                case -2:
                                    if (confirmPlatforms == 0)
                                    {
                                        for(int a = 0; a < 22; a++)
                                        {
                                            Tile tile2 = Framing.GetTileSafely(k, l + a);
                                            tile2.HasTile = false;
                                            tile2.IsHalfBlock = false;
                                            tile2.Slope = 0;
                                            tile2.LiquidAmount = 0;
                                            tile2.LiquidType = 0;
                                            tile2.WallType = (ushort)ModContent.WallType<EvostoneBrickWallTile>();
                                            if (_structure[i, j] == -4)
                                            {
                                                tile2 = Framing.GetTileSafely(k - 1, l + a);
                                                tile2.TileType = (ushort)ModContent.TileType<InvidiaPlatingTile>();
                                            }
                                            if (_structure[i, j] == -3)
                                            {
                                                tile2 = Framing.GetTileSafely(k + 1, l + a);
                                                tile2.TileType = (ushort)ModContent.TileType<InvidiaPlatingTile>();
                                            }
                                        }
                                    }
                                    break;
                                case -1:
                                    if (confirmPlatforms == 0)
                                    {
                                        tile.HasTile = false;
                                        tile.IsHalfBlock = false;
                                        tile.Slope = 0;
                                        tile.LiquidAmount = 0;
                                        tile.LiquidType = 0;
                                    }
                                    break;
                                case 1:
                                    tile.HasTile = true;
                                    tile.TileType = DarkShingles;
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = true;
                                    break;
                                case 2:
                                    tile.HasTile = true;
                                    tile.TileType = DarkShingles;
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 3:
                                    tile.HasTile = true;
                                    tile.TileType = DarkShingles;
                                    tile.Slope = (SlopeType)2;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 4:
                                    tile.HasTile = true;
                                    tile.TileType = DarkShingles;
                                    tile.Slope = (SlopeType)1;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 5:
                                    tile.HasTile = true;
                                    tile.TileType = EvostoneBrick;
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    tile.LiquidAmount = 0;
                                    tile.LiquidType = 0;
                                    break;
                                case 6:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<EvostoneTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 7:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<EvostoneTile>();
                                    tile.Slope = (SlopeType)3;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 8:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<SOTSGemLockTiles>(), true, true, -1, 2);
                                    }
                                    break;
                                case 9:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<EvostoneTile>();
                                    tile.Slope = (SlopeType)2;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 10:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<EvostoneTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = true;
                                    break;
                                case 11:
                                    tile.HasTile = true;
                                    tile.TileType = DarkShingles;
                                    tile.Slope = (SlopeType)4;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 12:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, TileID.ExposedGems, true, true, -1, 3);
                                    }
                                    break;
                                case 13:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<EvostoneTile>();
                                    tile.Slope = (SlopeType)4;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 14:
                                    tile.HasTile = true;
                                    tile.TileType = DarkShingles;
                                    tile.Slope = (SlopeType)3;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 15:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<SerpentStatueTile>(), true, true, -1, 0);
                                    }
                                    break;
                                case 16:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<SerpentStatueTile>(), true, true, -1, 1);
                                    }
                                    break;
                                case 17:
                                    if (confirmPlatforms == 0)
                                        tile.HasTile = false;
                                    WorldGen.PlaceTile(k, l, 50, true, true, -1, WorldGen.genRand.Next(5));
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 18:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        WorldGen.PlaceTile(k, l, TileID.FishingCrate, true, true, -1, 22);
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        tile.TileColor = PaintID.PurplePaint;
                                    }
                                    break;
                                case 19:
                                    if (confirmPlatforms == 0)
                                    {
                                        tile.HasTile = false;
                                        tile.IsHalfBlock = false;
                                        tile.Slope = 0;
                                        tile.LiquidAmount = 0;
                                        tile.LiquidType = 0;
                                    }
                                    tile.TileColor = PaintID.PurplePaint;
                                    break;
                                case 21:
                                    if (confirmPlatforms == 0)
                                        tile.HasTile = false;
                                    WorldGen.PlaceTile(k, l, EvostonePlatform, true, true, -1, 0);
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 22:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, 354, true, true, -1, 0);
                                    }
                                    break;
                                case 23:
                                    if (confirmPlatforms == 0)
                                        tile.HasTile = false;
                                    WorldGen.PlaceTile(k, l, EvostonePlatform, true, true, -1, 0);
                                    tile.Slope = (SlopeType)2;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 24:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<InvidiaPlatingTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 25:
                                    if (confirmPlatforms == 0)
                                        tile.HasTile = false;
                                    WorldGen.PlaceTile(k, l, EvostonePlatform, true, true, -1, 0);
                                    tile.Slope = (SlopeType)1;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 27:
                                    tile.HasTile = true;
                                    tile.TileType = EvostoneBrick;
                                    tile.Slope = (SlopeType)3;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 28:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        tile.LiquidAmount = 0;
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<GemChestTile>(), true, true, -1, 5);
                                    }
                                    break;
                                case 29:
                                    tile.HasTile = true;
                                    tile.TileType = EvostoneBrick;
                                    tile.Slope = (SlopeType)4;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 30:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        tile.LiquidAmount = 0;
                                        tile.LiquidType = 0;
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<RuinedChestTile>(), true, true, -1, 1);
                                    }
                                    break;
                                case 31:
                                    tile.HasTile = true;
                                    tile.TileType = 265;
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                            }
                        }
                    }
                }
            }
        }
        public static Texture2D pillarTexture;
        //public static readonly int[] xPos = [-339, -230, -119, 0, 119, 230, 339];
        public static readonly int[] PillarPos = [-392, - 339, -286, -176, -119, -62, 62, 119, 176, 286, 339, 392];
        public static void DrawPillars()
        {
            if (pillarTexture == null)
                pillarTexture = ModContent.Request<Texture2D>("SOTS/Items/Invidia/SanctuaryPillar", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Vector2 zero = Main.drawToScreen ? zero = Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);
            //Main.spriteBatch.Draw(pillarTexture, drawPos, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
            int sizeX = pillarTexture.Width / 16;
            int sizeY = pillarTexture.Height / 16;
            int endY = Bottom - 40;
            Vector2 screenCenter = Main.screenPosition + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2) - zero / 2;
            float cullDist = Main.screenWidth;
            float cullDistY = Main.screenHeight;
            for (int b = 0; b < PillarPos.Length; b++)
            {
                int posX = SideOfWorld - sizeX / 2 + PillarPos[b];
                //Main.NewText(MathF.Abs(screenCenter.X - posX * 16));
                if(MathF.Abs(screenCenter.X - posX * 16) < cullDist)
                {
                    for (int posY = Ceiling - 15; posY < endY; posY += 4)
                    {
                        if (MathF.Abs(screenCenter.Y - posY * 16) < cullDistY)
                        {
                            for (int y = 0; y < sizeY; ++y)
                            {
                                for (int x = 0; x < sizeX; ++x)
                                {
                                    int i = posX + x;
                                    int j = posY + y;
                                    Vector2 drawPos = new Vector2(i, j) * 16 + zero - Main.screenPosition;
                                    Vector3[] slices = new Vector3[9];
                                    Lighting.GetColor9Slice(i, j, ref slices);
                                    Vector3 vector = Lighting.GetColor(i, j).ToVector3();
                                    Vector3 tileLight;
                                    Vector2 position;
                                    Color color = new Color();
                                    Rectangle value = new Rectangle();
                                    for (int a = 0; a < 9; a++)
                                    {
                                        value.X = 0;
                                        value.Y = 0;
                                        value.Width = 4;
                                        value.Height = 4;
                                        switch (a)
                                        {
                                            case 1:
                                                value.Width = 8;
                                                value.X = 4;
                                                break;
                                            case 2:
                                                value.X = 12;
                                                break;
                                            case 3:
                                                value.Height = 8;
                                                value.Y = 4;
                                                break;
                                            case 4:
                                                value.Width = 8;
                                                value.Height = 8;
                                                value.X = 4;
                                                value.Y = 4;
                                                break;
                                            case 5:
                                                value.X = 12;
                                                value.Y = 4;
                                                value.Height = 8;
                                                break;
                                            case 6:
                                                value.Y = 12;
                                                break;
                                            case 7:
                                                value.Width = 8;
                                                value.Height = 4;
                                                value.X = 4;
                                                value.Y = 12;
                                                break;
                                            case 8:
                                                value.X = 12;
                                                value.Y = 12;
                                                break;
                                        }
                                        //value.Y += glowOffset.Y;
                                        position.X = drawPos.X + value.X;
                                        position.Y = drawPos.Y + value.Y;
                                        value.X += x * 16;
                                        value.Y += y * 16;
                                        tileLight.X = (slices[a].X + vector.X) * 0.5f;
                                        tileLight.Y = (slices[a].Y + vector.Y) * 0.5f;
                                        tileLight.Z = (slices[a].Z + vector.Z) * 0.5f;
                                        int num = (int)(tileLight.X * 255f);
                                        int num2 = (int)(tileLight.Y * 255f);
                                        int num3 = (int)(tileLight.Z * 255f);
                                        if (num > 255)
                                            num = 255;
                                        if (num2 > 255)
                                            num2 = 255;
                                        if (num3 > 255)
                                            num3 = 255;
                                        num3 <<= 16;
                                        num2 <<= 8;
                                        color.PackedValue = (uint)(num | num2 | num3) | 0xFF000000u;
                                        Main.spriteBatch.Draw(pillarTexture, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public static void SetNoise()
        {
            genNoise = new FastNoiseLite();
            genNoise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
            genNoise.SetFractalType(FastNoiseLite.FractalType.PingPong);
            genNoise.SetCellularDistanceFunction(FastNoiseLite.CellularDistanceFunction.EuclideanSq);
            genNoise.SetSeed(WorldGen.genRand.Next(500, 1500));

            genNoise2 = new FastNoiseLite();
            genNoise2.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
            genNoise2.SetFractalType(FastNoiseLite.FractalType.FBm);
            genNoise2.SetCellularDistanceFunction(FastNoiseLite.CellularDistanceFunction.EuclideanSq);
            genNoise2.SetSeed(WorldGen.genRand.Next(500, 1500));
        }
        public static int Ceiling => Main.UnderworldLayer;
        public static int Bottom => Main.maxTilesY - 1;
        public static int UnderworldHeight => Main.UnderworldLayer + 65;
        public static int SideOfWorld => SpawnPos <= 0 ? (SpawnPos = DetermineSpawnLocation()) : SpawnPos;
        public static int SpawnPos = -1;
        public static void PrepareUnderworldArea(int x, int y, int endX, int endY, int style = 0, int heightCutoff = 0)
        {
            ushort Evostone = (ushort)ModContent.TileType<EvostoneTile>();
            ushort OvergrownEvostone = (ushort)ModContent.TileType<OvergrownEvostoneTile>();
            float height = endY - y;
            float length = endX - x;
            int rightSide = endX;
            int bottom = Bottom;
            if (style < 0)
            {
                x += (int)length / 2;
                if (style == -2)
                {
                    bottom -= (int)(height / 2);
                }
            }
            if (style > 0)
            {
                rightSide -= (int)length / 2;
                if (style == 2)
                {
                    bottom -= (int)(height / 2);
                }
            }

            for (int i = x; i <= rightSide; i++)
            {
                float percentX = 1f - (endX - i) / length;
                float sinX = MathF.Sin(percentX * MathF.PI) + .1f;
                float randHeightMult = WorldGen.genRand.NextFloat(1 - 0.04f * sinX, 1 + 0.04f * sinX);
                int h = WorldGen.genRand.Next(3);
                for (int j = y; j <= bottom; j++)
                {
                    float percentY = 1f - (endY - j) / height;
                    float distX = MathF.Abs(percentX - .5f) * 2f;
                    float distY = MathF.Abs(percentY - .5f) * 2f;
                    float d = distX * distX + distY * distY; //Don't have to take sqrt because we are comparing it to 1
                    Tile t = Main.tile[i, j];
                    if (d < 1f && style == 0)
                    {
                        t.ClearEverything();
                    }
                    if (d > 0.9f * randHeightMult && j > UnderworldHeight + h + heightCutoff)
                    {
                        t.HasTile = true;
                        t.TileType = Evostone;
                        t.LiquidAmount = 0;
                    }
                }
            }
        }
        public static Rectangle SetRect()
        {
            int size = 5;
            int spread = 80;
            int left = SideOfWorld - size * spread;
            int right = SideOfWorld + size * spread;
            left -= 10;
            right += 10;
            Rectangle rect = new Rectangle(left - 10, Ceiling - 10, right - left + 20, Bottom - Ceiling + 20);
            return rect;
        }
        public static void GenerateSanctuary()
        {
            IsGenerating = true;
            InitTypes();
            int size = 5;
            int spread = 80;
            int left = SideOfWorld - size * spread;
            int right = SideOfWorld + size * spread;
            int outcropSize = 180;
            GenerateRectangle(left, Ceiling + 5, right, Bottom - 50, -1);
            PrepareUnderworldArea(left - 2, Ceiling + 5, right + 2, Bottom - 65, 0);
            left -= 10;
            right += 10;
            PrepareUnderworldArea(left - outcropSize, Ceiling + 20, left, Bottom - 4, -1);
            PrepareUnderworldArea(right, Ceiling + 20, right + outcropSize, Bottom - 4, 1);

            GeneratePillar(SideOfWorld + size * spread, UnderworldHeight);
            GeneratePillar(SideOfWorld - size * spread, UnderworldHeight);

            GenerateRectangle(SideOfWorld - size * spread - 5, Ceiling - 15, SideOfWorld + size * spread + 5, Ceiling + 5);

            for(int i = -3; i <= 3; i ++)
            {
                int x = SideOfWorld + i * 115;
                int centerPillarSize = i == 0 ? 60 : 47;
                int heightOffset = i == 0 ? 25 : WorldGen.genRand.Next(15, 21);
                if(i % 2 == 0)
                {
                    PrepareUnderworldArea(x - centerPillarSize - outcropSize, Ceiling + 60, x - centerPillarSize, Bottom - 4, -2, heightOffset - 2);
                    PrepareUnderworldArea(x + centerPillarSize, Ceiling + 60, x + outcropSize + centerPillarSize, Bottom - 4, 2, heightOffset - 2);
                    GenerateRectangle(x - centerPillarSize, UnderworldHeight + heightOffset, x + centerPillarSize, Bottom, 3);
                    if(i != 0)
                        GenerateTunnelHouse(x, UnderworldHeight + heightOffset);
                }
                else
                {
                    int y = UnderworldHeight;
                    bool centralPlatform = MathF.Abs(i) == 1;
                    if (centralPlatform)
                    {
                        y += WorldGen.genRand.Next(19, 23);
                        x += i * 4;
                    }
                    else
                    {
                        y += WorldGen.genRand.Next(6, 10);
                        x -= i * 2;
                    }
                    GeneratePlatform(x, y, 1);
                    if (centralPlatform)
                        GeneratePlatform(x, UnderworldHeight - 25, 0);
                }
            }

            for(int i = 0; i < PillarPos.Length; i++)
                GenerateRectangle(SideOfWorld + PillarPos[i] - 20, Ceiling - 15, SideOfWorld + PillarPos[i] + 20, Bottom, 4);

            GenerateRectangle(left, UnderworldHeight + 35, right, Bottom, 2);
            GenerateRectangle(right - 150, UnderworldHeight + 23, right, Bottom, 2);
            GenerateRectangle(left, UnderworldHeight + 23, left + 150, Bottom, 2);

            GenerateBottomCorridor();
            for (int i = 0; i < PillarPos.Length; i++)
                if (i == 1 || i == 2 || i == 3 || i == 5 || i == 6 || i == 8 || i == 9 || i == 10)
                    GeneratePillarRoom(SideOfWorld + PillarPos[i] - 19, Ceiling, 39, 20, i);

            left -= outcropSize / 2;
            right += outcropSize / 2;
            WorldGen.PlaceTile(SideOfWorld, Bottom - 45, ModContent.TileType<InvidiaGatewayTile>(), true, true, -1, 0);
            CleanUp(left, right, Ceiling - 30, Bottom, 0);
            CleanUp(left, right, Ceiling - 30, Bottom, 1);
            CleanUp(left, right, Ceiling - 30, Bottom, 2);
            CleanUp(left, right, Ceiling - 30, Bottom, 3);
            CleanUp(left, right, Ceiling - 30, Bottom, 4);
            SOTSWorldgenHelper.SmoothRegion(left / 2 + right / 2, Ceiling / 2 + Bottom / 2, right - left, Bottom - Ceiling, ModContent.TileType<OvergrownEvostoneTile>());
            SOTSWorldgenHelper.SmoothRegion(left / 2 + right / 2, Ceiling / 2 + Bottom / 2, right - left, Bottom - Ceiling, ModContent.TileType<OvergrownEvostoneBrickTile>());

            GenerateImportantChests();

            CleanUp(left, right, Ceiling - 30, Bottom, 5);
            for (int i = 0; i < PillarPos.Length; i++)
            {
                if (i < PillarPos.Length - 1)
                {
                    int nextPillar = PillarPos[i + 1];
                    int rightOfThis = SideOfWorld + PillarPos[i] + 20;
                    int leftOfNext = SideOfWorld + nextPillar - 20;
                    int sizeX = leftOfNext - rightOfThis;
                    float arch = 0.9f;
                    if (sizeX < 20)
                        arch = 0.325f;
                    GenerateGrandArch(rightOfThis, Ceiling + 5, sizeX, 30, arch);
                }
            }
            GenerateNewEmeraldGemStructure(SideOfWorld, Ceiling - 16);

            
            IsGenerating = false;
        }
        public static void GeneratePillar(int i, int j)
        {
            //GenerateRectangle(i - 12, j - 30 - 1, i + 12, j + 1, 1);
            GenerateRectangle(i - 12, Ceiling - 15, i + 12, j - 30);
            GenerateRectangle(i - 12, j, i + 12, Bottom);
        }
        public static void GenerateRectangle(int x, int y, int endX, int endY, int style = 0)
        {
            bool killBlocks = true;
            bool topLayer = style == 3;
            if (style == 3)
                style = 0;
            ushort Invidia = (ushort)ModContent.TileType<InvidiaPlatingTile>();
            ushort Evostone = (ushort)ModContent.TileType<EvostoneTile>();
            ushort OvergrownEvostone = (ushort)ModContent.TileType<OvergrownEvostoneTile>();
            ushort OvergrownEvostoneBrickTile = (ushort)ModContent.TileType<OvergrownEvostoneBrickTile>();
            ushort EvostoneWall = (ushort)ModContent.WallType<EvostoneBrickWallTile>();
            if(style == 4)
            {
                style = 1;
                EvostoneWall = (ushort)ModContent.WallType<EvostoneGrandPillarWall>();
            }
            int entranceLevel = Ceiling + 65;
            for (int j = y; j <= endY; j++)
            {
                if(killBlocks)
                    killBlocks = j < entranceLevel;
                for (int i = x; i <= endX; i++)
                {
                    Tile t = Main.tile[i, j];
                    if(style == 2)
                    {
                        Tile tU = Main.tile[i, j - 3];
                        if(!t.HasTile)
                        {
                            t.LiquidType = LiquidID.Lava;
                            t.LiquidAmount = 255;
                        }
                        if(tU.WallType != PillarWall)
                            tU.WallType = EvostoneWall;
                    }
                    else
                    {
                        if (style == 0 || style == -1)
                        {
                            t.ClearTile();
                            if (style == 0)
                            {
                                if(topLayer && j <= y && i != x && i != endX)
                                    t.TileType = Invidia;
                                else
                                    t.TileType = EvostoneBrick;
                                t.HasTile = true;
                                t.LiquidAmount = 0;
                            }
                            else
                            {
                                t.WallType = WallID.None;
                                continue;
                            }
                        }
                        if (i > x && i < endX && j > y && j < endY)
                        {
                            if(killBlocks && t.TileType != Invidia && t.TileType != EvostoneBrick && t.TileType != OvergrownEvostone && t.TileType != OvergrownEvostoneBrickTile)
                            {
                                t.ClearTile();
                            }
                            t.WallType = EvostoneWall;
                        }
                    }
                }
            }
        }
        public static void GeneratePlatform(int x, int y, int style = 0)
        {
            ushort Invidia = (ushort)ModContent.TileType<InvidiaPlatingTile>();
            ushort Evostone = EvostoneBrick;
            ushort Grass = (ushort)ModContent.TileType<OvergrownEvostoneBrickTile>();
            int[,] _structure;
            if (style == 0)
            {
                _structure = new int[,] {
                    {0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,0,0,3,3,3,4,4},
                    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,4,4,4,4,4,3,3,3,3,4,4,4,5},
                    {1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,3,5,4,4,4,5,3,3,4,4,4,4,4,3},
                    {1,1,1,1,0,0,0,0,9,7,4,0,0,0,0,0,3,5,5,5,3,0,3,5,5,4,4,4,3},
                    {1,1,1,1,1,1,1,0,6,0,0,0,0,0,0,0,0,0,6,0,0,0,0,0,3,5,5,4,4},
                    {1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,6,0,0,0,0,0,0,0,3,5,5},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,6,0,0,0,0,0,0,0,0,6,6},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,6,6},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
                };
            }
            else
            {
                _structure = new int[,] {
                    {0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                    {7,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,0,0,3,3,3,4,4},
                    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,4,4,4,4,4,3,3,3,3,4,4,4,5},
                    {1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,3,5,4,4,4,5,3,3,4,4,4,4,4,3},
                    {1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,3,5,5,5,3,0,3,5,5,4,4,4,3},
                    {1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,6,0,0,0,0,0,3,5,5,4,4},
                    {1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,6,0,0,0,0,0,0,0,3,5,5},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,6,0,0,0,0,0,0,0,0,6,6},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,6,6},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
                };
            }
            int len = _structure.GetLength(1) - 1;
            int PosX = x - len;  //spawnX and spawnY is where you want the anchor to be when this generates
            int PosY = y;
            for (int i = 0; i < _structure.GetLength(0); i++)
            {
                for (int j = len; j >= 0; j--)
                {
                    int k = PosX + j;
                    int k2 = PosX - j + len * 2;
                    int l = PosY + i;
                    if (WorldGen.InWorld(k, l, 30))
                    {
                        Tile tile = Framing.GetTileSafely(k, l);
                        Tile tile2 = Framing.GetTileSafely(k2, l);
                        switch (_structure[i, j])
                        {
                            case 0:
                                tile.HasTile = tile2.HasTile = true;
                                tile.TileType = tile2.TileType = Evostone;
                                tile.Slope = tile2.Slope = 0;
                                tile.IsHalfBlock = tile2.IsHalfBlock = false;
                                break;
                            case 6:
                                tile.HasTile = tile2.HasTile = true;
                                tile.TileType = tile2.TileType = Evostone;
                                tile.Slope = tile2.Slope = 0;
                                tile.IsHalfBlock = tile2.IsHalfBlock = false;
                                tile.IsActuated = tile2.IsActuated = true;
                                break;
                            case 9:
                                tile.HasTile = tile2.HasTile = true;
                                tile.TileType = tile2.TileType = Evostone;
                                tile.Slope = tile2.Slope = 0;
                                tile.IsHalfBlock = tile2.IsHalfBlock = false;
                                tile.IsActuated = tile2.IsActuated = true;
                                if (tile.WallType == 0)
                                    tile.WallType = tile2.WallType = EvostoneWall;
                                break;
                            case 7:
                                tile.HasTile = tile2.HasTile = true;
                                tile.TileType = tile2.TileType = Evostone;
                                tile.Slope = tile2.Slope = 0;
                                tile.IsHalfBlock = tile2.IsHalfBlock = true;
                                break;
                            case 2:
                                tile.HasTile = tile2.HasTile = true;
                                tile.TileType = tile2.TileType = Grass;
                                tile.Slope = tile2.Slope = 0;
                                tile.IsHalfBlock = tile2.IsHalfBlock = false;
                                break;
                            case 3:
                                tile.HasTile = tile2.HasTile = true;
                                tile.TileType = tile2.TileType = Invidia;
                                tile.Slope = tile2.Slope = 0;
                                tile.IsHalfBlock = tile2.IsHalfBlock = false;
                                break;
                            case 4:
                                tile.ClearTile();
                                tile2.ClearTile();
                                if (tile.WallType == 0)
                                    tile.WallType = tile2.WallType = EvostoneWall;
                                tile.LiquidAmount = tile2.LiquidAmount = 255;
                                tile.LiquidType = 0;
                                break;
                            case 5:
                                tile.HasTile = tile2.HasTile = true;
                                tile.TileType = tile2.TileType = Invidia;
                                tile.Slope = tile2.Slope = 0;
                                tile.IsHalfBlock = tile2.IsHalfBlock = true;
                                break;
                        }
                    }
                }
            }
        }
        public static void TryOvergrowing(int i, int j)
        {
            Tile t = Main.tile[i, j];
            bool passed = false;
            for (int k = -1; k <= 1; k++)
            {
                for (int l = -1; l <= 1; l++)
                {
                    if (!SOTSWorldgenHelper.TrueTileSolid(i + k, j + l))
                    {
                        passed = true;
                        break;
                    }
                }
            }
            if (passed)
                t.TileType = t.TileType == Evostone || t.TileType == Rune ? OvergrownEvostone : OvergrownEvostoneBrick;
        }
        public static void CleanUp(int left, int right, int top, int bottom, int pass)
        {
            SetNoise();
            float paddingZone = 30f;
            float noiseWormMult = 0.25f;
            int gridRate = 16;
            int offset = SideOfWorld % gridRate;
            for (int i = left; i <= right; i++)
            {
                for (int j = top; j <= bottom; j++)
                {
                    float fromLeft = MathF.Abs(i - left);
                    float fromRight = MathF.Abs(i - right);
                    float fromTop = MathF.Abs(j - top);
                    float fromBottom = MathF.Abs(j - bottom);
                    float percent = 1;
                    if (fromLeft < paddingZone || fromRight < paddingZone ||
                       fromTop < paddingZone || fromBottom < paddingZone)
                    {
                        float smallest = MathF.Min(MathF.Min(fromLeft, fromRight), MathF.Min(fromTop, fromBottom));
                        percent = smallest / paddingZone;
                    }
                    Tile t = Main.tile[i, j];
                    if (pass == 5)
                    {
                        if (t.HasTile)
                            TryPlacingAmbientTile(i, j);
                        WorldGen.TileFrame(i, j);
                    }
                    if (pass == 4)
                    {
                        if (t.HasTile)
                            TryErodingBlocks(i, j);
                    }
                    if (pass == 3)
                    {
                        if (t.TileType == Evostone || t.TileType == Rune)
                        {
                            TryOvergrowing(i, j);
                        }
                        if (t.LiquidType == 1 && i > left + 80 && i < right - 80)
                        {
                            t.LiquidType = 0; //Convert to water
                        }
                    }
                    if (pass == 2 || pass == 1)
                    {
                        if ((i - offset) % gridRate == 0 && (j - 2) % gridRate == 0)
                        {
                            if (pass == 2)
                            {
                                float noise = genNoise.GetNoise(i * 4f, j * 4f, 0);
                                bool noiseWorm = noise > -noiseWormMult * percent && noise < noiseWormMult * percent;
                                int edges = 0;
                                if (Main.tile[i, j - 1].TileType == RuneBrick || Main.tile[i, j - 1].TileType == Rune || Main.tile[i, j - 1].WallType == RuneWall)
                                    edges++;
                                if (Main.tile[i, j + 1].TileType == RuneBrick || Main.tile[i, j + 1].TileType == Rune || Main.tile[i, j + 1].WallType == RuneWall)
                                    edges++;
                                if (Main.tile[i - 1, j].TileType == RuneBrick || Main.tile[i - 1, j].TileType == Rune || Main.tile[i - 1, j].WallType == RuneWall)
                                    edges++;
                                if (Main.tile[i + 1, j].TileType == RuneBrick || Main.tile[i + 1, j].TileType == Rune || Main.tile[i + 1, j].WallType == RuneWall)
                                    edges++;
                                if (noiseWorm || edges == 1)// && !WorldGen.genRand.NextBool(8))
                                {
                                    if (t.TileType == RuneBrick || t.TileType == Rune || t.WallType == RuneWall)
                                        PlaceCircle(i, j, WorldGen.genRand.Next(3, 6));
                                }
                            }
                            if (WorldGen.genRand.NextBool(3) && pass == 1)
                            {
                                KillLine(i, j, 1, 0);
                                KillLine(i, j, -1, 0);
                                KillLine(i, j, 0, 1);
                                KillLine(i, j, 0, -1);
                            }
                        }
                    }
                    if (pass == 0)
                    {
                        if ((i - offset) % gridRate == 0 || (j - 2) % gridRate == 0)
                        {
                            if (t.TileType == EvostoneBrick)
                                t.TileType = RuneBrick;
                            if (t.TileType == Evostone)
                                t.TileType = Rune;
                            if (t.WallType == EvostoneWall)
                                t.WallType = RuneWall;
                        }
                        if (!t.HasTile)
                            t.TileType = 0;
                    }
                }
            }
        }
        public static void PlaceCircle(int x, int y, int r)
        {
            for (int i = x - r; i <= x + r; i++)
            {
                for(int j = y - r; j <= y + r; j++)
                {
                    float dist = MathF.Sqrt((i - x) * (i - x) + (j - y) * (j - y));
                    Tile t = Main.tile[i, j];
                    if(dist <= r + 0.45f && dist >= r - 0.45f)
                    {
                        if (t.HasTile)
                        {
                            if (t.TileType == EvostoneBrick)
                                t.TileType = RuneBrick;
                            if (t.TileType == Evostone)
                                t.TileType = Rune;
                        }
                        if (t.WallType == EvostoneWall)
                            t.WallType = RuneWall;
                    }
                    else
                    {
                        if (t.HasTile)
                        {
                            if (t.TileType == RuneBrick)
                                t.TileType = EvostoneBrick;
                            if (t.TileType == Rune)
                                t.TileType = Evostone;
                        }
                        if (t.WallType == RuneWall)
                            t.WallType = EvostoneWall;
                    }
                }
            }
        }
        public static void KillLine(int x, int y, int dirX = 1, int dirY = 0)
        {
            Tile t = Main.tile[x, y];
            while ((t.HasTile || t.WallType == RuneWall || t.WallType == EvostoneWall) && WorldGen.InWorld(x, y))
            {
                t = Main.tile[x, y];
                if (t.TileType == RuneBrick)
                    t.TileType = EvostoneBrick;
                else if (t.TileType == Rune)
                    t.TileType = Evostone;
                if (t.WallType == RuneWall)
                    t.WallType = EvostoneWall;
                x += dirX;
                y += dirY;
                Tile perpendicularT1 = Main.tile[x + dirY, y + dirX];
                Tile perpendicularT2 = Main.tile[x - dirY, y - dirX];
                if (perpendicularT1.TileType == RuneBrick || perpendicularT2.TileType == RuneBrick
                    || perpendicularT1.TileType == Rune || perpendicularT2.TileType == Rune
                    || perpendicularT1.WallType == RuneWall || perpendicularT2.WallType == RuneWall)
                {
                    break;
                }
            }
        }
        public static void FillChestWithLoot()
        {
            foreach (Chest chest in Main.chest.Where(c => c != null))
            {
                int i = chest.x;
                int j = chest.y;
                Tile t = Main.tile[i, j];
                if (t.TileType == ModContent.TileType<InvidiaChestTile>())
                {
                    int MainItem = -1;
                    int SecondItem = -1;
                    int frameX = t.TileFrameX;
                    int style = t.TileFrameX / 36 - 2;
                    if (style == 0) //new moon
                    {
                        MainItem = ModContent.ItemType<MoonShard1>();
                        SecondItem = ModContent.ItemType<EmptyNecklace>(); //Vanagloria
                    }
                    if (style == 1) //waxing crescent
                    {
                        MainItem = ModContent.ItemType<MoonShard2>();
                        SecondItem = ModContent.ItemType<Dreamcatcher>(); //Luxuria
                    }
                    if (style == 2) //second quarter
                    {
                        MainItem = ModContent.ItemType<MoonShard3>();
                        SecondItem = ModContent.ItemType<MartianWarhorn>(); //Ira
                    }
                    if (style == 3) //waxing gibbous
                    {
                        MainItem = ModContent.ItemType<MoonShard4>();
                        SecondItem = ModContent.ItemType<HardlightHook>(); //Avaritia
                    }
                    if (style == 4) //full moon
                    {
                        MainItem = ModContent.ItemType<MoonShard5>();
                        SecondItem = ModContent.ItemType<LevMirror>(); //Invidia
                    }
                    if (style == 5) //waning gibbous
                    {
                        MainItem = ModContent.ItemType<MoonShard6>();
                        SecondItem = ModContent.ItemType<Sunbulb>(); //Superbia
                    }
                    if (style == 6) //third quarter
                    {
                        MainItem = ModContent.ItemType<MoonShard7>();
                        SecondItem = ModContent.ItemType<UnholyGrail>(); //Acedia
                    }
                    if (style == 7) //waning crescent
                    {
                        MainItem = ModContent.ItemType<MoonShard8>();
                        //SecondItem = ModContent.ItemType<LevMirror>(); //Gula
                    }
                    int slot = 0;
                    if (MainItem != -1)
                        chest.AddItemToChest(MainItem, ref slot);
                    if (SecondItem != -1)
                        chest.AddItemToChest(SecondItem, ref slot);
                }
            }
        }
        public static void GeneratePillarRoom(int x, int y, int width = 30, int height = 20, int pillarNum = 0)
        {
            int dir = pillarNum == 1 || pillarNum == 9 || pillarNum == 3 || pillarNum == 6 ? 1 : -1;
            bool hasStairCase = pillarNum != 1 && pillarNum != 10;
            bool innerPillars = pillarNum != 1 && pillarNum != 2 && pillarNum != 9 && pillarNum != 10;
            InitTypes();
            int bonusWidth = width;
            if(pillarNum == 9 || pillarNum == 1)
            {
                bonusWidth = width + 14;
            }
            for(int j = 0; j < height; ++j)
            {
                for (int i = 0; i < bonusWidth; ++i)
                {
                    Tile t = Main.tile[x + i, y + j];
                    bool partOfOuterBuildingBottom = !innerPillars && (pillarNum == 1 || pillarNum == 10) && i >= 6 && i < width - 6 && j >= 3;
                    bool notPartOfWalls = ((i >= 3 && i < width - 3) || (j < height - 3 && j >= height - 6)) && j >= 3 && j < height - 3;
                    if(!innerPillars)
                    {
                        if (dir == 1 && i >= width - 3 && j >= 3 && j < height - 3)
                            notPartOfWalls = true;
                        if (dir == -1 && i < 3 && j >= 3 && j < height - 3)
                            notPartOfWalls = true;
                    }
                    if (notPartOfWalls || partOfOuterBuildingBottom)
                    {
                        t.ClearTile();
                        t.TileType = 0;
                        if (partOfOuterBuildingBottom && j == height - 3)
                        {
                            WorldGen.PlaceTile(x + i, y + j, EvostonePlatform);
                        }
                    }
                    else
                    {
                        t.TileType = EvostoneBrick;
                        t.HasTile = true;
                    }
                }
            }

            int secondRoomHeight = innerPillars ? 40 : 34;

            for (int i = 0; i < width; ++i)
            {
                int yPos = y + secondRoomHeight;
                for (int j = 0; j < 3; ++j)
                {
                    Tile t = Main.tile[x + i, yPos + j];
                    if(i >= 6 && i < width - 6 && hasStairCase)
                    {
                        WorldGen.PlaceTile(x + i, yPos + j, EvostonePlatform);
                        break;
                    }
                    t.TileType = EvostoneBrick;
                    t.HasTile = true;
                }
                if(!hasStairCase && ((dir == 1 && i >= width - 14) || (dir == -1 && i < 14)))
                {
                    WorldGen.PlaceTile(x + i + 14 * dir, yPos, EvostonePlatform);
                }
            }

            int wallPos = x + (dir == -1 ? width - 3 : 0);
            for(int j = height; j < secondRoomHeight; ++j)
            {
                int yPos = y + j;
                for (int i = 0; i < 3; ++i)
                {
                    Tile t = Main.tile[wallPos + i, yPos];
                    t.TileType = EvostoneBrick;
                    t.HasTile = true;
                }
            }

            if(hasStairCase)
            {
                for (int j = 1; j < 51; ++j)
                {
                    float percent = j / 50f;
                    int yPos = y + secondRoomHeight + j;

                    float radius = width / 4;
                    for (int i = -1; i <= 1; ++i)
                    {
                        for (int x2 = -1; x2 <= 1; x2++)
                        {
                            float sin = MathF.Sin(percent * dir * MathF.PI * 4 + MathHelper.ToRadians(i * 5));
                            int xPos = (int)(x + width / 2 + radius * sin + 0.5f);
                            Tile t = Main.tile[xPos, yPos];
                            WorldGen.PlaceTile(xPos + x2, yPos, EvostonePlatform);
                        }
                    }
                }

            }

            int platformSize = 10;
            for(int k = -1; k <= 1; k += 2)
            {
                int platformSizeBonus = dir == k ? 25 : platformSize;
                for (int i = 0; i < platformSizeBonus; ++i)
                {
                    int x2 = x + i * k + (k == 1 ? width : -1);
                    int y2 = y + height - 3;
                    Tile t = Main.tile[x2, y2];
                    if (!t.HasTile && i < platformSize)
                    {
                        WorldGen.PlaceTile(x2, y2, EvostonePlatform);
                    }
                    if (dir == k && innerPillars)
                    {
                        int y3 = y2 + i;
                        WorldGen.PlaceTile(x2, y3, EvostonePlatform);
                        t = Main.tile[x2, y3];
                        if (t.TileType == EvostonePlatform)
                        {
                            t.Slope = dir == -1 ? SlopeType.SlopeDownRight : SlopeType.SlopeDownLeft;
                            WorldGen.SquareTileFrame(x2, y3, true);
                        }
                        else
                            break;
                    }
                }
            }

            if (pillarNum == 1 || pillarNum == 10)
            {
                int x2 = x + width / 2;
                for (int j = -3; j < 14; ++j)
                {
                    int y2 = y + height + j;
                    int x3 = x2 + 5 * dir - j * dir;
                    Tile t = Main.tile[x3, y2];
                    WorldGen.PlaceTile(x3, y2, EvostonePlatform);
                    WorldGen.SquareTileFrame(x3, y2, true);
                    t.Slope = dir == 1 ? SlopeType.SlopeDownRight : SlopeType.SlopeDownLeft;
                }
            }
        }
        public static void GenerateBottomCorridor()
        {
            int top = -3;
            int height = 15;
            int width = 237;
            int biggerWidth = 374;
            int bot = Bottom - 60;
            int innerSize = 90;
            int innerOuterSize = 70;
            int outerSize = 364;
            bool apply;
            bool isOuter = false;
            for(int i = -biggerWidth; i <= biggerWidth; ++i)
            {
                apply = true;
                if(i > -innerOuterSize && i < innerOuterSize)
                    i = innerOuterSize;
                if (Math.Abs(i) < innerSize || (isOuter = Math.Abs(i) > outerSize))
                {
                    apply = false;
                    
                }
                for (int j = top; j <= height + 3; ++j)
                {
                    Tile t = Main.tile[SideOfWorld + i, bot + j];
                    if(j < top + 3 || j > height)
                    {
                        if (t.TileType == Evostone)
                            t.TileType = EvostoneBrick;
                        else if (t.TileType == OvergrownEvostone)
                            t.TileType = OvergrownEvostoneBrick;
                        t.Slope = SlopeType.Solid;
                        t.IsHalfBlock = false;
                    }
                    else if(apply)
                        t.ClearTile();
                    else
                    {
                        float diff;
                        if(isOuter)
                            diff = Math.Abs(i) - outerSize + Math.Abs(j - height / 2) + WorldGen.genRand.NextFloat(3);
                        else
                            diff = innerSize - Math.Abs(i) + Math.Abs(j - height / 2) + WorldGen.genRand.NextFloat(3); 
                        if(diff < 10)
                        {
                            t.ClearTile();
                        }
                        else
                        {
                            t.Slope = SlopeType.Solid;
                        }
                    }
                    if (t.WallType != PillarWall)
                        t.WallType = EvostoneWall;
                }
            }
            int size = 14;
            int Hall1 = WorldGen.genRand.Next(16, 25);
            int Hall2 = WorldGen.genRand.Next(33, 41);
            for(int x = -width; x <= width - size; x += width * 2 - size)
            {
                for (int j = 0; j < 60; ++j)
                {
                    for (int i = x; i <= x + size; ++i)
                    {
                        Tile t = Main.tile[SideOfWorld + i, bot - j];
                        if (t.HasTile && t.WallType != PillarWall)
                            t.WallType = EvostoneWall;
                        if(t.TileType != EvostonePlatform)
                            t.ClearTile();
                    }
                    if (j == Hall1 || j == Hall2)
                    {
                        int dir = WorldGen.genRand.Next(2) * 2 - 1;
                        int sizeH = WorldGen.genRand.Next(24, 40);
                        GenerateOffshootRoom(SideOfWorld + x + (dir == 1 ? size : 0), bot - j, sizeH, 9, dir);
                        if(WorldGen.genRand.NextBool(3))
                        {
                            GenerateOffshootRoom(SideOfWorld + x + (dir == -1 ? size : 0), bot - j + WorldGen.genRand.Next(6, 10), WorldGen.genRand.Next(24, 40), 9, -dir);
                        }
                    }
                }
            }
            GeneratePortalRoom(SideOfWorld, bot - 14);
        }
        public static void GenerateOffshootRoom(int i, int j, int hallSize = 31, int roomSize = 9, int dir = -1)
        {
            for (int a = -10; a <= hallSize; ++a)
            {
                int pos = i + a * dir;
                for(int b = -2; b <= 3; ++b)
                {
                    Tile t = Main.tile[pos, j + b];
                    if(b != 3)
                    {
                        if (t.HasTile && t.WallType != PillarWall)
                            t.WallType = EvostoneWall;
                        if(t.TileType != EvostonePlatform)
                            t.ClearTile();
                    }
                    else
                    {
                        if(!t.HasTile)
                        {
                            WorldGen.PlaceTile(pos, j + b, EvostonePlatform, true, true, -1, 0);
                        }
                    }
                }
            }
            int posX = i + (hallSize - roomSize - 2) * dir;
            StarterHouseWorldgenHelper.UseStarterHouseHalfCircle(posX , j - 1, 0, 9, 9, 0, 0);
            WorldGen.PlaceTile(posX, j + 2, EvostoneTable, true, true, -1, 0);
            if (Main.tile[posX, j + 2].HasTile && Main.tile[posX, j + 2].TileType == EvostoneTable)
            {
                if (!WorldGen.genRand.NextBool(3))
                    WorldGen.PlaceTile(posX + 2, j + 2, EvostoneChair, true, true, -1, 0);
                if (!WorldGen.genRand.NextBool(3))
                    WorldGen.PlaceTile(posX - 2, j + 2, EvostoneChair, true, true, -1, 1);
            }
        }
        public static void TryErodingBlocks(int i, int j)
        {
            float noise = genNoise2.GetNoise(i * 4, j * 4, 0);
            float size = Bottom - Ceiling;
            float percent = (j - Ceiling) / size;
            float noiseWormMult = 0.05f + 0.05f * percent;
            if(noise < noiseWormMult && noise > -noiseWormMult)
            {
                Tile t = Framing.GetTileSafely(i, j);
                bool capable = t.TileType == EvostoneBrick || t.TileType == RuneBrick;
                if (t.HasTile && capable)
                {
                    capable = t.TileType == EvostoneBrick || t.TileType == RuneBrick;
                    if(capable && WorldGen.genRand.NextFloat(1) < percent * 0.3f + 0.7f)
                    {
                        t.TileType = t.TileType == EvostoneBrick ? Evostone : Rune;
                        t.HasTile = true;
                    }
                    TryOvergrowing(i, j);
                }
            }
        }
        public static void TryPlacingAmbientTile(int i, int j)
        {
            Tile t = Main.tile[i, j];
            if (WorldGen.genRand.NextBool(60))
            {
                if (!Main.tile[i, j - 1].HasTile && !Main.tile[i, j - 2].HasTile)
                {
                    WorldGen.PlaceTile(i, j - 1, EvostoneTable, true, true, -1, 0);
                    if (Main.tile[i, j - 1].HasTile && Main.tile[i, j - 1].TileType == EvostoneTable)
                    {
                        if (!WorldGen.genRand.NextBool(3))
                            WorldGen.PlaceTile(i + 2, j - 1, EvostoneChair, true, true, -1, 0);
                        if (!WorldGen.genRand.NextBool(3))
                            WorldGen.PlaceTile(i - 2, j - 1, EvostoneChair, true, true, -1, 1);
                        //if (WorldGen.genRand.NextBool(4))
                        //    WorldGen.PlaceTile(i + WorldGen.genRand.Next(-1, 2), j - 3, ModContent.TileType<EarthenPlatingBulbTile>(), true, true, -1, 0);
                    }
                }
                return;
            }
            else if(Main.tile[i - 2, j].HasTile && Main.tile[i + 2, j].HasTile && !Main.tile[i - 2, j - 2].HasTile && !Main.tile[i + 2, j - 2].HasTile)
            {
                if (WorldGen.genRand.NextBool(60))
                {
                    WorldGen.PlaceTile(i, j - 1, ModContent.TileType<SerpentStatueTile>(), true, true, -1, WorldGen.genRand.Next(2));
                    return;
                }
                else if (WorldGen.genRand.NextBool(60))
                {
                    WorldGen.PlaceTile(i, j - 1, ModContent.TileType<RuinedStatueTile>(), true, true, -1, WorldGen.genRand.Next(2));
                    return;
                }
            }
            if (WorldGen.genRand.NextBool(3) && !Main.tile[i, j - 1].HasTile && !Main.tile[i + 1, j - 1].HasTile && !Main.tile[i, j - 2].HasTile && !Main.tile[i + 1, j - 2].HasTile)
            {
                Main.tile[i, j - 1].ClearTile();
                Main.tile[i + 1, j - 1].ClearTile();
                Main.tile[i, j - 2].ClearTile();
                Main.tile[i + 1, j - 2].ClearTile();
                WorldGen.PlaceTile(i, j - 1, ModContent.TileType<EvostonePots>(), true, true, -1, WorldGen.genRand.Next(9));
            }
            else if(WorldGen.genRand.NextBool(2) && !Main.tile[i, j - 1].HasTile)
            {
                if(WorldGen.genRand.NextBool(5) && !Main.tile[i, j - 2].HasTile)
                    WorldGen.PlaceTile(i, j - 1, ModContent.TileType<EvostoneAmbientTile1x2>(), true, true, -1, WorldGen.genRand.Next(2));
                else if (WorldGen.genRand.NextBool(4))
                    WorldGen.PlaceTile(i, j - 1, ModContent.TileType<EvostoneAmbientTile1x1>(), true, true, -1, WorldGen.genRand.Next(4));
                else if (WorldGen.genRand.NextBool(3) && !Main.tile[i + 1, j - 1].HasTile)
                    WorldGen.PlaceTile(i, j - 1, ModContent.TileType<EvostoneAmbientTile2x1>(), true, true, -1, WorldGen.genRand.Next(3));
            }
            if (t.Slope == SlopeType.Solid && (t.TileType == OvergrownEvostoneBrick || t.TileType == OvergrownEvostone))
            {
                OvergrownEvostoneBrickTile.GrowGrass(i, j);
                for (int a = 1; WorldGen.genRand.NextBool(a); ++a)
                    OvergrownEvostoneBrickTile.GrowCurseVine(i, j + a - 1);
            }
        }
        public static void GenerateTunnelHouse(int spawnX, int spawnY)
        {
            int[,] _structure = {
                {0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,3,4,4,3,3,3,4,4,3,0,0,0,0,0,0,0,0},
                {0,0,1,0,0,0,4,3,4,4,4,4,4,4,4,4,4,3,4,0,0,0,2,0,0},
                {0,0,4,4,3,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,3,4,4,0,0},
                {0,0,5,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,6,0,0},
                {0,0,0,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,0,0,0},
                {1,0,3,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,3,0,2},
                {4,4,4,4,7,7,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,4,4,4,4},
                {5,4,7,7,7,7,7,0,0,0,0,0,0,0,0,0,0,0,7,7,7,7,7,4,6},
                {0,0,7,7,7,7,7,0,0,0,0,0,0,0,0,0,0,0,7,7,7,7,7,0,0},
                {0,0,7,7,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,7,0,0},
                {0,0,7,7,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,7,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {8,8,8,8,8,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,8,8,8,8,8}
            };
            int PosX = spawnX - 12;  //spawnX and spawnY is where you want the anchor to be when this generates
            int PosY = spawnY - 17;
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
                                tile.HasTile = false;
                                tile.IsHalfBlock = false;
                                tile.Slope = 0;
                                break;
                            case 1:
                                tile.HasTile = true;
                                tile.TileType = DarkShingles;
                                tile.Slope = (SlopeType)1;
                                tile.IsHalfBlock = false;
                                break;
                            case 2:
                                tile.HasTile = true;
                                tile.TileType = DarkShingles;
                                tile.Slope = (SlopeType)2;
                                tile.IsHalfBlock = false;
                                break;
                            case 3:
                                tile.HasTile = true;
                                tile.TileType = DarkShingles;
                                tile.Slope = 0;
                                tile.IsHalfBlock = true;
                                break;
                            case 4:
                                tile.HasTile = true;
                                tile.TileType = DarkShingles;
                                tile.Slope = 0;
                                tile.IsHalfBlock = false;
                                break;
                            case 5:
                                tile.HasTile = true;
                                tile.TileType = DarkShingles;
                                tile.Slope = (SlopeType)4;
                                tile.IsHalfBlock = false;
                                break;
                            case 6:
                                tile.HasTile = true;
                                tile.TileType = DarkShingles;
                                tile.Slope = (SlopeType)3;
                                tile.IsHalfBlock = false;
                                break;
                            case 7:
                                tile.HasTile = true;
                                tile.TileType = (ushort)ModContent.TileType<EvostoneBrickTile>();
                                tile.Slope = 0;
                                tile.IsHalfBlock = false;
                                break;
                            case 8:
                                tile.HasTile = true;
                                tile.TileType = (ushort)ModContent.TileType<InvidiaPlatingTile>();
                                tile.Slope = 0;
                                tile.IsHalfBlock = false;
                                break;
                            case 9:
                                tile.HasTile = false;
                                WorldGen.PlaceTile(k, l, (ushort)EvostonePlatform, true, true, -1, 0);
                                tile.Slope = 0;
                                tile.IsHalfBlock = false;
                                break;
                        }
                    }
                }
            }
            _structure = new int[,] {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
                {0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
                {0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
                {0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
                {0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
                {0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
                {0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
                {0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
                {0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
                {0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
                {0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
                {0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0}
            };
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
                                tile.WallType = 0;
                                break;
                            case 1:
                                tile.WallType = EvostoneWall;
                                break;
                        }
                    }
                }
            }
        }
        public static void GenerateGrandArch(int x, int y, int width, int height, float archSize = 1.0f)
        {
            InitTypes();
            int endX = x + width;
            int endY = y + height;
            for(int i = x; i <= endX; ++i)
            {
                for (int j = y; j <= endY; ++j)
                {
                    float percentX = (i - x) / (float)width;
                    float percentY = (j - y) / (float)height;
                    float sin = 1f - archSize * MathF.Sin(percentX * MathF.PI);
                    if(sin >= percentY)
                    {
                        Tile t = Main.tile[i, j];
                        t.WallType = EvostoneWall;
                    }
                }
            }
        }
        public static void GeneratePortalRoom(int posX, int posY)
        {
            int[,] _structure = {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            };
            int width = _structure.GetLength(1);
            int PosX = posX - width;  //spawnX and spawnY is where you want the anchor to be when this generates
            int PosY = posY - 0;
            //i = vertical, j = horizontal
            for (int i = 0; i < _structure.GetLength(0); i++)
            {
                for (int j = width - 1; j >= 0; j--)
                {
                    int k = PosX + j + 1;
                    int k2 = PosX - j + width * 2 - 1;
                    int l = PosY + i;
                    if (WorldGen.InWorld(k, l, 30))
                    {
                        for(int a = 0; a < 2; a++)
                        {
                            Tile tile = a == 0 ? Framing.GetTileSafely(k, l) : Framing.GetTileSafely(k2, l);
                            switch (_structure[i, j])
                            {
                                case 0:
                                    break;
                                case 1:
                                    tile.HasTile = false;
                                    tile.IsHalfBlock = false;
                                    tile.Slope = 0;
                                    break;
                            }
                        }
                    }
                }
            }

            int mainDir = WorldGen.genRand.Next(2) * 2 - 1;
            GeneratePortalRoomTunnel(posX + 66 * mainDir, PosY + 4, posX, PosY - 5, 3.9f);
            GeneratePortalRoomTunnel(posX - 66 * mainDir, PosY + 4, posX, PosY - 5, 2.3f, 0.18f);
            //GeneratePortalRoomTunnel(posX + 50, PosY);
        }
        public static void GeneratePortalRoomTunnel(int posX, int posY, int endPointX, int endPointY, float targetSize = 3.75f, float baseLerpAmt = 0.3f)
        {
            int dir = Math.Sign(endPointX - posX);
            int toEnd = Math.Abs(posX - endPointX);
            int toEndY = Math.Abs(posY - endPointY);
            float endY = endPointY;
            float size = 17;
            float ySize = 3.5f;
            float SizeTarget = targetSize;
            for (int i = 0; i < toEnd; ++i)
            {
                float lerpAmt = baseLerpAmt;
                if (ySize < 1)
                    lerpAmt += 0.3f;
                else if (ySize > 4)
                    lerpAmt += 0.1f;
                ySize = MathHelper.Lerp(ySize, SizeTarget, lerpAmt);
                ySize += WorldGen.genRand.NextFloat(-0.9f, 0.9f);
                float percent = i / (float)toEnd; 
                if (percent > 0.5f)
                    size *= 0.7f;
                else if(percent > 0.3f)
                    size *= 0.9f;
                if(percent > 0.07f && percent < 0.2f)
                {
                    ySize *= WorldGen.genRand.NextFloat(0.8f, 0.9f);
                }
                else if (percent > 0.2f && percent < 0.25f)
                {
                    ySize += 0.1f;
                    ySize *= WorldGen.genRand.NextFloat(1f, 1.1f);
                }
                if (percent > 0.4f)
                {
                    ySize -= 0.1f;
                    ySize *= WorldGen.genRand.NextFloat(0.92f, 1f);
                }
                if(percent > 0.8f)
                {
                    endY += 0.6f;
                }

                float sin = MathF.Sin(percent * MathF.PI * 3.0f + 0.3f);
                int y = (int)MathHelper.Lerp(posY, endY, MathF.Sqrt( percent)) + (int)(sin * size * MathF.Sqrt(percent));
                for(float j = -ySize; j <= ySize; ++j)
                {
                    Tile t = Main.tile[posX + i * dir, y + (int)j];
                    t.ClearTile();
                    if(percent < 0.28f)
                    {
                        t.LiquidAmount = 255;
                        t.LiquidType = 0;
                    }
                }
            }
            int middleX = (posX + endPointX) / 2;
            int middleY = (posY + endPointY) / 2;
            SOTSWorldgenHelper.SmoothRegion(middleX, middleY, toEnd, toEndY);
        }
        public static void GenerateImportantChests()
        {
            Rectangle = SetRect();
            int width = Rectangle.Width / 4;
            int height = Rectangle.Height / 2;
            Rectangle[] cutouts = [
                new Rectangle(Rectangle.Left, Rectangle.Top, width, height), 
                new Rectangle(Rectangle.Left + width, Rectangle.Top, width, height),
                new Rectangle(Rectangle.Left + 2 * width, Rectangle.Top, width, height),
                new Rectangle(Rectangle.Left + 3 * width, Rectangle.Top, width, height),
                new Rectangle(Rectangle.Left, Rectangle.Top + height, width, height),
                new Rectangle(Rectangle.Left + width, Rectangle.Top + height, width, height),
                new Rectangle(Rectangle.Left + 2 * width, Rectangle.Top + height, width, height),
                new Rectangle(Rectangle.Left + 3 * width, Rectangle.Top + height, width, height),
            ];
            int[] chestOrder = WorldGen.genRand.NextBool() ? 
                                [1, 4, 0, 5,
                                 2, 3, 7, 6] : 
                                [5, 0, 4, 1, 
                                 6, 7, 3, 2];
            int k = 0;
            foreach(Rectangle rect in cutouts)
            {
                int failAttempts = 100;
                bool placedChest = false;
                while(!placedChest && --failAttempts >= 0)
                {
                    int i = rect.Left + WorldGen.genRand.Next(rect.Width);
                    int j = rect.Top + (k > 3 ? Math.Max(WorldGen.genRand.Next(rect.Height), WorldGen.genRand.Next(rect.Height)) : Math.Min(WorldGen.genRand.Next(rect.Height), WorldGen.genRand.Next(rect.Height)));
                    if (!WorldGen.InWorld(i, j, 10))
                        continue;
                    while(!Main.tile[i, j + 1].HasTile)
                        ++j;
                    Tile t = Main.tile[i, j];
                    if(!t.HasTile)
                    {
                        placedChest = -1 != WorldGen.PlaceChest(i, j, (ushort)ModContent.TileType<InvidiaChestTile>(), true, 2 + chestOrder[k % 8]);
                    }
                }
                //Main.NewText(failAttempts);
                k++;
            }
        }
    }
}