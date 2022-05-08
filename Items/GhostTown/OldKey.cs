using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GhostTown
{
	class OldKey : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'It's starting to show signs of rusting'");
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