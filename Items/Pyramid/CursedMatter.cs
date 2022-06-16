using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class CursedMatter : ModItem
	{
		public override void SetStaticDefaults()
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 6));
			this.SetResearchCost(25);
		}
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 30;
            Item.value = Item.sellPrice(0, 0, 7, 50);
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 999;
		}
	}
}