using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.OreItems
{
	public class StoneSwinger : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stone Swinger");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{

			item.damage = 7;
			item.melee = true;
			item.width = 48;
			item.height = 48;
			item.useTime = 48;
			item.useAnimation = 48;
			item.useStyle = 1;
			item.knockBack = 2;
			item.value = 0;
			item.rare = 0;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;            
			item.shoot = mod.ProjectileType("Pebble"); 
            item.shootSpeed = 6;


		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 32);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
              int numberProjectiles = 2;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(28)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}
}