using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.SpecialDrops
{
	public class WyvernSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wyvern Sword");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{

			item.damage = 46;
			item.melee = true;
			item.width = 44;
			item.height = 44;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 250000;
			item.rare = 8;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("FeatherShot"); 
            item.shootSpeed = 14;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 28;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SteelBar", 12);
			recipe.AddIngredient(ItemID.GiantHarpyFeather, 2);
			recipe.AddIngredient(ItemID.SoulofFlight, 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}