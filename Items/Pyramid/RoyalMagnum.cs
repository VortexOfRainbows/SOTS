using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Pyramid;

namespace SOTS.Items.Pyramid
{
	public class RoyalMagnum : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Royal Magnum");
            Tooltip.SetDefault("Releases golden light upon hitting the ground or an enemy");
            this.SetResearchCost(1);
        }
		public override void SetDefaults()
		{
            Item.damage = 14;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 42; 
            Item.height = 22;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;
            Item.knockBack = 2.5f;
            Item.value = Item.sellPrice(0, 1, 50, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = false;
            Item.shoot = ProjectileID.Bullet; 
            Item.shootSpeed = 3.25f;
			Item.useAmmo = AmmoID.Bullet;
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-0.25f, 0);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<SandBullet>();
		}
	}
}
