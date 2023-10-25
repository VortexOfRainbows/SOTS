using Microsoft.Xna.Framework;
using SOTS.Void;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;
using Terraria.Localization;

namespace SOTS
{
	[BackgroundColor(45, 50, 65, 192)]
	public class SOTSConfig : ModConfig
	{
		public static int voidBarNeedsLoading = 0;
		public static int PreviousBarMode = 0;
		public override void OnChanged()
        {
			voidBarNeedsLoading++;
        }
        public override ConfigScope Mode => ConfigScope.ClientSide;

		[Header("$Mods.SOTS.Configuration.Header.GPS")]
		[BackgroundColor(110, 80, 150, 192)]
		[Increment(1)]
		[Range(-2, 20)]
		[DefaultValue(-1)]
		public int StarterHouseType;

		[Header("$Mods.SOTS.Configuration.Header.UI")]
		[BackgroundColor(110, 80, 150, 192)]
		[Increment(5)]
		[Range(0, 3200)]
		[DefaultValue(810)]
		public int voidBarPointX;

		[BackgroundColor(110, 80, 150, 192)]
		[Increment(5)]
		[Range(0, 1600)]
		[DefaultValue(30)]
		public int voidBarPointY;

		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(false)]
		public bool lockVoidBar { get; set; }

		[BackgroundColor(194, 111, 234, 192)]
		[DefaultValue(false)]
		public bool alternateVoidBarDirection { get; set; }

		[BackgroundColor(194, 111, 234, 192)]
		[DefaultValue(false)]
		public bool alternateVoidBarStyle { get; set; }

		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(true)]
		public bool voidBarTextOn { get; set; }

		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(true)]
		public bool voidBarHoverTextOn { get; set; }

		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(false)]
		public bool simpleVoidText { get; set; }

		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(false)]
		public bool simpleVoidFill { get; set; }

		[Header("$Mods.SOTS.Configuration.Header.GS")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(false)]
		public bool lowFidelityMode { get; set; }
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(true)]
		public bool coloredTimeFreeze { get; set; }

		[Header("$Mods.SOTS.Configuration.Header.TPS")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(true)]
		public bool additionalTexturePackVisuals { get; set; }

		/*[Label("Experimental Ambient Audio")]
		[Tooltip("Adds bonus audio, such as ambient sounds, to a handful of tiles\nOnly works with the SOTS Texture Pack enabled")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(false)]
		public bool playAmbientAudio = false;*/
	}
	[BackgroundColor(45, 50, 65, 192)]
	public class SOTSServerConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

		[Header("$Mods.SOTS.Configuration.Header.BAL")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(true)]
		public bool NerfInsignia;

        [BackgroundColor(110, 80, 150, 192)]
        [DefaultValue(true)]
		[ReloadRequired]
        public bool AddPlightToVanillaRecipes;

        [Header("$Mods.SOTS.Configuration.Header.GPS")]
        [BackgroundColor(110, 80, 150, 192)]
        [DefaultValue(true)]
        public bool GeneratePhaseOreAfterDefeatingLux;

        [BackgroundColor(110, 80, 150, 192)]
        [DefaultValue(true)]
        public bool NaturallySpawningPeanutBushes;
    }
}