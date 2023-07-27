using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;
using Terraria.Localization;
using SOTS.NPCs.Town;

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

		public override bool IsBiomeActive(Player player)
		{
			bool inBiome = (SOTSWorld.phaseBiome > 50) && player.Center.Y < Main.worldSurface * 16 * 0.35f; //phase biome if nearby ore is greater than 50
			return inBiome;
		}
	}
	public class AnomalyBiome : ModBiome
	{
		public override SceneEffectPriority Priority => SceneEffectPriority.None;
		public override string BestiaryIcon => base.BestiaryIcon; //default icon
		public override string BackgroundPath => "SOTS/Biomes/AnomalyBestiary"; //default background
		public override Color? BackgroundColor => base.BackgroundColor; //default background color
		public override bool IsBiomeActive(Player player)
		{
			if(!SOTSWorld.AmberKeySlotted
				|| !SOTSWorld.AmethystKeySlotted
				|| !SOTSWorld.TopazKeySlotted
				|| !SOTSWorld.SapphireKeySlotted
				|| !SOTSWorld.EmeraldKeySlotted
				|| !SOTSWorld.RubyKeySlotted
				|| !SOTSWorld.DiamondKeySlotted)
            {
				return false;
            }
			if(player.Distance(Archaeologist.AnomalyPosition2) < 1280 && !Archaeologist.AnomalyPosition2.Equals(VoidAnomaly.finalPositionAfterShatter))
            {
				return true;
			}
			if (player.Distance(Archaeologist.AnomalyPosition3) < 1280 && !Archaeologist.AnomalyPosition3.Equals(VoidAnomaly.finalPositionAfterShatter))
			{
				return true;
			}
			return false;
		}
	}
}