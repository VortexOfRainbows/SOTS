using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.AbandonedVillage
{
	class OldKey : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(25);
		}
		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 32;
			Item.maxStack = 99; 
			Item.rare = ItemRarityID.Blue;
		}
	}
}