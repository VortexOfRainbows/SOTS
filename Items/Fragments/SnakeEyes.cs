using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;


namespace SOTS.Items.Fragments
{
	public class SnakeEyes : ModItem
	{ 
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snake Eyes");
			Tooltip.SetDefault("Increases crit chance by 8%");
		}
		public override void SetDefaults()
		{
            
            item.width = 32;     
            item.height = 38;     
            item.value = Item.sellPrice(0, 0, 40, 0);
            item.rare = 5;
			item.accessory = true;
			
		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeCrit += 8;
			player.rangedCrit += 8;
			player.magicCrit += 8;
			player.thrownCrit += 8;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Snakeskin", 10);
			recipe.AddIngredient(null, "FragmentOfEarth", 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
