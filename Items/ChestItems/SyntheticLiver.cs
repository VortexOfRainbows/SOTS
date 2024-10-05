using Microsoft.Xna.Framework;
using SOTS.Helpers;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.ChestItems
{
	public class SyntheticLiver : ModItem
	{
        public override Color? GetAlpha(Color lightColor)
        {
			lightColor = lightColor.MultiplyRGB(new Color((int)(ColorHelper.EvilColor.R * 3f), (int)(ColorHelper.EvilColor.G * 3f), (int)(ColorHelper.EvilColor.B * 3f)));
            return lightColor;
        }
        public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 56;     
            Item.height = 44;   
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
			Item.shopCustomPrice = Item.buyPrice(1, 0, 0, 0);
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			vPlayer.VoidFoodGainMultiplier += 0.25f;
			SOTSPlayer sPlayer = SOTSPlayer.ModPlayer(player);
			sPlayer.PotionStacking = true;
			sPlayer.DrainDebuffs = true;
		}
	}
}