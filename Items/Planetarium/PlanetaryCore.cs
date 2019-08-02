using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class PlanetaryCore : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Planetary Core");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{

			item.width = 48;
			item.height = 48;
			item.value = 150000;
			item.rare = 9;
			item.maxStack = 99;
		}
	}
}