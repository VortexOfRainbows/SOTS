using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Chess
{
	public class CheckeredBoard : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Checkered Board");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{

			item.width = 16;
			item.height = 16;
			item.value = 1250000;
			item.rare = 4;
			item.maxStack = 32;

		}
	}
}