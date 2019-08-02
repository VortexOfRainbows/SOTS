using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class Xtra : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Relic V : Absorber");
			Tooltip.SetDefault("X");
		}
		public override void SetDefaults()
		{
      
            item.width = 28;     
            item.height = 28;   
            item.value = 1000000000;
            item.rare = 3;
			item.expert = true;
			item.accessory = true;
			item.shootSpeed = 0;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"voXels", 1);
			recipe.AddIngredient(null,"ManaMaterial", 1);
			recipe.AddIngredient(null,"MythicalEnchant", 1);
			recipe.AddIngredient(null,"TinyWatermelon", 1);
			recipe.AddIngredient(null,"AntimaterialMandible", 15);
			recipe.AddIngredient(null,"CoreOfStatus", 5);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
					player.AddBuff(mod.BuffType("HeartDelay"), 300);
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			
				  
		}
		
	}
}
