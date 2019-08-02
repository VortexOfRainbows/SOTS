using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.TerraLine
{
	public class CopperGunIIIIII : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloody Cataclysm");
			Tooltip.SetDefault("Fires a fast barrage of lifesteal bullets");
		}
		public override void SetDefaults()
		{
            item.damage = 34;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 68;     //gun image width
            item.height = 38;   //gun image  height
            item.useTime = 2;  //how fast 
            item.useAnimation = 2;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 1000000;
            item.rare = 11;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("TrueBlood"); 
            item.shootSpeed = 16f;
			item.expert = true;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "CopperGunIIIII", 1);
			recipe.AddIngredient(null, "TrueHallowedRepeater", 1);
			recipe.AddIngredient(null, "TheHardCore", 3);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 1;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(3)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}
}
