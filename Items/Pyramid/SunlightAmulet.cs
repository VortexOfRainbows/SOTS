using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Items.Pyramid
{
	public class SunlightAmulet : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sunlight Amulet");
			Tooltip.SetDefault("Grants permanent hunter and dangersense effects");
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
            item.width = 28;     
            item.height = 28;   
            item.value = Item.sellPrice(0, 1, 00, 0);
            item.rare = ItemRarityID.Orange;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.detectCreature = true;
			player.dangerSense = true;
		}
	}
}