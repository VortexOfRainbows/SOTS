using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Fragments
{
	public class SporeBombs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spore Bombs");
			Tooltip.SetDefault("Throw a cluster of explosive spore sacks\n'The toxic grenades look almost edible'");
		}
		public override void SetDefaults()
		{
			
			item.CloneDefaults(279);
			item.damage = 14;
			item.useTime = 28;
			item.useAnimation = 28;
			item.ranged = true;
			item.thrown = false;
			item.value = Item.sellPrice(0, 0, 80, 0);
			item.rare = 3;
			item.width = 28;
			item.height = 32;
			item.maxStack = 1;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("SporeBomb"); 
            item.shootSpeed = 16.25f;
			item.consumable = false;
			
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			int numberProjectiles = 2;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(6)); // This defines the projectiles random spread . 30 degree spread.
				perturbedSpeed *= 1f - (0.05f * i);
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				if(Main.rand.Next(13) < 4)
					Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X * 0.8f, perturbedSpeed.Y * 0.8f, type, damage, knockBack, player.whoAmI);
			}
			return false; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "FragmentOfNature", 6);
			recipe.AddIngredient(null, "BerryBombs", 1);
			recipe.AddIngredient(ItemID.JungleSpores, 12);
			recipe.AddIngredient(ItemID.Stinger, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}