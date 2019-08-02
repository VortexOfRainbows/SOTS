using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.IceStuff
{
	public class BrassBeam : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brass Beam");
			Tooltip.SetDefault("Melts through blocks using its mining laser");
		}
		public override void SetDefaults()
		{
            item.magic = true;   //its a gun so set this to true
            item.width = 32;     //gun image width
            item.height = 40;   //gun image  height
            item.useTime = 3;  //how fast 
            item.useAnimation = 3;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 60000;
            item.rare = 6;
            item.UseSound = SoundID.Item30;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("BlockBeam"); 
            item.shootSpeed = 6;
			item.mana = 7;
		

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "AbsoluteBar", 2);
			recipe.AddIngredient(null, "BrassBar", 16);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 1;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(7)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}
}
