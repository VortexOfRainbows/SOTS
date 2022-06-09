using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Nature;
using SOTS.Items.Fragments;
using Terraria.DataStructures;

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
			Item.CloneDefaults(ItemID.ThrowingKnife);
			Item.damage = 13;
			Item.useTime = 29;
			Item.useAnimation = 29;
			Item.DamageType = DamageClass.Ranged;
			// Item.thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Item.value = Item.sellPrice(0, 0, 80, 0);
			Item.rare = ItemRarityID.Orange;
			Item.width = 28;
			Item.height = 32;
			Item.maxStack = 1;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<SporeBomb>(); 
            Item.shootSpeed = 15.75f;
			Item.consumable = false;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int numberProjectiles = 2;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(6)); 
				perturbedSpeed *= 1f - (0.05f * i);
				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
				if(Main.rand.Next(14) < 4)
					Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X * 0.8f, perturbedSpeed.Y * 0.8f, type, damage, knockback, player.whoAmI);
			}
			return false; 
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<FragmentOfNature>(), 6).AddIngredient(ModContent.ItemType<BerryBombs>(), 1).AddIngredient(ItemID.JungleSpores, 12).AddIngredient(ItemID.Stinger, 12).AddTile(TileID.Anvils).Register();
		}
	}
}