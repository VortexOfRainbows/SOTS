using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Nature;
using SOTS.Items.Fragments;
using Terraria.DataStructures;

namespace SOTS.Items.Nature
{
	public class BerryBombs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Berry Bombs");
			Tooltip.SetDefault("Throw a cluster of explosive berries");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.ThrowingKnife);
			Item.damage = 4;
			Item.useTime = 23;
			Item.useAnimation = 23;
			Item.DamageType = DamageClass.Ranged;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Blue;
			Item.width = 36;
			Item.height = 36;
			Item.maxStack = 1;
			Item.autoReuse = false;            
			Item.shoot = ModContent.ProjectileType<BerryBomb>(); 
            Item.shootSpeed = 15.5f;
			Item.consumable = false;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int numberProjectiles = 2;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(5)); // This defines the projectiles random spread . 30 degree spread.
				perturbedSpeed *= 1f - (0.05f * i);
				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
				if(Main.rand.Next(7) <= 1)
					Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X * 0.8f, perturbedSpeed.Y * 0.8f, type, damage, knockback, player.whoAmI);
			}
			return false; 
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddRecipeGroup(RecipeGroupID.Wood, 20).AddIngredient(ModContent.ItemType<FragmentOfNature>(), 4).AddIngredient(ItemID.BlueBerries, 1).AddTile(TileID.WorkBenches).Register();
		}
	}
}