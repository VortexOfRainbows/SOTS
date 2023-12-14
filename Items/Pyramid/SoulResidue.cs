using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Pyramid
{
	public class SoulResidue : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(25);
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 2, 50);
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 9999;
			Item.alpha = 80;
		}
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * (1 - Item.alpha / 255f);
        }
    }
}