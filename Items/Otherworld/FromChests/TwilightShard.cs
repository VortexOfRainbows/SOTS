using Terraria;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld.FromChests
{
	public class TwilightShard : ModItem	
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twilight Shard");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
            item.width = 14;     
            item.height = 34;
            item.value = Item.sellPrice(0, 0, 4, 0);
            item.rare = 9;
		}
	}
	public class StarlightAlloy : ModItem
	{
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 24;
			item.height = 28;
			item.value = Item.sellPrice(0, 0, 45, 0);
			item.rare = 9;
		}
	}
	public class HardlightAlloy : ModItem
	{
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 24;
			item.height = 28;
			item.value = Item.sellPrice(0, 0, 45, 0);
			item.rare = 9;
		}
	}
	public class OtherworldlyAlloy : ModItem
	{
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 24;
			item.height = 28;
			item.value = Item.sellPrice(0, 0, 45, 0);
			item.rare = 9;
		}
	}
}