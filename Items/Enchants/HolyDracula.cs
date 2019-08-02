using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class HolyDracula : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Relic I : Dracula");
			Tooltip.SetDefault("Fires a barrage of homing, life-stealing bolts");
		}
		public override void SetDefaults()
		{
            item.damage = 60;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 68;     //gun image width
            item.height = 46;   //gun image  height
            item.useTime = 1;  //how fast 
            item.useAnimation = 2;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 1000000000;
            item.rare = 10;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("Dracula"); 
            item.shootSpeed = 21;
			item.expert = true;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "CopperGunIIIIII", 1);
			recipe.AddIngredient(null, "SwarmSnapper", 1);
			recipe.AddIngredient(null, "StormStinger", 1);
			recipe.AddIngredient(null, "TheBatalisk", 1);
			recipe.AddIngredient(null, "TheHardCore", 7);
			recipe.AddIngredient(null,"AntimaterialMandible", 15);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 1;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(24)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				  if(Main.rand.Next(3) == 0)
				  {
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				  }
              }
			  
			  Vector2 perturbedSpeed2 = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(24)); // This defines the projectiles random spread . 30 degree spread.
				  
			  Vector2 perturbedSpeed3 = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(24)); // This defines the projectiles random spread . 30 degree spread.
			  
			  Vector2 perturbedSpeed4 = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(24)); // This defines the projectiles random spread . 30 degree spread.
			  
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed2.X, perturbedSpeed2.Y,  mod.ProjectileType("Dracula2"), damage, knockBack, player.whoAmI);
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed3.X, perturbedSpeed3.Y,  mod.ProjectileType("Dracula2"), damage, knockBack, player.whoAmI);
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed4.X, perturbedSpeed4.Y,  mod.ProjectileType("Dracula2"), damage, knockBack, player.whoAmI);
			  
              return false; 
	}
	}
}
