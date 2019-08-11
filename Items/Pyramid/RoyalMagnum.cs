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
      Tooltip.SetDefault("The King's Personal Gun");
		}
		public override void SetDefaults()
		{
            item.damage = 8;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 42;     //gun image width
            item.height = 36;   //gun image  height
            item.useTime = 15;  //how fast 
            item.useAnimation = 15;
            item.useStyle = 5;    
            item.noMelee = false; //so the item's animation doesn't do damage
            item.knockBack = 4;
            item.value = Item.sellPrice(0, 1, 50, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item36;
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
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
		}
	}
}
