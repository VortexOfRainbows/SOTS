using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.TerraLine
{
	public class HallowStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hallow Staff");
			Tooltip.SetDefault("Shoots a scatter of colorful bolts");
		}
		public override void SetDefaults()
		{
            item.damage = 44;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 48;     //gun image width
            item.height = 48;   //gun image  height
            item.useTime = 34;  //how fast 
            item.useAnimation = 34;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 100000;
            item.rare = 5;
            item.UseSound = SoundID.Item28;
            item.autoReuse = true;
            item.shoot = 121; 
            item.shootSpeed = 12;
			item.mana = 13;
			item.reuseDelay = 8;
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HallowedBar, 22);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 2;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(12)); // This defines the projectiles random spread . 30 degree spread.
				  
				 if((Main.rand.Next(5) == 0))
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				  
				  if((Main.rand.Next(5) == 0))
				   Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, 122, damage, knockBack, player.whoAmI);
				   
				  if((Main.rand.Next(5) == 0))
				    Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, 123, damage, knockBack, player.whoAmI);
					
				  if((Main.rand.Next(5) == 0))
					 Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, 124, damage, knockBack, player.whoAmI);
					 
				  if((Main.rand.Next(5) == 0))
					  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, 125, damage, knockBack, player.whoAmI);
					  
				  if((Main.rand.Next(5) == 0))
					   Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, 126, damage, knockBack, player.whoAmI);
					
					
				  if((Main.rand.Next(5) == 0))
					   Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, 521, damage, knockBack, player.whoAmI);

              }
              return false; 
	}
	}
}
