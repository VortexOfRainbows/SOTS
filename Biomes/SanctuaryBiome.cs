using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SOTS.Buffs.Debuffs;
using SOTS.Helpers;
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
		public override int Music => NPC.CountNPCS(ModContent.NPCType<SubspaceSerpentHead>()) > 0 ? MusicHelper.SubspaceSerpent : MusicHelper.Sanctuary;
		public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

		// Populate the Bestiary Filter
		//public override string BestiaryIcon => base.BestiaryIcon; //default icon
		public override string BackgroundPath => "SOTS/Biomes/SanctuaryBestiary"; //default background
		public override Color? BackgroundColor => base.BackgroundColor; //default background color
		public override string MapBackground => BackgroundPath;
		public override bool IsBiomeActive(Player player)
		{
			return (player.ZoneUnderworldHeight || player.Center.Y > (Main.maxTilesY - 250) * 16) && SOTSWorld.SanctuaryBiome >= 50;
		}
        public override void OnInBiome(Player player)
        {
			player.AddBuff(ModContent.BuffType<SanctuarySilence>(), 6, true);
        }
        public override int BiomeTorchItemType => ItemID.DemonTorch;
    }
    //public class ExampleWaterStyle : ModWaterStyle
	//{
    //    private Asset<Texture2D> rainTexture;
    //    public override void Load()
    //    {
    //        rainTexture = Mod.Assets.Request<Texture2D>("Content/Biomes/ExampleRain");
    //    }
    //    public override int ChooseWaterfallStyle()
    //    {
    //        return ModContent.GetInstance<ExampleWaterfallStyle>().Slot;
    //    }
    //    public override int GetSplashDust()
    //    {
    //        return ModContent.DustType<ExampleSolutionDust>();
    //    }
    //    public override int GetDropletGore()
    //    {
    //        return ModContent.GoreType<ExampleDroplet>();
    //    }
    //    public override void LightColorMultiplier(ref float r, ref float g, ref float b)
    //    {
    //        r = 1f;
    //        g = 1f;
    //        b = 1f;
    //    }
    //    public override Color BiomeHairColor()
    //    {
    //        return Color.White;
    //    }
    //    public override byte GetRainVariant()
    //    {
    //        return (byte)Main.rand.Next(3);
    //    }
    //    public override Asset<Texture2D> GetRainTexture() => rainTexture;
    //}
}