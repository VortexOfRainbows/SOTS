using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using SOTS.Items.Planetarium;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using SOTS.Items.Pyramid;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using SOTS.Items.Pyramid.AltPyramidBlocks;
using System.Linq;
using SOTS.Items.Tide;
using SOTS.Items.Permafrost;
using SOTS.Items.Planetarium.Furniture;
using SOTS.Items.Planetarium.Blocks;
using SOTS.Items.Earth;
using Terraria.GameContent;
using SOTS.Items;
using System;
using SOTS.Items.Conduit;
using SOTS.Items.Pyramid.PyramidWalls;
using Terraria.DataStructures;

namespace SOTS
{
    public class SOTSTile : GlobalTile
    {
        /// <summary>
        /// Gets the texture of a tile and applies paint to it. Adapted from Vanilla code.
        /// </summary>
        public static Texture2D GetTileDrawTexture(int tileX, int tileY)
        {
            Tile tile = Main.tile[tileX, tileY];
            Texture2D result = TextureAssets.Tile[tile.TileType].Value;
            int tileStyle = 0;
            Texture2D texture2D = Main.instance.TilePaintSystem.TryGetTileAndRequestIfNotReady(tile.TileType, tileStyle, tile.TileColor);
            if (texture2D != null)
            {
                result = texture2D;
            }
            return result;
        }
        public static Color NaturePlatingColor = new Color(119, 141, 138);
        public static Color EarthenPlatingColor = new Color(112, 90, 86);
        public static Color PermafrostPlatingColor = new Color(165, 179, 198);
        public static Vector3 NaturePlatingLight = new Vector3(0.275f, 0.4f, 0.215f);
        public static Vector3 EarthenPlatingLight = new Vector3(0.36f, 0.32f, 0.11f);
        public static Vector3 PermafrostPlatingLight = new Vector3(0.225f, 0.30f, 0.30f);
        public static int[] pyramidTiles;
        public static void LoadArrays() //called in SOTS.Load()
        {
            pyramidTiles = new int[] { TileType<CursedHive>(), TileType<PyramidBrickTile>(), TileType<PyramidSlabTile>(), TileType<OvergrownPyramidTile>(), TileType <CursedTumorTile>(), TileType<RuinedPyramidBrickTile>(), TileType<PyramidRubbleTile>() };
        }
        public override bool CanReplace(int i, int j, int type, int tileTypeBeingPlaced)
        {
            if (pyramidTiles.Contains(type))
            {
                return NPC.downedBoss2;
            }
            return base.CanReplace(i, j, type, tileTypeBeingPlaced);
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
                if (!Main.tile[i + x, j + y].HasTile&& !Main.tile[i + x + 1, j + y].HasTile&& !Main.tile[i + x + 1, j + y + 1].HasTile&& !Main.tile[i + x, j + y + 1].HasTile)
                {
                    bool success = WorldGen.PlaceTile(i + x, j + y, TileType<VibrantCrystalLargeTile>(), true, false, -1, 0);
                    if (success)
                    {
                        int rand = WorldGen.genRand.Next(8);
                        Main.tile[i + x, j + y].TileFrameX = (short)(rand * 36);
                        Main.tile[i + x + 1, j + y].TileFrameX = (short)(rand * 36 + 18);
                        Main.tile[i + x, j + y + 1].TileFrameX = (short)(rand * 36);
                        Main.tile[i + x + 1, j + y + 1].TileFrameX = (short)(rand * 36 + 18);
                        NetMessage.SendTileSquare(-1, i + x, j + y, 3, TileChangeType.None);
                        return true;
                    }
                }
            }
            else if (!Main.tile[i + x, j + y].HasTile)
            {
                WorldGen.PlaceTile(i + x, j + y, shardType, true, false, -1, 0);
                Main.tile[i + x, j + y].TileFrameX = (short)(WorldGen.genRand.Next(18) * 18);
                NetMessage.SendTileSquare(-1, i + x, j + y, 1, TileChangeType.None);
                return true;
            }
            return false;
        }
        public override void RandomUpdate(int i, int j, int type)
        {
            Tile tile = Main.tile[i, j];
            if(tile.Slope != 0 || tile.IsHalfBlock || !tile.HasUnactuatedTile)
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
                    if(Main.tile[i, j].WallType == WallType<VibrantWallWall>())
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
                        if(!Main.tile[i + x, j + y].HasTile && !Main.tile[i + x + 1, j + y].HasTile && !Main.tile[i + x + 1, j + y + 1].HasTile&& !Main.tile[i + x, j + y + 1].HasTile)
                        {
                            float amt = 0;
                            for (var l = i - nearbyRadius; l <= i + nearbyRadius; l++)
                            {
                                for (var m = j - nearbyRadius; m <= j + nearbyRadius; m++)
                                {
                                    if (Main.tile[l, m].HasTile && Main.tile[l, m].TileType == TileType<VibrantCrystalLargeTile>())
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
                                    Main.tile[i + x, j + y].TileFrameX = (short)(rand * 36);
                                    Main.tile[i + x + 1, j + y].TileFrameX = (short)(rand * 36 + 18);
                                    Main.tile[i + x, j + y + 1].TileFrameX = (short)(rand * 36);
                                    Main.tile[i + x + 1, j + y + 1].TileFrameX = (short)(rand * 36 + 18);
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
                    if (!Main.tile[i + x, j + y].HasTile)
                    {
                        int amt = 0;
                        for (var l = i - nearbyRadius; l <= i + nearbyRadius; l++)
                        {
                            for (var m = j - nearbyRadius; m <= j + nearbyRadius; m++)
                            {
                                if (Main.tile[l, m].HasTile&& Main.tile[l, m].TileType == shardType)
                                {
                                    amt++;
                                }
                            }
                        }

                        if (amt < 2)
                        {
                            WorldGen.PlaceTile(i + x, j + y, shardType, true, false, -1, 0);
                            Main.tile[i + x, j + y].TileFrameX = (short)(WorldGen.genRand.Next(18) * 18);
                            NetMessage.SendTileSquare(-1, i + x, j + y, 1, TileChangeType.None);
                        }
                    }
                }
            }
            if(tile.TileType == TileID.Grass && tile.WallType == 0 && SOTS.ServerConfig.NaturallySpawningPeanutBushes)
            {
                int rate = 3500; //Dye plants = 3000
                if (WorldGen.genRand.NextBool(rate))
                {
                    int nearbyAllowance = 60;
                    Tile tileAbove = Main.tile[i, j - 1];
                    if (!(tileAbove.HasTile && tileAbove.TileType != TileID.Plants && tileAbove.TileType != TileID.Cobweb && tileAbove.TileType != TileID.Plants2))
                    {
                        int LeftRange = Utils.Clamp(i - nearbyAllowance, 1, Main.maxTilesX - 1 - 1);
                        int RightRange = Utils.Clamp(i + nearbyAllowance, 1, Main.maxTilesX - 1 - 1);
                        int UpRange = Utils.Clamp(j - nearbyAllowance, 1, Main.maxTilesY - 1 - 1);
                        int DownRange = Utils.Clamp(j + nearbyAllowance, 1, Main.maxTilesY - 1 - 1);
                        int totalNearby = 0;
                        for (int k = LeftRange; k < RightRange; k++)
                        {
                            for (int l = UpRange; l < DownRange; l++)
                            {
                                if (Main.tile[k, l].HasTile && Main.tile[k, l].TileType == TileType<PeanutBushTile>())
                                {
                                    totalNearby++;
                                }
                            }
                        }
                        if (totalNearby < 2) //Will not grow if there are more than 1 within 60 blocks
                        {
                            WorldGen.KillTile(i, j - 1);
                            WorldGen.PlaceTile(i, j - 1, TileType<PeanutBushTile>(), false, true, -1, WorldGen.genRand.Next(3));
                            //Main.NewText("Placed a peanut at: " + i + ", " + j);
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
            Tile tileAbove = Main.tile[i, j - 1];
            if (tileAbove.TileType == (ushort)TileType<AvaritianGatewayTile>() || tileAbove.TileType == (ushort)TileType<AcediaGatewayTile>())
            {
                int TileFrame = tileAbove.TileFrameX / 18 + (tileAbove.TileFrameY / 18 * 9);
                if (TileFrame >= 65 && TileFrame <= 69)
                    return false;
            }
            if (tileAbove.TileType == (ushort)TileType<BigCrystalTile>())
            {
                int TileFrameX = tileAbove.TileFrameX / 18;
                int TileFrameY = tileAbove.TileFrameY / 18;
                if (TileFrameY == 13 && TileFrameX >= 2 && TileFrameX <= 11)
                    return false;
            }
            if (tileAbove.TileType == (ushort)TileType<PotGeneratorTile>() && !SOTSWorld.downedAdvisor)
            {
                return false;
            }
            if (tileAbove.TileType == (ushort)TileType<SarcophagusTile>() || tileAbove.TileType == (ushort)TileType<RubyKeystoneTile>() || tileAbove.TileType == (ushort)TileType<Items.Earth.Glowmoth.SilkCocoonTile>())
            {
                return false;
            }
            if (tileAbove.TileType == (ushort)TileType<AncientGoldGateTile>() && tileAbove.TileFrameY < 360)
            {
                return false;
            }
            if (Main.tile[i, j + 1].TileType == (ushort)TileType<ArkhalisChainTile>() && Main.tile[i, j + 1].TileFrameX >= 18)
            {
                return false;
            }
            if (Main.tile[i - 1, j].TileType == (ushort)TileType<PyramidGateTile>() || Main.tile[i + 1, j].TileType == (ushort)TileType<PyramidGateTile>())
            {
                return false;
            }
            if (tileAbove.TileType == (ushort)TileType<FrostArtifactTile>() && !SOTSWorld.downedAmalgamation)
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
            GenerateDustInPreDraw(i, j, type);
            Tile tile = Framing.GetTileSafely(i, j);
            if (tile.WallType == WallType<NatureWallWall>() && tile.TileType != TileType<DissolvingNatureTile>())
                DissolvingNatureTile.DrawEffects(i, j, Mod, true);
            if (tile.WallType == WallType<EarthWallWall>() && tile.TileType != TileType<DissolvingEarthTile>())
                DissolvingEarthTile.DrawEffects(i, j, Mod, true);
            if (tile.WallType == WallType<AuroraWallWall>() && tile.TileType != TileType<DissolvingAuroraTile>())
                DissolvingAuroraTile.DrawEffects(i, j, Mod, true);
            if (tile.WallType == WallType<AetherWallWall>() && tile.TileType != TileType<DissolvingAetherTile>())
                DissolvingAetherTile.DrawEffects(i, j, Mod, true);
            if (tile.WallType == WallType<DelugeWallWall>() && tile.TileType != TileType<DissolvingDelugeTile>())
                DissolvingDelugeTile.DrawEffects(i, j, Mod, true);
            if (tile.WallType == WallType<UmbraWallWall>() && tile.TileType != TileType<DissolvingUmbraTile>())
                DissolvingUmbraTile.DrawEffects(i, j, Mod, true);
            if (tile.WallType == WallType<NetherWallWall>() && tile.TileType != TileType<DissolvingNetherTile>())
                DissolvingNetherTile.DrawEffects(i, j, Mod, true);
            if ((!Main.tile[i - 1, j].HasTile|| !Main.tileSolid[Main.tile[i - 1, j].TileType]) && (!Main.tile[i, j - 1].HasTile|| !Main.tileSolid[Main.tile[i, j - 1].TileType]))
            {
                if (tile.WallType == WallType<BrillianceWallWall>() || tile.TileType == (ushort)TileType<DissolvingBrillianceTile>())
                    DissolvingBrillianceTile.DrawEffects(i, j, Mod, true);
            }
            if (Main.tile[i, j + 1].HasTile && (Main.tile[i, j + 1].TileType == TileType<DissolvingBrillianceTile>() || Main.tile[i, j + 1].WallType == WallType<BrillianceWallWall>()) && Main.tileSolid[type])
                DissolvingBrillianceTile.DrawEffects(i, j + 1, Mod, true);
            if (Main.tile[i + 1, j].HasTile && (Main.tile[i + 1, j].TileType == TileType<DissolvingBrillianceTile>() || Main.tile[i + 1, j].WallType == WallType<BrillianceWallWall>()) && Main.tileSolid[type])
                DissolvingBrillianceTile.DrawEffects(i + 1, j, Mod, true);
            return base.PreDraw(i, j, type, spriteBatch);
        }
        public override void PostDraw(int i, int j, int type, SpriteBatch spriteBatch)
        {
            if (Main.tile[i - 1, j].HasTile&& Main.tile[i - 1, j].TileType == TileType<HardlightBlockTile>() && type != TileType<HardlightBlockTile>() && Main.tileSolid[type])
                HardlightBlockTile.Draw(i - 1, j, spriteBatch);
            base.PostDraw(i, j, type, spriteBatch);
        }
        public static void DrawSlopedGlowMask(int i, int j, int type, Texture2D texture, Color drawColor, Vector2 positionOffset, bool overrideTileFrame = false)
        {
            Tile tile = Main.tile[i, j];
            int TileFrameX = tile.TileFrameX;
            int TileFrameY = tile.TileFrameY;
            if (overrideTileFrame)
            {
                TileFrameX = 0;
                TileFrameY = 0;
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
            if ((tile.Slope == 0 && !tile.IsHalfBlock) || (Main.tileSolid[tile.TileType] && Main.tileSolidTop[tile.TileType])) //second one should be for platforms
            {
                Main.spriteBatch.Draw(texture, drawCoordinates, new Rectangle(TileFrameX, TileFrameY, width, height), drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            else if (tile.IsHalfBlock)
            {
                Main.spriteBatch.Draw(texture, new Vector2(drawCoordinates.X, drawCoordinates.Y + 8), new Rectangle(TileFrameX, TileFrameY, width, 8), drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            else
            {
                byte b = (byte)tile.Slope;
                Rectangle TileFrame;
                Vector2 drawPos;
                if (b == (int)SlopeType.SlopeDownLeft || b == (int)SlopeType.SlopeDownRight)
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
                        TileFrame = new Rectangle(TileFrameX + length, TileFrameY, 2, height2);
                        drawPos = new Vector2(i * 16 + length, j * 16 + a * 2) + offsets;
                        Main.spriteBatch.Draw(texture, drawPos, TileFrame, drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
                    }
                    TileFrame = new Rectangle(TileFrameX, TileFrameY + 14, 16, 2);
                    drawPos = new Vector2(i * 16, j * 16 + 14) + offsets;
                    Main.spriteBatch.Draw(texture, drawPos, TileFrame, drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
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
                        TileFrame = new Rectangle(TileFrameX + length, TileFrameY + 16 - height2, 2, height2);
                        drawPos = new Vector2(i * 16 + length, j * 16) + offsets;
                        Main.spriteBatch.Draw(texture, drawPos, TileFrame, drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
                    }
                    drawPos = new Vector2(i * 16, j * 16) + offsets;
                    TileFrame = new Rectangle(TileFrameX, TileFrameY, 16, 2);
                    Main.spriteBatch.Draw(texture, drawPos, TileFrame, drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
                }
            }
        }
        public override void ModifyLight(int i, int j, int type, ref float r, ref float g, ref float b)
        {
            if (SOTS.SOTSTexturePackEnabledWithTiles && (type == TileID.Hellstone || type == TileID.MetalBars))
            {
                Tile tile = Main.tile[i, j];
                if (type == TileID.Hellstone || (type == TileID.MetalBars && (tile.TileFrameX / 18) == 10)) //hellstone is number 11
                {
                    r = 0.894f * 0.9f;
                    b = 0.4157f * 0.9f;
                    g = 0.2745f * 0.9f;
                }
            }
        }
        public void GenerateDustInPreDraw(int i, int j, int type)
        {
            if (SOTSWorld.GlobalFrozen)
                return;
            if (SOTS.SOTSTexturePackEnabledWithTiles && type == TileID.Hellstone)
            {
                Tile tile = Main.tile[i, j];
                Tile tileAbove = Main.tile[i, j - 1];
                if (tileAbove.IsActuated || !tileAbove.HasTile)
                {
                    if ((Main.drawToScreen && WorldGen.genRand.NextBool(7)) || !Main.drawToScreen)
                    {
                        int yOff = 0;
                        if (tile.IsHalfBlock || tile.Slope == SlopeType.SlopeDownLeft || tile.Slope == SlopeType.SlopeDownRight)
                            yOff = 8;
                        Dust dust = Dust.NewDustDirect(new Vector2((i * 16 + Main.rand.Next(9)), (float)(j * 16 - 2 + yOff)), 0, 8, DustID.Smoke, 0f, 0f, 100);
                        dust.alpha += WorldGen.genRand.Next(100);
                        dust.velocity *= 0.2f;
                        dust.velocity.Y -= 0.5f + WorldGen.genRand.Next(10) * 0.1f;
                        dust.fadeIn = 0.5f + WorldGen.genRand.Next(10) * 0.1f;
                    }
                }
            }
        }
        public override int[] AdjTiles(int type)
        {
            if (SOTSPlayer.ModPlayer(Main.LocalPlayer).LazyCrafterAmulet)
            {
                return new int[] { TileID.WorkBenches, TileID.Furnaces, TileID.Anvils, TileID.AlchemyTable, TileID.Bottles, TileID.Tables }; 
            }
            return base.AdjTiles(type);
        }
        public override bool CanPlace(int i, int j, int type)
        {
            if (DoNotPlaceNearConduit(i, j, type, ModContent.TileType<DissolvingNatureTile>(), 0, ModContent.TileType<NatureConduitTile>()))
                return false;
            if (DoNotPlaceNearConduit(i, j, type, ModContent.TileType<DissolvingEarthTile>(), 0, ModContent.TileType<EarthenConduitTile>()))
                return false;
            return base.CanPlace(i, j, type);
        }
        public static bool DoNotPlaceNearConduit(int i, int j, int myType, int PlaceTile, int frameY, int PreventNearby = -1)
        {
            if (myType == PlaceTile || myType == ModContent.TileType<ConduitChassisTile>())
            {
                if (myType == ModContent.TileType<ConduitChassisTile>())
                    frameY = 82;
                if (PreventNearby == -1)
                    PreventNearby = ModContent.TileType<NatureConduitTile>();
                for (int k = -2; k <= 2; k++)
                {
                    for (int h = -2; h <= 2; h++)
                    {
                        if (Math.Abs(h) != 2 || Math.Abs(k) != 2) //will not check outer 4 corners
                        {
                            int x = i + k;
                            int y = j + h;
                            Tile checkTile = Main.tile[x, y];
                            if (checkTile.HasTile && checkTile.TileType == PreventNearby && checkTile.TileFrameY == frameY)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        public override void Drop(int i, int j, int type)
        {
            TryDroppingSwallowedPenny(i, j, type);
        }
        public static void TryDroppingSwallowedPenny(int i, int j, int type)
        {
            if (type == TileID.Pots || type == TileType<PyramidPots>() || type == TileType<SkyPots>())
            {
                bool PyramidPot = type == TileType<PyramidPots>();
                bool SkyPot = type == TileType<SkyPots>();
                int chanceToDrop = 1000;
                if (SkyPot)
                {
                    chanceToDrop = 900;
                }
                if (PyramidPot)
                {
                    chanceToDrop = 800;
                }
                if (Main.rand.NextBool(chanceToDrop))
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    int left = i - (tile.TileFrameX % 36) / 18;
                    int top = j - (tile.TileFrameY % 36) / 18;
                    Item.NewItem(new EntitySource_TileBreak(i, j), left * 16, top * 16, 32, 32, ItemType<SwallowedPenny>());
                }
            }
        }
    }
}