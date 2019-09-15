using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items
{
    public class LuckyPurpleBalloon : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lucky Purple Balloon");
			Tooltip.SetDefault("Grants an additional fishing line\nCounts as a light pet");
		}
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Carrot);
            item.shoot = mod.ProjectileType("LuckyPurpleBalloon");
            item.buffType = mod.BuffType("PurpleBalloon");
			item.value = 75000;
			item.rare = 4;
        }
 
        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }
    }
}