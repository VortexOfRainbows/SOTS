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
            if(type == TileType<CursedTumorTile>() || type == TileType<MalditeTile>())
            {
                if (WorldGen.genRand.NextBool(60))
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
                        int nearbyRadius = 6;
                        for (var l = i - nearbyRadius; l <= i + nearbyRadius; l++)
                        {
                            for (var m = j - nearbyRadius; m <= j + nearbyRadius; m++)
                            {
                                if (Main.tile[l, m].active() && Main.tile[l, m].type == TileType<RoyalRubyShardTile>())
                                {
                                    amt++;
                                }
                            }
                        }

                        if (amt < 2)
                        {
                            WorldGen.PlaceTile(i + x, j + y, TileType<RoyalRubyShardTile>(), true, false, -1, 0);
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
            return base.PreDraw(i, j, type, spriteBatch);
        }
        public override void PostDraw(int i, int j, int type, SpriteBatch spriteBatch)
        {
            if (Main.tile[i - 1, j].active() && Main.tile[i - 1, j].type == TileType<HardlightBlockTile>() && type != TileType<HardlightBlockTile>() && Main.tileSolid[type])
                HardlightBlockTile.Draw(i - 1, j, spriteBatch);
            base.PostDraw(i, j, type, spriteBatch);
        }
        /*public static void DrawTileAsSlope(Tile tile, int i, int j, Texture2D texture, Color color, bool ignoreColorChanges) //unimplemented method
        {
            int type = tile.type;
            if (tile.slope() > (byte)0)
            {
                if(!ignoreColorChanges)
                {
                    if (tile.inActive())
                        color = tile.actColor(color);
                    else if (Main.tileShine2[(int)type])
                        color = Main.shine(color, (int)type);
                }
                if (TileID.Sets.Platforms[type])
                {
                    if (Main.canDrawColorTile(i, j))
                        Main.spriteBatch.Draw(
                            (Texture2D)Main.tileAltTexture[(int)type, (int)trackTile.color()],
                            new Vector2(
                                (float)(index3 * 16 - (int)Main.screenPosition.X) -
                                (float)(((double)width1 - 16.0) / 2.0),
                                (float)(index2 * 16 - (int)Main.screenPosition.Y + num5)) +
                            vector2_1,
                            new Microsoft.Xna.Framework.Rectangle?(
                                new Microsoft.Xna.Framework.Rectangle((int)num3, (int)num4, 16,
                                    16)), color1, 0.0f, new Vector2(), 1f, effects, 0.0f);
                    else
                        Main.spriteBatch.Draw(Main.tileTexture[(int)type],
                            new Vector2(
                                (float)(index3 * 16 - (int)Main.screenPosition.X) -
                                (float)(((double)width1 - 16.0) / 2.0),
                                (float)(index2 * 16 - (int)Main.screenPosition.Y + num5)) +
                            vector2_1,
                            new Microsoft.Xna.Framework.Rectangle?(
                                new Microsoft.Xna.Framework.Rectangle((int)num3, (int)num4, 16,
                                    16)), color1, 0.0f, new Vector2(), 1f, effects, 0.0f);
                    if (trackTile.slope() == (byte)1 &&
                        Main.tile[index3 + 1, index2 + 1].active() &&
                        (Main.tile[index3 + 1, index2 + 1].slope() != (byte)2 &&
                         !Main.tile[index3 + 1, index2 + 1].halfBrick()) &&
                        (!TileID.Sets.BlocksStairs[(int)Main.tile[index3 + 1, index2 + 1].type] &&
                         !TileID.Sets.BlocksStairsAbove[(int)Main.tile[index3, index2 + 1].type]))
                    {
                        if (TileID.Sets.Platforms[(int)Main.tile[index3 + 1, index2 + 1].type] &&
                            Main.tile[index3 + 1, index2 + 1].slope() == (byte)0)
                        {
                            if (Main.canDrawColorTile(index3, index2))
                                Main.spriteBatch.Draw(
                                    (Texture2D)Main.tileAltTexture[(int)type,
                                        (int)trackTile.color()],
                                    new Vector2(
                                        (float)(index3 * 16 - (int)Main.screenPosition.X) -
                                        (float)(((double)width1 - 16.0) / 2.0),
                                        (float)(index2 * 16 - (int)Main.screenPosition.Y + num5 +
                                                 16)) + vector2_1,
                                    new Microsoft.Xna.Framework.Rectangle?(
                                        new Microsoft.Xna.Framework.Rectangle(324, (int)num4, 16,
                                            16)), color1, 0.0f, new Vector2(), 1f, effects, 0.0f);
                            else
                                Main.spriteBatch.Draw(Main.tileTexture[(int)type],
                                    new Vector2(
                                        (float)(index3 * 16 - (int)Main.screenPosition.X) -
                                        (float)(((double)width1 - 16.0) / 2.0),
                                        (float)(index2 * 16 - (int)Main.screenPosition.Y + num5 +
                                                 16)) + vector2_1,
                                    new Microsoft.Xna.Framework.Rectangle?(
                                        new Microsoft.Xna.Framework.Rectangle(324, (int)num4, 16,
                                            16)), color1, 0.0f, new Vector2(), 1f, effects, 0.0f);
                        }
                        else if (Main.canDrawColorTile(index3, index2))
                            Main.spriteBatch.Draw(
                                (Texture2D)Main.tileAltTexture[(int)type,
                                    (int)trackTile.color()],
                                new Vector2(
                                    (float)(index3 * 16 - (int)Main.screenPosition.X) -
                                    (float)(((double)width1 - 16.0) / 2.0),
                                    (float)(index2 * 16 - (int)Main.screenPosition.Y + num5 +
                                             16)) + vector2_1,
                                new Microsoft.Xna.Framework.Rectangle?(
                                    new Microsoft.Xna.Framework.Rectangle(198, (int)num4, 16, 16)),
                                color1, 0.0f, new Vector2(), 1f, effects, 0.0f);
                        else
                            Main.spriteBatch.Draw(Main.tileTexture[(int)type],
                                new Vector2(
                                    (float)(index3 * 16 - (int)Main.screenPosition.X) -
                                    (float)(((double)width1 - 16.0) / 2.0),
                                    (float)(index2 * 16 - (int)Main.screenPosition.Y + num5 +
                                             16)) + vector2_1,
                                new Microsoft.Xna.Framework.Rectangle?(
                                    new Microsoft.Xna.Framework.Rectangle(198, (int)num4, 16, 16)),
                                color1, 0.0f, new Vector2(), 1f, effects, 0.0f);
                    }
                    else if (trackTile.slope() == (byte)2 &&
                             Main.tile[index3 - 1, index2 + 1].active() &&
                             (Main.tile[index3 - 1, index2 + 1].slope() != (byte)1 &&
                              !Main.tile[index3 - 1, index2 + 1].halfBrick()) &&
                             (!TileID.Sets.BlocksStairs[
                                  (int)Main.tile[index3 - 1, index2 + 1].type] &&
                              !TileID.Sets.BlocksStairsAbove[
                                  (int)Main.tile[index3, index2 + 1].type]))
                    {
                        if (TileID.Sets.Platforms[(int)Main.tile[index3 - 1, index2 + 1].type] &&
                            Main.tile[index3 - 1, index2 + 1].slope() == (byte)0)
                        {
                            if (Main.canDrawColorTile(index3, index2))
                                Main.spriteBatch.Draw(
                                    (Texture2D)Main.tileAltTexture[(int)type,
                                        (int)trackTile.color()],
                                    new Vector2(
                                        (float)(index3 * 16 - (int)Main.screenPosition.X) -
                                        (float)(((double)width1 - 16.0) / 2.0),
                                        (float)(index2 * 16 - (int)Main.screenPosition.Y + num5 +
                                                 16)) + vector2_1,
                                    new Microsoft.Xna.Framework.Rectangle?(
                                        new Microsoft.Xna.Framework.Rectangle(306, (int)num4, 16,
                                            16)), color1, 0.0f, new Vector2(), 1f, effects, 0.0f);
                            else
                                Main.spriteBatch.Draw(Main.tileTexture[(int)type],
                                    new Vector2(
                                        (float)(index3 * 16 - (int)Main.screenPosition.X) -
                                        (float)(((double)width1 - 16.0) / 2.0),
                                        (float)(index2 * 16 - (int)Main.screenPosition.Y + num5 +
                                                 16)) + vector2_1,
                                    new Microsoft.Xna.Framework.Rectangle?(
                                        new Microsoft.Xna.Framework.Rectangle(306, (int)num4, 16,
                                            16)), color1, 0.0f, new Vector2(), 1f, effects, 0.0f);
                        }
                        else if (Main.canDrawColorTile(index3, index2))
                            Main.spriteBatch.Draw(
                                (Texture2D)Main.tileAltTexture[(int)type,
                                    (int)trackTile.color()],
                                new Vector2(
                                    (float)(index3 * 16 - (int)Main.screenPosition.X) -
                                    (float)(((double)width1 - 16.0) / 2.0),
                                    (float)(index2 * 16 - (int)Main.screenPosition.Y + num5 +
                                             16)) + vector2_1,
                                new Microsoft.Xna.Framework.Rectangle?(
                                    new Microsoft.Xna.Framework.Rectangle(162, (int)num4, 16, 16)),
                                color1, 0.0f, new Vector2(), 1f, effects, 0.0f);
                        else
                            Main.spriteBatch.Draw(Main.tileTexture[(int)type],
                                new Vector2(
                                    (float)(index3 * 16 - (int)Main.screenPosition.X) -
                                    (float)(((double)width1 - 16.0) / 2.0),
                                    (float)(index2 * 16 - (int)Main.screenPosition.Y + num5 +
                                             16)) + vector2_1,
                                new Microsoft.Xna.Framework.Rectangle?(
                                    new Microsoft.Xna.Framework.Rectangle(162, (int)num4, 16, 16)),
                                color1, 0.0f, new Vector2(), 1f, effects, 0.0f);
                    }
                }
                else if (TileID.Sets.HasSlopeFrames[(int)trackTile.type])
                {
                    if (Main.canDrawColorTile(index3, index2))
                        Main.spriteBatch.Draw(
                            (Texture2D)Main.tileAltTexture[(int)type, (int)trackTile.color()],
                            new Vector2(
                                (float)(index3 * 16 - (int)Main.screenPosition.X) -
                                (float)(((double)width1 - 16.0) / 2.0),
                                (float)(index2 * 16 - (int)Main.screenPosition.Y + num5)) +
                            vector2_1,
                            new Microsoft.Xna.Framework.Rectangle?(
                                new Microsoft.Xna.Framework.Rectangle((int)num3 + num8,
                                    (int)num4 + y1, 16, 16)), color1, 0.0f, new Vector2(), 1f,
                            effects, 0.0f);
                    else
                        Main.spriteBatch.Draw(Main.tileTexture[(int)type],
                            new Vector2(
                                (float)(index3 * 16 - (int)Main.screenPosition.X) -
                                (float)(((double)width1 - 16.0) / 2.0),
                                (float)(index2 * 16 - (int)Main.screenPosition.Y + num5)) +
                            vector2_1,
                            new Microsoft.Xna.Framework.Rectangle?(
                                new Microsoft.Xna.Framework.Rectangle((int)num3 + num8,
                                    (int)num4 + y1, 16, 16)), color1, 0.0f, new Vector2(), 1f,
                            effects, 0.0f);
                }
                else if (trackTile.slope() > (byte)2)
                {
                    if (trackTile.slope() == (byte)3)
                    {
                        for (var index4 = 0; index4 < 8; ++index4)
                        {
                            var width2 = 2;
                            var num6 = index4 * 2;
                            var num9 = index4 * -2;
                            var height2 = 16 - index4 * 2;
                            if (Main.canDrawColorTile(index3, index2))
                                Main.spriteBatch.Draw(
                                    (Texture2D)Main.tileAltTexture[(int)type,
                                        (int)trackTile.color()],
                                    new Vector2(
                                        (float)(index3 * 16 - (int)Main.screenPosition.X) -
                                        (float)(((double)width1 - 16.0) / 2.0) + (float)num6,
                                        (float)(index2 * 16 - (int)Main.screenPosition.Y + num5 +
                                                 index4 * width2 + num9)) + vector2_1,
                                    new Microsoft.Xna.Framework.Rectangle?(
                                        new Microsoft.Xna.Framework.Rectangle(
                                            (int)num3 + num6 + num8,
                                            (int)num4 + 16 - height2 + y1, width2, height2)),
                                    color1, 0.0f, new Vector2(), 1f, effects, 0.0f);
                            else
                                Main.spriteBatch.Draw(Main.tileTexture[(int)type],
                                    new Vector2(
                                        (float)(index3 * 16 - (int)Main.screenPosition.X) -
                                        (float)(((double)width1 - 16.0) / 2.0) + (float)num6,
                                        (float)(index2 * 16 - (int)Main.screenPosition.Y + num5 +
                                                 index4 * width2 + num9)) + vector2_1,
                                    new Microsoft.Xna.Framework.Rectangle?(
                                        new Microsoft.Xna.Framework.Rectangle(
                                            (int)num3 + num6 + num8,
                                            (int)num4 + 16 - height2 + y1, width2, height2)),
                                    color1, 0.0f, new Vector2(), 1f, effects, 0.0f);
                        }
                    }
                    else
                    {
                        for (var index4 = 0; index4 < 8; ++index4)
                        {
                            var width2 = 2;
                            var num6 = 16 - index4 * width2 - width2;
                            var height2 = 16 - index4 * width2;
                            var num9 = index4 * -2;
                            if (Main.canDrawColorTile(index3, index2))
                                Main.spriteBatch.Draw(
                                    (Texture2D)Main.tileAltTexture[(int)type,
                                        (int)trackTile.color()],
                                    new Vector2(
                                        (float)(index3 * 16 - (int)Main.screenPosition.X) -
                                        (float)(((double)width1 - 16.0) / 2.0) + (float)num6,
                                        (float)(index2 * 16 - (int)Main.screenPosition.Y + num5 +
                                                 index4 * width2 + num9)) + vector2_1,
                                    new Microsoft.Xna.Framework.Rectangle?(
                                        new Microsoft.Xna.Framework.Rectangle(
                                            (int)num3 + num6 + num8,
                                            (int)num4 + 16 - height2 + y1, width2, height2)),
                                    color1, 0.0f, new Vector2(), 1f, effects, 0.0f);
                            else
                                Main.spriteBatch.Draw(Main.tileTexture[(int)type],
                                    new Vector2(
                                        (float)(index3 * 16 - (int)Main.screenPosition.X) -
                                        (float)(((double)width1 - 16.0) / 2.0) + (float)num6,
                                        (float)(index2 * 16 - (int)Main.screenPosition.Y + num5 +
                                                 index4 * width2 + num9)) + vector2_1,
                                    new Microsoft.Xna.Framework.Rectangle?(
                                        new Microsoft.Xna.Framework.Rectangle(
                                            (int)num3 + num6 + num8,
                                            (int)num4 + 16 - height2 + y1, width2, height2)),
                                    color1, 0.0f, new Vector2(), 1f, effects, 0.0f);
                        }
                    }

                    if (Main.canDrawColorTile(index3, index2))
                        Main.spriteBatch.Draw(
                            (Texture2D)Main.tileAltTexture[(int)type, (int)trackTile.color()],
                            new Vector2(
                                (float)(index3 * 16 - (int)Main.screenPosition.X) -
                                (float)(((double)width1 - 16.0) / 2.0),
                                (float)(index2 * 16 - (int)Main.screenPosition.Y + num5)) +
                            vector2_1,
                            new Microsoft.Xna.Framework.Rectangle?(
                                new Microsoft.Xna.Framework.Rectangle((int)num3 + num8,
                                    (int)num4 + y1, 16, 2)), color1, 0.0f, new Vector2(), 1f,
                            effects, 0.0f);
                    else
                        Main.spriteBatch.Draw(Main.tileTexture[(int)type],
                            new Vector2(
                                (float)(index3 * 16 - (int)Main.screenPosition.X) -
                                (float)(((double)width1 - 16.0) / 2.0),
                                (float)(index2 * 16 - (int)Main.screenPosition.Y + num5)) +
                            vector2_1,
                            new Microsoft.Xna.Framework.Rectangle?(
                                new Microsoft.Xna.Framework.Rectangle((int)num3 + num8,
                                    (int)num4 + y1, 16, 2)), color1, 0.0f, new Vector2(), 1f,
                            effects, 0.0f);
                }
                else
                {
                    if (trackTile.slope() == (byte)1)
                    {
                        for (var index4 = 0; index4 < 8; ++index4)
                        {
                            var width2 = 2;
                            var num6 = index4 * 2;
                            var height2 = 14 - index4 * width2;
                            if (Main.canDrawColorTile(index3, index2))
                                Main.spriteBatch.Draw(
                                    (Texture2D)Main.tileAltTexture[(int)type,
                                        (int)trackTile.color()],
                                    new Vector2(
                                        (float)(index3 * 16 - (int)Main.screenPosition.X) -
                                        (float)(((double)width1 - 16.0) / 2.0) + (float)num6,
                                        (float)(index2 * 16 - (int)Main.screenPosition.Y + num5 +
                                                 index4 * width2)) + vector2_1,
                                    new Microsoft.Xna.Framework.Rectangle?(
                                        new Microsoft.Xna.Framework.Rectangle(
                                            (int)num3 + num6 + num8, (int)num4 + y1, width2,
                                            height2)), color1, 0.0f, new Vector2(), 1f, effects,
                                    0.0f);
                            else
                                Main.spriteBatch.Draw(Main.tileTexture[(int)type],
                                    new Vector2(
                                        (float)(index3 * 16 - (int)Main.screenPosition.X) -
                                        (float)(((double)width1 - 16.0) / 2.0) + (float)num6,
                                        (float)(index2 * 16 - (int)Main.screenPosition.Y + num5 +
                                                 index4 * width2)) + vector2_1,
                                    new Microsoft.Xna.Framework.Rectangle?(
                                        new Microsoft.Xna.Framework.Rectangle(
                                            (int)num3 + num6 + num8, (int)num4 + y1, width2,
                                            height2)), color1, 0.0f, new Vector2(), 1f, effects,
                                    0.0f);
                        }
                    }

                    if (trackTile.slope() == (byte)2)
                    {
                        for (var index4 = 0; index4 < 8; ++index4)
                        {
                            var width2 = 2;
                            var num6 = 16 - index4 * width2 - width2;
                            var height2 = 14 - index4 * width2;
                            if (Main.canDrawColorTile(index3, index2))
                                Main.spriteBatch.Draw(
                                    (Texture2D)Main.tileAltTexture[(int)type,
                                        (int)trackTile.color()],
                                    new Vector2(
                                        (float)(index3 * 16 - (int)Main.screenPosition.X) -
                                        (float)(((double)width1 - 16.0) / 2.0) + (float)num6,
                                        (float)(index2 * 16 - (int)Main.screenPosition.Y + num5 +
                                                 index4 * width2)) + vector2_1,
                                    new Microsoft.Xna.Framework.Rectangle?(
                                        new Microsoft.Xna.Framework.Rectangle(
                                            (int)num3 + num6 + num8, (int)num4 + y1, width2,
                                            height2)), color1, 0.0f, new Vector2(), 1f, effects,
                                    0.0f);
                            else
                                Main.spriteBatch.Draw(Main.tileTexture[(int)type],
                                    new Vector2(
                                        (float)(index3 * 16 - (int)Main.screenPosition.X) -
                                        (float)(((double)width1 - 16.0) / 2.0) + (float)num6,
                                        (float)(index2 * 16 - (int)Main.screenPosition.Y + num5 +
                                                 index4 * width2)) + vector2_1,
                                    new Microsoft.Xna.Framework.Rectangle?(
                                        new Microsoft.Xna.Framework.Rectangle(
                                            (int)num3 + num6 + num8, (int)num4 + y1, width2,
                                            height2)), color1, 0.0f, new Vector2(), 1f, effects,
                                    0.0f);
                        }
                    }

                    if (Main.canDrawColorTile(index3, index2))
                        Main.spriteBatch.Draw(
                            (Texture2D)Main.tileAltTexture[(int)type, (int)trackTile.color()],
                            new Vector2(
                                (float)(index3 * 16 - (int)Main.screenPosition.X) -
                                (float)(((double)width1 - 16.0) / 2.0),
                                (float)(index2 * 16 - (int)Main.screenPosition.Y + num5 + 14)) +
                            vector2_1,
                            new Microsoft.Xna.Framework.Rectangle?(
                                new Microsoft.Xna.Framework.Rectangle((int)num3 + num8,
                                    (int)num4 + 14 + y1, 16, 2)), color1, 0.0f, new Vector2(), 1f,
                            effects, 0.0f);
                    else
                        Main.spriteBatch.Draw(Main.tileTexture[(int)type],
                            new Vector2(
                                (float)(index3 * 16 - (int)Main.screenPosition.X) -
                                (float)(((double)width1 - 16.0) / 2.0),
                                (float)(index2 * 16 - (int)Main.screenPosition.Y + num5 + 14)) +
                            vector2_1,
                            new Microsoft.Xna.Framework.Rectangle?(
                                new Microsoft.Xna.Framework.Rectangle((int)num3 + num8,
                                    (int)num4 + 14 + y1, 16, 2)), color1, 0.0f, new Vector2(), 1f,
                            effects, 0.0f);
                }
            }
        }*/
    }
}