using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class GoblinRockBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Goblinsteel Bar");
			Tooltip.SetDefault("Forged with a special Goblin Forge");
		}
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 24;
			item.value = 125;
			item.rare = 4;
			item.maxStack = 99;
		}
	}
}