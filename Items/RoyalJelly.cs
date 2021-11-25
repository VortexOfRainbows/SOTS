using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class RoyalJelly : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Royal Jelly");
			Tooltip.SetDefault("Increases healing recieved from potions by 40\n'I could make a very, very bad joke right now...'");
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
            item.width = 20;     
            item.height = 32;   
            item.value = Item.sellPrice(0, 0, 80, 0);
            item.rare = ItemRarityID.Orange;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.additionalHeal += 40;
		}
	}
}