using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Fragments
{
	public class BerryBombs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Berry Bombs");
			Tooltip.SetDefault("Throw a cluster of explosive berries");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(279);
			item.damage = 4;
			item.useTime = 23;
			item.useAnimation = 23;
			item.ranged = true;
			item.thrown = false;
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = 1;
			item.width = 32;
			item.height = 36;
			item.maxStack = 1;
			item.autoReuse = false;            
			item.shoot = mod.ProjectileType("BerryBomb"); 
            item.shootSpeed = 15.5f;
			item.consumable = false;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			int numberProjectiles = 2;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5)); // This defines the projectiles random spread . 30 degree spread.
				perturbedSpeed *= 1f - (0.05f * i);
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				if(Main.rand.Next(7) <= 1)
					Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X * 0.8f, perturbedSpeed.Y * 0.8f, type, damage, knockBack, player.whoAmI);
			}
			return false; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wood, 20);
			recipe.AddIngredient(null, "FragmentOfNature", 4);
			recipe.AddIngredient(ItemID.BlueBerries, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}