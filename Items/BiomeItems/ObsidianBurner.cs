using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems
{
	public class ObsidianBurner : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Obsidian Spacer");
		}
		public override void SetDefaults()
		{
            item.damage = 23;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 48;     //gun image width
            item.height = 34;   //gun image  height
            item.useTime = 12;  //how fast 
            item.useAnimation =12;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 175000;
            item.rare = 4;
            item.UseSound = SoundID.Item9;
            item.autoReuse = true;
            item.shoot = 20; 
            item.shootSpeed = 11;
			item.mana = 0;
		

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MeteorHelmet, 1);
			recipe.AddIngredient(ItemID.MeteorSuit, 1);
			recipe.AddIngredient(ItemID.MeteorLeggings, 1);
			recipe.AddIngredient(ItemID.SpaceGun, 1);
			recipe.AddIngredient(null, "ObsidianScale", 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 2;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(4)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}
}
