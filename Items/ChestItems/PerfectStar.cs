using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.ChestItems
{
	public class PerfectStar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Perfect Star");
			Tooltip.SetDefault("'The perfect weapon'");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			Item.damage = 24;
			Item.DamageType = DamageClass.Magic;
			Item.width = 68;
			Item.height = 30;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 4f;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = null;
			Item.autoReuse = false;
			Item.channel = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Earth.PerfectStar>();
			Item.shootSpeed = 18f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.mana = 12;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position.X, position.Y, 0, 0, ModContent.ProjectileType<Projectiles.Earth.PerfectStar>(), damage, knockback, player.whoAmI, (int)(Item.useTime / SOTSPlayer.ModPlayer(player).attackSpeedMod));
			return false;
        }
	}
}