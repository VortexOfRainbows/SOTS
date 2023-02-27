using Microsoft.Xna.Framework;
using SOTS.Void;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;
using Terraria.Localization;

namespace SOTS
{
	[Label("$Mods.SOTS.Configs.Label.Config")]
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

		[Label("$Mods.SOTS.Configs.Header.UI")]
		[Label("$Mods.SOTS.Configs.Label.VBX")]
		[Tooltip("$Mods.SOTS.Configs.Tooltip.VBX")]
		[BackgroundColor(110, 80, 150, 192)]
		[Increment(5)]
		[Range(0, 3200)]
		[DefaultValue(810)]
		public int voidBarPointX;

		[Label("$Mods.SOTS.Configs.Label.VBY")]
		[Tooltip("$Mods.SOTS.Configs.Tooltip.VBY")]
		[BackgroundColor(110, 80, 150, 192)]
		[Increment(5)]
		[Range(0, 1600)]
		[DefaultValue(30)]
		public int voidBarPointY;

		[Label("$Mods.SOTS.Configs.Label.LVB")]
		[Tooltip("$Mods.SOTS.Configs.Tooltip.LVB")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(false)]
		public bool lockVoidBar { get; set; }

		[Label("$Mods.SOTS.Configs.Label.AVBD")]
		[Tooltip("$Mods.SOTS.Configs.Tooltip.AVBD")]
		[BackgroundColor(194, 111, 234, 192)]
		[DefaultValue(false)]
		public bool alternateVoidBarDirection { get; set; }

		[Label("$Mods.SOTS.Configs.Label.AVBS")]
		[Tooltip("$Mods.SOTS.Configs.Tooltip.AVBS")]
		[BackgroundColor(194, 111, 234, 192)]
		[DefaultValue(false)]
		public bool alternateVoidBarStyle { get; set; }

		[Label("$Mods.SOTS.Configs.Label.VBTT")]
		[Tooltip("$Mods.SOTS.Configs.Tooltip.VBTT")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(true)]
		public bool voidBarTextOn { get; set; }

		[Label("$Mods.SOTS.Configs.Label.VBHT")]
		[Tooltip("$Mods.SOTS.Configs.Tooltip.VBHT")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(true)]
		public bool voidBarHoverTextOn { get; set; }

		[Label("$Mods.SOTS.Configs.Label.SVBT")]
		[Tooltip("$Mods.SOTS.Configs.Tooltip.SVBT")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(false)]
		public bool simpleVoidText { get; set; }

		[Label("$Mods.SOTS.Configs.Label.SVBF")]
		[Tooltip("$Mods.SOTS.Configs.Tooltip.SVBF")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(false)]
		public bool simpleVoidFill { get; set; }

		[Header("$Mods.SOTS.Configs.Header.GS")]
		[Label("$Mods.SOTS.Configs.Label.PM")]
		[Tooltip("$Mods.SOTS.Configs.Tooltip.PM")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(false)]
		public bool lowFidelityMode { get; set; }
		[Label("$Mods.SOTS.Configs.Label.CTF")]
		[Tooltip("$Mods.SOTS.Configs.Tooltip.CTF")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(true)]
		public bool coloredTimeFreeze { get; set; }

		[Header("$Mods.SOTS.Configs.Header.TPS")]
		[Label("$Mods.SOTS.Configs.Label.AVE")]
		[Tooltip("$Mods.SOTS.Configs.Tooltip.AVE")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(true)]
		public bool additionalTexturePackVisuals { get; set; }

		/*[Label("Experimental Ambient Audio")]
		[Tooltip("Adds bonus audio, such as ambient sounds, to a handful of tiles\nOnly works with the SOTS Texture Pack enabled")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(false)]*/
		public bool playAmbientAudio = false;
	}
}