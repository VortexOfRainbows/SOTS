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
			Item.useTime = 8;
			Item.useAnimation = 15;
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
		public override bool? UseItem(Player player)
		{
			Vector2 mousePos = Main.MouseWorld;
			Vector2 tileLocation = mousePos / 16f;
			//PhaseWorldgenHelper.ClearPrevious = true;
			//PhaseWorldgenHelper.Generate();
			WorldGen.PlaceTile((int)tileLocation.X, (int)tileLocation.Y, ModContent.TileType<Furniture.Nature.NaturePlatingPlatformTile>(), true, true, -1, 0);
			//counter++;
			return true;
		}
	}
}