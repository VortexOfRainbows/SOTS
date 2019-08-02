using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class FacelessDemonFish : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Faceless Demon");
			Tooltip.SetDefault("A faceless fish\nIts body is made partially out of metal, good for swimming in lava, bad for eating");
		}
		public override void SetDefaults()
		{

			item.width = 34;
			item.height = 34;
			item.value = 7500;
			item.rare = 5;
			item.maxStack = 99;
		}
	}
}