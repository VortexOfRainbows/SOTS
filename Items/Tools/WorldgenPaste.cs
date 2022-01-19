using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace SOTS.Items.Tools
{
	public class WorldgenPaste : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Worldgen Paste");
			Tooltip.SetDefault("Development tool, NOT MEANT FOR GAMEPLAY");
		}
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 34;
			item.useTime = 12;
			item.useAnimation = 12;
			item.useStyle = ItemUseStyleID.Stabbing;
			item.value = 0;
			item.rare = ItemRarityID.Cyan;
			item.UseSound = SoundID.Item1;
		}
		public override void HoldItem(Player player)
		{
			player.rulerGrid = true;
		}
		public override bool UseItem(Player player)
		{
			Vector2 mousePos = Main.MouseWorld;
			Vector2 tileLocation = mousePos / 16f;
			int size = 32; //radius of the geode
			float depthMult = 1f; //thickness multiplier of each layer
			SOTSWorldgenHelper.GenerateVibrantGeode((int)tileLocation.X, (int)tileLocation.Y, size, size, depthMult, (float)Math.Sqrt(depthMult));
			return true;
		}
	}
}