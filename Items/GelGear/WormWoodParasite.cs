using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using SOTS.Void;
using Terraria.ModLoader;

namespace SOTS.Items.GelGear
{
	public class WormWoodParasite : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Worm Wood Parasite");
			Tooltip.SetDefault("Increases void regen speed by 1\nIncreases max void by 15\nLowers life regen speed");
		}
		public override void SetDefaults()
		{
      
            item.width = 34;     
            item.height = 30;   
            item.value = 90000;
            item.rare = 4;
			item.accessory = true;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "WormWoodCore", 1);
			recipe.AddIngredient(null, "SlimeyFeather", 8);
			recipe.AddIngredient(null, "GelBar", 6);
			recipe.AddIngredient(ItemID.Wood, 4);
			recipe.AddIngredient(ItemID.PinkGel, 30);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidMeterMax2 += 15;
			voidPlayer.voidRegen += 0.1f;
			player.lifeRegen -= 1;
			
		}
	}
}
