using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GhostTown
{
	public class AncientSteelLongbow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Steel Longbow");
			Tooltip.SetDefault("Fired arrows explode into piercing shrapnel upon hitting enemies");
		}
		public override void SetDefaults()
		{
			item.damage = 14;
			item.ranged = true;
			item.width = 30;
			item.height = 64;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 4f;
			item.value = Item.sellPrice(0, 0, 80, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = null;
			item.autoReuse = false;
			item.channel = true;
			item.shoot = ModContent.ProjectileType<Projectiles.Evil.AncientSteelLongbow>();
			item.shootSpeed = 8f;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.useAmmo = AmmoID.Arrow;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, 0, 0, ModContent.ProjectileType<Projectiles.Evil.AncientSteelLongbow>(), damage, knockBack, player.whoAmI, (int)(item.useTime / SOTSPlayer.ModPlayer(player).attackSpeedMod), type);
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AncientSteelBar>(), 14);
			recipe.AddRecipeGroup(RecipeGroupID.Wood, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}