using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.TerraLine
{
	public class TrueHallowStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Hallow Staff");
			Tooltip.SetDefault("Shoots a scatter of homing magical bolts");
		}
		public override void SetDefaults()
		{
            item.damage = 66;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 48;     //gun image width
            item.height = 48;   //gun image  height
            item.useTime = 30;  //how fast 
            item.useAnimation = 30;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 100000;
            item.rare = 5;
            item.UseSound = SoundID.Item28;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("NeBolt"); 
            item.shootSpeed = 12;
			item.mana = 13;
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
			recipe.AddIngredient(null, "HallowStaff", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 2;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(12)); // This defines the projectiles random spread . 30 degree spread.
				  

                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);


              }
              return false; 
	}
	}
}
