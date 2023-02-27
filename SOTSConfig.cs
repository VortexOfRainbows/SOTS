using Microsoft.Xna.Framework;
using SOTS.Void;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;
using Terraria.Localization;

namespace SOTS
{//TODO: 文本无法正常显示
	[Label("$Mods.SOTS.Configuration.Label.Config")]
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

		[Header("$Mods.SOTS.Configuration.Header.UI")]
		[Label("$Mods.SOTS.Configuration.Label.VBX")]
		[Tooltip("$Mods.SOTS.Configuration.Tooltip.VBX")]
		[BackgroundColor(110, 80, 150, 192)]
		[Increment(5)]
		[Range(0, 3200)]
		[DefaultValue(810)]
		public int voidBarPointX;

		[Label("$Mods.SOTS.Configuration.Label.VBY")]
		[Tooltip("$Mods.SOTS.Configuration.Tooltip.VBY")]
		[BackgroundColor(110, 80, 150, 192)]
		[Increment(5)]
		[Range(0, 1600)]
		[DefaultValue(30)]
		public int voidBarPointY;

		[Label("$Mods.SOTS.Configuration.Label.LVB")]
		[Tooltip("$Mods.SOTS.Configuration.Tooltip.LVB")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(false)]
		public bool lockVoidBar { get; set; }

		[Label("$Mods.SOTS.Configuration.Label.AVBD")]
		[Tooltip("$Mods.SOTS.Configuration.Tooltip.AVBD")]
		[BackgroundColor(194, 111, 234, 192)]
		[DefaultValue(false)]
		public bool alternateVoidBarDirection { get; set; }

		[Label("$Mods.SOTS.Configuration.Label.AVBS")]
		[Tooltip("$Mods.SOTS.Configuration.Tooltip.AVBS")]
		[BackgroundColor(194, 111, 234, 192)]
		[DefaultValue(false)]
		public bool alternateVoidBarStyle { get; set; }

		[Label("$Mods.SOTS.Configuration.Label.VBTT")]
		[Tooltip("$Mods.SOTS.Configuration.Tooltip.VBTT")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(true)]
		public bool voidBarTextOn { get; set; }

		[Label("$Mods.SOTS.Configuration.Label.VBHT")]
		[Tooltip("$Mods.SOTS.Configuration.Tooltip.VBHT")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(true)]
		public bool voidBarHoverTextOn { get; set; }

		[Label("$Mods.SOTS.Configuration.Label.SVBT")]
		[Tooltip("$Mods.SOTS.Configuration.Tooltip.SVBT")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(false)]
		public bool simpleVoidText { get; set; }

		[Label("$Mods.SOTS.Configuration.Label.SVBF")]
		[Tooltip("$Mods.SOTS.Configuration.Tooltip.SVBF")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(false)]
		public bool simpleVoidFill { get; set; }

		[Header("$Mods.SOTS.Configuration.Header.GS")]
		[Label("$Mods.SOTS.Configuration.Label.PM")]
		[Tooltip("$Mods.SOTS.Configuration.Tooltip.PM")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(false)]
		public bool lowFidelityMode { get; set; }
		[Label("$Mods.SOTS.Configuration.Label.CTF")]
		[Tooltip("$Mods.SOTS.Configuration.Tooltip.CTF")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(true)]
		public bool coloredTimeFreeze { get; set; }

		[Header("$Mods.SOTS.Configuration.Header.TPS")]
		[Label("$Mods.SOTS.Configuration.Label.AVE")]
		[Tooltip("$Mods.SOTS.Configuration.Tooltip.AVE")]
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