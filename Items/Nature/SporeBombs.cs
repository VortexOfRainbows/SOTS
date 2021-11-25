using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Nature;
using SOTS.Items.Fragments;

namespace SOTS.Items.Nature
{
	public class SporeBombs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spore Bombs");
			Tooltip.SetDefault("Throw a cluster of explosive spore sacks\nReleased spores deal 50% damage\n'The toxic grenades look almost edible'");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.ThrowingKnife);
			item.damage = 13;
			item.useTime = 29;
			item.useAnimation = 29;
			item.ranged = true;
			item.thrown = false;
			item.value = Item.sellPrice(0, 0, 80, 0);
			item.rare = ItemRarityID.Orange;
			item.width = 28;
			item.height = 32;
			item.maxStack = 1;
			item.autoReuse = true;            
			item.shoot = ModContent.ProjectileType<SporeBomb>(); 
            item.shootSpeed = 15.75f;
			item.consumable = false;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			int numberProjectiles = 2;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(6)); 
				perturbedSpeed *= 1f - (0.05f * i);
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				if(Main.rand.Next(14) < 4)
					Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X * 0.8f, perturbedSpeed.Y * 0.8f, type, damage, knockBack, player.whoAmI);
			}
			return false; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfNature>(), 6);
			recipe.AddIngredient(ModContent.ItemType<BerryBombs>(), 1);
			recipe.AddIngredient(ItemID.JungleSpores, 12);
			recipe.AddIngredient(ItemID.Stinger, 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}