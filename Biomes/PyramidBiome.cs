using Microsoft.Xna.Framework;
using SOTS.Items.Pyramid;
using System;
using System.Linq;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace SOTS.Biomes
{
	public class PyramidBiome : ModBiome
	{
		//public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("ExampleMod/ExampleWaterStyle"); // Sets a water style for when inside this biome
		//public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("ExampleMod/ExampleSurfaceBackgroundStyle");
		//public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Crimson;

		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/CursedPyramid");
		public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

		// Populate the Bestiary Filter
		//public override string BestiaryIcon => base.BestiaryIcon; //default icon
		public override string BackgroundPath => "SOTS/Biomes/PyramidBestiary"; //default background
		public override Color? BackgroundColor => base.BackgroundColor; //default background color
		public override string MapBackground => BackgroundPath;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Pyramid");
		}
		public override bool IsBiomeActive(Player player)
		{
			bool pyramidBiome;
			int tileBehindX = (int)(player.Center.X / 16);
			int tileBehindY = (int)(player.Center.Y / 16);
			Tile tile = Framing.GetTileSafely(tileBehindX, tileBehindY);
			if (SOTSWall.unsafePyramidWall.Contains(tile.WallType) || tile.WallType == (ushort)ModContent.WallType<TrueSandstoneWallWall>())
			{
				pyramidBiome = true;
			}
			else
			{
				pyramidBiome = SOTSWorld.pyramidBiome > 0; //if there is a sarcophagus, acedia portal, or zepline block on screen
			}
			return pyramidBiome;
		}
	}
}