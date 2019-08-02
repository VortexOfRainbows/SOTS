using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems
{
	public class ObsidianShredder : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Obsidian Shredder");
		}
		public override void SetDefaults()
		{
            item.damage = 7;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 26;     //gun image width
            item.height = 30;   //gun image  height
            item.useTime = 5;  //how fast 
            item.useAnimation =5;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 100000;
            item.rare = 4;
            item.UseSound = SoundID.Item30;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("ObbyNeedle"); 
            item.shootSpeed = 2;
			item.mana = 1;
		

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "CrusherEmblem", 2);
			recipe.AddIngredient(ItemID.Diamond, 1);
			
			recipe.AddIngredient(null, "ObsidianScale", 8);
			recipe.AddIngredient(ItemID.MeteoriteBar, 9);
			recipe.AddIngredient(ItemID.Obsidian, 1);
			
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 1;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(7)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}
}
