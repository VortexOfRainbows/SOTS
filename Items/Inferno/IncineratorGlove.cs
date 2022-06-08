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
			Item.DamageType = DamageClass.Magic;
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
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Vector2 normal = velocity.SafeNormalize(Vector2.Zero);
			Projectile.NewProjectile(source, position, normal * 5, type, damage, knockback, player.whoAmI, Item.useTime);
			Projectile.NewProjectile(source, position, normal * 48, ModContent.ProjectileType<PlasmaSphere>(), damage, knockback, player.whoAmI, Item.useTime);
			return false;
        }
		public override int GetVoid(Player player)
		{
			return 6;
		}
	}
}