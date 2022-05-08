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
			Item.damage = 10;
			Item.ranged = true;
			Item.width = 34;
			Item.height = 64;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 4f;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = null;
			Item.autoReuse = false;
			Item.channel = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Permafrost.GlazeBow>();
			Item.shootSpeed = 18f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.useAmmo = AmmoID.Arrow;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, 0, 0, ModContent.ProjectileType<Projectiles.Permafrost.GlazeBow>(), damage, knockBack, player.whoAmI, (int)(Item.useTime / SOTSPlayer.ModPlayer(player).attackSpeedMod), type);
			return false;
        }
	}
}