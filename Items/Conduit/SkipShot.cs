using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using SOTS.Void;
using Terraria.ModLoader;
using Terraria.DataStructures;
using SOTS.Projectiles.Pyramid;
using System;
using static SOTS.ItemHelpers;

namespace SOTS.Items.Conduit
{
	public class SkipShot : VoidItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.damage = 20;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 34;
			Item.height = 94;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 1.5f;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ModContent.RarityType<AnomalyRarity>();
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = false;            
			Item.shoot = ProjectileID.WoodenArrowFriendly; 
            Item.shootSpeed = 12.5f;
			Item.useAmmo = AmmoID.Arrow;
			Item.noMelee = true;
			Item.scale = 0.8f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<SkipSoul>(20).AddIngredient<SkipShard>(5).AddIngredient(ItemID.GoldBow).AddTile(TileID.Anvils).Register();
			CreateRecipe(1).AddIngredient<SkipSoul>(20).AddIngredient<SkipShard>(5).AddIngredient(ItemID.PlatinumBow).AddTile(TileID.Anvils).Register();
		}
		public override int GetVoid(Player player)
		{
			return 5;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, 0);
		}
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			if(type == ProjectileID.WoodenArrowFriendly)
				type = ModContent.ProjectileType<Projectiles.Anomaly.SkipArrow>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			for(int i = -1; i <= 1; i++)
            {
				float speedMod = 1f - 0.125f * Math.Abs(i);
				Vector2 velocity2 = velocity.RotatedBy(MathHelper.ToRadians(i * 4)) * speedMod;
				Projectile.NewProjectile(source, position, velocity2 + new Vector2(0, -1), type, damage, knockback, player.whoAmI);
			}
            return false;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * ((255 - Item.alpha) / 255f);
		}
	}
}