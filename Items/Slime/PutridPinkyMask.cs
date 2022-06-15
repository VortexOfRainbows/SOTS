using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Slime
{
	[AutoloadEquip(EquipType.Head)]
	public class PutridPinkyMask : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 20;
			Item.vanity = true;
			Item.value = Item.sellPrice(0, 0, 75, 0);
			Item.rare = ItemRarityID.Blue;
		}
	}
}