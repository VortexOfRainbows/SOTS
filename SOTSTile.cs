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
            pyramidTiles = new int[] { TileType<CursedHive>(), TileType<PyramidBrickTile>(), TileType<PyramidSlabTile>(), TileType<OvergrownPyramidTile>(), TileType <CursedTumorTile>(), TileType<RuinedPyramidBrickTile>(), TileType<PyramidRubbleTile>() };
        }
        public static bool GenerateVibrantCrystal(int i, int j)
        {
            int shardType = TileType<VibrantCrystalTile>();
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
            if (WorldGen.genRand.Next(5) <= 1)
            {
                if (x == -1 || y == -1)
                {
                    x -= 1;
                    y -= 1;
                }
                if (!Main.tile[i + x, j + y].active() && !Main.tile[i + x + 1, j + y].active() && !Main.tile[i + x + 1, j + y + 1].active() && !Main.tile[i + x, j + y + 1].active())
                {
                    bool success = WorldGen.PlaceTile(i + x, j + y, TileType<VibrantCrystalLargeTile>(), true, false, -1, 0);
                    if (success)
                    {
                        int rand = WorldGen.genRand.Next(8);
                        Main.tile[i + x, j + y].frameX = (short)(rand * 36);
                        Main.tile[i + x + 1, j + y].frameX = (short)(rand * 36 + 18);
                        Main.tile[i + x, j + y + 1].frameX = (short)(rand * 36);
                        Main.tile[i + x + 1, j + y + 1].frameX = (short)(rand * 36 + 18);
                        NetMessage.SendTileSquare(-1, i + x, j + y, 3, TileChangeType.None);
                        return true;
                    }
                }
            }
            else if (!Main.tile[i + x, j + y].active())
            {
                WorldGen.PlaceTile(i + x, j + y, shardType, true, false, -1, 0);
                Main.tile[i + x, j + y].frameX = (short)(WorldGen.genRand.Next(18) * 18);
                NetMessage.SendTileSquare(-1, i + x, j + y, 1, TileChangeType.None);
                return true;
            }
            return false;
        }
        public override void RandomUpdate(int i, int j, int type)
        {
            Tile tile = Main.tile[i, j];
            if(tile.slope() != 0 || tile.halfBrick())
            {
                return;
            }
            if(type == TileType<CursedTumorTile>() || type == TileType<VibrantOreTile>())
            {
                int rate = 60;
                int shardType = TileType<RoyalRubyShardTile>();
                int nearbyRadius = 6;
                if (type == TileType<VibrantOreTile>())
                {
                    rate = 15;
                    nearbyRadius = 10;
                    shardType = TileType<VibrantCrystalTile>();
                    int bigRate = 100;
                    int maxBigs = 4;
                    if(Main.tile[i, j].wall == WallType<VibrantWallWall>())
                    {
                        bigRate = 10; 
                        maxBigs = 2;
                    }
                    if (WorldGen.genRand.NextBool(bigRate))
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
                        if(x == -1 || y == -1)
                        {
                            x -= 1;
                            y -= 1;
                        }
                        if(!Main.tile[i + x, j + y].active() && !Main.tile[i + x + 1, j + y].active() && !Main.tile[i + x + 1, j + y + 1].active() && !Main.tile[i + x, j + y + 1].active())
                        {
                            float amt = 0;
                            for (var l = i - nearbyRadius; l <= i + nearbyRadius; l++)
                            {
                                for (var m = j - nearbyRadius; m <= j + nearbyRadius; m++)
                                {
                                    if (Main.tile[l, m].active() && Main.tile[l, m].type == TileType<VibrantCrystalLargeTile>())
                                    {
                                        amt += 0.3f;
                                    }
                                }
                            }
                            if (amt < maxBigs)
                            {
                                bool success = WorldGen.PlaceTile(i + x, j + y, TileType<VibrantCrystalLargeTile>(), true, false, -1, 0);
                                if (success)
                                {
                                    int rand = WorldGen.genRand.Next(8);
                                    Main.tile[i + x, j + y].frameX = (short)(rand * 36);
                                    Main.tile[i + x + 1, j + y].frameX = (short)(rand * 36 + 18);
                                    Main.tile[i + x, j + y + 1].frameX = (short)(rand * 36);
                                    Main.tile[i + x + 1, j + y + 1].frameX = (short)(rand * 36 + 18);
                                    NetMessage.SendTileSquare(-1, i + x, j + y, 3, TileChangeType.None);
                                    return;
                                }
                            }
                        }
                    }
                    nearbyRadius = 1;
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
            if (Main.tile[i, j - 1].type == (ushort)TileType<BigCrystalTile>())
            {
                int frameX = Main.tile[i, j - 1].frameX / 18;
                int frameY = Main.tile[i, j - 1].frameY / 18;
                if (frameY == 13 && frameX >= 2 && frameX <= 11)
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
            if (tile.wall == WallType<NatureWallWall>() && tile.type != TileType<DissolvingNatureTile>())
                DissolvingNatureTile.DrawEffects(i, j, spriteBatch, mod, true);
            if (tile.wall == WallType<EarthWallWall>() && tile.type != TileType<DissolvingEarthTile>())
                DissolvingEarthTile.DrawEffects(i, j, spriteBatch, mod, true);
            if (tile.wall == WallType<AuroraWallWall>() && tile.type != TileType<DissolvingAuroraTile>())
                DissolvingAuroraTile.DrawEffects(i, j, spriteBatch, mod, true);
            if (tile.wall == WallType<AetherWallWall>() && tile.type != TileType<DissolvingAetherTile>())
                DissolvingAetherTile.DrawEffects(i, j, spriteBatch, mod, true);
            if (tile.wall == WallType<DelugeWallWall>() && tile.type != TileType<DissolvingDelugeTile>())
                DissolvingDelugeTile.DrawEffects(i, j, spriteBatch, mod, true);
            if (tile.wall == WallType<UmbraWallWall>() && tile.type != TileType<DissolvingUmbraTile>())
                DissolvingUmbraTile.DrawEffects(i, j, spriteBatch, mod, true);
            if (tile.wall == WallType<NetherWallWall>() && tile.type != TileType<DissolvingNetherTile>())
                DissolvingNetherTile.DrawEffects(i, j, spriteBatch, mod, true);
            if ((!Main.tile[i - 1, j].active() || !Main.tileSolid[Main.tile[i - 1, j].type]) && (!Main.tile[i, j - 1].active() || !Main.tileSolid[Main.tile[i, j - 1].type]))
            {
                if (tile.wall == WallType<BrillianceWallWall>() || tile.type == (ushort)TileType<DissolvingBrillianceTile>())
                    DissolvingBrillianceTile.DrawEffects(i, j, spriteBatch, mod, true);
            }
            if (Main.tile[i, j + 1].active() && (Main.tile[i, j + 1].type == TileType<DissolvingBrillianceTile>() || Main.tile[i, j + 1].wall == WallType<BrillianceWallWall>()) && Main.tileSolid[type])
                DissolvingBrillianceTile.DrawEffects(i, j + 1, spriteBatch, mod, true);
            if (Main.tile[i + 1, j].active() && (Main.tile[i + 1, j].type == TileType<DissolvingBrillianceTile>() || Main.tile[i + 1, j].wall == WallType<BrillianceWallWall>()) && Main.tileSolid[type])
                DissolvingBrillianceTile.DrawEffects(i + 1, j, spriteBatch, mod, true);
            return base.PreDraw(i, j, type, spriteBatch);
        }
        public override void PostDraw(int i, int j, int type, SpriteBatch spriteBatch)
        {
            if (Main.tile[i - 1, j].active() && Main.tile[i - 1, j].type == TileType<HardlightBlockTile>() && type != TileType<HardlightBlockTile>() && Main.tileSolid[type])
                HardlightBlockTile.Draw(i - 1, j, spriteBatch);
            base.PostDraw(i, j, type, spriteBatch);
        }
        public static void DrawSlopedGlowMask(int i, int j, int type, Texture2D texture, Color drawColor, Vector2 positionOffset, bool overrideFrame = false)
        {
            Tile tile = Main.tile[i, j];
            int frameX = tile.frameX;
            int frameY = tile.frameY;
            if (overrideFrame)
            {
                frameX = 0;
                frameY = 0;
            }
            int width = 16;
            int height = 16;
            Vector2 location = new Vector2(i * 16, j * 16);
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Vector2 offsets = -Main.screenPosition + zero + positionOffset;
            Vector2 drawCoordinates = location + offsets;
            if (tile.slope() == 0 && !tile.halfBrick())
            {
                Main.spriteBatch.Draw(texture, drawCoordinates, new Rectangle(frameX, frameY, width, height), drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            else if (tile.halfBrick())
            {
                Main.spriteBatch.Draw(texture, new Vector2(drawCoordinates.X, drawCoordinates.Y + 8), new Rectangle(frameX, frameY, width, 8), drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            else
            {
                byte b = tile.slope();
                Rectangle frame;
                Vector2 drawPos;
                if (b == 1 || b == 2)
                {
                    int length;
                    int height2;
                    for (int a = 0; a < 8; ++a)
                    {
                        if (b == 2)
                        {
                            length = 16 - a * 2 - 2;
                            height2 = 14 - a * 2;
                        }
                        else
                        {
                            length = a * 2;
                            height2 = 14 - length;
                        }
                        frame = new Rectangle(frameX + length, frameY, 2, height2);
                        drawPos = new Vector2(i * 16 + length, j * 16 + a * 2) + offsets;
                        Main.spriteBatch.Draw(texture, drawPos, frame, drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
                    }
                    frame = new Rectangle(frameX, frameY + 14, 16, 2);
                    drawPos = new Vector2(i * 16, j * 16 + 14) + offsets;
                    Main.spriteBatch.Draw(texture, drawPos, frame, drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
                }
                else
                {
                    int length;
                    int height2;
                    for (int a = 0; a < 8; ++a)
                    {
                        if (b == 3)
                        {
                            length = a * 2;
                            height2 = 16 - length;
                        }
                        else
                        {
                            length = 16 - a * 2 - 2;
                            height2 = 16 - a * 2;
                        }
                        frame = new Rectangle(frameX + length, frameY + 16 - height2, 2, height2);
                        drawPos = new Vector2(i * 16 + length, j * 16) + offsets;
                        Main.spriteBatch.Draw(texture, drawPos, frame, drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
                    }
                    drawPos = new Vector2(i * 16, j * 16) + offsets;
                    frame = new Rectangle(frameX, frameY, 16, 2);
                    Main.spriteBatch.Draw(texture, drawPos, frame, drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
                }
            }
        }
    }
}