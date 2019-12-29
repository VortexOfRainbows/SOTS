using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;


namespace SOTS.Items.SpecialDrops
{
	public class FireSpitter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fire Spitter");
		}
		public override void SetDefaults()
		{
            item.damage = 11; 
            item.ranged = true; 
            item.width = 46;    
            item.height = 16;   
            item.useTime = 23; 
            item.useAnimation = 23;
            item.useStyle = 5;    
            item.noMelee = true;
            item.knockBack = 0;
			item.value = Item.sellPrice(0, 0, 10, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item20;
            item.autoReuse = true;
            item.shoot = 85; 
            item.shootSpeed = 5.5f;
			item.useAmmo = 23; //setting ammo to reaquire gel

		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 2;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(4)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}

}
