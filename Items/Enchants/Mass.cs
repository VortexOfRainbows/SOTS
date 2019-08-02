using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class Mass : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Relic XX : Mass");
			Tooltip.SetDefault("L");
		}
		public override void SetDefaults()
		{
      
            item.width = 24;     
            item.height = 24;   
            item.value = 1000000000;
            item.rare = 9;
			item.expert = true;
			item.accessory = true;
			item.shootSpeed = 0;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"LMaterial", 1);
			recipe.AddIngredient(null,"TimeMaterial", 1);
			recipe.AddIngredient(null,"BulletShark", 1);
			recipe.AddIngredient(null,"TerraStar", 1);
			recipe.AddIngredient(null,"AntimaterialMandible", 15);
			recipe.AddIngredient(null,"TheHardCore", 3);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.rangedDamage += 0.55f;
			player.rangedCrit += 25;
		
			
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			modPlayer.vulcanBoost = true;
			
				  
		}
		
	}
}
