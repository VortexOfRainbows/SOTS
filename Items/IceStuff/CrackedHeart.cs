using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.IceStuff
{
	public class CrackedHeart : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Divided Heart");
			Tooltip.SetDefault("Lowers enemy health by 5%");
		}
		public override void SetDefaults()
		{
      
            item.width = 28;     
            item.height = 24;   
            item.value = 100000;
            item.rare = 6;
			item.accessory = true;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"AbsoluteBar", 8);
			recipe.AddIngredient(ItemID.PanicNecklace, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
				SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
                modPlayer.StartingDamage += 5;
			
				  
		}
		
	}
}
