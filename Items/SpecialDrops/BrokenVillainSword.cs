using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class BrokenVillainSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Broken Villain Sword");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 38;
            item.value = Item.sellPrice(0, 7, 50, 0);
			item.rare = ItemRarityID.Yellow;
			item.maxStack = 99;
		}
	}
}