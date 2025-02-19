using Microsoft.Xna.Framework;
using SOTS.Projectiles.AbandonedVillage;
using SOTS.Void;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.AbandonedVillage
{
	public class StreetCleaner : VoidItem 
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.damage = 24;
			Item.width = 70;
			Item.height = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 30;
			Item.useTime = 3;
			Item.reuseDelay = 15;
			Item.shootSpeed = 13f;
			Item.knockBack = 2.5f;
			Item.UseSound = SoundID.Item34;
			Item.shoot = ModContent.ProjectileType<FriendlyBridgeBurnerFlame>();
			Item.value = Item.sellPrice(gold: 10);
			Item.rare = ItemRarityID.LightPurple;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.DamageType = DamageClass.Ranged;
		}
        public override Vector2? HoldoutOffset() => new Vector2(-4, -4);
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Vector2 offset = new Vector2(HoldoutOffset().Value.X, HoldoutOffset().Value.Y * player.direction);
			Projectile.NewProjectileDirect(source, position + offset.RotatedBy(velocity.ToRotation()) + velocity.SNormalize() * 64, velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-8, 8))), type, damage, knockback, player.whoAmI, 0, 0);
			return false;
		}
        public override int GetVoid(Player player)
        {
            return 10;
        }
    }
}
		