using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpectreCog
{
	public class SpectreBomb : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spectre Destruction Catalyst");
			Tooltip.SetDefault("Can literally destroy an entire world use with caution.\nMight also destroy your computer");
		}
		public override void SetDefaults()
		{
            item.width = 44;     //gun image width
            item.height = 54;   //gun image  height
            item.useTime = 120;  //how fast 
            item.useAnimation = 120;
            item.useStyle = 1;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 0;
            item.rare = 10;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("ClockworkBomb"); 
            item.shootSpeed = 12;
			item.noUseGraphic = true;
			
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ReanimationMaterial", 8);
			
            recipe.AddIngredient(null, "SpectreManipulator", 1);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}
