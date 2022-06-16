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
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 28;     
            Item.height = 28;   
            Item.value = Item.sellPrice(0, 1, 00, 0);
            Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.detectCreature = true;
			player.dangerSense = true;
		}
	}
}