using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class FatBass : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fat Bass");
			Tooltip.SetDefault("It's huge and is worth a lot\nIt's too big to cook, you cannot prepare it by cooking it");
		}
		public override void SetDefaults()
		{

			item.width = 34;
			item.height = 34;
			item.value = 125000;
			item.rare = 5;
			item.maxStack = 99;
		}
	}
}