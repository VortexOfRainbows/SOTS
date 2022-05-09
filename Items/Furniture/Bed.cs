using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture
{
    public abstract class Bed<TDrop> : FurnTile where TDrop : ModItem
    {
        protected override int ItemType => ModContent.ItemType<TDrop>();

        protected override void SetDefaults(TileObjectData t)
        {
            Main.tileLavaDeath[Type] = true;
            Main.tileWaterDeath[Type] = false;
            TileID.Sets.HasOutlines[Type] = true;
            t.Width = 4;
            t.Height = 2;
            t.CoordinateHeights = new int[2] { 16, 18 };
            t.CoordinateWidth = 16;
            t.CoordinatePadding = 2;
            t.Direction = TileObjectDirection.PlaceLeft;
            t.StyleHorizontal = true;
            t.Origin = new Point16(1, 1);
            t.UsesCustomCanPlace = true;
            t.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 4, 0);
            t.LavaDeath = true;
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.addAlternate(1);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);
            disableSmartCursor = true;
            adjTiles = new int[] { TileID.Beds };
            bed = true;
        }
        public override bool HasSmartInteract()
        {
            return true;
        }
        public override bool NewRightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];
            int spawnX = i - tile.TileFrameX / 18;
            int spawnY = j + 2;
            spawnX += tile.TileFrameX >= 72 ? 5 : 2;
            if (tile.TileFrameY % 38 != 0)
            {
                spawnY--;
            }
            player.FindSpawn();
            if (player.SpawnX == spawnX && player.SpawnY == spawnY)
            {
                player.RemoveSpawn();
                Main.NewText(Language.GetText("Game.SpawnPointRemoved").Value, 255, 240, 20, false);
            }
            else if (Player.CheckSpawn(spawnX, spawnY))
            {
                player.ChangeSpawn(spawnX, spawnY);
                Main.NewText(Language.GetText("Game.SpawnPointSet").Value, 255, 240, 20, false);
            }
            return true;
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.showItemIcon = true;
            player.showItemIcon2 = ItemType;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 64, 32, ItemType);
        }
    }
}