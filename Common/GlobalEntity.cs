using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Items.Void;
using SOTS.NPCs.Boss;
using SOTS.Items;
using SOTS.Buffs;
using SOTS.NPCs.Constructs;
using SOTS.Items.Celestial;
using SOTS.NPCs.Boss.Polaris;
using SOTS.NPCs.Inferno;
using SOTS.NPCs.Boss.Advisor;
using SOTS.Items.Pyramid;
using SOTS.Items.Otherworld;
using SOTS.Items.Pyramid.PyramidWalls;
using SOTS.NPCs.TreasureSlimes;
using SOTS.Items.Fragments;
using SOTS.Dusts;
using SOTS.Items.Crushers;
using SOTS.Items.Nature;
using SOTS.Items.Tools;
using SOTS.Items.Slime;
using SOTS.Items.Evil;
using SOTS.Items.Tide;
using SOTS.Items.Permafrost;
using SOTS.Items.Otherworld.Blocks;
using SOTS.Items.Inferno;
using SOTS.Items.Chaos;
using SOTS.Items.AbandonedVillage;
using SOTS.NPCs.Phase;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;
using System.Linq;
using SOTS.NPCs;
using SOTS.Items.Otherworld.FromChests;
using Terraria.GameContent.Bestiary;
using SOTS.Biomes;
using SOTS.Items.Temple;
using Terraria.Localization;
using SOTS.Items.Furniture.Functional;
using SOTS.NPCs.Boss.Glowmoth;
using SOTS.NPCs.Boss.Lux;
using SOTS.Items.Earth.Glowmoth;
using Terraria;
using System.IO;
using Terraria.ModLoader.IO;
using Terraria.ID;

namespace SOTS.Common
{
    public class GlobalEntityNPC : GlobalNPC
    {
        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            bitWriter.WriteBit(RecentlyTeleported);
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            RecentlyTeleported = bitReader.ReadBit();
        }
        public override bool InstancePerEntity => true;
        public bool RecentlyTeleported = false;
        private int TeleportCounter = 0;
        public override void PostAI(NPC npc)
        {
            if(RecentlyTeleported)
            {
                TeleportCounter++;
                if(TeleportCounter >= 1200)
                {
                    TeleportCounter = 0;
                    RecentlyTeleported = false;
                    if(Main.netMode == NetmodeID.Server)
                    {
                        npc.netUpdate = true;
                    }
                }
            }
        }
        public override bool CheckActive(NPC npc)
        {
            return RecentlyTeleported ? false : base.CheckActive(npc);
        }
    }
    public class GlobalEntityItem : GlobalItem
    {
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(RecentlyTeleported);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            RecentlyTeleported = reader.ReadBoolean();
        }
        public override bool InstancePerEntity => true;
        public bool RecentlyTeleported = false;
        public override bool OnPickup(Item item, Player player)
        {
            RecentlyTeleported = false;
            NetMessage.SendData(MessageID.SyncItem, -1, player.whoAmI, null, item.whoAmI);
            return base.OnPickup(item, player);
        }
        public void NetUpdate(Item item)
        {
            NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item.whoAmI);
        }
    }
}