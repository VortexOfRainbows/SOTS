using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class SpikyPufferfish : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spiky Pufferfish");
			Tooltip.SetDefault("Can be used as a bullet");
		}public override void SetDefaults()
		{
			item.damage = 1;
			item.ranged = true;
			item.width = 20;
			item.height = 20;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 0.12f;
			item.value = 125;
			item.rare = 2;
			item.shoot = mod.ProjectileType("SpikyPufferfish");   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 1.24f;                  //The speed of the projectile
			item.ammo = AmmoID.Bullet;   
            item.UseSound = SoundID.Item23;
		}
		public override void CaughtFishStack(ref int stack)
		{
			stack = Main.rand.Next(99,297);
		}
	}
}