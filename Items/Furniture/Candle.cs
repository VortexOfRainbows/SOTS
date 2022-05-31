using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture
{
    public abstract class Candle<TDrop> : FurnTile where TDrop : ModItem
    {
        protected override int ItemType => ModContent.ItemType<TDrop>();
        protected override bool Multi => false;
        protected virtual Vector3 LightClr => new Vector3(1f, 1f, 1f);
        protected override void SetStaticDefaults(TileObjectData t)
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;
            Main.tileWaterDeath[Type] = false;
            t.CopyFrom(TileObjectData.StyleOnTable1x1);
            t.CoordinateHeights = new int[1] { 20 };
            t.LavaDeath = true;
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
        }
        public override void HitWire(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            short frameAdjustment = (short)(tile.TileFrameX > 0 ? -18 : 18);
            Main.tile[i, j].TileFrameX += frameAdjustment;
            Wiring.SkipWire(i, j);
            NetMessage.SendTileSquare(-1, i, j, 1, TileChangeType.None);
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (Main.tile[i, j].TileFrameX == 0)
            {
                var light = LightClr;
                r = light.X;
                g = light.Y;
                b = light.Z;
            }
        }
        public override void MouseOver(int i, int j)
        {
            Player localPlayer = Main.LocalPlayer;
            localPlayer.noThrow = 2;
            localplayer.cursorItemIconEnabled = true;
            localplayer.cursorItemIconID = ModContent.ItemType<TDrop>();
        }
        public override bool RightClick(int i, int j)
        {
            WorldGen.KillTile(i, j);
            if (!Main.tile[i, j].HasTile && Main.netMode != NetmodeID.SinglePlayer)
            {
                NetMessage.SendData(MessageID.TileChange, -1, -1, null, 0, i, j);
            }
            return true;
        }
    }
}