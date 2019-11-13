using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;


namespace SOTS.Items.Celestial
{
	public class StellarSerpentLauncher : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Serpent Launcher");
			Tooltip.SetDefault("Launches a blood seeking serpent");
		}
		public override void SetDefaults()
		{
            item.damage = 60;   
            item.ranged = true;   
            item.width = 44;    
            item.height = 18;  
            item.useTime = 27;  
            item.useAnimation = 27;
            item.useStyle = 5;    
            item.noMelee = true; 
            item.knockBack = 1;
            item.value = Item.sellPrice(0, 10, 25, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item61;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("StellarSnake"); 
            item.shootSpeed = 21.5f;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(4, 0);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
              Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("StellarSnake"), damage, knockBack, player.whoAmI);
              return false; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "StarShard", 15);
			recipe.AddIngredient(ItemID.PiranhaGun, 1);
			recipe.AddIngredient(null, "SnakeBow", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
