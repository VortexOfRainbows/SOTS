using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.OreItems
{
	public class SpikedGemblem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gemblem of Cthulhu");
			Tooltip.SetDefault("Increases thrown damage by 15%, thrown crit by 5%, and provides an extra minion\nGrants the ability to dash");
		}
		public override void SetDefaults()
		{
      
            item.width = 30;     
            item.height = 30; 
            item.rare = 5;
			item.value = 100000;
			item.accessory = true;
			item.defense = 3;
			item.expert = true;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Gemblem", 1);
		    recipe.AddIngredient(ItemID.EoCShield, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
			player.thrownDamage += 0.15f ;
			player.thrownCrit += 5 ;
			player.maxMinions += 1;
			player.dash = 2;
			
			
		}
	}
}
