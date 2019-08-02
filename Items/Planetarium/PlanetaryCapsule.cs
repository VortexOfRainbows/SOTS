using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class PlanetaryCapsule : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Planetary Capsule");
			Tooltip.SetDefault("Upon breaking, the capsule will explode into beams of magic planetary wizardry\nA beam will alway hit a target if they are within range of eachother");
		}
		public override void SetDefaults()
		{
            item.damage = 42; 
            item.thrown = true; 
            item.width = 38;   
            item.height = 38;  
            item.useTime = 22;  
            item.useAnimation = 22;
            item.useStyle = 1;    
            item.noMelee = true; 
            item.knockBack = 1;
            item.value = 450;
            item.rare = 9;
			item.consumable = true;
			item.maxStack = 999;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot =  mod.ProjectileType("PlanetaryCapsule"); 
            item.shootSpeed = 14.5f;
			item.noUseGraphic = true;
		}public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PlanetaryCore", 1);
			recipe.AddIngredient(null, "EmptyPlanetariumOrb", 3);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 333);
			recipe.AddRecipe();
		}
	}
}
