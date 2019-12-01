using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GelGear
{
	public class WormWoodHelix : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Worm Wood Helix");
			Tooltip.SetDefault("Launches fusion shots");
		}
		public override void SetDefaults()
		{
            item.damage = 22;  
            item.ranged = true;    
            item.width = 40;  
            item.height = 24;   
            item.useTime = 24;  
            item.useAnimation = 24;
            item.useStyle = 5;    
            item.noMelee = true; 
            item.knockBack = 3;
            item.value = Item.sellPrice(0, 1, 80, 0);
            item.rare = 4;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = 10; 
            item.shootSpeed = 8f;
			item.reuseDelay = 8;
			item.useAmmo = AmmoID.Bullet;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
					Vector2 projVelocity1 = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(45));
					Vector2 projVelocity2 = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(315));
					Projectile.NewProjectile(position.X, position.Y, projVelocity1.X * 0.35f, projVelocity1.Y * 0.35f, mod.ProjectileType("Fusion1"), damage, knockBack, Main.myPlayer);
					Projectile.NewProjectile(position.X, position.Y, projVelocity2.X * 0.35f, projVelocity2.Y * 0.35f, mod.ProjectileType("Fusion2"), damage, knockBack, Main.myPlayer);
				
				return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "WormWoodCore", 1);
			recipe.AddIngredient(null, "SlimeyFeather", 2);
			recipe.AddIngredient(null, "GelBar", 12);
			recipe.AddIngredient(ItemID.Wood, 12);
			recipe.AddIngredient(ItemID.PinkGel, 24);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
