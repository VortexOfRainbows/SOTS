using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Pyramid
{
	public class SoulResidue : ModItem
	{
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(200, 200, 200, 0);
        }
        public override void SetDefaults()
		{
			item.width = 18;
			item.height = 28;
			item.value = Item.sellPrice(0, 0, 2, 50);
			item.rare = ItemRarityID.Orange;
			item.maxStack = 999;
		}
	}
}