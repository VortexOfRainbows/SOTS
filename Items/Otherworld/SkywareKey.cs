using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld
{
	class SkywareKey : ModItem
	{
		public override void SetDefaults()
		{
			//item.CloneDefaults(ItemID.GoldenKey);
			item.width = 22;
			item.height = 32;
			item.maxStack = 99;
			item.rare = 1;

			//item.useAnimation = 15;
			//item.useTime = 10;
			//item.useStyle = 1;
			//item.createTile = mod.TileType("LockedSkywareChest");
			//	item.placeStyle = 1;
		}
	}
}