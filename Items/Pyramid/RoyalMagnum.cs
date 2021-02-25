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
            item.damage = 14;
            item.ranged = true;
            item.width = 42; 
            item.height = 22;
            item.useTime = 14;
            item.useAnimation = 14;
            item.useStyle = 5;    
            item.noMelee = true;
            item.knockBack = 2.5f;
            item.value = Item.sellPrice(0, 1, 50, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item11;
            item.autoReuse = false;
            item.shoot = 14; 
            item.shootSpeed = 3.25f;
			item.useAmmo = AmmoID.Bullet;
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
