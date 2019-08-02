using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.IceStuff
{
	public class BlizzardsBliss : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blizzard's Bliss");
			Tooltip.SetDefault("Fires a rapid stream of icicles");
		}
		public override void SetDefaults()
		{
            item.damage = 45;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 54;     //gun image width
            item.height = 34;   //gun image  height
            item.useTime = 3;  //how fast 
            item.useAnimation = 3;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 120000;
            item.rare = 9;
            item.UseSound = SoundID.Item30;
            item.autoReuse = true;
            item.shoot = 337; 
            item.shootSpeed = 14.5f;
			item.mana = 3;
		

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "AbsoluteBar", 16);
			recipe.AddIngredient(ItemID.BlizzardStaff, 1);
			recipe.AddIngredient(ItemID.Razorpine, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 2;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(6)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}
}
