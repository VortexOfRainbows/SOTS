using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using SOTS.Void;

namespace SOTS.Items.ChestItems
{
	public class WorldgenScanner : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Worldgen Scanner");
			Tooltip.SetDefault("Displays worldgen info\nIt is recommended that you create a new world whenever any integral worldgen elements have been overriden\nSmall worlds have a higher chance of error");

		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 44;
			item.useTime = 48;
			item.useAnimation = 48;
			item.useStyle = 3;
			item.value = 0;
			item.rare = 9;
			item.UseSound = SoundID.Item1;
		}
		public override bool UseItem(Player player)
		{
			bool LihzahrdAltar = false;
			bool CrimsonChest = false;
			bool CorruptionChest = false;
			bool HallowedChest = false;
			bool IceChest = false;
			bool JungleChest = false;

			int ShadowOrbs = 0;

			bool IceMonument = false;
			bool Sarcophagus = false;

			int WaterWalkingBoots = 0;
			int LavaCharms = 0;
			int FlowerBoots = 0;

			for (int scanX = 0; scanX < Main.maxTilesX; scanX++)
			{
				for (int scanY = 0; scanY < Main.maxTilesY; scanY++)
				{
					Tile tile = Main.tile[scanX, scanY];
					if (tile.type == TileID.LihzahrdAltar)
						LihzahrdAltar = true;
					if (tile.type == mod.TileType("FrostArtifactTile"))
						IceMonument = true;
					if (tile.type == mod.TileType("SarcophagusTile"))
						Sarcophagus = true;

					if (tile.type == TileID.ShadowOrbs)
						ShadowOrbs++;
				}
			}
			foreach (Chest chest in Main.chest.Where(c => c != null))
			{
				if(chest.item[0].type == ItemID.VampireKnives)
					CrimsonChest = true;
				if (chest.item[0].type == ItemID.ScourgeoftheCorruptor)
					CorruptionChest = true;
				if (chest.item[0].type == ItemID.RainbowGun)
					HallowedChest = true;
				if (chest.item[0].type == ItemID.StaffoftheFrostHydra)
					IceChest = true;
				if (chest.item[0].type == ItemID.PiranhaGun)
					JungleChest = true;

				if (chest.item[0].type == ItemID.WaterWalkingBoots)
					WaterWalkingBoots++;
				if (chest.item[0].type == ItemID.LavaCharm)
					LavaCharms++;
				if (chest.item[0].type == ItemID.FlowerBoots)
					FlowerBoots++;
			}
				//string defenseText = defense.ToString();
			if(LihzahrdAltar) {
				Main.NewText("Lihzahrd Altar: Found", 0, 255, 0);
			} else {
				Main.NewText("Lihzahrd Altar: Not Found", 255, 0, 0);
			}
			if (IceMonument){
				Main.NewText("Frost Artifact: Found", 0, 255, 0);
			} else {
				Main.NewText("Frost Artifact: Not Found", 255, 0, 0);
			}
			if (Sarcophagus){
				Main.NewText("Sarcophagus: Found", 0, 255, 0);
			} else {
				Main.NewText("Sarcophagus: Not Found", 255, 0, 0);
			}
			if (!CrimsonChest && !CorruptionChest)
			{
				Main.NewText("Vampire Knives: Not Found", 255, 0, 0);
				Main.NewText("Scourge of the Corrupter: Not Found", 255, 0, 0);
			}
			if (!HallowedChest)
			{
				Main.NewText("Rainbow Gun: Not Found", 255, 0, 0);
			}
			if (!IceChest)
			{
				Main.NewText("Staff of the Frost Hydra: Not Found", 255, 0, 0);
			}
			if (!JungleChest)
			{
				Main.NewText("Pirahna Gun: Not Found", 255, 0, 0);
			}
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);

			ShadowOrbs /= 4;
			Main.NewText("Shadow Orbs/Crimson Hearts: " + ShadowOrbs, ShadowOrbs <= 15 ? (byte)255 : (byte)0, ShadowOrbs >= 12 ? (byte)255 : (byte)0, 0);
			Main.NewText("Water Walking Boots: " + WaterWalkingBoots, WaterWalkingBoots <= 10 ? (byte)255 : (byte)0, WaterWalkingBoots >= 3 ? (byte)255 : (byte)0, 0);
			Main.NewText("Lava Charms: " + LavaCharms, LavaCharms <= 10 ? (byte)255 : (byte)0, LavaCharms >= 3 ? (byte)255 : (byte)0, 0);
			Main.NewText("Flower Boots: " + FlowerBoots, FlowerBoots <= 10 ? (byte)255 : (byte)0, FlowerBoots >= 3 ? (byte)255 : (byte)0, 0);
			return true;
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