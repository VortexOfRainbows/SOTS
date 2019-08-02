using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops.Legendary
{
	public class ParticleFish : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Subatomic Fish");
			Tooltip.SetDefault("Found in the smallest puddles that can support fish\nLegendary Fish");
		}
		public override void SetDefaults()
		{

			item.width = 34;
			item.height = 34;
			item.value = 100;
			item.rare = 9;
			item.maxStack = 99;
		}
	}
}