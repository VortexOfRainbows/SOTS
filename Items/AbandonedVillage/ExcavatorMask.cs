using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.AbandonedVillage
{
	[AutoloadEquip(EquipType.Head)]
	public class ExcavatorMask : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 22;
			Item.vanity = true;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Orange;
		}
	}
}