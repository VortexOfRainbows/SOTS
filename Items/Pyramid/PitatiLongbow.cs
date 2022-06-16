using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Pyramid;
using Terraria.DataStructures;

namespace SOTS.Items.Pyramid
{
	public class PitatiLongbow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pitati Longbow");
			Tooltip.SetDefault("Fires an additional bouncing emerald bolt with each shot");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 18;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 22;
			Item.height = 62;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 2.5f;
			Item.value = Item.sellPrice(0, 1, 20, 0);
			Item.rare = ItemRarityID.Orange;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = false;            
			Item.shoot = ProjectileID.WoodenArrowFriendly; 
            Item.shootSpeed = 20;
			Item.useAmmo = AmmoID.Arrow;
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-0.5f, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int numberProjectiles = 1;
			for (int i = 0; i < numberProjectiles; i++)
			{
			    Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(2)); // This defines the projectiles random spread . 30 degree spread.
			    Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
			    Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X * 0.25f, perturbedSpeed.Y * 0.25f, ModContent.ProjectileType<EmeraldBolt>(), damage, knockback, player.whoAmI);
			}
			return false; 
		}
	}
}