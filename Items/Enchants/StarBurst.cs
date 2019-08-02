using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class StarBurst : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Relic XXX : Star Burst");
			Tooltip.SetDefault("V");
		}
		public override void SetDefaults()
		{
      
            item.width = 34;     
            item.height = 34;   
            item.value = 1000000000;
            item.rare = 4;
			item.expert = true;
			item.accessory = true;
			item.shootSpeed = 0;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"ShadeMaterial", 1);
			recipe.AddIngredient(null,"RimBow", 1);
			recipe.AddIngredient(null,"Metallurgy", 1);
			recipe.AddIngredient(null,"Recycler", 1);
			recipe.AddIngredient(null,"AntimaterialMandible", 15);
			recipe.AddIngredient(null,"TheHardCore", 5);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			modPlayer.starBurst = true;
			modPlayer.recycleOn = true;
			player.meleeDamage += modPlayer.recycleDamageBoost;
			player.rangedDamage += modPlayer.recycleDamageBoost;
			player.magicDamage +=  modPlayer.recycleDamageBoost;
			player.thrownDamage +=  modPlayer.recycleDamageBoost;
			if(modPlayer.recycleDamageBoost > 0.6f)
			{
				 modPlayer.recycleDamageBoost = 0.6f;
			}
			
				  
		}
		
	}
}
