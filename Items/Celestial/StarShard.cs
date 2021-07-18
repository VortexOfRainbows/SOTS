using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Celestial
{
	public class StarShard : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Shard");
			Tooltip.SetDefault("'An assortment of various celestial materials'");
		}
		public override void SetDefaults()
		{

			item.width = 34;
			item.height = 38;
            item.value = Item.sellPrice(0, 0, 75, 0);
			item.rare = 8;
			item.maxStack = 99;
		}
		public override void PostUpdate()
		{
			Lighting.AddLight((int)((item.position.X + item.width / 2) / 16f), (int)((item.position.Y + item.height / 2) / 16f), 2f, 2f, 2f);
		}
	}
}