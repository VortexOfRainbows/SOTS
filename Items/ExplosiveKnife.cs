using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items
{
	public class ExplosiveKnife : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosive Knife");
			Tooltip.SetDefault("'Quite a deadly combination'");
		}
		public override void SetDefaults()
		{
			
			item.CloneDefaults(279);
			item.damage = 15;
			item.useTime = 17;
			item.useAnimation = 17;
			item.thrown = true;
			item.rare = 2;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("ExplosiveKnife"); 
            item.shootSpeed = 12f;
			item.consumable = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 1;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(1)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(279, 15);
			recipe.AddIngredient(168, 15);
			recipe.AddIngredient(null, "Goblinsteel", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 15);
			recipe.AddRecipe();
		}
	}
}