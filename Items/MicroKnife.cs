using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class MicroKnife : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Triplet Knife");
			Tooltip.SetDefault("Right click to unleash elemental powers");
		}
		public override void SetDefaults()
		{
            item.damage = 45;  //gun damage
            item.melee = true;   //its a gun so set this to true
            item.width = 36;     //gun image width
            item.height = 36;   //gun image  height
            item.useTime = 25;  //how fast 
            item.useAnimation = 5;
            item.useStyle = 1;    
            item.knockBack = 2.4f;
            item.value = 175000;
            item.rare = 5;
            item.UseSound = SoundID.Item18;
            item.autoReuse = true;
			item.useTurn = true;
			item.shootSpeed = 22;
			item.shoot = mod.ProjectileType("PikeProj");
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
				if(player.altFunctionUse == 2)
				{
			
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(2)); // This defines the projectiles random spread . 30 degree spread.
				  player.velocity.X = perturbedSpeed.X;
				  player.velocity.Y = perturbedSpeed.Y;
				  player.statLife += 5;
				  player.HealEffect(5);
				  
				  Projectile.NewProjectile(position.X, position.Y, -4, 0, 181, damage, knockBack, player.whoAmI);
				  Projectile.NewProjectile(position.X, position.Y, -4, -4, 181, damage, knockBack, player.whoAmI);
				  Projectile.NewProjectile(position.X, position.Y, 4, 0, 181, damage, knockBack, player.whoAmI);
				  Projectile.NewProjectile(position.X, position.Y, 4, 4, 181, damage, knockBack, player.whoAmI);
				  Projectile.NewProjectile(position.X, position.Y, -4, 4, 181, damage, knockBack, player.whoAmI);
				  Projectile.NewProjectile(position.X, position.Y, 4, -4, 181, damage, knockBack, player.whoAmI);
				  Projectile.NewProjectile(position.X, position.Y, 0, -4, 181, damage, knockBack, player.whoAmI);
				  Projectile.NewProjectile(position.X, position.Y, 0, 4, 181, damage, knockBack, player.whoAmI);
					for(int i = 1; i < 125; i++)
					{
					Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 65);
					Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 58);
					
					}
				return false;
				}
				else
				{
					return true;
				}
				
			
			
		}
		 public override bool AltFunctionUse(Player player)
        {
            return true;
        }
 
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)     //2 is right click
            {
			item.noUseGraphic = true;
            item.useStyle = 5; 
            }
            else
            {
			item.noUseGraphic = false;
            item.useStyle = 1;    
            }
            return base.CanUseItem(player);
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "FrostKnife", 1);
			recipe.AddIngredient(null, "ForbiddenKnife", 1);
			recipe.AddIngredient(null, "HoneyBlade", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
