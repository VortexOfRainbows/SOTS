using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class SuperStick : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Stick");
			Tooltip.SetDefault("Doesn't catch on fire");
		}
		public override void SetDefaults()
		{

			item.damage = 11;
			item.magic = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 21;
			item.useAnimation = 21;
			item.useStyle = 1;
			item.knockBack = 2;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;            
			item.shoot = 504; 
            item.shootSpeed = 7;
			item.mana = 2;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WandofSparking, 1);
			recipe.AddIngredient(ItemID.Wood, 100);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
			  
              int numberProjectiles = 3;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(8)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);

              }
              return false; 
		  }
	}
}