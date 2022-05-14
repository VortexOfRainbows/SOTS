using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Tide
{
	public class Riptide : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Riptide");
			Tooltip.SetDefault("Right click while in water or rain to launch yourself forward, doing 120% damage\nImmunity to fall damage while held");
		}
		public override void SetDefaults()
		{
			Item.damage = 25;
			Item.DamageType = DamageClass.Melee;
			Item.width = 58;
			Item.height = 58;
			Item.useTime = 29;
			Item.useAnimation = 29;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 7f;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.shoot = mod.ProjectileType("Riptide");
			Item.shootSpeed = 3.0f;
			Item.noUseGraphic = true;
			Item.noMelee = true;
		}
        public override void HoldItem(Player player)
		{ 
			player.noFallDmg = true;
			base.HoldItem(player);
        }
        public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float mult = 1f;
			if(player.altFunctionUse == 2)
            {
				mult = 1.2f;
				speedX *= 0.55f;
				speedY *= 0.55f;
			}
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, (int)(damage * mult), (int)(knockBack * mult), player.whoAmI, player.altFunctionUse == 2 ? 1 : 0);
			return false;
		}
		public override float UseTimeMultiplier(Player player)
		{
			return player.altFunctionUse == 2 ? 0.45f : 1;
		}
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
			recipe.AddIngredient(ItemID.Trident, 1);
			recipe.AddIngredient(ModContent.ItemType<Fragments.FragmentOfTide>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new Recipe(mod);
			recipe.AddIngredient(ItemID.Spear, 1);
			recipe.AddIngredient(ItemID.GoldBar, 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(ItemID.Trident, 1);
			recipe.AddRecipe();

			recipe = new Recipe(mod);
			recipe.AddIngredient(ItemID.Spear, 1);
			recipe.AddIngredient(ItemID.PlatinumBar, 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(ItemID.Trident, 1);
			recipe.AddRecipe();
		}
		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}
	}
}