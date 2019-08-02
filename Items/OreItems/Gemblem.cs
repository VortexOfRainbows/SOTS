using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.OreItems
{
	public class Gemblem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gemblem");
			Tooltip.SetDefault("Increases thrown damage by 10%, thrown crit by 3%, and provides an extra minion");
		}
		public override void SetDefaults()
		{
      
            item.width = 30;     
            item.height = 30;   
            item.rare = 4;
			item.value = 50000;
			item.accessory = true;
			item.defense = 1;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Topaz, 1);
			recipe.AddIngredient(ItemID.Amethyst, 1);
			recipe.AddIngredient(ItemID.Sapphire, 1);
			recipe.AddIngredient(ItemID.Emerald, 1);
			recipe.AddIngredient(ItemID.Ruby, 1);
			recipe.AddIngredient(ItemID.Diamond, 1);
			recipe.AddIngredient(ItemID.Amber, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
			player.thrownDamage += 0.10f ;
			player.thrownCrit += 3 ;
			player.maxMinions += 1;
			
			
		}
	}
}
