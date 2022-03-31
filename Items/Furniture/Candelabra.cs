using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;


namespace SOTS.Items.Furniture
{
    public abstract class Candelabra<TDrop> : FurnTile where TDrop : ModItem
    {
        protected override int ItemType => ModContent.ItemType<TDrop>();
        protected virtual Vector3 LightClr => new Vector3(1f, 1f, 1f);
        protected override void SetDefaults(TileObjectData t)
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileLavaDeath[Type] = true;
            t.CopyFrom(TileObjectData.Style2x2);
            t.Width = 2;
            t.Height = 2;
            t.CoordinateHeights = new int[] { 16, 18 };
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            disableSmartCursor = true;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 0;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 32, ModContent.ItemType<TDrop>());
        }
        public override void HitWire(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            int x = i - (tile.frameX % 36) / 18;
            int y = j - tile.frameY / 18;
            if (tile.frameX >= 36)
            {
                Main.tile[x, y].frameX += -36;
                Main.tile[x + 1, y].frameX += -36;
                Main.tile[x, y + 1].frameX += -36;
                Main.tile[x + 1, y + 1].frameX += -36;
            }
            else
            {
                Main.tile[x, y].frameX += 36;
                Main.tile[x + 1, y].frameX += 36;
                Main.tile[x, y + 1].frameX += 36;
                Main.tile[x + 1, y + 1].frameX += 36;
            }
            Wiring.SkipWire(x, y);
            Wiring.SkipWire(x + 1, y);
            Wiring.SkipWire(x, y + 1);
            Wiring.SkipWire(x + 1, y + 1);
            NetMessage.SendTileSquare(-1, x, y, 2, TileChangeType.None);
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (Main.tile[i, j].frameX == 0)
            {
                var light = LightClr;
                r = light.X;
                g = light.Y;
                b = light.Z;
            }
        }
    }
}