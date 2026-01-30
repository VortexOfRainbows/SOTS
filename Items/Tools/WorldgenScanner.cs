using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using SOTS.Void;
using SOTS.Items.Planetarium;
using SOTS.Items.Pyramid;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using SOTS.NPCs.Boss.Advisor;
using SOTS.Items.Permafrost;

namespace SOTS.Items.Tools
{
    public class WorldgenScanner : ModItem
    {
        public override void SetStaticDefaults()
        {
            this.SetResearchCost(1);
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 34;
            Item.useTime = 48;
            Item.useAnimation = 48;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.value = 0;
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.Item1;
        }
        public override bool? UseItem(Player player)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                /*bool CrimsonChest = false;
				bool CorruptionChest = false;
				bool HallowedChest = false;
				bool IceChest = false;
				bool JungleChest = false; */

                int ShadowOrbs = 0;

                bool spawnAdvisor = false;
                bool LihzahrdAltar = false;
                bool IceMonument = false;
                bool Sarcophagus = false;
                bool GreedGateway = false;
                bool SlothGateway = false;

                int WaterWalkingBoots = 0;
                int LavaCharms = 0;
                int FlowerBoots = 0;
                int PeanutBushes = 0;

                for (int scanX = 20; scanX < Main.maxTilesX - 20; scanX++)
                {
                    for (int scanY = 20; scanY < Main.maxTilesY - 20; scanY++)
                    {
                        Tile tile = Framing.GetTileSafely(scanX, scanY);
                        if (tile.TileType == TileID.LihzahrdAltar)
                            LihzahrdAltar = true;
                        if (tile.TileType == ModContent.TileType<FrostArtifactTile>())
                            IceMonument = true;
                        if (tile.TileType == ModContent.TileType<SarcophagusTile>())
                            Sarcophagus = true;
                        if (tile.TileType == ModContent.TileType<AvaritianGatewayTile>())
                        {
                            if (!NPC.AnyNPCs(ModContent.NPCType<TheAdvisorHead>()))
                            {
                                spawnAdvisor = AvaritianGatewayTile.SpawnAdvisor(scanX, scanY);
                            }
                            GreedGateway = true;
                        }
                        if (tile.TileType == ModContent.TileType<AcediaGatewayTile>())
                        {
                            SlothGateway = true;
                        }
                        if (tile.TileType == TileID.ShadowOrbs)
                            ShadowOrbs++;
                        if (tile.TileType == ModContent.TileType<PeanutBushTile>())
                            PeanutBushes++;
                    }
                }
                foreach (Chest chest in Main.chest.Where(c => c != null))
                {
                    /* if (chest.item[0].type == ItemID.VampireKnives)
						CrimsonChest = true;
					if (chest.item[0].type == ItemID.ScourgeoftheCorruptor)
						CorruptionChest = true;
					if (chest.item[0].type == ItemID.RainbowGun)
						HallowedChest = true;
					if (chest.item[0].type == ItemID.StaffoftheFrostHydra)
						IceChest = true;
					if (chest.item[0].type == ItemID.PiranhaGun)
						JungleChest = true; */

                    if (chest.item[0].type == ItemID.WaterWalkingBoots)
                        WaterWalkingBoots++;
                    if (chest.item[0].type == ItemID.LavaCharm)
                        LavaCharms++;
                    if (chest.item[0].type == ItemID.FlowerBoots)
                        FlowerBoots++;
                }
                if (!LihzahrdAltar)
                    ChatMessage("LihzahrdAltarNotFound", new Color(255, 0, 0));
                if (!IceMonument)
                    ChatMessage("FrostArtifactNotFound", new Color(255, 0, 0));
                if (!Sarcophagus)
                    ChatMessage("SarcophagusNotFound", new Color(255, 0, 0));
                if (!GreedGateway)
                    ChatMessage("AvaritiaGatewayNotFound", new Color(255, 0, 0));
                if (!SlothGateway)
                    ChatMessage("AcediaGatewayNotFound", new Color(255, 0, 0));
                if (LihzahrdAltar && IceMonument && Sarcophagus && GreedGateway && SlothGateway)
                    ChatMessage("WorldGeneratedWithoutIssues", new Color(0, 255, 0));

                ShadowOrbs /= 4;

                if (spawnAdvisor)
                    ChatMessage("AdvisorSpawned", new Color(45, 140, 170));
                if (ShadowOrbs < 15)
                    ChatMessage("ShadowOrbsCount", new Color(255, ShadowOrbs >= 10 ? (byte)255 : (byte)0, 0), ShadowOrbs);
                if (WaterWalkingBoots < 5)
                    ChatMessage("WaterWalkingBootsCount", new Color(255, WaterWalkingBoots >= 2 ? (byte)255 : (byte)0, 0), WaterWalkingBoots);
                if (LavaCharms < 5)
                    ChatMessage("LavaCharmsCount", new Color(255, LavaCharms >= 2 ? (byte)255 : (byte)0, 0), LavaCharms);
                if (FlowerBoots < 5)
                    ChatMessage("FlowerBootsCount", new Color(255, FlowerBoots >= 2 ? (byte)255 : (byte)0, 0), FlowerBoots);
                ChatMessage("PeanutBushesCount", new Color(255, PeanutBushes >= 2 ? (byte)255 : (byte)0, 0), PeanutBushes);
            }
            return true;
        }
        void ChatMessage(string key, Color color, params object[] args)
        {
            string text = Language.GetTextValue("Mods.SOTS.WorldgenScannerMessages." + key, args);
            if (Main.netMode == NetmodeID.Server)
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), color);
            else
                Main.NewText(text, color);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddRecipeGroup("IronBar", 5).AddTile(TileID.Anvils).Register();
        }
    }
}