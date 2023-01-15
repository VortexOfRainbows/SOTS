using Microsoft.Xna.Framework;
using SOTS.Void;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;

namespace SOTS
{
	[Label("Config")]
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

		[Header("UI")]
		[Label("Void Bar X")]
		[Tooltip("The Void Bar's horizontal position on the screen\nMeasured in pixels from left")]
		[BackgroundColor(110, 80, 150, 192)]
		[Increment(5)]
		[Range(0, 3200)]
		[DefaultValue(810)]
		public int voidBarPointX;

		[Label("Void Bar Y")]
		[Tooltip("The Void Bar's vertical position on the screen\nMeasured in pixels from top")]
		[BackgroundColor(110, 80, 150, 192)]
		[Increment(5)]
		[Range(0, 1600)]
		[DefaultValue(30)]
		public int voidBarPointY;

		[Label("Lock Void Bar")]
		[Tooltip("Prevents the Void Bar from being moved by your cursor\nAlso prevents the Void Bar from automatically switching positions when using the 'Bars' Health and Mana Style interface setting")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(false)]
		public bool lockVoidBar { get; set; }

		[Label("Alternative Void Bar Direction")]
		[Tooltip("Changes the Void Bar's roll down direction")]
		[BackgroundColor(194, 111, 234, 192)]
		[DefaultValue(false)]
		public bool alternateVoidBarDirection { get; set; }

		[Label("Alternative Void Bar Style")]
		[Tooltip("Modifies the Void Bar's visuals slightly")]
		[BackgroundColor(194, 111, 234, 192)]
		[DefaultValue(false)]
		public bool alternateVoidBarStyle { get; set; }

		[Label("Void Bar Top Text")]
		[Tooltip("Enable/disable the text above the Void Bar")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(true)]
		public bool voidBarTextOn { get; set; }

		[Label("Void Bar Hover Text")]
		[Tooltip("Enable/disable the text that appears when you hover over the Void Bar")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(true)]
		public bool voidBarHoverTextOn { get; set; }

		[Label("Simple Void Bar Text")]
		[Tooltip("Removes Void Minion quantity indicators from the Void Bar text")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(false)]
		public bool simpleVoidText { get; set; }

		[Label("Simple Void Bar Fill")]
		[Tooltip("Removes the division lines between inner fill Void Bar elements")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(false)]
		public bool simpleVoidFill { get; set; }

		[Header("Graphics Settings")]
		[Label("Performance Mode")]
		[Tooltip("Reduces various visual effects in order to increase framerate\nMostly affects boss-related visuals and projectile trails")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(false)]
		public bool lowFidelityMode { get; set; }
		[Label("Colored Time Freeze")]
		[Tooltip("Whether the color of the screen during time freeze is tinted or black and white")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(true)]
		public bool coloredTimeFreeze { get; set; }

		[Header("Texture Pack Settings")]
		[Label("Additional Visual Effects")]
		[Tooltip("Adds bonus visuals, such as dusts, to a handful of tiles\nOnly works with the SOTS Texture Pack enabled")]
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