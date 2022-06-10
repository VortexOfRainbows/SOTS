using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Slime
{
	public class WormWoodHelix : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Goopwood Helix");
			Tooltip.SetDefault("Converts musket balls into helix shots");
		}
		public override void SetDefaults()
		{
            Item.damage = 20;  
            Item.DamageType = DamageClass.Ranged;    
            Item.width = 60;  
            Item.height = 28;   
            Item.useTime = 24;  
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true; 
            Item.knockBack = 3;
            Item.value = Item.sellPrice(0, 1, 80, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = 10; 
            Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Bullet;
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2f, 0.5f);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			if(type == ProjectileID.Bullet)
			{
				velocity *= 0.65f;
				Vector2 projVelocity1 = velocity.RotatedBy(MathHelper.ToRadians(45));
				Vector2 projVelocity2 = velocity.RotatedBy(MathHelper.ToRadians(315));
				Projectile.NewProjectile(source, position.X + velocity.X * 4, position.Y + velocity.Y * 4, projVelocity1.X * 0.325f, projVelocity1.Y * 0.325f, ModContent.ProjectileType<Projectiles.Slime.Fusion1>(), damage, knockback, Main.myPlayer);
				Projectile.NewProjectile(source, position.X + velocity.Y * 4, position.Y + velocity.Y * 4, projVelocity2.X * 0.325f, projVelocity2.Y * 0.325f, ModContent.ProjectileType<Projectiles.Slime.Fusion2>(), damage, knockback, Main.myPlayer);
			}
			else
			{
				Vector2 projVelocity1 = velocity.RotatedBy(MathHelper.ToRadians(-2f));
				Vector2 projVelocity2 = velocity.RotatedBy(MathHelper.ToRadians(2f));
				Projectile.NewProjectile(source, position.X + velocity.X, position.Y, projVelocity1.X, projVelocity1.Y, type, damage, knockback, Main.myPlayer);
				Projectile.NewProjectile(source, position.X + velocity.Y, position.Y, projVelocity2.X, projVelocity2.Y, type, damage, knockback, Main.myPlayer);
			}
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<CorrosiveGel>(24).AddIngredient<Wormwood>(24).AddTile(TileID.Anvils).Register();
		}
	}
}
