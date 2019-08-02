using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class CactusBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cactus Bow");
		}
		public override void SetDefaults()
		{
            item.damage = 6;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 24;     //gun image width
            item.height = 48;   //gun image  height
            item.useTime = 29;  //how fast 
            item.useAnimation = 29;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0.2f;
            item.value = 250;
            item.rare = 1;
            item.UseSound = SoundID.Item5;
            item.autoReuse = false;
            item.shoot = 336; 
            item.shootSpeed = 6;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Cactus , 15);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 2;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(11)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}
}
