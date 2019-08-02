using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems
{
	public class ObsidianScale : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Obsidian Scale");
			Tooltip.SetDefault("Tough");
		}
		public override void SetDefaults()
		{

			item.width = 24;
			item.height = 24;
			item.value = 125;
			item.rare = 6;
			item.maxStack = 99;
		}
	}
}