using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SOTS.Items
{
	public class GildedDragon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Dragon");
			Tooltip.SetDefault("Fires a barrage of bullets\nPropels you backwards when fired\n20% of shots fired will be planetary flames\nPlanetary flames will home and hurt enemies twice");
		}
		public override void SetDefaults()
		{
            item.damage = 20;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 56;     //gun image width
            item.height = 26;   //gun image  height
            item.useTime = 2;  //how fast 
            item.useAnimation = 9;
            item.useStyle = 5;    
            item.noMelee = false; //so the item's animation doesn't do damage
            item.knockBack = 4;
            item.value = 225000;
            item.rare = 7;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = 10; 
            item.shootSpeed = 4;
			item.reuseDelay = 12;
			item.useAmmo = AmmoID.Bullet;

		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
			  player.velocity.X -= (speedX * 0.05f);
			  player.velocity.Y -= (speedY * 0.2f);
			  if(Main.rand.Next(5) != 0)
			  {
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			  }
			  else
			  {
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("PlanetaryFlame"), damage, knockBack, player.whoAmI);
			  }
			  if(Main.rand.Next(5) != 0)
			  {
				Projectile.NewProjectile(position.X + (speedY * 4), position.Y - (speedX * 4), speedX, speedY, type, damage, knockBack, player.whoAmI);
			  }
			  else
			  {
				Projectile.NewProjectile(position.X + (speedY * 4), position.Y - (speedX * 4), speedX, speedY, mod.ProjectileType("PlanetaryFlame"), damage, knockBack, player.whoAmI);
			  }
			  if(Main.rand.Next(5) != 0)
			  {
				Projectile.NewProjectile(position.X - (speedY * 4), position.Y + (speedX * 4), speedX, speedY, type, damage, knockBack, player.whoAmI);
			  }
			  else
			  {
				Projectile.NewProjectile(position.X - (speedY * 4), position.Y + (speedX * 4), speedX, speedY, mod.ProjectileType("PlanetaryFlame"), damage, knockBack, player.whoAmI);
			  }
              return false; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PlanetaryDragon", 1);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 24);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
