using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class AntimaterialMandible : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Antimaterial Mandible");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{

			item.width = 10;
			item.height = 24;
			item.value = 12500;
			item.rare = 4;
			item.maxStack = 99;
		}
	}
}