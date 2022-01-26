using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Void;
using System;
using SOTS.Projectiles.Inferno;

namespace SOTS.Items.Inferno
{
	public class IncineratorGlove : VoidItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Incinerator Glove");
			Tooltip.SetDefault("Charge up a short-ranged ball of hot plasma\nGains 40% additional damage each charge\nMaxes out at 80 charges\nOnly does half damage prior to firing");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 4));
		}
		public override void SafeSetDefaults()
		{
			item.damage = 33;
			item.magic = true;
			item.width = 36;
			item.height = 36;
			item.value = Item.sellPrice(gold: 10);
			item.rare = ItemRarityID.Pink;
			item.useTime = 40;
			item.useAnimation = 40;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.autoReuse = false;            
			item.shoot = ModContent.ProjectileType<IncineratorGloveProjectile>(); 
			item.shootSpeed = 5f;
			item.knockBack = 5;
			item.channel = true;
			item.UseSound = SoundID.Item15; 
			item.noUseGraphic = true;
			item.noMelee = true;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Vector2 normal = new Vector2(speedX, speedY).SafeNormalize(Vector2.Zero);
			Projectile.NewProjectile(position, normal * 5, type, damage, knockBack, player.whoAmI, item.useTime);
			Projectile.NewProjectile(position, normal * 48, ModContent.ProjectileType<PlasmaSphere>(), damage, knockBack, player.whoAmI, item.useTime);
			return false;
        }
		public override void GetVoid(Player player)
		{
			voidMana = 6;
		}
	}
}