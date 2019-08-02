using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class RottingBloodKoi : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rotting Blood Koi");
			Tooltip.SetDefault("It smells like rotting blood");
		}
		public override void SetDefaults()
		{

			item.width = 26;
			item.height = 28;
			item.value = 2250;
			item.rare = 2;
			item.maxStack = 99;
		}
	}
}