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
			Item.width = 34;
			Item.height = 34;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.useStyle = ItemUseStyleID.Thrust;
			Item.value = 0;
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = SoundID.Item1;
		}
		public override void HoldItem(Player player)
		{
			player.rulerGrid = true;
		}
		public override bool? UseItem(Player player)
		{
			Vector2 mousePos = Main.MouseWorld;
			Vector2 tileLocation = mousePos / 16f;
			//PhaseWorldgenHelper.ClearPrevious = true;
			//PhaseWorldgenHelper.Generate();
			SOTSWorldgenHelper.FindAndGenerateBigGeode(Main.rand.Next(2) * 2 -1);
			//counter++;
			return true;
		}
	}
}