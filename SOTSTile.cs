using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using SOTS.Items.Otherworld;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;

namespace SOTS
{
    public class SOTSTile : GlobalTile
    {
        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            if (!IsValidTileAbove(i, j, type))
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
            if (Main.tile[i, j - 1].type == (ushort)mod.TileType("AvaritianGatewayTile") || Main.tile[i, j - 1].type == (ushort)mod.TileType("AcediaGatewayTile"))
            {
                int frame = Main.tile[i, j - 1].frameX / 18 + (Main.tile[i, j - 1].frameY / 18 * 9);
                if (frame >= 65 && frame <= 69)
                    return false;
            }
            if (Main.tile[i, j - 1].type == (ushort)ModContent.TileType<PotGeneratorTile>())
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
            if (tile.wall == ModContent.WallType<NatureWallWall>() && tile.type != (ushort)ModContent.TileType<DissolvingNatureTile>())
                DissolvingNatureTile.DrawEffects(i, j, spriteBatch, mod, true);
            if (tile.wall == ModContent.WallType<EarthWallWall>() && tile.type != (ushort)ModContent.TileType<DissolvingEarthTile>())
                DissolvingEarthTile.DrawEffects(i, j, spriteBatch, mod, true);
            if (tile.wall == ModContent.WallType<AuroraWallWall>() && tile.type != (ushort)ModContent.TileType<DissolvingAuroraTile>())
                DissolvingAuroraTile.DrawEffects(i, j, spriteBatch, mod, true);
            if (tile.wall == ModContent.WallType<AetherWallWall>() && tile.type != (ushort)ModContent.TileType<DissolvingAetherTile>())
                DissolvingAetherTile.DrawEffects(i, j, spriteBatch, mod, true);
            if (tile.wall == ModContent.WallType<DelugeWallWall>() && tile.type != (ushort)ModContent.TileType<DissolvingDelugeTile>())
                DissolvingDelugeTile.DrawEffects(i, j, spriteBatch, mod, true);
            return base.PreDraw(i, j, type, spriteBatch);
        }
    }
}