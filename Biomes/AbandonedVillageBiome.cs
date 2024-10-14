using Microsoft.Xna.Framework;
using SOTS.Items.Furniture.Earthen;
using SOTS.Items.Pyramid;
using System;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Biomes
{
	public class AbandonedVillageBiome : ModBiome
	{
		//public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("ExampleMod/ExampleWaterStyle"); // Sets a water style for when inside this biome
		//public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("ExampleMod/ExampleSurfaceBackgroundStyle");
		//public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Crimson;

		//public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/CursedPyramid");
		public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

		// Populate the Bestiary Filter
		//public override string BestiaryIcon => base.BestiaryIcon; //default icon
		public override string BackgroundPath => WorldGen.crimson ? "SOTS/Biomes/AVCrimson" : "SOTS/Biomes/AVCorrupt"; //default background
		public override Color? BackgroundColor => base.BackgroundColor; //default background color
		public override string MapBackground => BackgroundPath;
        public override bool IsBiomeActive(Player player)
        {
            return SOTSWorld.AVBiome > 100 && (player.ZoneCorrupt || player.ZoneCrimson);
        }
        public override int BiomeTorchItemType => ModContent.ItemType<EarthenPlatingTorch>();
    }
}