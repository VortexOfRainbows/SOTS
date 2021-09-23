using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Pyramid
{
	public class SnakeBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snake Bow");
			Tooltip.SetDefault("Launches snakes that latch on to enemies");
		}
		public override void SetDefaults()
		{
			item.damage = 19;
			item.ranged = true;
			item.width = 30;
			item.height = 48;
			item.useTime = 22;
			item.useAnimation = 22;
			item.useStyle = 5;
			item.knockBack = 2.5f;
			item.value = Item.sellPrice(0, 0, 75, 0);
			item.rare = 6;
			item.noMelee = true;
			item.UseSound = SoundID.Item5;
			item.autoReuse = false;            
			item.shoot = 1; 
            item.shootSpeed = 20;
			item.useAmmo = AmmoID.Arrow;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Snakeskin", 18);
			recipe.AddIngredient(ItemID.Leather, 4);
			recipe.AddIngredient(ItemID.WoodenBow, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("Snake"), damage, knockBack, player.whoAmI);
			return false; 
		}
	}
}