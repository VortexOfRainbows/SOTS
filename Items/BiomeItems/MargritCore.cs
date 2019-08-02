using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems
{
	public class MargritCore : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Margrit Core");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{

			item.width = 22;
			item.height = 22;
			item.value = 75000;
			item.rare = 6;
			item.maxStack = 99;
		}
	}
}