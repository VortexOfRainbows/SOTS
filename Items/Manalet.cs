using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class Manalet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mana Brace");
			Tooltip.SetDefault("Reduces mana usage by 20% and magic damage by 8%");
		}
		public override void SetDefaults()
		{
      
            item.width = 28;     
            item.height = 28;   
            item.value = 10000;
            item.rare = 2;
	
			item.accessory = true;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BluePowerChamber", 1);
			recipe.AddIngredient(ItemID.ManaFlower, 1);
			recipe.AddIngredient(ItemID.JungleSpores, 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			

			player.magicDamage -= 0.8f ;
			player.manaCost -= 0.20f;
		}
	}
}
