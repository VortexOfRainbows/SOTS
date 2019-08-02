using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Secrets.IceCream
{
	public class IceCreamOre : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alpha Virus");
			Tooltip.SetDefault("A superbug built to wipe out planets\nIt looks like strawberry icecream\nThe superbug can destroy worlds in mere minutes, it is highly advised to never release the virus unless you have a way to contain it");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 99999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 16;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = 9;
			item.value = 0;
			item.consumable = true;
			item.createTile = mod.TileType("IceCreamOreTile");
		}
		public override void CaughtFishStack(ref int stack)
		{
			stack = Main.rand.Next(4,8);
		}
	}
}