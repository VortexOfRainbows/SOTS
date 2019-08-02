using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class Plague : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Relic X : Plague");
			Tooltip.SetDefault("O");
		}
		public override void SetDefaults()
		{
      
            item.width = 26;     
            item.height = 26;   
            item.value = 1000000000;
            item.rare = 3;
			item.expert = true;
			item.accessory = true;
			item.shootSpeed = 0;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"OMaterial", 1);
			recipe.AddIngredient(null,"StarMaterial", 1);
			recipe.AddIngredient(null,"NuclearEnchant", 1);
			recipe.AddIngredient(ItemID.SoulofFright, 25);
			recipe.AddIngredient(null,"AntimaterialMandible", 15);
			recipe.AddIngredient(null,"TheHardCore", 5);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
					player.AddBuff(mod.BuffType("PlagueSpread"), 300);
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			
				  
		}
		
	}
}
