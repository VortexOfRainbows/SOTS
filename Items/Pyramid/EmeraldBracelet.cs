using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.ID;

namespace SOTS.Items.Pyramid
{
	public class EmeraldBracelet : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Emerald Bracelet");
			Tooltip.SetDefault("Increases void gain by 2");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 30;     
            Item.height = 28;   
            Item.value = Item.sellPrice(0, 1, 50, 0);
            Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.bonusVoidGain += 2f;
		}
	}
}