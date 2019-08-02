using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.OreItems
{
	public class HallowedSickle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hallowed Sickle");
			Tooltip.SetDefault("Fires out a pink worm");
		}
		public override void SetDefaults()
		{
            item.damage = 48;  //gun damage
            item.melee = true;   //its a gun so set this to true
            item.width = 46;     //gun image width
            item.height = 46;   //gun image  height
            item.useTime = 64;  //how fast 
            item.useAnimation = 32;
            item.useStyle = 1;    
            item.knockBack = 1;
            item.value = 90000;
            item.rare = 6;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
			item.shoot = mod.ProjectileType("DNA1Leader"); 
            item.shootSpeed = 3;

			
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 1;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(45)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "WormWoodStaff", 1);
			recipe.AddIngredient(null, "SoulSingularity", 24);
			recipe.AddIngredient(ItemID.HallowedBar, 18);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
	}
}
