using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld
{
	public class GravityAnchor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gravity Anchor");
			Tooltip.SetDefault("Normalizes gravity and grants immunity to knockback");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 26;     
            Item.height = 36;   
            Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer.ModPlayer(player).normalizedGravity = true;
			player.noKnockback = true;
		}
    }
}