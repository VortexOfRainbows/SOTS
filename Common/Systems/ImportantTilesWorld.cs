using Microsoft.Xna.Framework;
using SOTS.Items.Gems;
using SOTS.Items.Planetarium;
using SOTS.Items.Permafrost;
using SOTS.Items.Pyramid;
using SOTS.Items.Secrets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Invidia;
using static SOTS.SOTS;
using static Terraria.ModLoader.ModContent;
using SOTS.Items.Temple;
using Mono.Cecil.Cil;
using SOTS.Items.Fragments;
using SOTS.Helpers;

namespace SOTS.Common.Systems
{
    public static class ImportantTileID
    {
        //public const int MaxTileLocations = 15;
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
        public const int damoclesChain = 13;
        public const int bigCrystal = 14;
        public const int GulaPortal = 15;
        public const int InvidiaPortal = 16;
        public const int IraPortal = 17;
    }
    public class ImportantTile(int id, ushort TileType, int TileFrame = -1, int offsetX = 0, int offsetY = 0, Point16? pos = null)
    {
        public Point16? Position = pos;
        public int ID = id;
        public ushort TileType = TileType;
        public int TileFrame = TileFrame;
        public bool IsArchaeologistSpot = true;
        public void AssignPoint(Tile tile, int i, int j, bool force = false)
        {
            if ((Position == null || force) && tile.TileType == TileType && (TileFrame == -1 || tile.TileFrameX == TileFrame) /*&& tile.HasTile*/) //Do not check hasTile cause that can be done faster outside this method
            {
                Position = new Point16(i, j);
                if(!force)
                    CenterPoint(offsetX, offsetY);
            }
        }
        public void CenterPoint(int iOffset, int jOffset)
        {
            if (Position == null)
                return;
            else
                Position = new Point16(Position.Value.X + iOffset, Position.Value.Y + jOffset);
        }
        public bool TileInCorrectLocation()
        {
            if (Position == null)
            {
                if (ImportantTilesWorld.DebugChatMessages)
                    if (Main.netMode == NetmodeID.Server)
                        Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(TileType + ": Does not have a location"), Color.Gray);
                    else
                        Main.NewText(TileType + ": Does not have a location");
                return false;
            }
            int x = Position.Value.X;
            int y = Position.Value.Y;
            Tile tile = Main.tile[x, y];
            if (tile.HasTile && tile.TileType == TileType)
            {
                return true;
            }
            else
            {
                Position = null;
                if (ImportantTilesWorld.DebugChatMessages)
                    if (Main.netMode == NetmodeID.Server)
                        Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(TileType + ": Reset tile location (" + x + ", " + y + ")"), Color.Gray);
                    else
                        Main.NewText(TileType + ": Reset tile location");
                ImportantTilesWorld.TileLocationJustReset = true;
            }
            return false;
        }
        public Point16 Value => Position.Value;
        public bool HasValue => Position.HasValue;
    }
    public class GatewayImportantTile(int id, ushort TileType, int TileFrame = -1, int offsetX = 0, int offsetY = 0, Point16? pos = null) : ImportantTile(id, TileType, TileFrame, offsetX, offsetY, pos)
    {
        public bool IsPowered = false;
        public int LeftElementType;
        public int RightElementType;
        public bool IsConnectedLeftElement = false;
        public bool IsConnectedRightElement = false;
        public float MiddlePercent = 0f;
        //public void TryConnectingToConduit()
        //{
        //    if (ImportantTilesWorld.AvaritiaPortal.HasValue)
        //    {
        //        int x = ImportantTilesWorld.AvaritiaPortal.Value.X;
        //        int y = ImportantTilesWorld.AvaritiaPortal.Value.Y;
        //        Tile tile = Main.tile[x, y];
        //        bool chaos = tileEntity.ConduitTile.DissolvingTileType == ModContent.TileType<DissolvingBrillianceTile>();
        //        bool otherworld = tileEntity.ConduitTile.DissolvingTileType == ModContent.TileType<DissolvingAetherTile>();
        //        if (tile.HasUnactuatedTile && tile.TileType == ModContent.TileType<AvaritianGatewayTile>() &&
        //            (chaos || otherworld))
        //        {
        //            Vector2 avaritiaPortal = new Vector2(x * 16, y * 16) + new Vector2(8, 8);
        //            bool succeededDraw = tileEntity.DrawConduitToLocation(tileEntity.Position.X, tileEntity.Position.Y, avaritiaPortal, 1f, ColorHelper.OtherworldColor);
        //            if (otherworld && !hasDrawnToAvaritiaPortalOtherworld && succeededDraw) //This way, it only draws the acedia portal glow once, no matter how many conduits
        //            {
        //                float Percent = tileEntity.DissolvingTileCount / 20f;
        //                Percent *= Percent;
        //                hasDrawnToAvaritiaPortalOtherworld = true;
        //                DrawGatewayGlowmask(x, y, Main.spriteBatch, Percent, -1);
        //                AvaritiaPortalMiddleAlpha += Percent * 0.5f;
        //            }
        //            if (chaos && !hasDrawnToAvaritiaPortalChaos && succeededDraw) //This way, it only draws the acedia portal glow once, no matter how many conduits
        //            {
        //                float Percent = tileEntity.DissolvingTileCount / 20f;
        //                Percent *= Percent;
        //                hasDrawnToAvaritiaPortalChaos = true;
        //                DrawGatewayGlowmask(x, y, Main.spriteBatch, Percent, 1);
        //                AvaritiaPortalMiddleAlpha += Percent * 0.5f;
        //            }
        //        }
        //    }
        //}
    }
    public class ImportantTilePlacement : GlobalTile
    {
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            //Manually reassigns a special tile location. Other special tile locations are only reassigned on WorldLoad.
            //The only reason these are specially assigned is because they are the only ones that can be placed in the world
            if(type == TileType<ForgottenLampTile>())
            {
                ImportantTilesWorld.DreamLamp.AssignPoint(Main.tile[i, j], i, j, true);
                ImportantTilesWorld.DreamLamp.CenterPoint(0, -1);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    ImportantTilesWorld.SyncImportantTileLocations(Main.LocalPlayer, ImportantTilesWorld.DreamLamp);
                if(ImportantTilesWorld.DebugChatMessages)
                    Main.NewText("(" + i + ", " + j + ")");
            }
            if (type == TileType<StrangeKeystoneTile>())
            {
                if (item.type == ItemType<StrangeKeystone>())
                {
                    ImportantTilesWorld.CoconutIslandMonument.AssignPoint(Main.tile[i, j], i, j, force : true);
                    ImportantTilesWorld.CoconutIslandMonument.CenterPoint(0, -1);
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        ImportantTilesWorld.SyncImportantTileLocations(Main.LocalPlayer, ImportantTilesWorld.CoconutIslandMonument);
                }
                if (item.type == ItemType<StrangeKeystoneBroken>())
                {
                    ImportantTilesWorld.CoconutIslandMonumentBroken.AssignPoint(Main.tile[i, j], i, j, force: true);
                    ImportantTilesWorld.CoconutIslandMonumentBroken.CenterPoint(0, -1);
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        ImportantTilesWorld.SyncImportantTileLocations(Main.LocalPlayer, ImportantTilesWorld.CoconutIslandMonumentBroken);
                }
                if (ImportantTilesWorld.DebugChatMessages)
                    Main.NewText("(" + i + ", " + j + ")");
            }
        }
    }
    public class ImportantTilesWorld : ModSystem
    {
        private static int ListIndex = 0;
        private static List<ImportantTile> List = new List<ImportantTile>();
        private static ImportantTile AddToList(int TileType, int TileFrame = -1, int offsetX = 0, int offsetY = 0, Point16? pos = null)
        {
            ImportantTile i = new(ListIndex++, (ushort)TileType, TileFrame, offsetX, offsetY, pos);
            List.Add(i);
            return i;
        }
        private static GatewayImportantTile AddGatewayToList(int TileType, int TileFrame = -1, int offsetX = 0, int offsetY = 0, Point16? pos = null)
        {
            GatewayImportantTile i = new(ListIndex++, (ushort)TileType, TileFrame, offsetX, offsetY, pos);
            List.Add(i);
            return i;
        }
        public override void PostSetupContent()
        {
            Initialize();
        }
        public static void Initialize()
        {
            AcediaPortal = AddGatewayToList(TileType<AcediaGatewayTile>(), -1, 4, 7);
            AvaritiaPortal = AddGatewayToList(TileType<AvaritianGatewayTile>(), -1, 4, 7);
            int i = TileType<SOTSGemLockTiles>();
            GemlockAmethyst = AddToList(i, 216, 1, 1);
            GemlockTopaz = AddToList(i, 162, 1, 1);
            GemlockSapphire = AddToList(i, 54, 1, 1);
            GemlockEmerald = AddToList(i, 108, 1, 1);
            GemlockRuby = AddToList(i, 0, 1, 1);
            GemlockDiamond = AddToList(i, 270, 1, 1);
            GemlockAmber = AddToList(i, 324, 1, 1);
            IceMonument = AddToList(TileType<FrostArtifactTile>(), -1, 1, 0);
            i = TileType<StrangeKeystoneTile>();
            CoconutIslandMonumentBroken = AddToList(i, 54, 1, 2);
            CoconutIslandMonument = AddToList(i, -1, 1, 2);
            DreamLamp = AddToList(TileType<ForgottenLampTile>(), -1, 1, 0);
            DamoclesChain = AddToList(TileType<Items.Tide.ArkhalisChainTile>());
            BigCrystal = AddToList(TileType<Items.Earth.BigCrystalTile>(), -1, 6, 8);
            GulaPortal = AddGatewayToList(TileType<GulaGatewayTile>(), -1, 4, 7);
            InvidiaPortal = AddGatewayToList(TileType<InvidiaGatewayTile>(), -1, 14, 20);
            IraPortal = AddGatewayToList(TileType<IraGatewayTile>(), -1, 4, 7);

            DreamLamp.IsArchaeologistSpot = false;

            AcediaPortal.LeftElementType = TileType<DissolvingNatureTile>();
            AcediaPortal.RightElementType = TileType<DissolvingEarthTile>();

            AvaritiaPortal.LeftElementType = TileType<DissolvingAetherTile>();
            AvaritiaPortal.RightElementType = TileType<DissolvingBrillianceTile>();

            GulaPortal.LeftElementType = TileType<DissolvingEarthTile>();
            GulaPortal.RightElementType = TileType<DissolvingUmbraTile>();

            IraPortal.LeftElementType = TileType<DissolvingNatureTile>();
            IraPortal.RightElementType = TileType<DissolvingNetherTile>();

            InvidiaPortal.LeftElementType = TileType<DissolvingDelugeTile>();
            InvidiaPortal.RightElementType = TileType<DissolvingNetherTile>();
        }
        public static GatewayImportantTile AcediaPortal { get; private set; }
        public static GatewayImportantTile AvaritiaPortal { get; private set; }
        public static ImportantTile GemlockAmethyst { get; private set; }
        public static ImportantTile GemlockTopaz { get; private set; }
        public static ImportantTile GemlockSapphire { get; private set; }
        public static ImportantTile GemlockEmerald { get; private set; }
        public static ImportantTile GemlockRuby { get; private set; }
        public static ImportantTile GemlockDiamond { get; private set; }
        public static ImportantTile GemlockAmber { get; private set; }
        public static ImportantTile IceMonument { get; private set; }
        public static ImportantTile CoconutIslandMonumentBroken { get; private set; }
        public static ImportantTile CoconutIslandMonument { get; private set; }
        public static ImportantTile DreamLamp { get; private set; }
        public static ImportantTile DamoclesChain { get; private set; }
        public static ImportantTile BigCrystal { get; private set; }
        public static GatewayImportantTile GulaPortal { get; private set; }
        public static GatewayImportantTile InvidiaPortal { get; private set; }
        public static GatewayImportantTile IraPortal { get; private set; }
        public static bool DebugChatMessages = false;
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
                List[pointType].Position = ptToSync;
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
        public static void SyncImportantTileLocations(Player clientSender, ImportantTile landmark, int destinationClient = -1)
        {
            int x = 0;
            int y = 0;
            if (!landmark.Position.HasValue)
                x = y = -1;
            else
            {
                x = landmark.Position.Value.X; 
                y = landmark.Position.Value.Y;
            }
            int playerWhoAmI = clientSender != null ? clientSender.whoAmI : -1;
            var packet = Instance.GetPacket();
            packet.Write((byte)SOTSMessageType.SyncTileLocations);
            packet.Write(playerWhoAmI);
            packet.Write(landmark.ID);
            packet.Write(x);
            packet.Write(y);
            packet.Send(destinationClient);
        }
        public static void SyncAllLocations(int toClient = -1)
        {
            foreach(ImportantTile landmark in List)
                SyncImportantTileLocations(null, landmark, toClient);
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
                    if(Main.netMode == NetmodeID.Server && DebugChatMessages)
                        Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Server runs this"), Color.Gray);
                }
                if ((finishedThreading || (finishedFirstPacketSend && newPlayerRequestingPackets)) && Main.netMode == NetmodeID.Server)
                {
                    if(DebugChatMessages)
                        Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("SyncedData"), Color.Gray);
                    SyncAllLocations();
                    newPlayerRequestingPackets = false;
                    finishedThreading = false;
                    finishedFirstPacketSend = true;
                }
                if ((finishedFirstPacketSend || Main.netMode == NetmodeID.SinglePlayer) && SOTSWorld.GlobalCounter % 120 == 0 && SOTSWorld.GlobalCounter > 600) //this will be checked every 2 second
                {
                    TileLocationJustReset = false;
                    CheckCurrentLocations();
                    if(TileLocationJustReset && Main.netMode == NetmodeID.Server)
                    {
                        if(DebugChatMessages)
                            Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("SyncedData"), Color.Gray);
                        SyncAllLocations();
                    }
                }
            }
        }
        public static void AddNewNumberToPrevious(int toAdd)
        {
            PreviousTeleports[4] = PreviousTeleports[3];
            PreviousTeleports[3] = PreviousTeleports[2];
            PreviousTeleports[2] = PreviousTeleports[1];
            PreviousTeleports[1] = PreviousTeleports[0];
            PreviousTeleports[0] = toAdd;
        }
        public static int[] PreviousTeleports = new int[5] { -1, -1, -1, -1, -1 };
        ///<summary>
        /// Gets the location of a random important tile
        /// Also yield the direction for the Archaeologist to face and the ID of the tile
        ///</summary>
        public static Vector2? RandomImportantLocation(ref int importantTileID, ref int directionToGo)
        {
            importantTileID = -1;
            List<ImportantTile> validLandmarks = new();
            foreach(ImportantTile landmark in List)
                if(landmark.IsArchaeologistSpot)
                    validLandmarks.Add(landmark);
            Vector2? myDestination = null;
            int totalAttempts = 0;
            while (myDestination == null && validLandmarks.Count > 0)
            {
                int yOffset = 0;
                int randomPossibilites = Main.rand.Next(validLandmarks.Count);
                ImportantTile destination = validLandmarks[randomPossibilites];
                importantTileID = destination.ID;
                if (destination.HasValue && (!PreviousTeleports.Contains(importantTileID) || Main.rand.NextBool(4) || validLandmarks.Count < 4))
                {
                    Vector2 testDestination = new Vector2(destination.Position.Value.X * 16, destination.Position.Value.Y * 16);
                    bool valid = false;
                    int attempts = 55;
                    while(attempts > 0)
                    {
                        if(importantTileID == ImportantTileID.damoclesChain)
                        {
                            yOffset = 5;
                        }
                        int xOffset = Main.rand.Next(-10, 11);
                        if (xOffset == 0 || xOffset == -1)
                            xOffset = -2;
                        if (xOffset == 1)
                            xOffset = 2;
                        directionToGo = -Math.Sign(xOffset);
                        int tileX = destination.Position.Value.X + xOffset;
                        int tileY = destination.Position.Value.Y + yOffset;
                        bool tileSpace = false;
                        for (int j = -6; j <= 12; j++)
                        {
                            if (WorldgenHelpers.SOTSWorldgenHelper.TrueTileSolid(tileX, tileY + j, false))
                            {
                                bool validLiquidAndClear = true;
                                for(int i = -4; i >= -1; i--)
                                {
                                    if(WorldgenHelpers.SOTSWorldgenHelper.TrueTileSolid(tileX, tileY + j + i, false) || 
                                        Framing.GetTileSafely(tileX, tileY + j + i).LiquidType == LiquidID.Lava || 
                                        Framing.GetTileSafely(tileX, tileY + j + i).LiquidType == LiquidID.Honey)
                                    {
                                        validLiquidAndClear = false;
                                    }    
                                }
                                if(validLiquidAndClear)
                                {
                                    tileSpace = true;
                                    break;
                                }
                            }
                        }
                        if (tileSpace)
                        {
                            testDestination = new Vector2(tileX * 16, tileY * 16 - 24);
                            valid = true;
                            break;
                        }
                        attempts--;
                    }
                    if (valid)
                        myDestination = testDestination;
                    else
                        validLandmarks.RemoveAt(randomPossibilites);
                }
                else
                    validLandmarks.RemoveAt(randomPossibilites);
                totalAttempts++;
            }
            if (!myDestination.HasValue)
                return null;
            AddNewNumberToPrevious(importantTileID);
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
            foreach (ImportantTile landmark in List)
                landmark.Position = null;
            for (int i = 15; i < Main.maxTilesX - 15; i++)
            {
                for(int j = 15; j < Main.maxTilesY - 15; j++)
                {
                    Tile tile = Main.tile[i, j];
                    if(tile.HasTile)
                    {
                        foreach(ImportantTile landmark in List)
                        {
                            landmark.AssignPoint(tile, i, j);
                        }
                    }
                }
            }
            finishedThreading = true;
        }
        public static bool TileLocationJustReset { get; set; }
        ///<summary>
        /// Checks if a saved value for a special tile position is still valid. If not, the tile is removed from being a special position tile
        ///</summary>
        public static void CheckCurrentLocations()
        {
            foreach(ImportantTile landmark in List)
            {
                landmark.TileInCorrectLocation();
            }
        }
    }
}