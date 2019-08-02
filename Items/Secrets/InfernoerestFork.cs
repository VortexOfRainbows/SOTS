using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Secrets
{
	public class InfernoerestFork : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infernoerest Fork");
		}
		public override void SetDefaults()
		{
            item.damage = 255;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 142;     //gun image width
            item.height = 142;   //gun image  height
            item.useTime = 10;  //how fast 
            item.useAnimation = 87;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 8;
            item.value = 8100000;
            item.rare = 10;
            item.UseSound = SoundID.Item45;
            item.autoReuse = true;
            item.shoot = 295; 
            item.shootSpeed = 8;
			item.mana = 18;			
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun


		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
			
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			 
				Projectile.NewProjectile(position.X + (speedY * 4), position.Y - (speedX * 4), speedX, speedY, type, damage, knockBack, player.whoAmI);
			
				Projectile.NewProjectile(position.X - (speedY * 4), position.Y + (speedX * 4), speedX, speedY, type, damage, knockBack, player.whoAmI);
				
				Projectile.NewProjectile(position.X + (speedY * 8), position.Y - (speedX * 8), speedX, speedY, type, damage, knockBack, player.whoAmI);
			
				Projectile.NewProjectile(position.X - (speedY * 8), position.Y + (speedX * 8), speedX, speedY, type, damage, knockBack, player.whoAmI);
				
				Projectile.NewProjectile(position.X + (speedY * 12), position.Y - (speedX * 12), speedX, speedY, type, damage, knockBack, player.whoAmI);
			
				Projectile.NewProjectile(position.X - (speedY * 12), position.Y + (speedX * 12), speedX, speedY, type, damage, knockBack, player.whoAmI);
				
				Projectile.NewProjectile(position.X + (speedY * 16), position.Y - (speedX * 16), speedX, speedY, type, damage, knockBack, player.whoAmI);
			
				Projectile.NewProjectile(position.X - (speedY * 16), position.Y + (speedX * 16), speedX, speedY, type, damage, knockBack, player.whoAmI);
			 
              return false; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "InfernoestFork", 3);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
