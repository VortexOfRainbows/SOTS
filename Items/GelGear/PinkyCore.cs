using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GelGear
{
	public class PinkyCore : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putrid Cell");
			Tooltip.SetDefault("Access to infinite putrid bullets");
		}public override void SetDefaults()
		{
			item.damage = 8;
			item.ranged = true;
			item.width = 36;
			item.height = 36;
			item.maxStack = 1;
			item.consumable = false;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 0.2f;
			item.value = 200000;
			item.rare = 6;
			item.shoot = mod.ProjectileType("PinkyMusketBall");   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 0.3f;                  //The speed of the projectile
			item.ammo = AmmoID.Bullet;   
			item.expert = true;//The ammo class this ammo belongs to.
		}
		public override bool ConsumeAmmo(Player p)
		{
				return false;
			
		}
	}
}