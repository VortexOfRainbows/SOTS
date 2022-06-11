using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Projectiles;

namespace SOTS.Items.Pyramid
{
	public class ShiftingSands : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shifting Sands");
			Tooltip.SetDefault("Pushes back nearby enemies with a wave of sand");
		}
		public override void SetDefaults()
		{
            Item.damage = 16;
            Item.DamageType = DamageClass.Magic; 
            Item.width = 34;    
            Item.height = 34; 
            Item.useTime = 23; 
            Item.useAnimation = 23;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 6.5f;
            Item.value = Item.sellPrice(0, 1, 20, 0);
            Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item34;
            Item.noMelee = true; 
            Item.autoReuse = true;
            Item.shootSpeed = 2f; 
			Item.shoot = ModContent.ProjectileType<SandPuff>();
			Item.mana = 16;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float numberProjectiles = 12; 
            float rotation = MathHelper.ToRadians(30);
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedBy(rotation * i);
                Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
            }
            return false;
		}
	}
}
