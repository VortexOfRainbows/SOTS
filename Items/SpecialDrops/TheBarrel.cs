using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;


namespace SOTS.Items.SpecialDrops
{
	public class TheBarrel : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Barrel");
			Tooltip.SetDefault("Smells fishy...");
		}
		public override void SetDefaults()
		{
            item.damage = 11;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 40;     //gun image width
            item.height = 26;   //gun image  height
            item.useTime = 42;  //how fast 
            item.useAnimation = 42;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0;
            item.value = 100000;
            item.rare = 4;
            item.UseSound = SoundID.Item13;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("Zephyr"); 
            item.shootSpeed = 4;
			

		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 1;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(8)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}

}
