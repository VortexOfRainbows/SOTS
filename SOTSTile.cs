using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace SOTS
{
    public class SOTSTile : GlobalTile
    {
        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            if(Main.tile[i, j - 1].type == (ushort)mod.TileType("AvaritianGatewayTile") || Main.tile[i, j - 1].type == (ushort)mod.TileType("AcediaGatewayTile"))
            {
                int frame = Main.tile[i, j - 1].frameX / 18 + (Main.tile[i, j - 1].frameY / 18 * 9);
                if(frame >= 65 && frame <= 69)
                   return false;
            }
            return base.CanKillTile(i, j, type, ref blockDamaged);
        }
        public override bool CanExplode(int i, int j, int type)
        {
            if (Main.tile[i, j - 1].type == (ushort)mod.TileType("AvaritianGatewayTile") || Main.tile[i, j - 1].type == (ushort)mod.TileType("AcediaGatewayTile"))
            {
                int frame = Main.tile[i, j - 1].frameX / 18 + (Main.tile[i, j - 1].frameY / 18 * 9);
                if (frame >= 65 && frame <= 69)
                    return false;
            }
            return base.CanExplode(i, j, type);
        }
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            base.KillTile(i, j, type, ref fail, ref effectOnly, ref noItem);
        }
    }
}