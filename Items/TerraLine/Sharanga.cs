using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.TerraLine
{
	public class Sharanga : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sharanga");
			Tooltip.SetDefault("Running hell");
		}
		public override void SetDefaults()
		{
            item.damage = 35;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 26;     //gun image width
            item.height = 50;   //gun image  height
            item.useTime = 16;  //how fast 
            item.useAnimation = 16;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 1000;
            item.rare = 4;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = 41; 
            item.shootSpeed = 16;
		
			item.reuseDelay = 20;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SteelBar", 16);
			recipe.AddIngredient(null, "RedPowerChamber", 1);
			recipe.AddIngredient(ItemID.HellwingBow, 1);
			recipe.AddIngredient(ItemID.MoltenFury, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 1;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, 316, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}
}
