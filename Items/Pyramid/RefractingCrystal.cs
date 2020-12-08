using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;
using Terraria.DataStructures;

namespace SOTS.Items.Pyramid
{
	public class RefractingCrystal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Refracting Crystal");
			Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 18;
			item.maxStack = 999;
			item.value = Item.sellPrice(1, 0, 0, 0);
			item.rare = 6;
		}
	}
}