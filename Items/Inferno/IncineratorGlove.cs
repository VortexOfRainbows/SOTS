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
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 4));
		}
		public override void SafeSetDefaults()
		{
			Item.damage = 33;
			Item.magic = true;
			Item.width = 36;
			Item.height = 36;
			Item.value = Item.sellPrice(gold: 10);
			Item.rare = ItemRarityID.Pink;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = false;            
			Item.shoot = ModContent.ProjectileType<IncineratorGloveProjectile>(); 
			Item.shootSpeed = 5f;
			Item.knockBack = 5;
			Item.channel = true;
			Item.UseSound = SoundID.Item15; 
			Item.noUseGraphic = true;
			Item.noMelee = true;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Vector2 normal = new Vector2(speedX, speedY).SafeNormalize(Vector2.Zero);
			Projectile.NewProjectile(position, normal * 5, type, damage, knockBack, player.whoAmI, Item.useTime);
			Projectile.NewProjectile(position, normal * 48, ModContent.ProjectileType<PlasmaSphere>(), damage, knockBack, player.whoAmI, Item.useTime);
			return false;
        }
		public override int GetVoid(Player player)
		{
			return 6;
		}
	}
}