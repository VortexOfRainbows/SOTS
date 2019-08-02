using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class BarrierEnchant : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Barrier Relic");
			Tooltip.SetDefault("15% increased summon and throwing damage\nGrants 2 extra minion slots\nGrants the ability to dash");
		}
		public override void SetDefaults()
		{
      
            item.width = 34;     
            item.height = 34;   
            item.value = 50000;
            item.rare = 6;
			item.expert = true;
			item.accessory = true;
			item.defense = 25;
			item.shootSpeed = 0;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WormScarf,1);
			recipe.AddIngredient(ItemID.EoCShield,1);
			recipe.AddIngredient(ItemID.PaladinsShield,1);
			recipe.AddIngredient(ItemID.Tabi,1);
			recipe.AddIngredient(null,"AntimaterialMandible", 5);
			recipe.AddIngredient(null,"SteelBar", 18);
			recipe.AddIngredient(null,"SpikedGemblem", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
			player.minionDamage += 0.15f;
			player.thrownDamage += 0.15f;
			player.pickSpeed -= 0.15f;
			player.maxMinions += 2;
			player.dash = 1;
					
		}
		
	}
}
