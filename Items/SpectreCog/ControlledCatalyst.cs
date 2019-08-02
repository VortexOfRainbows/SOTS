using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.SpectreCog
{
	public class ControlledCatalyst: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Controlled Destruction Catalyst");
			Tooltip.SetDefault("Sprays a ray of destruction");
		}
		public override void SetDefaults()
		{

			item.width = 40;
			item.height = 28;
			item.useTime = 2;
			item.useAnimation = 2;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 1000000;
			item.rare = 5;
			item.UseSound = SoundID.Item12;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("ControlledChaos"); 
            item.shootSpeed = 26;
			item.mana = 2;
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
			  
              int numberProjectiles = 1;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(4)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);

              }
              return false; 
			  
	}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ReanimationMaterial", 12);
			
            recipe.AddIngredient(null, "SpectreBomb", 1);
			
            recipe.AddIngredient(null, "ManicMiner", 1);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}