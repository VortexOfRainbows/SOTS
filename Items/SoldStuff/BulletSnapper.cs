using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
namespace SOTS.Items.SoldStuff
{
	public class BulletSnapper : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bullet Snapper");
			Tooltip.SetDefault("Fires a high velocity bullet\nThe bullet hits five times");
		}
		public override void SetDefaults()
		{
            item.damage = 48;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 48;     //gun image width
            item.height = 38;   //gun image  height
            item.useTime = 84;  //how fast 
            item.useAnimation = 84;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 3;
            item.value = 175000;
            item.rare = 6;
            item.UseSound = SoundID.Item36;
            item.autoReuse = false;
            item.shoot = 10; 
            item.shootSpeed = 48;
			item.useAmmo = AmmoID.Bullet;

		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 5;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(0)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
		public override bool ConsumeAmmo(Player p)
		{
			return false;
}

	}
}
