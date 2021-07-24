using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using SOTS.Void;
using SOTS.Items.Otherworld;
using SOTS.Items.Pyramid;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using SOTS.NPCs.Boss;
using SOTS.NPCs.Boss.Advisor;

namespace SOTS.Items.ChestItems
{
	public class WorldgenScanner : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Planetary Analyzer");
			Tooltip.SetDefault("Displays some worldgen info that may be useful\n\n'Calls down the protector of that palace in the sky'");
		}
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 34;
			item.useTime = 48;
			item.useAnimation = 48;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.value = 0;
			item.rare = ItemRarityID.Cyan;
			item.UseSound = SoundID.Item1;
		}
		public override bool UseItem(Player player)
		{
			if(Main.netMode != 1)
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
						if (tile.type == TileID.LihzahrdAltar)
							LihzahrdAltar = true;
						if (tile.type == mod.TileType("FrostArtifactTile"))
							IceMonument = true;
						if (tile.type == mod.TileType("SarcophagusTile"))
							Sarcophagus = true;
						if (tile.type == ModContent.TileType<AvaritianGatewayTile>())
						{
							if(!NPC.AnyNPCs(ModContent.NPCType<TheAdvisorHead>()))
							{
								spawnAdvisor = AvaritianGatewayTile.SpawnAdvisor(scanX, scanY, mod);
							}
							GreedGateway = true;
						}
						if (tile.type == ModContent.TileType<AcediaGatewayTile>())
						{
							SlothGateway = true;
						}
						if (tile.type == TileID.ShadowOrbs)
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
				NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(text), color);
			else
				Main.NewText(text, color);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("IronBar", 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}