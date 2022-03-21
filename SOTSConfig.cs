using Microsoft.Xna.Framework;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;

namespace SOTS
{
	[Label("Config")]
	[BackgroundColor(45, 50, 65, 192)]
	public class SOTSConfig : ModConfig
	{
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
		[Tooltip("Prevents the Void Bar from being moved by your cursor")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(false)]
		public bool lockVoidBar { get; set; }

		[Label("Void Bar Text")]
		[Tooltip("Enable/disable the text above the Void Bar")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(true)]
		public bool voidBarTextOn { get; set; }

		[Label("Simple Void Bar Text")]
		[Tooltip("Removes Void Minion quantity indicators from the Void Bar text")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(false)]
		public bool simpleVoidText { get; set; }

		[Label("Void Bar Blur")]
		[Tooltip("Slightly blurs the inner fill of the Void Bar\nIs somewhat more intensive to run")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(false)]
		public bool voidBarBlur { get; set; }

		[Label("Simple Void Bar Fill")]
		[Tooltip("Removes the division lines between inner fill Void Bar elements")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(false)]
		public bool simpleVoidFill { get; set; }

		[Header("Graphics Settings")]
		[Label("Performance Mode")]
		[Tooltip("Reduces various visual effects in order to increase framerate\nMostly affects boss-related visuals")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(false)]
		public bool lowFidelityMode { get; set; }
		[Label("Colored Time Freeze")]
		[Tooltip("Whether the color of the screen during time freeze is tinted or black and white")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(true)]
		public bool coloredTimeFreeze { get; set; }
	}
}