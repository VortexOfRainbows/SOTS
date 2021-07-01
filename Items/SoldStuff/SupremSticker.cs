using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SoldStuff
{
	public class SupremSticker : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Suprem");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
            item.width = 60;     
            item.height = 22;   
            item.value = Item.buyPrice(15, 0, 0, 0);
            item.rare = ItemRarityID.Red;
			item.accessory = true;
			item.defense = 69696969;
			item.expert = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.endurance = -696968f;
		}
	}
}
