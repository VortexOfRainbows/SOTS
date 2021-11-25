using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	public class FrigidHourglass : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Hourglass");
			Tooltip.SetDefault("Freezes your void meter for 15 seconds every minute\nProvides immunity to Slow, Chilled, Frozen, and Frostburn\nIncreases movement speed by 10%");
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
            item.width = 26;     
            item.height = 38;   
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Lime;
			item.accessory = true;
			item.expert = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.moveSpeed += 0.1f;
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.frozenMaxDuration += 900;
			voidPlayer.frozenMinTimer -= 900;
			player.buffImmune[BuffID.Slow] = true;
			player.buffImmune[BuffID.Chilled] = true;
			player.buffImmune[BuffID.Frozen] = true;
			player.buffImmune[BuffID.Frostburn] = true;
		}
	}
}