using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.ChestItems
{
	public class PerfectStar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Perfect Star");
			Tooltip.SetDefault("Can be charged up to 3 times, gaining damage and piercing with each charge\n'The perfect weapon'");
		}
		public override void SetDefaults()
		{
			item.damage = 14;
			item.magic = true;
			item.width = 68;
			item.height = 30;
			item.useTime = 30;
			item.useAnimation = 30;
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
			item.mana = 10;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, 0, 0, ModContent.ProjectileType<Projectiles.Permafrost.GlazeBow>(), damage, knockBack, player.whoAmI, (int)(item.useTime / SOTSPlayer.ModPlayer(player).attackSpeedMod), type);
			return false;
        }
	}
}