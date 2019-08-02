using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class ShinyEnchant : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shiny Relic");
			Tooltip.SetDefault("Grants a permanent Battle Potion effect\nGreat for hunting for rare drops\nAlso increases melee damage by 8%");
		}
		public override void SetDefaults()
		{
      
            item.width = 32;     
            item.height = 34;   
            item.value = 50000;
            item.rare = 6;
			item.accessory = true;
			item.defense = 5;
			item.shootSpeed = 0;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BattlePotion, 5);
			recipe.AddIngredient(ItemID.GoldBroadsword, 1);
			recipe.AddIngredient(ItemID.PlatinumBroadsword, 1);
			recipe.AddIngredient(ItemID.SoulofNight, 9);
			recipe.AddIngredient(null,"AntimaterialMandible", 5);
			recipe.AddIngredient(null,"BrassBar", 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
					player.AddBuff(BuffID.Battle, 300);
					player.enemySpawns = true;
			
			player.meleeDamage += 0.08f;
					
		}
		
	}
}
