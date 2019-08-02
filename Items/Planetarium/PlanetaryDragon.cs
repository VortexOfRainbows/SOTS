using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Planetarium
{
	public class PlanetaryDragon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Planetary Dragon");
			Tooltip.SetDefault("Fires a barrage of bullets\nPropels you backwards when fired\n10% of shots fired will be planetary flames\nPlanetary flames will home and hurt enemies twice");
		}
		public override void SetDefaults()
		{
            item.damage = 6;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 56;     //gun image width
            item.height = 26;   //gun image  height
            item.useTime = 3;  //how fast 
            item.useAnimation = 9;
            item.useStyle = 5;    
            item.noMelee = false; //so the item's animation doesn't do damage
            item.knockBack = 4;
            item.value = 150000;
            item.rare = 9;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = 10; 
            item.shootSpeed = 3;
			item.reuseDelay = 18;
			item.useAmmo = AmmoID.Bullet;

		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
			  player.velocity.X -= (speedX * 0.1f);
			  player.velocity.Y -= (speedY * 0.3f);
			  if(Main.rand.Next(10) != 0)
			  {
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			  }
			  else
			  {
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("PlanetaryFlame"), damage, knockBack, player.whoAmI);
			  }
			  if(Main.rand.Next(10) != 0)
			  {
				Projectile.NewProjectile(position.X + (speedY * 4), position.Y - (speedX * 4), speedX, speedY, type, damage, knockBack, player.whoAmI);
			  }
			  else
			  {
				Projectile.NewProjectile(position.X + (speedY * 4), position.Y - (speedX * 4), speedX, speedY, mod.ProjectileType("PlanetaryFlame"), damage, knockBack, player.whoAmI);
			  }
			  if(Main.rand.Next(10) != 0)
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
			recipe.AddIngredient(null, "PlanetaryCore", 1);
			recipe.AddIngredient(null, "EmptyPlanetariumOrb", 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
