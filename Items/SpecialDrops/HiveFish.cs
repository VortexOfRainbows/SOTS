using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class HiveFish : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hive Fish");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{

			item.width = 34;
			item.height = 34;
			item.value = 6000;
			item.rare = 3;
			item.maxStack = 99;
		}
	}
}