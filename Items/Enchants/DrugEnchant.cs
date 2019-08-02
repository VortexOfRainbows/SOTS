using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class DrugEnchant : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Drugged Relic");
			Tooltip.SetDefault("Gives permanent lifeforce, swiftness, feather fall, magic power, and gills potion effects");
		}
		public override void SetDefaults()
		{
      
            item.width = 32;     
            item.height = 34;   
            item.value = 50000;
            item.rare = 6;
			item.accessory = true;
			item.defense = 8;
			item.shootSpeed = 0;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"AntimaterialMandible", 5);
			recipe.AddIngredient(null,"Waterweed", 1);
			recipe.AddIngredient(null,"Sandshrub", 1);
			recipe.AddIngredient(null,"Healherb", 1);
			recipe.AddIngredient(null,"Bunnyflower", 1);
			recipe.AddIngredient(null,"Skyroot", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
					player.AddBuff(BuffID.Gills, 300);
					player.AddBuff(BuffID.MagicPower, 300);
					player.AddBuff(BuffID.Lifeforce, 300);
					player.AddBuff(BuffID.Swiftness, 300);
					player.AddBuff(BuffID.Featherfall, 300);
					
		}
		
	}
}
