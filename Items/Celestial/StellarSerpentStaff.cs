using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Celestial
{
	public class StellarSerpentStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Serpent Staff");
			Tooltip.SetDefault("Launch a sworm of unstable energy serpents");
		}
		public override void SetDefaults()
		{
            item.damage = 40;  
            item.magic = true;  
            item.width = 46;    
            item.height = 46;   
            item.useTime = 40;  
            item.useAnimation = 40;
            item.useStyle = 1;    
			item.mana = 27;
            item.knockBack = 5.5f;
			item.value = Item.sellPrice(0, 9, 50, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item119;
            item.autoReuse = true;
			item.shoot = mod.ProjectileType("UnstableSerpent"); 
            item.shootSpeed = 21.5f;
			item.noMelee = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 3;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians((i * 7.5f) + 37.5f));
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI, 1);
              }
              return false; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "StarShard", 15);
			recipe.AddIngredient(null, "WormWoodStaff", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
