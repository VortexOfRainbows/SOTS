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
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 60;     
            Item.height = 22;   
            Item.value = Item.buyPrice(15, 0, 0, 0);
            Item.rare = ItemRarityID.Red;
			Item.accessory = true;
			Item.defense = 69696969;
			Item.expert = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.endurance = -696968f;
		}
	}
}
