using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture
{
    public abstract class ContainerType : ModTile // Directly ported from Aequus to have an abstract chest type class. By the way, Nalyd T. does contribute to this mod, and also fun fact, this is him writing the text you are reading right now! Hi nalyd - vortex!!
    {
        protected virtual void AddMapEntires()
        {

        }
        protected virtual void ChestStatics()
        {
            Main.tileSpelunker[Type] = true;
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 1200;
            Main.tileValue[Type] = 500;
        }
        protected virtual int ChestDrop => ItemID.Chest;
        protected virtual int DustType => DustID.Dirt;
        protected virtual string ChestName => "Chest";
        public override void SetDefaults()
        {
            ChestStatics();
            Main.tileContainer[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            TileObjectData.newTile.HookCheck = new PlacementHook(new Func<int, int, int, int, int, int>(Chest.FindEmptyChest), -1, 0, true);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int>(Chest.AfterPlacement_Hook), -1, 0, false);
            TileObjectData.newTile.AnchorInvalidTiles = new[] { (int)TileID.MagicalIceBlock, };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
            AddMapEntires();
            DustType = DustType;
            disableSmartCursor = true;
            AdjTiles = new int[] { TileID.Containers };
            chest = ChestName;
            chestDrop = ChestDrop;
        }
        public override ushort GetMapOption(int i, int j) => (ushort)(Main.tile[i, j].TileFrameX / 36);
        public override bool HasSmartInteract() => true;
        public override bool IsLockedChest(int i, int j) => false;
        protected string MapChestName(string name, int i, int j)
        {
            //if (ChestName != "Chest")
            //    name = ChestName;
            int x = i;
            int y = j;
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX % 36 != 0)
            {
                x--;
            }
            if (tile.TileFrameY != 0)
            {
                y--;
            }
            int chest = Chest.FindChest(x, y);
            if (chest < 0)
            {
                return Language.GetTextValue("LegacyChestType.0");
            }
            else if (Main.chest[chest].name == "")
            {
                return name;
            }
            else
            {
                return name + ": " + Main.chest[chest].name;
            }
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Chest.DestroyChest(i, j);
        }
        protected virtual bool ManageLockedChest(Player player, int i, int j, int x, int y)
        {
            return false;
        }
        public override bool NewRightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];
            Main.mouseRightRelease = false;
            int x = i;
            int y = j;
            if (tile.TileFrameX % 36 != 0)
            {
                x--;
            }
            if (tile.TileFrameY != 0)
            {
                y--;
            }
            if (player.sign >= 0)
            {
                SoundEngine.PlaySound(SoundID.MenuClose);
                player.sign = -1;
                Main.editSign = false;
                Main.npcChatText = "";
            }
            if (Main.editChest)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                Main.editChest = false;
                Main.npcChatText = "";
            }
            if (player.editedChestName)
            {
                NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f, 0f, 0f, 0, 0, 0);
                player.editedChestName = false;
            }
            bool isLocked = IsLockedChest(x, y);
            if (Main.netMode == NetmodeID.MultiplayerClient && !isLocked)
            {
                if (x == player.chestX && y == player.chestY && player.chest >= 0)
                {
                    player.chest = -1;
                    Recipe.FindRecipes();
                    SoundEngine.PlaySound(SoundID.MenuClose);
                }
                else
                {
                    NetMessage.SendData(MessageID.RequestChestOpen, -1, -1, null, x, y, 0f, 0f, 0, 0, 0);
                    Main.stackSplit = 600;
                }
            }
            else
            {
                if (isLocked)
                {
                    if (ManageLockedChest(player, i, j, x, y) && Chest.Unlock(x, y))
                    {
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            NetMessage.SendData(MessageID.Unlock, -1, -1, null, player.whoAmI, 1f, x, y);
                        }
                    }
                }
                else
                {
                    int chest = Chest.FindChest(x, y);
                    if (chest >= 0)
                    {
                        Main.stackSplit = 600;
                        if (chest == player.chest)
                        {
                            player.chest = -1;
                            SoundEngine.PlaySound(SoundID.MenuClose);
                        }
                        else
                        {
                            player.chest = chest;
                            Main.playerInventory = true;
                            Main.recBigList = false;
                            player.chestX = x;
                            player.chestY = y;
                            SoundEngine.PlaySound(player.chest < 0 ? SoundID.MenuOpen : SoundID.MenuTick);
                        }
                        Recipe.FindRecipes();
                    }
                }
            }
            return true;
        }
        protected virtual int ShowHoverItem(Player player, int i, int j, int x, int y)
        {
            return 0;
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];
            int x = i;
            int y = j;
            if (tile.TileFrameX % 36 != 0)
            {
                x--;
            }
            if (tile.TileFrameY != 0)
            {
                y--;
            }
            int chest = Chest.FindChest(x, y);
            player.cursorItemIconID = -1;
            if (chest < 0)
            {
                player.cursorItemIconText = Language.GetTextValue("LegacyChestType.0");
            }
            else
            {
                player.cursorItemIconText = Main.chest[chest].name.Length > 0 ? Main.chest[chest].name : ChestName;
                if (player.cursorItemIconText == ChestName)
                {
                    player.cursorItemIconID = ShowHoverItem(player, i, j, x, y);
                    player.cursorItemIconText = "";
                }
            }
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
        }
        public override void MouseOverFar(int i, int j)
        {
            MouseOver(i, j);
            Player player = Main.LocalPlayer;
            if (player.cursorItemIconText == "")
            {
                player.cursorItemIconEnabled = false;
                player.cursorItemIconID = 0;
            }
        }
    }
}