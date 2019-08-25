using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class UnsafeLihzahrdBrickWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unsafe Lihzahrd Brick Wall");
			Tooltip.SetDefault("Allows mobs to spawn behind");
		}
		public override void SetDefaults()
		{
			
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 14;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = 0;
			item.value = 0;
			item.consumable = true;
			item.createWall = WallID.LihzahrdBrickUnsafe;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LihzahrdBrick, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LihzahrdBrickWall, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
              int numberProjectiles = 1;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(12)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}
}