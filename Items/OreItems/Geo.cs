using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.OreItems
{
	public class Geo : ModItem
	{ int accuracy = 28;
              int numberProjectiles = 3;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Geo");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
            item.damage = 79;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 30;     //gun image width
            item.height = 44;   //gun image  height
            item.useTime = 11;  //how fast 
            item.useAnimation = 11;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 500000;
            item.rare = 9;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("RainbeamStaffProj"); 
            item.shootSpeed = 12;
		
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"ManaSapper", 1);
			recipe.AddIngredient(null,"HeatStroke", 1);
			recipe.AddIngredient(null,"Zapper", 1);
			recipe.AddIngredient(null,"Splatterator", 1);
			recipe.AddIngredient(null,"BlunderBow", 1);
			recipe.AddIngredient(null,"ReboundBow", 1);
			recipe.AddIngredient(null,"TrueHallowedRepeater", 1);
			recipe.AddIngredient(ItemID.CobaltRepeater, 1);
			recipe.AddIngredient(ItemID.PalladiumRepeater, 1);
			recipe.AddIngredient(ItemID.MythrilRepeater, 1);
			recipe.AddIngredient(ItemID.OrichalcumRepeater, 1);
			recipe.AddIngredient(ItemID.AdamantiteRepeater, 1);
			recipe.AddIngredient(ItemID.TitaniumRepeater, 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {	
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(12)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}
}
