using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture
{
    public abstract class Chandelier<TDrop> : FurnTile where TDrop : ModItem
    {
        protected virtual Vector3 LightClr { get; }
        protected override int ItemType => ModContent.ItemType<TDrop>();

        protected override void SetStaticDefaults(TileObjectData t)
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileWaterDeath[Type] = false;
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new int[3] { 16, 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.Origin = new Point16(1, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 1);
            TileObjectData.newTile.LavaDeath = true;
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch); AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            TileID.Sets.DisableSmartCursor[Type] = true;
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX == 0)
            {
                var v = LightClr;
                r = v.X;
                g = v.Y;
                b = v.Z;
            }
        }
        public override void HitWire(int i, int j)
        {
            int x = i - Main.tile[i, j].TileFrameX / 18 % 3;
            int y = j - Main.tile[i, j].TileFrameY / 18 % 3;
            for (int m = x; m < x + 3; m++)
            {
                for (int n = y; n < y + 3; n++)
                {
                    /*if (Main.tile[m, n] == null)
                    {
                        Main.tile[m, n] = new Tile();
                    }*/
                    if (Main.tile[m, n].HasTile && Main.tile[m, n].TileType == Type)
                    {
                        if (Main.tile[m, n].TileFrameX < 54)
                        {
                            Main.tile[m, n].TileFrameX += 54;
                        }
                        else
                        {
                            Main.tile[m, n].TileFrameX -= 54;
                        }
                    }
                }
            }
            NetMessage.SendTileSquare(-1, x, y, 3, TileChangeType.None);
            if (!Wiring.running)
            {
                return;
            }
            for (int k = 0; k < 3; k++)
            {
                for (int l = 0; l < 3; l++)
                {
                    Wiring.SkipWire(x + k, y + l);
                }
            }
        }
    }
}