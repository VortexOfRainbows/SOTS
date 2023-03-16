using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Earth.Glowmoth
{
	public class GlowSpores : ModItem
	{	
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 20;     
            Item.height = 34;   
            Item.value = Item.sellPrice(0, 0, 60, 0);
            Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.additionalPotionMana += 40;
			player.GetCritChance(DamageClass.Magic) += 5;
		}
	}
}