using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class TigerEnchant : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Delta Relic");
			Tooltip.SetDefault("25% increased melee\n25% increased mining speed\nAllows the ability to climb walls and dash\nGives a chance to dodge attacks");
		}
		public override void SetDefaults()
		{
      
            item.width = 36;     
            item.height = 32;   
            item.value = 50000;
            item.rare = 8;
			item.accessory = true;
			item.shootSpeed = 0;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "AntimaterialMandible", 5);
			recipe.AddIngredient(ItemID.MasterNinjaGear,1);
			recipe.AddIngredient(ItemID.WarriorEmblem,1);
			recipe.AddIngredient(ItemID.AdamantiteBar,22);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
			player.meleeDamage += 0.25f;
			player.pickSpeed -= 0.25f;
			player.dash = 1;
			player.spikedBoots = 2;
			player.blackBelt = true;
					
		}
		
	}
}
