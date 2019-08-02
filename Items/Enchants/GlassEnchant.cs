using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class GlassEnchant : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glass Relic");
			Tooltip.SetDefault("25% increase to all damage done and taken");
		}
		public override void SetDefaults()
		{
      
            item.width = 26;     
            item.height = 32;   
            item.value = 50000;
            item.rare = 3;
			item.expert = true;
			item.accessory = true;
			item.shootSpeed = 0;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Glass,150);
			recipe.AddIngredient(null,"AntimaterialMandible", 5);
			recipe.AddIngredient(null,"CrusherEmblem", 2);
			recipe.AddIngredient(null,"CoreOfCreation", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
			player.minionDamage += 0.25f;
			player.meleeDamage += 0.25f;
			player.magicDamage += 0.25f;
			player.rangedDamage += 0.25f;
			player.thrownDamage += 0.25f;
			player.endurance -= 0.25f;
					
		}
		
	}
}
