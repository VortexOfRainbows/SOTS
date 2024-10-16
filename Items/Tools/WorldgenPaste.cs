using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.WorldgenHelpers;

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
		private int x = 0;
		public override bool? UseItem(Player player)
		{
			//Main.NewText(SOTSWorld.AmberKeySlotted);
			//Main.NewText(SOTSWorld.DreamLampSolved);
			x++;
			Vector2 mousePos = Main.MouseWorld;
			Vector2 tileLocation = mousePos / 16f;
			int dir = (x % 2 * 2) -1;
			int x2 = (int)tileLocation.X;
            int y2 = (int)tileLocation.Y;
			//WorldGen.PlaceTile(x2, y2, ModContent.TileType<FakeMarble>());

			SOTSWorldgenHelper.GenerateTestRoom();
            //AbandonedVillageWorldgenHelper.PrepareUnderground(new Rectangle(x2, y2, 250, 250));
            //AVHouseWorldgenHelper.GenerateHouse0(x2, y2);
            //AbandonedVillageWorldgenHelper.GenerateNewMineEntrance(x2, y2);
            //AbandonedVillageWorldgenHelper.PlaceStairDecor(x2, y2, 5);
            //AbandonedVillageWorldgenHelper.DesignateAVRectangle(x2, y2, 400, 320);
            //AbandonedVillageWorldgenHelper.GenerateTunnel(ref x2, ref y2, 0, doRopesPlatforms: false);

            //AbandonedVillageWorldgenHelper.GenerateRectangle(x2, y2, 30, 30);
            //AbandonedVillageWorldgenHelper.GenerateNewRubyGemStructure(x2, y2);
            // AbandonedVillageWorldgenHelper.DesignateDesiredEvilBiome();
            //AbandonedVillageWorldgenHelper.GeneratePortalBossRoom(x2, y2);
            //AbandonedVillageWorldgenHelper.GenerateEntireShaft(x2, y2, 5, dir, 1);
            //AbandonedVillageWorldgenHelper.GenerateVaultRoomLoot2(x2, y2, 59, 1);
            //AbandonedVillageWorldgenHelper.GenerateVaultRoom(x2, y2, dir);
            //AbandonedVillageWorldgenHelper.GenerateMineralariumRoom(x2, y2, dir);
            //AbandonedVillageWorldgenHelper.GenerateDownwardPathCircle((int)tileLocation.X, (int)tileLocation.Y);
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