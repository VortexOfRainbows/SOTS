using Microsoft.Xna.Framework;
using Terraria;
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
            Item.rare = 4;
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
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			if(type == ProjectileID.Bullet)
			{
				speedX *= 0.65f;
				speedY *= 0.65f;
				Vector2 projVelocity1 = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(45));
				Vector2 projVelocity2 = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(315));
				Projectile.NewProjectile(position.X + speedX * 4, position.Y + speedY * 4, projVelocity1.X * 0.325f, projVelocity1.Y * 0.325f, mod.ProjectileType("Fusion1"), damage, knockBack, Main.myPlayer);
				Projectile.NewProjectile(position.X + speedX * 4, position.Y + speedY * 4, projVelocity2.X * 0.325f, projVelocity2.Y * 0.325f, mod.ProjectileType("Fusion2"), damage, knockBack, Main.myPlayer);
			}
			else
			{
				Vector2 projVelocity1 = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(-2f));
				Vector2 projVelocity2 = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(2f));
				Projectile.NewProjectile(position.X + speedX, position.Y, projVelocity1.X, projVelocity1.Y, type, damage, knockBack, Main.myPlayer);
				Projectile.NewProjectile(position.X + speedX, position.Y, projVelocity2.X, projVelocity2.Y, type, damage, knockBack, Main.myPlayer);
			}
			return false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CorrosiveGel>(), 24);
			recipe.AddIngredient(null, "Wormwood", 24);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
