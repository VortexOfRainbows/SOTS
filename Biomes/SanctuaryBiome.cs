using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SOTS.Buffs.Debuffs;
using SOTS.Helpers;
using SOTS.NPCs.Boss;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Biomes
{
	public class SanctuaryBiome : ModBiome
	{
		public override ModWaterStyle WaterStyle => ModContent.GetInstance<SanctuaryWaterStyle>(); // Sets a water style for when inside this biome
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
    public class SanctuaryWaterStyle : ModWaterStyle
	{
        public override string Texture => base.Texture;
        public override int ChooseWaterfallStyle()
        {
            return ModContent.GetInstance<SanctuaryWaterfallStyle>().Slot;
        }
        public override int GetSplashDust()
        {
            return DustID.Water_Desert; //ModContent.DustType<SanctuarySolutionDust>();
        }
        public override int GetDropletGore()
        {
            return ModContent.GoreType<SanctuaryDroplet>();
        }
        public override void LightColorMultiplier(ref float r, ref float g, ref float b)
        {
            r = 1f;
            g = 0.85f;
            b = 0.85f;
        }
        public override Color BiomeHairColor()
        {
            return ColorHelper.EmeraldColor;
        }
    }
    public class SanctuaryDroplet : ModGore
    {
        public override void SetStaticDefaults()
        {
            ChildSafety.SafeGore[Type] = true;
            GoreID.Sets.LiquidDroplet[Type] = true;

            UpdateType = GoreID.WaterDrip;
        }
    }
    public class SanctuaryWaterfallStyle : ModWaterfallStyle
    {
        //public override void AddLight(int i, int j) =>
        //    Lighting.AddLight(new Vector2(i, j).ToWorldCoordinates(), Color.White.ToVector3() * 0.5f);
    }
}