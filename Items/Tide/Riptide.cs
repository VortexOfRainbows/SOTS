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
			item.damage = 25;
			item.melee = true;
			item.width = 58;
			item.height = 58;
			item.useTime = 29;
			item.useAnimation = 29;
			item.useStyle = 5;
			item.knockBack = 7f;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = 3;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("Riptide");
			item.shootSpeed = 3.0f;
			item.noUseGraphic = true;
			item.noMelee = true;
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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Trident, 1);
			recipe.AddIngredient(ModContent.ItemType<Fragments.FragmentOfTide>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Spear, 1);
			recipe.AddIngredient(ItemID.GoldBar, 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(ItemID.Trident, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Spear, 1);
			recipe.AddIngredient(ItemID.PlatinumBar, 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(ItemID.Trident, 1);
			recipe.AddRecipe();
		}
		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[item.shoot] < 1;
		}
	}
}