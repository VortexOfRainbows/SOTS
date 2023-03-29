using Microsoft.Xna.Framework;
using SOTS.Items.Gems;
using SOTS.Items.Otherworld;
using SOTS.Items.Permafrost;
using SOTS.Items.Pyramid;
using SOTS.Items.Secrets;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static SOTS.SOTS;

namespace SOTS.Common.Systems
{
    public class ImportantTilePlacement : GlobalTile
    {
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            //Manually reassigns a special tile location. Other special tile locations are only reassigned on WorldLoad (and will be by archaelogist when he is added later)
            //The only reason these are specially assigned is because they are the only ones that can be placed in the world
            if(type == ModContent.TileType<ForgottenLampTile>())
            {
                ImportantTilesWorld.AssignPoint(Main.tile[i, j], i, j, ref ImportantTilesWorld.dreamLamp, type, force: true);
                ImportantTilesWorld.CenterPoint(ref ImportantTilesWorld.dreamLamp, 0, -1);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    ImportantTilesWorld.SyncImportantTileLocations(Main.LocalPlayer, new Point16(i, j - 1), ImportantTileID.dreamLamp);
                Main.NewText("(" + i + ", " + j + ")");
            }
            if (type == ModContent.TileType<StrangeKeystoneTile>())
            {
                if (item.type == ModContent.ItemType<StrangeKeystone>())
                {
                    ImportantTilesWorld.AssignPoint(Main.tile[i, j], i, j, ref ImportantTilesWorld.coconutIslandMonument, type, force : true);
                    ImportantTilesWorld.CenterPoint(ref ImportantTilesWorld.coconutIslandMonument, 0, -1);
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        ImportantTilesWorld.SyncImportantTileLocations(Main.LocalPlayer, new Point16(i, j -1), ImportantTileID.coconutIslandMonument);
                }
                if (item.type == ModContent.ItemType<StrangeKeystoneBroken>())
                {
                    ImportantTilesWorld.AssignPoint(Main.tile[i, j], i, j, ref ImportantTilesWorld.coconutIslandMonumentBroken, type, force: true);
                    ImportantTilesWorld.CenterPoint(ref ImportantTilesWorld.coconutIslandMonumentBroken, 0, -1);
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        ImportantTilesWorld.SyncImportantTileLocations(Main.LocalPlayer, new Point16(i, j - 1), ImportantTileID.coconutIslandMonumentBroken);
                }
                Main.NewText("(" + i + ", " + j + ")");
            }
        }
    }
    public static class ImportantTileID
    {
        public const int MaxTileLocations = 13;
        public const int AcediaPortal = 0;
        public const int AvaritiaPortal = 1;
        public const int gemlockAmethyst = 2;
        public const int gemlockTopaz = 3;
        public const int gemlockSapphire = 4;
        public const int gemlockEmerald = 5;
        public const int gemlockRuby = 6;
        public const int gemlockDiamond = 7;
        public const int gemlockAmber = 8;
        public const int iceMonument = 9;
        public const int coconutIslandMonumentBroken = 10;
        public const int coconutIslandMonument = 11;
        public const int dreamLamp = 12;
    }
    public class ImportantTilesWorld : ModSystem
    {
        public static void HandlePacket(BinaryReader reader, int whoAmI, int msgType)
        {
            if(msgType == (int)SOTSMessageType.SyncTileLocations)
            {
                int playernumber2 = reader.ReadInt32();
                int pointType = reader.ReadInt32();
                int pointX = reader.ReadInt32();
                int pointY = reader.ReadInt32();
                Point16? ptToSync;
                if (pointX == -1 || pointY == -1)
                    ptToSync = null;
                else
                    ptToSync = new Point16(pointX, pointY);
                switch (pointType)
                {
                    case ImportantTileID.AcediaPortal:
                        AcediaPortal = ptToSync;
                        break;
                    case ImportantTileID.AvaritiaPortal:
                        AvaritiaPortal = ptToSync;
                        break;
                    case ImportantTileID.gemlockAmethyst:
                        gemlockAmethyst = ptToSync;
                        break;
                    case ImportantTileID.gemlockTopaz:
                        gemlockTopaz = ptToSync;
                        break;
                    case ImportantTileID.gemlockSapphire:
                        gemlockSapphire = ptToSync;
                        break;
                    case ImportantTileID.gemlockEmerald:
                        gemlockEmerald = ptToSync;
                        break;
                    case ImportantTileID.gemlockRuby:
                        gemlockRuby = ptToSync;
                        break;
                    case ImportantTileID.gemlockDiamond:
                        gemlockDiamond = ptToSync;
                        break;
                    case ImportantTileID.gemlockAmber:
                        gemlockAmber = ptToSync;
                        break;
                    case ImportantTileID.iceMonument:
                        iceMonument = ptToSync;
                        break;
                    case ImportantTileID.coconutIslandMonumentBroken:
                        coconutIslandMonumentBroken = ptToSync;
                        break;
                    case ImportantTileID.coconutIslandMonument:
                        coconutIslandMonument = ptToSync;
                        break;
                    case ImportantTileID.dreamLamp:
                        dreamLamp = ptToSync;
                        break;
                }
                if (Main.netMode == NetmodeID.Server)
                {
                    var packet = Instance.GetPacket();
                    packet.Write((byte)msgType);
                    packet.Write(playernumber2);
                    packet.Write(pointType);
                    packet.Write(pointX);
                    packet.Write(pointY);
                    packet.Send(-1, playernumber2);
                }
            }
            if(msgType == (int)SOTSMessageType.RequestTileLocations)
            {
                if(Main.netMode == NetmodeID.Server)
                {
                    newPlayerRequestingPackets = true;
                }
            }
        }
        public static void RequestNewPackets()
        {
            var packet = Instance.GetPacket();
            packet.Write((byte)SOTSMessageType.RequestTileLocations);
            packet.Send();
        }
        public static void SyncImportantTileLocations(Player clientSender, Point16? point, int pointType, int destinationClient = -1)
        {
            int x = 0;
            int y = 0;
            if (!point.HasValue)
                x = y = -1;
            else
            {
                x = point.Value.X; 
                y = point.Value.Y;
            }
            int playerWhoAmI = clientSender != null ? clientSender.whoAmI : -1;
            var packet = Instance.GetPacket();
            packet.Write((byte)SOTSMessageType.SyncTileLocations);
            packet.Write(playerWhoAmI);
            packet.Write(pointType);
            packet.Write(x);
            packet.Write(y);
            packet.Send(destinationClient);
        }
        public static void SyncAllLocations(int toClient = -1)
        {
            //Making a helper class instead of just point16 could really help out with this situation...
            SyncImportantTileLocations(null, AcediaPortal, ImportantTileID.AcediaPortal, toClient);
            SyncImportantTileLocations(null, AvaritiaPortal, ImportantTileID.AvaritiaPortal, toClient);
            SyncImportantTileLocations(null, gemlockAmethyst, ImportantTileID.gemlockAmethyst, toClient);
            SyncImportantTileLocations(null, gemlockTopaz, ImportantTileID.gemlockTopaz, toClient);
            SyncImportantTileLocations(null, gemlockSapphire, ImportantTileID.gemlockSapphire, toClient);
            SyncImportantTileLocations(null, gemlockEmerald, ImportantTileID.gemlockEmerald, toClient);
            SyncImportantTileLocations(null, gemlockRuby, ImportantTileID.gemlockRuby, toClient);
            SyncImportantTileLocations(null, gemlockDiamond, ImportantTileID.gemlockDiamond, toClient);
            SyncImportantTileLocations(null, gemlockAmber, ImportantTileID.gemlockAmber, toClient);
            SyncImportantTileLocations(null, iceMonument, ImportantTileID.iceMonument, toClient);
            SyncImportantTileLocations(null, coconutIslandMonumentBroken, ImportantTileID.coconutIslandMonumentBroken, toClient);
            SyncImportantTileLocations(null, coconutIslandMonument, ImportantTileID.coconutIslandMonument, toClient);
            SyncImportantTileLocations(null, dreamLamp, ImportantTileID.dreamLamp, toClient);
        }
        public static bool awaitTileCheck = true;
        public static bool finishedThreading = false;
        public static bool finishedFirstPacketSend = false;
        public static bool newPlayerRequestingPackets = false;
        public override void OnWorldLoad()
        {
            awaitTileCheck = true;
        }
        public override void OnWorldUnload()
        {
            awaitTileCheck = true;
        }
        public override void PostUpdateEverything()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (awaitTileCheck)
                {
                    ThreadTileResetting();
                    awaitTileCheck = false;
                    if(Main.netMode == NetmodeID.Server)
                        Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Server runs this"), Color.Gray);
                }
                if ((finishedThreading || (finishedFirstPacketSend && newPlayerRequestingPackets)) && Main.netMode == NetmodeID.Server)
                {
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("SyncedData"), Color.Gray);
                    SyncAllLocations();
                    newPlayerRequestingPackets = false;
                    finishedThreading = false;
                    finishedFirstPacketSend = true;
                }
                if ((finishedFirstPacketSend || Main.netMode == NetmodeID.SinglePlayer) && SOTSWorld.GlobalCounter % 120 == 0 && SOTSWorld.GlobalCounter > 600) //this will be checked every 2 second
                {
                    wasTileLocationJustReset = false;
                    CheckCurrentLocations();
                    if(wasTileLocationJustReset && Main.netMode == NetmodeID.Server)
                    {
                        Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("SyncedData"), Color.Gray);
                        SyncAllLocations();
                    }
                }
            }
        }
        public static Point16? AcediaPortal = null;
        public static Point16? AvaritiaPortal = null;

        public static Point16? gemlockAmethyst = null;
        public static Point16? gemlockTopaz = null;
        public static Point16? gemlockSapphire = null;
        public static Point16? gemlockEmerald = null;
        public static Point16? gemlockRuby = null;
        public static Point16? gemlockDiamond = null;
        public static Point16? gemlockAmber = null;

        public static Point16? iceMonument = null;
        public static Point16? coconutIslandMonumentBroken = null;
        public static Point16? coconutIslandMonument = null;

        public static Point16? dreamLamp = null;
        ///<summary>
        /// Gets the location of a random important tile
        ///</summary>
        public static Vector2? RandomImportantLocation()
        {
            List<Point16?> destinations = new List<Point16?>()
            { AcediaPortal, AvaritiaPortal, gemlockAmethyst, gemlockTopaz, gemlockSapphire, gemlockEmerald, gemlockRuby, gemlockDiamond, gemlockAmber, iceMonument, coconutIslandMonumentBroken, coconutIslandMonument, dreamLamp };
            Vector2? myDestination = null;
            while (myDestination == null && destinations.Count > 0)
            {
                int randomPossibilites = Main.rand.Next(destinations.Count);
                Point16? destination = destinations[randomPossibilites];
                if (destination != null)
                    myDestination = new Vector2(destination.Value.X * 16, destination.Value.Y * 16);
                else
                    destinations.RemoveAt(randomPossibilites);
            }
            return myDestination + new Vector2(8, 8);
        }
        ///<summary>
        /// Searches for special tiles in a world in order to record their positions
        ///</summary>
        public static void ThreadTileResetting()
        {
            ThreadPool.QueueUserWorkItem(ResetTileLocations, null);
        }
        public static void ResetTileLocations(object state)
        {
            AcediaPortal = null;
            AvaritiaPortal = null;
            gemlockAmethyst = null;
            gemlockTopaz = null;
            gemlockSapphire = null;
            gemlockEmerald = null;
            gemlockRuby = null;
            gemlockDiamond = null;
            gemlockAmber = null;
            iceMonument = null;
            coconutIslandMonumentBroken = null;
            coconutIslandMonument = null;
            dreamLamp = null;
            for (int i = 15; i < Main.maxTilesX - 15; i++)
            {
                for(int j = 15; j < Main.maxTilesY - 15; j++)
                {
                    Tile tile = Main.tile[i, j];
                    AssignPoint(tile, i, j, ref AcediaPortal, ModContent.TileType<AcediaGatewayTile>());
                    AssignPoint(tile, i, j, ref AvaritiaPortal, ModContent.TileType<AvaritianGatewayTile>());
                    AssignPoint(tile, i, j, ref gemlockAmethyst, ModContent.TileType<SOTSGemLockTiles>(), 216);
                    AssignPoint(tile, i, j, ref gemlockTopaz, ModContent.TileType<SOTSGemLockTiles>(), 162);
                    AssignPoint(tile, i, j, ref gemlockSapphire, ModContent.TileType<SOTSGemLockTiles>(), 54);
                    AssignPoint(tile, i, j, ref gemlockEmerald, ModContent.TileType<SOTSGemLockTiles>(), 108);
                    AssignPoint(tile, i, j, ref gemlockRuby, ModContent.TileType<SOTSGemLockTiles>(), 0);
                    AssignPoint(tile, i, j, ref gemlockDiamond, ModContent.TileType<SOTSGemLockTiles>(), 270);
                    AssignPoint(tile, i, j, ref gemlockAmber, ModContent.TileType<SOTSGemLockTiles>(), 324);
                    AssignPoint(tile, i, j, ref iceMonument, ModContent.TileType<FrostArtifactTile>());
                    AssignPoint(tile, i, j, ref coconutIslandMonumentBroken, ModContent.TileType<StrangeKeystoneTile>(), 54);
                    AssignPoint(tile, i, j, ref coconutIslandMonument, ModContent.TileType<StrangeKeystoneTile>(), 0);
                    AssignPoint(tile, i, j, ref dreamLamp, ModContent.TileType<ForgottenLampTile>());
                }
            }
            CenterPoint(ref AcediaPortal, 4, 7);
            CenterPoint(ref AvaritiaPortal, 4, 7);
            CenterPoint(ref gemlockAmethyst, 1, 1);
            CenterPoint(ref gemlockTopaz, 1, 1);
            CenterPoint(ref gemlockSapphire, 1, 1);
            CenterPoint(ref gemlockEmerald, 1, 1);
            CenterPoint(ref gemlockRuby, 1, 1);
            CenterPoint(ref gemlockDiamond, 1, 1);
            CenterPoint(ref gemlockAmber, 1, 1);
            CenterPoint(ref iceMonument, 1, 0);
            CenterPoint(ref coconutIslandMonumentBroken, 1, 2);
            CenterPoint(ref coconutIslandMonument, 1, 2);
            CenterPoint(ref dreamLamp, 1, 0);
            finishedThreading = true;
        }
        public static void CenterPoint(ref Point16? pt, int iOffset, int jOffset)
        {
            if (pt == null)
                return;
            else
            {
                pt = new Point16(pt.Value.X + iOffset, pt.Value.Y + jOffset);
            }
        }
        public static void AssignPoint(Tile tile, int i, int j, ref Point16? pt, int typeToCheck, int frameXRequired = -1, bool force = false)
        {
            if ((pt == null || force) && tile.HasTile && tile.TileType == typeToCheck && (frameXRequired == -1 || tile.TileFrameX == frameXRequired))
            {
                pt = new Point16(i, j);
            }
        }
        public static bool wasTileLocationJustReset = false;
        public static bool TileInCorrectLocation(ref Point16? pt, int typeToCheck)
        {
            if (pt == null)
            {
                if(Main.netMode == NetmodeID.Server)
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(typeToCheck + ": Does not have a location"), Color.Gray);
                else
                    Main.NewText(typeToCheck + ": Does not have a location");
                return false;
            }
            int x = pt.Value.X;
            int y = pt.Value.Y;
            Tile tile = Main.tile[x, y];
            if (tile.HasTile && tile.TileType == typeToCheck)
            {
                return true;
            }
            else
            {
                pt = null;
                if (Main.netMode == NetmodeID.Server)
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(typeToCheck + ": Reset tile location (" + x + ", " + y + ")"), Color.Gray);
                else
                    Main.NewText(typeToCheck + ": Reset tile location");
                wasTileLocationJustReset = true;
            }
            return false;
        }
        ///<summary>
        /// Checks if a saved value for a special tile position is still valid. If not, the tile is removed from being a special position tile
        ///</summary>
        public static void CheckCurrentLocations()
        {
            TileInCorrectLocation(ref AcediaPortal, ModContent.TileType<AcediaGatewayTile>());
            TileInCorrectLocation(ref AvaritiaPortal, ModContent.TileType<AvaritianGatewayTile>());
            TileInCorrectLocation(ref gemlockAmethyst, ModContent.TileType<SOTSGemLockTiles>());
            TileInCorrectLocation(ref gemlockTopaz, ModContent.TileType<SOTSGemLockTiles>());
            TileInCorrectLocation(ref gemlockSapphire, ModContent.TileType<SOTSGemLockTiles>());
            TileInCorrectLocation(ref gemlockEmerald, ModContent.TileType<SOTSGemLockTiles>());
            TileInCorrectLocation(ref gemlockRuby, ModContent.TileType<SOTSGemLockTiles>());
            TileInCorrectLocation(ref gemlockDiamond, ModContent.TileType<SOTSGemLockTiles>());
            TileInCorrectLocation(ref gemlockAmber, ModContent.TileType<SOTSGemLockTiles>());
            TileInCorrectLocation(ref iceMonument, ModContent.TileType<FrostArtifactTile>());
            TileInCorrectLocation(ref coconutIslandMonumentBroken, ModContent.TileType<StrangeKeystoneTile>());
            TileInCorrectLocation(ref coconutIslandMonument, ModContent.TileType<StrangeKeystoneTile>());
            TileInCorrectLocation(ref dreamLamp, ModContent.TileType<ForgottenLampTile>());
        }
    }
}