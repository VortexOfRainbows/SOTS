using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Crushers
{
	public class CrushingTransformer : ModItem
	{	
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases melee speed by 8%\nIncreases Crusher charge speed by 12%");
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
            item.width = 24;     
            item.height = 30;   
            item.value = Item.buyPrice(0, 15, 0, 0);
            item.rare = ItemRarityID.Orange;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeSpeed += 0.08f;
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			vPlayer.CrushTransformer += 0.12f;
		}
	}
}