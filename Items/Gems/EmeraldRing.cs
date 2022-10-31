using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Gems
{
	public class EmeraldRing : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Leviathan's Ring");
			Tooltip.SetDefault("Enemies have a 20% chance to drop double the loot");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 24;     
            Item.height = 22;   
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer.ModPlayer(player).ImposterRing = true;
		}
	}
}