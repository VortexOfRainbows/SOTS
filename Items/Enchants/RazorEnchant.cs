using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class RazorEnchant : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Razor Relic");
			Tooltip.SetDefault("15% increased melee and throwing damage\n15% increased mining speed");
		}
		public override void SetDefaults()
		{
      
            item.width = 26;     
            item.height = 34;   
            item.value = 50000;
            item.rare = 8;
			item.expert = true;
			item.accessory = true;
			item.defense = 3;
			item.shootSpeed = 0;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ShurikenSword", 1);
			recipe.AddIngredient(null, "AntimaterialMandible", 5);
			recipe.AddIngredient(null, "SteelBar", 12);
			recipe.AddIngredient(ItemID.WormScarf,1);
			recipe.AddIngredient(ItemID.SawtoothShark,1);
			recipe.AddIngredient(ItemID.Swordfish,1);
			recipe.AddIngredient(ItemID.Glass, 50);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
			player.meleeDamage += 0.15f;
			player.thrownDamage += 0.15f;
			player.pickSpeed -= 0.15f;
					
		}
		
	}
}
