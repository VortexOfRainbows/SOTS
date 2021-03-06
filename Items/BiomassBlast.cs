using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items
{
	public class BiomassBlast : ModItem
	{	int counter = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Biomass Blast");
			Tooltip.SetDefault("Launches an acorn that rapidly accelerates its growth upon hitting an enemy or tile");
		}
		public override void SetDefaults()
		{
            item.damage = 13; 
            item.magic = true; 
            item.width = 28;   
            item.height = 30;   
            item.useTime = 39;   
            item.useAnimation = 39;
            item.useStyle = 5;    
            item.noMelee = true;  
            item.knockBack = 3.25f;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 5;
            item.UseSound = SoundID.Item8;
            item.shoot = mod.ProjectileType("AcornOfJustice"); 
            item.shootSpeed = 15.5f;
			item.mana = 15;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Scatterseed", 1);
			recipe.AddIngredient(null, "Snakeskin", 12);
			recipe.AddIngredient(null, "DissolvingNature", 1);
			recipe.AddIngredient(ItemID.Vilethorn, 1);
			recipe.AddIngredient(ItemID.StaffofRegrowth, 1);
			recipe.AddIngredient(ItemID.Acorn, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Scatterseed", 1);
			recipe.AddIngredient(null, "Snakeskin", 12);
			recipe.AddIngredient(null, "DissolvingNature", 1);
			recipe.AddIngredient(ItemID.CrimsonRod, 1);
			recipe.AddIngredient(ItemID.StaffofRegrowth, 1);
			recipe.AddIngredient(ItemID.Acorn, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
