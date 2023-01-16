using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using SOTS.WorldgenHelpers;
using SOTS.Items.Earth.Glowmoth;

namespace SOTS.Items.Tools
{
	public class WorldgenFail: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Worldgen Paste 2");
			Tooltip.SetDefault("Development tool, NOT MEANT FOR GAMEPLAY\nUsing this may break your world, or spawn a ton of ore, or generate a whole pyramid\nWhatever the case, it's probably best not to use it");
		}
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 32;
			Item.useAnimation = 32;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = 0;
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}
		public override void HoldItem(Player player)
		{
			player.rulerGrid = true;
		}
		int num = 0;
		public override bool? UseItem(Player player)
		{
			Vector2 mousePos = Main.MouseWorld;
			Vector2 tileLocation = mousePos / 16f;
			int x = (int)tileLocation.X;
			int y = (int)tileLocation.Y;
			if(num % 8 == 0)
				GemStructureWorldgenHelper.GenerateAmethystDesertCamp(x, y);
			if (num % 8 == 1)
				GemStructureWorldgenHelper.GenerateTopazLihzahrdCamp(x, y);
			if (num % 8 == 2)
				GemStructureWorldgenHelper.GenerateSapphireIceCamp(x, y);
			if (num % 8 == 3)
				GemStructureWorldgenHelper.GenerateEmeraldVoidRuins(x, y);
			if (num % 8 == 4)
				GemStructureWorldgenHelper.GenerateRubyAbandonedLab(x, y);
			if (num % 8 == 5)
				GemStructureWorldgenHelper.GenerateRubyAbandonedLab(x, y, true);
			if (num % 8 == 6)
				GemStructureWorldgenHelper.GenerateDiamondSkyStructure(x, y);
			if (num % 8 == 7)
				GemStructureWorldgenHelper.GenerateAmberWaterVault(x, y);
			num++;
			return true;
		}
	}
}