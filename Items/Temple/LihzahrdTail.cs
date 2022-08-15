using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Temple
{
	public class LihzahrdTail : ModItem
	{	
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases buff duration by 67%\nIncreases healing and mana recieved from potions by 50");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 30;     
            Item.height = 34;   
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Lime;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.PotionBuffDegradeRate -= 0.4f;
			modPlayer.additionalHeal += 50;
			modPlayer.additionalPotionMana += 50;
		}
	}
}