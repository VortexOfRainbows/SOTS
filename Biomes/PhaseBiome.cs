using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace SOTS.Biomes
{
	public class PhaseBiome : ModBiome
	{
		//public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("ExampleMod/ExampleWaterStyle"); // Sets a water style for when inside this biome
		//public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("ExampleMod/ExampleSurfaceBackgroundStyle");
		//public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Crimson;

		//public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Planetarium");
        public override SceneEffectPriority Priority => SceneEffectPriority.None;

        // Populate the Bestiary Filter
        public override string BestiaryIcon => base.BestiaryIcon; //default icon
		public override string BackgroundPath => base.BackgroundPath; //default background
		public override Color? BackgroundColor => base.BackgroundColor; //default background color

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phase Ore Clusters");
		}
		public override bool IsBiomeActive(Player player)
		{
			bool inBiome = (SOTSWorld.phaseBiome > 50) && player.Center.Y < Main.worldSurface * 16 * 0.35f; //phase biome if nearby ore is greater than 50
			return inBiome;
		}
	}
}