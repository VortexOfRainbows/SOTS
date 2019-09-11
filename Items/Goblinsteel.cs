using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class Goblinsteel : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Goblinsteel Bar");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 24;
			item.value = 125;
			item.rare = 2;
			item.maxStack = 99;
		}
	}
}