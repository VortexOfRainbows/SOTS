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
			Tooltip.SetDefault("Development tool, NOT MEANT FOR GAMEPLAY\nUsing this may break your world, or spawn a ton of ore, or generate a whole pyramid\nWhatever the case, it's probably best not to use it");
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
		int counter = 0;
		public override bool UseItem(Player player)
		{
			Vector2 mousePos = Main.MouseWorld;
			Vector2 tileLocation = mousePos / 16f;
			PhaseWorldgenHelper.Generate();
			counter++;
			return true;
		}
	}
}