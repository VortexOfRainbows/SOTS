using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;


namespace SOTS.Items.TerraLine
{
	public class TheBatalisk : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Batalisk");
			Tooltip.SetDefault("Fires your current bullets along with bats\n50% chance to not consume ammo");
		}
		public override void SetDefaults()
		{
            item.damage = 31;   
            item.ranged = true;   
            item.width = 52;    
            item.height = 32;  
            item.useTime = 5;  
            item.useAnimation = 5;
            item.useStyle = 5;    
            item.noMelee = true; 
            item.knockBack = 1;
            item.value = 750000;
            item.rare = 9;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = 10; 
            item.shootSpeed = 10;
			item.useAmmo = AmmoID.Bullet;
			

		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 1;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(12)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
			  Vector2 perturbedSpeed2 = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(24)); // This defines the projectiles random spread . 30 degree spread.
				  
			  Vector2 perturbedSpeed3 = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(24)); // This defines the projectiles random spread . 30 degree spread.
			  
			  Vector2 perturbedSpeed4 = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(24)); // This defines the projectiles random spread . 30 degree spread.
			  
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed2.X, perturbedSpeed2.Y, 316, damage, knockBack, player.whoAmI);
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed3.X, perturbedSpeed3.Y, 316, damage, knockBack, player.whoAmI);
				  
			  
              return false; 
	}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BatScepter, 1);
			recipe.AddIngredient(ItemID.ChainGun, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool ConsumeAmmo(Player p)
		{
			if(Main.rand.Next(2) == 0)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}
}
