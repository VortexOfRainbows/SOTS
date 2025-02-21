using Microsoft.Xna.Framework;
using SOTS.Buffs.Debuffs;
using SOTS.NPCs.Boss;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Biomes
{
	public class SanctuaryBiome : ModBiome
	{
		//public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("ExampleMod/ExampleWaterStyle"); // Sets a water style for when inside this biome
		//public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("ExampleMod/ExampleSurfaceBackgroundStyle");
		//public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Crimson;
		public override int Music => NPC.CountNPCS(ModContent.NPCType<SubspaceSerpentHead>()) > 0 ? MusicLoader.GetMusicSlot(Mod, "Sounds/Music/SubspaceSerpent") : MusicLoader.GetMusicSlot(Mod, "Sounds/Music/BananaLizard/Sanctuary");
		public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

		// Populate the Bestiary Filter
		//public override string BestiaryIcon => base.BestiaryIcon; //default icon
		public override string BackgroundPath => "SOTS/Biomes/SanctuaryBestiary"; //default background
		public override Color? BackgroundColor => base.BackgroundColor; //default background color
		public override string MapBackground => BackgroundPath;
		public override bool IsBiomeActive(Player player)
		{
			return player.ZoneUnderworldHeight && SOTSWorld.SanctuaryBiome >= 50;
		}
        public override void OnInBiome(Player player)
        {
			player.AddBuff(ModContent.BuffType<SanctuarySilence>(), 6, true);
        }
        public override int BiomeTorchItemType => ItemID.DemonTorch;
    }
}