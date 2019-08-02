using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.OreItems
{
	public class StirringSands : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Topaz Stirring Sands");
			Tooltip.SetDefault("Generates sand with the power of the topaz");
		}
		public override void SetDefaults()
		{

			item.damage = 15;
			item.melee = true;
			item.width = 54;
			item.height = 54;
			item.useTime = 28;
			item.useAnimation = 28;
			item.useStyle = 1;
			item.knockBack = 5;
			item.value = 10000;
			item.rare = 3;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;            
			item.shoot = 42; 
            item.shootSpeed = 12;


		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"StoneSwinger", 1);
			recipe.AddIngredient(ItemID.Topaz, 3);
			recipe.AddIngredient(ItemID.TinBroadsword, 1);
			recipe.AddIngredient(ItemID.AntlionMandible, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
              int numberProjectiles = 1;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(4)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}
}