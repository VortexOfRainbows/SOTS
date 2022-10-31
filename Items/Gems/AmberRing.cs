using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Gems
{
	public class AmberRing : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Masochist's Ring");
			Tooltip.SetDefault("Getting hit grants a random buff for 30 seconds");
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
			Item.defense = 1;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer.ModPlayer(player).MasochistRing = true;
		}
	}
}