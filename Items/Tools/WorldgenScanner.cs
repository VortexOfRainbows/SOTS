using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using SOTS.Void;
using SOTS.Items.Otherworld;
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
			DisplayName.SetDefault("Planetary Analyzer");
			Tooltip.SetDefault("Displays some worldgen info that may be useful\n'Calls down the protector of that palace in the sky'");
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
			if(Main.netMode != NetmodeID.MultiplayerClient)
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
							if(!NPC.AnyNPCs(ModContent.NPCType<TheAdvisorHead>()))
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
					Main.NewText("Lihzahrd Altar: Not Found", 255, 0, 0);
				if (!IceMonument)
					Main.NewText("Frost Artifact: Not Found", 255, 0, 0);
				if (!Sarcophagus)
					Main.NewText("Sarcophagus: Not Found", 255, 0, 0);
				if(!GreedGateway)
					Main.NewText("Avaritia Gateway: Not Found", 255, 0, 0);
				if (!SlothGateway)
					Main.NewText("Acedia Gateway: Not Found", 255, 0, 0);
				if(LihzahrdAltar && IceMonument && Sarcophagus && GreedGateway && SlothGateway)
				{
					Main.NewText("World has generated without major issues", 0, 255, 0);
				}
				VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);

				ShadowOrbs /= 4;

				if(spawnAdvisor)
					ChatMessage("A distant whirring can be heard from the sky", new Color(45, 140, 170));
				if(ShadowOrbs < 15)
					ChatMessage("Shadow Orbs/Crimson Hearts: " + ShadowOrbs, new Color(255, ShadowOrbs >= 10 ? (byte)255 : (byte)0, 0));
				if(WaterWalkingBoots < 5)
					ChatMessage("Water Walking Boots: " + WaterWalkingBoots, new Color(255, WaterWalkingBoots >= 2 ? (byte)255 : (byte)0, 0));
				if (LavaCharms < 5)
					ChatMessage("Lava Charms: " + LavaCharms, new Color(255, LavaCharms >= 2 ? (byte)255 : (byte)0, 0));
				if (FlowerBoots < 5)
					ChatMessage("Flower Boots: " + FlowerBoots, new Color(255, FlowerBoots >= 2 ? (byte)255 : (byte)0, 0));
			}
			return true;
		}
		public void ChatMessage(string text, Color color)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
				return;
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