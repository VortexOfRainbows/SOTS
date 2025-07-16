using Terraria;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class MrGlorp : ModItem
	{	
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 34;     
            Item.height = 42;   
            Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = ModContent.RarityType<AnomalyRarity>();
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
            player.SOTSPlayer().MrBurns = true;
            player.VoidPlayer().voidMeterMax2 += 100;
        }
    }
}