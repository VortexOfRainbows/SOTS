using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.Secrets
{
    public class DeoxysABall : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pokeball");
			Tooltip.SetDefault("Legendary drop\nLevels up as you progress\nDisable buff to show level");
		}
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Carrot);
            item.shoot = mod.ProjectileType("DeoxysAttack");
            item.buffType = mod.BuffType("DeoxysAttackBuff");
			item.rare = -11;
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