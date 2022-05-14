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
			Item.damage = 19;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 30;
			Item.height = 48;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 2.5f;
			Item.value = Item.sellPrice(0, 0, 75, 0);
			Item.rare = 6;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = false;            
			Item.shoot = 1; 
            Item.shootSpeed = 20;
			Item.useAmmo = AmmoID.Arrow;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
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