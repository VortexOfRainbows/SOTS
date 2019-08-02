using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.IceStuff
{
	public class Waverang : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Waverang");
			Tooltip.SetDefault("It sure is cold");
		}
		public override void SetDefaults()
		{
            item.damage = 66;  
            item.thrown = true;  
            item.width = 44;   
            item.height = 44;  
            item.useTime = 45;
            item.useAnimation = 45;
            item.useStyle = 1;    
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 120000;
            item.rare = 6;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot =  mod.ProjectileType("Waverang"); 
            item.shootSpeed = 18;
			item.noUseGraphic = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "AbsoluteBar", 16);
			recipe.AddIngredient(null, "Crosspipe", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
