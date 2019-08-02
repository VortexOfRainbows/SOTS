using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.OreItems
{
	public class Duallex : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Emerald Duallex");
			Tooltip.SetDefault("Doubles power with the speed of the emerald");
		}
		public override void SetDefaults()
		{

			item.damage = 16;
			item.melee = true;
			item.width = 50;
			item.height = 50;
			item.useTime = 29;
			item.useAnimation = 29;
			item.useStyle = 1;
			item.knockBack = 5;
			item.value = 10000;
			item.rare = 3;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;            
			item.shoot = 207; 
            item.shootSpeed = 13;


		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"StoneSwinger", 1);
			recipe.AddIngredient(ItemID.Emerald, 3);
			recipe.AddIngredient(ItemID.TungstenBroadsword, 1);
			recipe.AddIngredient(ItemID.JungleSpores, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
              int numberProjectiles = 2;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(4)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}
}