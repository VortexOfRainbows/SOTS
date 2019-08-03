using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items
{
	public class BloodBoiler : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Boiler");
			Tooltip.SetDefault("Their blood shall enrich you!");
		}
		public override void SetDefaults()
		{

			item.damage = 56;
			item.melee = true;
			item.width = 48;
			item.height = 51;
			item.useTime = 22;
			item.useAnimation = 22;
			item.useStyle = 1;
			item.knockBack = 3f;
			item.value = Item.sellPrice(2, 50, 0, 0);
			item.rare = 9;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;            
			item.shoot = 304; 
            item.shootSpeed = 14;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BloodButcherer, 1);
			recipe.AddIngredient(ItemID.VampireKnives, 1);
			recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
			recipe.AddIngredient(ItemID.BeetleHusk, 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
         {
              int numberProjectiles = 4; //amount of projectiles
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10)); // This defines the projectiles random spread. 4 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
        }
	}
}