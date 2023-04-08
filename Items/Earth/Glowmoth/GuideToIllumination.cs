using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Earth.Glowmoth
{
	public class GuideToIllumination : ModItem
	{	
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 28;     
            Item.height = 38;   
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.SparkleDamage = true;
		}
	}
}