using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;


namespace SOTS.Items
{
	public class Crosspipe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crosspipe");
			Tooltip.SetDefault("Blow on dem enemies!");
		}
		public override void SetDefaults()
		{
            item.damage = 22;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 40;     //gun image width
            item.height = 40;   //gun image  height
            item.useTime = 18;  //how fast 
            item.useAnimation = 18;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 4;
            item.value = 0;
            item.rare = 3;
            item.UseSound = SoundID.Item17;
            item.autoReuse = true;
            item.shoot = 51; 
            item.shootSpeed = 33;
			item.reuseDelay = 18;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SteelBar", 4);
			recipe.AddIngredient(ItemID.Blowpipe, 1);
			recipe.AddIngredient(ItemID.Bone, 50);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
			  float numberProjectiles = 8; // This defines how many projectiles to shot
              float rotation = MathHelper.ToRadians(45);
              position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f; //this defines the distance of the projectiles form the player when the projectile spawns
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 6))) * .4f; // This defines the projectile roatation and speed. .4f == projectile speed
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}

}
