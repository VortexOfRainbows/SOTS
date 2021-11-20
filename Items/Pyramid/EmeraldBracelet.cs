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
			Tooltip.SetDefault("Increases void regen by 1.25");
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
            item.width = 30;     
            item.height = 28;   
            item.value = Item.sellPrice(0, 1, 50, 0);
            item.rare = ItemRarityID.LightRed;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidRegen += 0.125f;
		}
	}
}