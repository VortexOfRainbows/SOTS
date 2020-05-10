using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.GelGear
{
	public class PinkyCore : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putrid Cell");
			Tooltip.SetDefault("Access to infinite putrid bullets\nDecreases void regen by 1 while in the inventory");
		}public override void SetDefaults()
		{
			item.damage = 5;
			item.ranged = true;
			item.width = 36;
			item.height = 36;
			item.maxStack = 1;
			item.consumable = false;           
			item.knockBack = 0.2f;
            item.value = Item.sellPrice(0, 1, 25, 0);
			item.rare = 6;
			item.shoot = mod.ProjectileType("PinkyMusketBall");  
			item.shootSpeed = 0.3f;           
			item.ammo = AmmoID.Bullet;   
			item.expert = true;
		}
		public override void UpdateInventory(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidRegen -= 0.1f;
		}
		public override bool ConsumeAmmo(Player p)
		{
			return false;
		}
	}
}