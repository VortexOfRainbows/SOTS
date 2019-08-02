using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;


namespace SOTS.Items.TerraLine
{
	public class SwarmSnapper : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Swarm Snapper");
			Tooltip.SetDefault("Fires your current bullets along with bees\n20% chance to not consume ammo");
		}
		public override void SetDefaults()
		{
            item.damage = 7;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 54;     //gun image width
            item.height = 20;   //gun image  height
            item.useTime = 8;  //how fast 
            item.useAnimation = 8;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 500000;
            item.rare = 5;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = 10; 
			item.useAmmo = AmmoID.Bullet;
            item.shootSpeed = 8;
			

		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 1;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(3)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
			  Vector2 perturbedSpeed2 = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(18)); // This defines the projectiles random spread . 30 degree spread.
				  
			  Vector2 perturbedSpeed3 = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(18)); // This defines the projectiles random spread . 30 degree spread.
			  
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed2.X, perturbedSpeed2.Y, 181, damage, knockBack, player.whoAmI);
				  
			  
              return false; 
	}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BeeGun, 1);
			recipe.AddIngredient(ItemID.Minishark, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool ConsumeAmmo(Player p)
		{
			if(Main.rand.Next(5) == 0)
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
