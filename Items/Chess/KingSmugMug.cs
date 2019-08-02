using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.Chess
{
    public class KingSmugMug : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dapper Mug");
			Tooltip.SetDefault("Summons a Dapperaichu, counts as a light pet");
		}
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Carrot);
            item.shoot = mod.ProjectileType("PetName");
            item.buffType = mod.BuffType("Dapper");
			item.damage = 1;
			item.expert = true;
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