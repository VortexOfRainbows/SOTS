using Terraria;
using Terraria.ID;
using SOTS.Void;
 
namespace SOTS.Items.SpecialDrops
{
    public class HungryHunter : VoidItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hungry Hunter");
			Tooltip.SetDefault("Latches onto enemies and regenerates void upon hit\nIncreases max void by 10 while in the inventory");
		}
        public override void SafeSetDefaults()
        {
            item.width = 32;
            item.height = 34;
            item.value = Item.sellPrice(0, 1, 20, 0);
            item.rare = 5;
            item.noMelee = true;
            item.useStyle = 5;
            item.useAnimation = 40;
            item.useTime = 40;
            item.knockBack = 4.5f;
            item.damage = 19;
            item.noUseGraphic = true; 
            item.shoot = mod.ProjectileType("HungryHunter");
            item.shootSpeed = 21.5f;
            item.UseSound = SoundID.Item1;
            item.melee = true; 
            item.channel = true;
        }
		public override void UpdateInventory(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidMeterMax2 += 10;
		}
		public override void GetVoid(Player player)
		{
			voidMana = 10;
		}
    }
}