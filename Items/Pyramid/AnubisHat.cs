using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Items.Pyramid
{
	[AutoloadEquip(EquipType.Head)]
	public class AnubisHat : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 28;
			item.vanity = true;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Blue;
		}
	}
}