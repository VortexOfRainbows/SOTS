using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Gems
{
	public class NumismaticRing : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Numismatic Ring");
			Tooltip.SetDefault("Generate income when the void is gained\nDecreases void regeneration speed by 30%");
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
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			vPlayer.voidRegenSpeed -= 0.3f;
			vPlayer.VoidGenerateMoney += 1f;
		}
	}
}