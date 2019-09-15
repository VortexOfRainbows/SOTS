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


namespace SOTS.Items.ChestItems
{
	public class CrestofDasuver : ModItem
	{ 	int critbonus = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crest of Dasuver");
			Tooltip.SetDefault("Increases crit chance by 5%");
		}
		public override void SetDefaults()
		{
            
            item.width = 34;     
            item.height = 32;     
            item.value = 100000;
            item.rare = 5;
			item.accessory = true;
			item.defense = 4;
			
		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeCrit += 5;
			player.rangedCrit += 5;
			player.magicCrit += 5;
			player.thrownCrit += 5;
			
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"ShieldofDesecar", 1);
			recipe.AddIngredient(null,"ShieldofStekpla", 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
