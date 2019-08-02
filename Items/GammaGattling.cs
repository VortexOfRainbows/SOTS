using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class GammaGattling : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gamma Gattling");
		}
		public override void SetDefaults()
		{
            item.damage = 35;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 50;     //gun image width
            item.height = 28;   //gun image  height
            item.useTime = 3;  //how fast 
            item.useAnimation = 3;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 1000000;
            item.rare = 6;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = 207; 
            item.shootSpeed = 20;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ChainGun, 1);
			recipe.AddIngredient(ItemID.TacticalShotgun, 1);
			recipe.AddIngredient(ItemID.CursedFlame, 35);
			recipe.AddIngredient(null, "SoulSingularity", 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 2;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(9)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}
}
