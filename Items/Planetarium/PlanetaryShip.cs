using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace SOTS.Items.Planetarium
{
	public class PlanetaryShip : ModItem
	{ 	float speedUp = 0;
		bool overheated = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Planetary Ship");
			Tooltip.SetDefault("Launch a ship that fires flames at your enemies\nThe ship will also fire out during immunity frames");
		}
		public override void SetDefaults()
		{
            item.damage = 27; 
            item.width = 32;   
            item.height = 32;  
            item.useTime = 75;  
            item.useAnimation = 75;
            item.useStyle = 1;    
            item.noMelee = true; 
            item.knockBack = 1;
            item.value = 150000;
            item.rare = 9;
			item.maxStack = 1;
            item.UseSound = SoundID.Item74;
            item.autoReuse = true;
            item.shoot =  mod.ProjectileType("PlanetaryShip"); 
            item.shootSpeed = 3.5f;
			item.noUseGraphic = true;
			
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PlanetaryCore", 1);
			recipe.AddIngredient(null, "EmptyPlanetariumOrb", 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
		
		public override void UpdateInventory(Player player)
		{
			if(player.immune == true && (Main.rand.Next(90)) == 0)
			  {
				  Projectile.NewProjectile(player.Center.X, player.Center.Y,  (Main.rand.Next(-5,6)),(Main.rand.Next(-5,6)), mod.ProjectileType("PlanetaryShip"), item.damage, 0, Main.myPlayer, 0.0f, 0);
			  }
		}
	}
}
