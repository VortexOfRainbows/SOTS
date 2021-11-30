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
			item.width = 14;
			item.height = 32;
			item.maxStack = 99; 
			item.rare = ItemRarityID.Blue;
		}
	}
}