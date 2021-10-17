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
	}
}