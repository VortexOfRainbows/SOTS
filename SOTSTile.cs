using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using SOTS.Items.Otherworld;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using SOTS.Items.Pyramid;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using SOTS.Items.Pyramid.AltPyramidBlocks;
using System.Linq;
using SOTS.Items.Tide;
using SOTS.Items.Permafrost;
using SOTS.Items.Otherworld.Furniture;
using SOTS.Items.Otherworld.Blocks;
using SOTS.Items.Earth;

namespace SOTS
{
    public class SOTSTile : GlobalTile
    {
        public static int[] pyramidTiles;
        public static void LoadArrays() //called in SOTS.Load()
        {
            pyramidTiles = new int[] { TileType<CursedHive>(), TileType<PyramidBrickTile>(), TileType<PyramidSlabTile>(), TileType<OvergrownPyramidTile>(), TileType<MalditeTile>(), TileType <CursedTumorTile>(), TileType<RuinedPyramidBrickTile>(), TileType<PyramidRubbleTile>() };
        }
        public override void RandomUpdate(int i, int j, int type)
        {
            if(type == TileType<CursedTumorTile>() || type == TileType<MalditeTile>() || type == TileType<VibrantOreTile>())
            {
                int rate = 60;
                int shardType = TileType<RoyalRubyShardTile>();
                int nearbyRadius = 6;
                if (type == TileType<VibrantOreTile>())
                {
                    rate = 50;
                    nearbyRadius = 2;
                    shardType = TileType<VibrantCrystalTile>();
                }
                if (WorldGen.genRand.NextBool(rate))
                {
                    int side = WorldGen.genRand.Next(4);
                    int x = 0;
                    int y = 0;
                    switch (side)
                    {
                        case 0:
                            x = -1;
                            break;
                        case 1:
                            x = 1;
                            break;
                        case 2:
                            y = 1;
                            break;
                        case 3:
                            y = -1;
                            break;
                    }
                    if (!Main.tile[i + x, j + y].active())
                    {
                        int amt = 0;
                        for (var l = i - nearbyRadius; l <= i + nearbyRadius; l++)
                        {
                            for (var m = j - nearbyRadius; m <= j + nearbyRadius; m++)
                            {
                                if (Main.tile[l, m].active() && Main.tile[l, m].type == shardType)
                                {
                                    amt++;
                                }
                            }
                        }

                        if (amt < 2)
                        {
                            WorldGen.PlaceTile(i + x, j + y, shardType, true, false, -1, 0);
                            Main.tile[i + x, j + y].frameX = (short)(WorldGen.genRand.Next(18) * 18);
                            NetMessage.SendTileSquare(-1, i + x, j + y, 1, TileChangeType.None);
                        }
                    }
                }
            }
            base.RandomUpdate(i, j, type);
        }
        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            if (!IsValidTileAbove(i, j, type))
            {
                return false;
            }
            if(pyramidTiles.Contains(type) && !NPC.downedBoss2)
            {
                return false;
            }
            return base.CanKillTile(i, j, type, ref blockDamaged);
        }
        public override bool CanExplode(int i, int j, int type)
        {
            if(!IsValidTileAbove(i, j, type))
            {
                return false;
            }
            return base.CanExplode(i, j, type);
        }
        public override bool Slope(int i, int j, int type)
        {
            if (!IsValidTileAbove(i, j, type))
            {
                return false;
            }
            return base.Slope(i, j, type);
        }
        public bool IsValidTileAbove(int i, int j, int type)
        {
            if (Main.tile[i, j - 1].type == (ushort)TileType<AvaritianGatewayTile>() || Main.tile[i, j - 1].type == (ushort)TileType<AcediaGatewayTile>())
            {
                int frame = Main.tile[i, j - 1].frameX / 18 + (Main.tile[i, j - 1].frameY / 18 * 9);
                if (frame >= 65 && frame <= 69)
                    return false;
            }
            if (Main.tile[i, j - 1].type == (ushort)TileType<PotGeneratorTile>() && !SOTSWorld.downedAdvisor)
            {
                return false;
            }
            if (Main.tile[i, j - 1].type == (ushort)TileType<SarcophagusTile>() || Main.tile[i, j - 1].type == (ushort)TileType<RubyKeystoneTile>())
            {
                return false;
            }
            if (Main.tile[i, j - 1].type == (ushort)TileType<AncientGoldGateTile>() && Main.tile[i, j - 1].frameY < 360)
            {
                return false;
            }
            if (Main.tile[i, j + 1].type == (ushort)TileType<ArkhalisChainTile>() && Main.tile[i, j + 1].frameX >= 18)
            {
                return false;
            }
            if (Main.tile[i - 1, j].type == (ushort)TileType<PyramidGateTile>() || Main.tile[i + 1, j].type == (ushort)TileType<PyramidGateTile>())
            {
                return false;
            }
            if (Main.tile[i, j - 1].type == (ushort)TileType<FrostArtifactTile>() && !SOTSWorld.downedAmalgamation)
            {
                return false;
            }
            return true;
        }
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            base.KillTile(i, j, type, ref fail, ref effectOnly, ref noItem);
        }
        public override bool PreDraw(int i, int j, int type, SpriteBatch spriteBatch)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            if (tile.wall == WallType<NatureWallWall>() && tile.type != (ushort)TileType<DissolvingNatureTile>())
                DissolvingNatureTile.DrawEffects(i, j, spriteBatch, mod, true);
            if (tile.wall == WallType<EarthWallWall>() && tile.type != (ushort)TileType<DissolvingEarthTile>())
                DissolvingEarthTile.DrawEffects(i, j, spriteBatch, mod, true);
            if (tile.wall == WallType<AuroraWallWall>() && tile.type != (ushort)TileType<DissolvingAuroraTile>())
                DissolvingAuroraTile.DrawEffects(i, j, spriteBatch, mod, true);
            if (tile.wall == WallType<AetherWallWall>() && tile.type != (ushort)TileType<DissolvingAetherTile>())
                DissolvingAetherTile.DrawEffects(i, j, spriteBatch, mod, true);
            if (tile.wall == WallType<DelugeWallWall>() && tile.type != (ushort)TileType<DissolvingDelugeTile>())
                DissolvingDelugeTile.DrawEffects(i, j, spriteBatch, mod, true);
            if (tile.wall == WallType<UmbraWallWall>() && tile.type != (ushort)TileType<DissolvingUmbraTile>())
                DissolvingUmbraTile.DrawEffects(i, j, spriteBatch, mod, true);
            if (tile.wall == WallType<NetherWallWall>() && tile.type != (ushort)TileType<DissolvingNetherTile>())
                DissolvingNetherTile.DrawEffects(i, j, spriteBatch, mod, true);
            return base.PreDraw(i, j, type, spriteBatch);
        }
        public override void PostDraw(int i, int j, int type, SpriteBatch spriteBatch)
        {
            if (Main.tile[i - 1, j].active() && Main.tile[i - 1, j].type == TileType<HardlightBlockTile>() && type != TileType<HardlightBlockTile>() && Main.tileSolid[type])
                HardlightBlockTile.Draw(i - 1, j, spriteBatch);
            base.PostDraw(i, j, type, spriteBatch);
        }
        public static void DrawSlopedGlowMask(int i, int j, int type, Texture2D texture, Color drawColor, Vector2 positionOffset)
        {
            Tile tile = Main.tile[i, j];
            int frameX = tile.frameX;
            int frameY = tile.frameY;
            int width = 16;
            int height = 16;
            Vector2 location = new Vector2(i * 16, j * 16);
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Vector2 drawCoordinates = location + zero - Main.screenPosition;
            if (tile.slope() == 0 && !tile.halfBrick())
            {
                Main.spriteBatch.Draw(texture, drawCoordinates, new Rectangle(frameX, frameY, width, height), drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            else if (tile.halfBrick())
            {
                Main.spriteBatch.Draw(texture, new Vector2(drawCoordinates.X, drawCoordinates.Y + 10), new Rectangle(frameX, frameY, width, 6), drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            else
            {
                byte b = tile.slope();
                for (int a = 0; a < 8; a++)
                {
                    int num10 = a << 1;
                    var frame = new Rectangle(frameX, frameY + a * 2, num10, 2);
                    int xOffset = 0;
                    switch (b)
                    {
                        case 2:
                            frame.X = 16 - num10;
                            xOffset = 16 - num10;
                            break;
                        case 3:
                            frame.Width = 16 - num10;
                            break;
                        case 4:
                            frame.Width = 14 - num10;
                            frame.X = num10 + 2;
                            xOffset = num10 + 2;
                            break;
                    }
                    Main.spriteBatch.Draw(texture, new Vector2(drawCoordinates.X + (float)xOffset, drawCoordinates.Y + a * 2), frame, drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
            }
        }
    }
}