using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SOTS.Items
{
	public class Ichscar: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ichscar");
			Tooltip.SetDefault("It's a shotgun for whatever reason");
		}
		public override void SetDefaults()
		{
            item.damage = 34;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 64;     //gun image width
            item.height = 32;   //gun image  height
            item.useTime = 11;  //how fast 
            item.useAnimation = 11;
            item.useStyle = 5;    
            item.noMelee = false; //so the item's animation doesn't do damage
            item.knockBack = 4;
            item.value = 92500;
            item.rare = 5;
            item.UseSound = SoundID.Item36;
            item.autoReuse = true;
            item.shoot = 279; 
            item.shootSpeed = 20;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Shotgun, 1);
			recipe.AddIngredient(null, "CrusherEmblem", 1);
			recipe.AddIngredient(ItemID.Vertebrae, 20);
			recipe.AddIngredient(null, "IchorGun", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 2;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(12)); // This defines the projectiles random spread . 30 degree spread.
				  
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
					Vector2 perturbedSpeed2 = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5)); // This defines the projectiles random spread . 30 degree spread.
				
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed2.X, perturbedSpeed2.Y, 280, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}
}
