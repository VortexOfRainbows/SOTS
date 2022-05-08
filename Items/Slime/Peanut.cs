using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Slime
{
	public class Peanut : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Peanut");
			Tooltip.SetDefault("'The favorite snack of a devilish slime'");
		}
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 26;
			Item.maxStack = 999;      
			Item.knockBack = 1.15f;
			Item.value = Item.sellPrice(0, 0, 0, 25);
			Item.rare = 1;
			Item.ammo = Item.type;
			Item.consumable = true;
		}
	}
}