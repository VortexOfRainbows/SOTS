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
using System.Transactions;
using System.Collections.Generic;
using System.Text;
using SOTS.Items.Pyramid.AltPyramidBlocks;
using SOTS.Items.Pyramid;

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
                                    tile.TileType = (ushort)ModContent.TileType<DarkShinglesTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = true;
                                    break;
                                case 2:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<DarkShinglesTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 3:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<DarkShinglesTile>();
                                    tile.Slope = (SlopeType)2;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 4:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<DarkShinglesTile>();
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
                                    tile.TileType = (ushort)ModContent.TileType<DarkShinglesTile>();
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
                                    tile.TileType = (ushort)ModContent.TileType<DarkShinglesTile>();
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
                                    WorldGen.PlaceTile(k, l, ModContent.TileType<EvostonePlatformTile>(), true, true, -1, 0);
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
                                    WorldGen.PlaceTile(k, l, ModContent.TileType<EvostonePlatformTile>(), true, true, -1, 0);
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
                                    WorldGen.PlaceTile(k, l, ModContent.TileType<EvostonePlatformTile>(), true, true, -1, 0);
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
        public static readonly int[] xPos = [-392, - 339, -286, -176, -119, -62, 62, 119, 176, 286, 339, 392];
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
            for (int b = 0; b < xPos.Length; b++)
            {
                int posX = SideOfWorld - sizeX / 2 + xPos[b];
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
        public static int SideOfWorld => Main.maxTilesX * 21 / 24;
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
                        if (t.TileType == TileID.Hellstone || !t.HasTile)
                        {
                            t.HasTile = true;
                            t.TileType = Evostone;
                            t.LiquidAmount = 0;
                            //t.Slope = 0;
                            //t.IsHalfBlock = false;
                        }
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
                }
                else
                {
                    int y = UnderworldHeight;
                    if (MathF.Abs(i) == 1)
                    {
                        y += WorldGen.genRand.Next(19, 23);
                        x += i * 4;
                    }
                    else
                    {
                        y += WorldGen.genRand.Next(6, 10);
                        x -= i * 2;
                    }
                    GeneratePlatform(x, y, 0);
                    if (MathF.Abs(i) == 1)
                        GeneratePlatform(x, UnderworldHeight - WorldGen.genRand.Next(23, 28), 0);
                }
            }

            for(int i = 0; i < xPos.Length; i++)
                GenerateRectangle(SideOfWorld + xPos[i] - 20, Ceiling, SideOfWorld + xPos[i] + 20, Bottom, 4);

            GenerateRectangle(left, UnderworldHeight + 35, right, Bottom, 2);
            GenerateRectangle(right - 150, UnderworldHeight + 23, right, Bottom, 2);
            GenerateRectangle(left, UnderworldHeight + 23, left + 150, Bottom, 2);

            GenerateBottomCorridor();

            left -= outcropSize / 2;
            right += outcropSize / 2;
            CleanUp(left, right, Ceiling - 30, Bottom);
            SOTSWorldgenHelper.SmoothRegion(left / 2 + right / 2, Ceiling / 2 + Bottom / 2, right - left, Bottom - Ceiling, ModContent.TileType<OvergrownEvostoneTile>());
            GenerateNewEmeraldGemStructure(SideOfWorld, Ceiling - 16);
            WorldGen.PlaceTile(SideOfWorld, UnderworldHeight + 24, ModContent.TileType<InvidiaGatewayTile>(), true, true, -1, 0);
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
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,0,0,3,3,3,1,1},
                    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,1,1,1,1,1,3,3,3,3,1,1,1,3},
                    {1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,1,1,1,3,3,3,1,1,1,1,1,3},
                    {1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,0,3,3,3,1,1,1,3},
                    {1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,1,1},
                    {1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,1,1,1,0,0,0,0,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,1,1,1,1,1,0,0,0,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,1,1,1,1,1,1,0,0,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,0,0,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0}
                };
            }
            else
            {
                _structure = new int[,] {
                    {0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,0,0,3,3,3,1,1},
                    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,1,1,1,1,1,3,3,3,3,1,1,1,3},
                    {1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,1,1,1,3,3,3,1,1,1,1,1,3},
                    {1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,0,3,3,3,1,1,1,3},
                    {1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,1,1},
                    {1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,1,1,1,0,0,0,0,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,1,1,1,1,1,0,0,0,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,1,1,1,1,1,1,0,0,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,0,0,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0}
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
        public static void CleanUp(int left, int right, int top, int bottom)
        {
            SetNoise();
            float paddingZone = 30f;
            float noiseWormMult = 0.25f;
            int gridRate = 16;
            int offset = SideOfWorld % gridRate;
            for(int pass = 0; pass <= 4; pass++)
            {
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
                        if(pass == 4)
                        {
                            TryErodingBlocks(i, j);
                            if(t.TileType == OvergrownEvostoneBrick || t.TileType == OvergrownEvostone)
                            {
                                OvergrownEvostoneBrickTile.GrowGrass(i, j);
                                for (int a = 1; WorldGen.genRand.NextBool(a); ++a)
                                    OvergrownEvostoneBrickTile.GrowCurseVine(i, j + a - 1);
                            }
                        }
                        if(pass == 3)
                        {
                            if (t.TileType == Evostone || t.TileType == Rune)
                            {
                                TryOvergrowing(i, j);
                            }
                            if(t.LiquidType == 1 && i > left + 80 && i < right - 80)
                            {
                                t.LiquidType = 0; //Convert to water
                            }
                        }
                        if (pass == 2 || pass == 1)
                        {
                            if((i - offset) % gridRate == 0 && (j - 2) % gridRate == 0)
                            {
                                if(pass == 2)
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
                                if(WorldGen.genRand.NextBool(3) && pass == 1)
                                {
                                    KillLine(i, j, 1, 0);
                                    KillLine(i, j, -1, 0);
                                    KillLine(i, j, 0, 1);
                                    KillLine(i, j, 0, -1);
                                }
                            }
                        }
                        if(pass == 0)
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
                        }
                        WorldGen.TileFrame(i, j);
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
                Tile t = Main.tile[chest.x, chest.y];
                if(t.TileType == ModContent.TileType<InvidiaChestTile>())
                {
                    int slot = 0;
                    int chestType = t.TileFrameX / 36;
                    int primaryItem = ModContent.ItemType<MoonShard1>();
                    if(chestType == 2)
                        primaryItem = ModContent.ItemType<MoonShard1>();
                    if (chestType == 3)
                        primaryItem = ModContent.ItemType<MoonShard2>();
                    if (chestType == 4)
                        primaryItem = ModContent.ItemType<MoonShard3>();
                    if (chestType == 5)
                        primaryItem = ModContent.ItemType<MoonShard4>();
                    if (chestType == 6)
                        primaryItem = ModContent.ItemType<MoonShard5>();
                    if (chestType == 7)
                        primaryItem = ModContent.ItemType<MoonShard6>();
                    if (chestType == 8)
                        primaryItem = ModContent.ItemType<MoonShard7>();
                    if (chestType == 9)
                        primaryItem = ModContent.ItemType<MoonShard8>();
                    chest.AddItemToChest(primaryItem, ref slot, 1);
                }
            }
        }
        public static void GenerateRoom(int x, int y)
        {
            InitTypes();
            List<Rectangle> rects = [
                new Rectangle(x, y, 15, 15), 
                new Rectangle(x + WorldGen.genRand.Next(-5, 6), y + WorldGen.genRand.Next(-5, 6), 15, 15), 
                new Rectangle(x + WorldGen.genRand.Next(-5, 6), y + WorldGen.genRand.Next(-5, 6), 15, 15)];
            int wallSize = 2;
            foreach (Rectangle rect in rects)
            {
                for(int i = rect.X; i < rect.Right; i++)
                {
                    for (int j = rect.Y; j < rect.Bottom; j++)
                    {
                        bool inside = false;
                        foreach (Rectangle other in rects)
                        {
                            if (other.Contains(i + wallSize, j) &&
                                other.Contains(i, j + wallSize) &&
                                other.Contains(i - wallSize, j) && 
                                other.Contains(i, j - wallSize))
                            {
                                inside = true;
                                break;
                            }
                        }
                        Tile t = Main.tile[i, j];
                        if (inside)
                        {
                            t.ClearTile();
                            t.WallType = EvostoneWall;
                        }
                        else
                        {
                            t.TileType = EvostoneBrick;
                            t.Slope = SlopeType.Solid;
                            t.IsHalfBlock = false;
                            t.HasTile = true;
                        }
                    }
                }
            }
        }
        public static void GenerateBottomCorridor()
        {
            int height = 15;
            int width = 240;
            int bot = Bottom - 60;
            for(int i = -width; i <= width; ++i)
            {
                for (int j = -3; j <= height + 3; ++j)
                {
                    Tile t = Main.tile[SideOfWorld + i, bot + j];
                    if(j < 0 || j > height)
                    {
                        if (t.TileType == Evostone)
                            t.TileType = EvostoneBrick;
                        else if (t.TileType == OvergrownEvostone)
                            t.TileType = OvergrownEvostoneBrick;
                        t.Slope = SlopeType.Solid;
                        t.IsHalfBlock = false;
                    }
                    else
                        t.ClearTile();
                    if (t.WallType != PillarWall)
                        t.WallType = EvostoneWall;
                }
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
    }
}