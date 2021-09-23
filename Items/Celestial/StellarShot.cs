using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;


namespace SOTS.Items.Celestial
{
	public class StellarShot : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Shot");
			Tooltip.SetDefault("Launches a combination of bullets and stars");
		}
		public override void SetDefaults()
		{
            item.damage = 44;   
            item.ranged = true;   
            item.width = 80;    
            item.height = 36;  
            item.useTime = 18;  
            item.useAnimation = 18;
            item.useStyle = 5;    
            item.noMelee = true; 
            item.knockBack = 1;
            item.value = Item.sellPrice(0, 10, 25, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item36;
            item.autoReuse = true;
            item.shoot = 10; 
            item.shootSpeed = 21.5f;
			item.useAmmo = AmmoID.Bullet;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(4, 0);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int numberProjectiles = 3 + Main.rand.Next(2);
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(4));
                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X * (0.825f + (.1f * i)), perturbedSpeed.Y * (0.825f + (.1f * i)), type, damage, knockBack, player.whoAmI);
            }
			Vector2 perturbedSpeed2 = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(32));
				  
			Vector2 perturbedSpeed3 = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(32)); 
			  
			if(numberProjectiles == 3)
            Projectile.NewProjectile(position.X, position.Y, perturbedSpeed2.X * 0.75f, perturbedSpeed2.Y * 0.75f, mod.ProjectileType("StellarStar"), damage, knockBack, player.whoAmI);
			  
            Projectile.NewProjectile(position.X, position.Y, perturbedSpeed3.X * 0.75f, perturbedSpeed3.Y * 0.75f, mod.ProjectileType("StellarStar"), damage, knockBack, player.whoAmI);
				  
            return false; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "StarShard", 15);
			recipe.AddIngredient(ItemID.TacticalShotgun, 1);
			recipe.AddIngredient(ItemID.OnyxBlaster, 1);
			recipe.AddIngredient(null, "GhoulBlaster", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
