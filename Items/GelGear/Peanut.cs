using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GelGear
{
	public class Peanut : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Peanut");
			Tooltip.SetDefault("'The favorite snack of a Devilish slime'");
		}public override void SetDefaults()
		{
			item.width = 26;
			item.height = 26;
			item.maxStack = 999;      
			item.knockBack = 1.15f;
			item.value = Item.sellPrice(0, 0, 0, 25);
			item.rare = 1;       
		}
	}
}