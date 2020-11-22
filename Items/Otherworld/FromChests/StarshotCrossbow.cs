using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld.FromChests
{
	public class StarshotCrossbow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starshot Crossbow");
			Tooltip.SetDefault("Fire a star that scatters arrows in every direction");
		}
		public override void SetDefaults()
		{
			item.damage = 34;
			item.ranged = true;
			item.width = 46;
			item.height = 20;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 5f;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.UseSound = SoundID.Item5;
			item.autoReuse = false;
			item.channel = true;
			item.shoot = mod.ProjectileType("Starshot");
			item.shootSpeed = 24f;
			item.useAmmo = AmmoID.Arrow;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.crit = 4;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, 0, 0, mod.ProjectileType("StarshotCrossbow"), 0, 0, player.whoAmI, 0, type);
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("Starshot"), damage, knockBack, player.whoAmI, 0, type);
			return false;
        }
        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "StarlightAlloy", 12);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}