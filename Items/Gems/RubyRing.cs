using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Gems
{
	public class RubyRing : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vulture's Ring");
			Tooltip.SetDefault("Killed enemies always drop hearts\nCollecting hearts lengthens the durations of active buffs");
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
			SOTSPlayer.ModPlayer(player).VultureRing = true;
		}
	}
}