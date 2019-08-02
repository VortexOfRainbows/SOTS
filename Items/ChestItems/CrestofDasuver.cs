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
			Tooltip.SetDefault("All is more\nGrants 1% bonus crit chance for every full inventory slot\nGrants 1 defense for each empty inventory slot");
		}
		public override void SetDefaults()
		{
            
            item.width = 34;     
            item.height = 32;     
            item.value = 100000;
            item.rare = 5;
			item.accessory = true;
			
		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			item.defense = 0;
			critbonus = 0;
			for(int i = 0; i < 50; i++)
			{
			Item inventoryItem = player.inventory[i];
				if(inventoryItem.type != 0)
				{
					critbonus++;
					
					
				}
				if(inventoryItem.type == 0)
				{
					item.defense++;
				}
			}
			player.meleeCrit += critbonus;
			player.rangedCrit += critbonus;
			player.magicCrit += critbonus;
			player.thrownCrit += critbonus;
			
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"ShieldofDesecar", 1);
			recipe.AddIngredient(null,"ShieldofStekpla", 1);
			recipe.AddIngredient(null,"SteelBar", 12);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
