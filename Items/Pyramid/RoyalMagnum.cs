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
		}
		public override void SetDefaults()
		{
            Item.damage = 14;
            Item.ranged = true;
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
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = ModContent.ProjectileType<SandBullet>();
            return true; 
		}
	}
}
