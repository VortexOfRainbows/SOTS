using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Gems
{
	public class AmethystRing : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Belphegor's Ring");
			Tooltip.SetDefault("Chain together blocks when mining, breaking up to 50 blocks at once");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 22;     
            Item.height = 20;   
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer.ModPlayer(player).LazyMinerRing = true;
		}
	}
}