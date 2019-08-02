using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class CrackTheSky : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Relic III : Crack The Sky");
			Tooltip.SetDefault("Send beams to heaven");
		}
		public override void SetDefaults()
		{
            item.damage = 92;  //gun damage
            item.thrown = true;   //its a gun so set this to true
            item.width =58;     //gun image width
            item.height = 58;   //gun image  height
            item.useTime = 67;  //how fast 
            item.useAnimation = 67;
            item.useStyle = 1;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 9;
            item.value = 1000000000;
            item.rare = 4;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("CrackTheSkyProj"); 
            item.shootSpeed = 12;
			item.noUseGraphic = true;
			item.expert = true;
			
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "TheHardCore", 6);
			recipe.AddIngredient(null, "CoreOfStatus", 2);
			recipe.AddIngredient(null,"AntimaterialMandible", 15);
			recipe.AddIngredient(null,"SpectreManipulator", 1);
			recipe.AddIngredient(ItemID.SkyFracture, 1);
			recipe.AddIngredient(ItemID.RazorbladeTyphoon, 1);
			recipe.AddIngredient(ItemID.FallenStar, 22);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 1;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(0)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}
}
