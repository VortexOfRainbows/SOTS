using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	[AutoloadEquip(EquipType.Head)]
	
	public class AnubisHat : ModItem
	{	int Probe = -1;
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 28;

			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = 5;
			
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Anubis Hat");
			Tooltip.SetDefault("Vanity Item");
		}
	}
}