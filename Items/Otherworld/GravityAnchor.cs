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
		}
		public override void SetDefaults()
		{
            item.width = 26;     
            item.height = 36;   
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = ItemRarityID.Orange;
			item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer.ModPlayer(player).normalizedGravity = true;
			player.noKnockback = true;
		}
    }
}