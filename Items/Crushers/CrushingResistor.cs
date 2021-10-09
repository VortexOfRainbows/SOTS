using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Crushers
{
	public class CrushingResistor : ModItem
	{	
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("The second charge of a Crusher has a 33% chance to not consume void");
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
            item.width = 34;     
            item.height = 28;   
            item.value = Item.buyPrice(0, 6, 0, 0);
            item.rare = ItemRarityID.Orange;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			vPlayer.CrushResistor = true;
		}
	}
}