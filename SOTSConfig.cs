using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace SOTS
{
	[Label("Config")]
	[BackgroundColor(45, 50, 65, 192)]
	public class SOTSConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Header("Graphics Settings")]
		[Label("Performance Mode")]
		[Tooltip("Reduces various visual effects in order to increase framerate\nMostly affects boss-related visuals")]
		[BackgroundColor(110, 80, 150, 192)]
		[DefaultValue(false)]
		public bool lowFidelityMode { get; set; }

		[Header("UI")]
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
	}
}