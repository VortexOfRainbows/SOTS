using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.Chess
{
    public class QueenSpikey : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spikey Rock");
			Tooltip.SetDefault("Summons a TurtleTem, counts as a light pet");
		}
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Carrot);
            item.shoot = mod.ProjectileType("PetName");
            item.buffType = mod.BuffType("Leaf");
			item.expert = true;
			item.damage = 36;
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