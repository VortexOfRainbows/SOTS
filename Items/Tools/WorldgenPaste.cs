using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using SOTS.WorldgenHelpers;
using SOTS.Items.Earth.Glowmoth;

namespace SOTS.Items.Tools
{
	public class WorldgenPaste : ModItem
	{
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
			//Main.NewText(SOTSWorld.AmberKeySlotted);
			//Main.NewText(SOTSWorld.DreamLampSolved);
			Vector2 mousePos = Main.MouseWorld;
			Vector2 tileLocation = mousePos / 16f;
			AbandonedVillageWorldgenHelper.GenerateDownwardPath((int)tileLocation.X, (int)tileLocation.Y);
			//PhaseWorldgenHelper.ClearPrevious = true;
			//PhaseWorldgenHelper.Generate();
			//WorldGen.PlaceTile((int)tileLocation.X, (int)tileLocation.Y, ModContent.TileType<SilkCocoonTile>(), true, true, -1, 0);
			//counter++;
			//Vector2? Location = Common.Systems.ImportantTilesWorld.RandomImportantLocation();
			//if(Location.HasValue)
			//{
			//	player.Center = Location.Value;
			//}
			//SOTSWorld.DreamLampSolved = false;
			return true;
		}
	}
}