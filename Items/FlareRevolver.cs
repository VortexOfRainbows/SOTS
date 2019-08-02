using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;


namespace SOTS.Items
{
	public class FlareRevolver : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flare Revolver");
		}
		public override void SetDefaults()
		{
            item.damage = 9;  //gun damage
            item.width = 38;     //gun image width
            item.height = 28;   //gun image  height
            item.useTime = 13;  //how fast 
            item.useAnimation = 13;
            item.useStyle = 5;    
            item.noMelee = false; //so the item's animation doesn't do damage
            item.knockBack = 2;
            item.value = 72500;
            item.rare = 4;
            item.UseSound = SoundID.Item98;
            item.autoReuse = true;
            item.shoot = 10; 
            item.shootSpeed = 11;
			item.useAmmo = AmmoID.Flare;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FlareGun, 4);
			recipe.AddIngredient(null, "SteelBar", 14);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 2;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(12)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}
}
