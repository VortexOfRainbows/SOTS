using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Projectiles.Earth;

namespace SOTS.Items.Earth
{
	public class Geostorm : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Geostorm");
			Tooltip.SetDefault("Bombards your cursor with crystals");
		}
		public override void SafeSetDefaults()
		{
			Item.damage = 20;
			Item.magic = true;
			Item.width = 26;
			Item.height = 38;
			Item.useTime = 50;
			Item.useAnimation = 50;
			Item.useStyle = ItemUseStyleID.HoldingUp;
			Item.knockBack = 1.5f;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<GeostormCrystal>();
            Item.shootSpeed = 5.5f; //arbitrary
			Item.noMelee = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 cursorPos = Main.MouseWorld;
			Projectile.NewProjectile(cursorPos.X, cursorPos.Y, 0, 0, type, damage, knockBack, player.whoAmI, -1);
			return false;
		}
		public override int GetVoid(Player player)
		{
			return 15;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<VibrantBar>(), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}