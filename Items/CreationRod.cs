using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items
{
	public class CreationRod : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Creation Rod");
			Tooltip.SetDefault("Grief your world!");
		}
		public override void SetDefaults()
		{
            
            item.width = 28;     //gun image width
            item.height = 28;   //gun image  height
            item.useTime = 6;  //how fast 
            item.useAnimation = 6;
            item.useStyle = 1;    
			item.damage = 4;
            item.knockBack = 4;
            item.value = 0;
            item.rare = 4;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = 42; 
            item.shootSpeed = 8;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DirtRod, 1);
			recipe.AddIngredient(ItemID.Sandgun, 1);
			recipe.AddIngredient(ItemID.IceRod, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
			  
              int numberProjectiles = 12;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360)); // This defines the projectiles random spread . 30 degree spread.
                  int chanze = Main.rand.Next(6);
				  if(chanze == 0)
				  {
					   Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				  }
				   if(chanze == 1)
				  {
					  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, 17, damage, knockBack, player.whoAmI);
				  }
				   if(chanze == 2)
				  {
					  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, 65, damage, knockBack, player.whoAmI);
				  }
				   if(chanze == 3)
				  {
					  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, 68, damage, knockBack, player.whoAmI);
				  }
				   if(chanze == 4)
				  {
					  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, 354, damage, knockBack, player.whoAmI);	
				  }
				   if(chanze == 5)
				  {
					  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, 109, damage, knockBack, player.whoAmI);	
				  }
				 
				  
				  
				  
				  
				  
	  
              }
              return false; 
	}
	}
}
