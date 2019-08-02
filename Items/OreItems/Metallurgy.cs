using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.OreItems
{
	public class Metallurgy : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Metallurgy");
			Tooltip.SetDefault("Like the waterbolt? Do it justice...");
		}
		public override void SetDefaults()
		{
            item.damage = 50;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 45;     //gun image width
            item.height = 45;   //gun image  height
            item.useTime = 20;  //how fast 
            item.useAnimation = 20;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 500000;
            item.rare = 8;
            item.UseSound = SoundID.Item28;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("Rainbolt"); 
            item.shootSpeed = 7;
			item.mana = 4;
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			
			recipe.AddIngredient(null, "Cobalter", 1);
			recipe.AddIngredient(null, "PalladiumSunStaff", 1);
			recipe.AddIngredient(null, "MythrilShotStaff", 1);
			recipe.AddIngredient(null, "OrichalcumFlower", 1);
			recipe.AddIngredient(null, "AdamantiteConcentraiter", 1);
			recipe.AddIngredient(null, "Titanic", 1);
			recipe.AddIngredient(null, "TrueHallowStaff", 1);
			recipe.AddIngredient(null, "ConcentraitionStaff", 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 5;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(11)); // This defines the projectiles random spread . 30 degree spread.
				  
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				

              }
              return false; 
	}
	}
}
