using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.SpectreCog
{
	public class Spurocket : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spurocket");
			Tooltip.SetDefault("O");
		}
		public override void SetDefaults()
		{
			item.damage = 150;
			item.width = 66;
			item.height = 20;
			item.useTime = 26;
			item.useAnimation = 26;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 1000000;
			item.rare = 9;
			item.ranged = true;
			item.UseSound = SoundID.Item12;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("EctoRocket"); 
            item.shootSpeed = 4;
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ReanimationMaterial", 16);
			
            recipe.AddIngredient(null, "SpectreManipulator", 1);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
		
	}
}