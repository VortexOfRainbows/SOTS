using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Crushers
{
	public class CrushingAmplifier : ModItem
	{	
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Extends the range of Crushers by 1\nAlso increases melee damage by 5%");
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
            item.width = 32;     
            item.height = 34;   
            item.value = Item.buyPrice(0, 6, 0, 0);
            item.rare = ItemRarityID.Orange;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			vPlayer.BonusCrushRangeMax++;
			vPlayer.BonusCrushRangeMin++;
			player.meleeDamage += 0.05f;
		}
	}
}