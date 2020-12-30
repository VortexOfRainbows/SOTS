using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;


namespace SOTS.Items.Pyramid
{
	public class RoyalMagnum : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Royal Magnum");
      Tooltip.SetDefault("Summon a phantom javelin upon hitting an enemy");
		}
		public override void SetDefaults()
		{

            item.damage = 11;
            item.ranged = true;
            item.width = 42; 
            item.height = 22;
            item.useTime = 15;
            item.useAnimation = 15;
            item.useStyle = 5;    
            item.noMelee = true;
            item.knockBack = 4;
            item.value = Item.sellPrice(0, 1, 50, 0);
            item.rare = 4;
            item.UseSound = SoundID.Item11;
            item.autoReuse = false;
            item.shoot = 14; 
            item.shootSpeed = 26;
			item.useAmmo = AmmoID.Bullet;

		}

		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
         {
              int numberProjectiles = 1; //amount of projectiles
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(1)); // This defines the projectiles random spread. 4 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("SandBullet"), damage, knockBack, player.whoAmI);
              }
              return false; 
		}
	}
}
