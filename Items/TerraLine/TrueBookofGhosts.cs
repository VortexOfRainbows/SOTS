using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.TerraLine
{
	public class TrueBookofGhosts : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Book of Ghosts");
			Tooltip.SetDefault("There cannot be shadows without light");
		}
		public override void SetDefaults()
		{
            item.damage = 63;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 28;     //gun image width
            item.height = 30;   //gun image  height
            item.useTime = 20;  //how fast 
            item.useAnimation = 20;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0;
            item.value = 30000;
            item.rare = 11;
            item.UseSound = SoundID.Item8;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("Ghost"); 
            item.shootSpeed = 1;
			item.expert = true;
			item.mana = 8;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BookofGhosts", 1);
			recipe.AddIngredient(ItemID.SpectreStaff, 1);
		
			recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 7;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(120)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}
}
