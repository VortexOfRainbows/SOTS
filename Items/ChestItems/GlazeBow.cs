using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.ChestItems
{
	public class GlazeBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glaze Bow");
			Tooltip.SetDefault("Coalesces ice mist to power up your arrows\nWhen briefly charged, arrows will travel faster and hit with an icy explosion that deals 200% damage\nWhen fully charged, arrows bloom into wisps of ice, dealing 600% damage");
		}
		public override void SetDefaults()
		{
			item.damage = 10;
			item.ranged = true;
			item.width = 34;
			item.height = 64;
			item.useTime = 22;
			item.useAnimation = 22;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 4f;
			item.value = Item.sellPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.UseSound = null;
			item.autoReuse = false;
			item.channel = true;
			item.shoot = ModContent.ProjectileType<Projectiles.Permafrost.GlazeBow>();
			item.shootSpeed = 18f;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.useAmmo = AmmoID.Arrow;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, 0, 0, ModContent.ProjectileType<Projectiles.Permafrost.GlazeBow>(), damage, knockBack, player.whoAmI, item.useTime, type);
			return false;
        }
	}
}