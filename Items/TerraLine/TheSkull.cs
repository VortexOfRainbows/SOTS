using Terraria;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework;

using Terraria.ModLoader;

namespace SOTS.Items.TerraLine
{
	public class TheSkull : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ocean Spray");
			Tooltip.SetDefault("Fires fast but doesn't shoot fire");
		}
		public override void SetDefaults()
		{
            item.damage = 8;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 32;     //gun image width
            item.height = 46;   //gun image  height
            item.useTime = 6;  //how fast 
            item.useAnimation = 6;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 4;
            item.value = 10000;
            item.rare = 4;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("AquaArrow"); 
            item.shootSpeed = 12;
			

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Book, 10);
			recipe.AddIngredient(ItemID.Bone, 50);
			recipe.AddIngredient(ItemID.GoldBar, 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 1;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(6)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}
}
