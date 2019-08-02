using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class FlashsparkBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flashspark Boots");
			Tooltip.SetDefault("Unstable speed!\nAllows for quick breaking and acceleration");
		}
		public override void SetDefaults()
		{
      
            item.width = 42;     
            item.height = 36;   
            item.value = 15000000;
            item.rare = 8;
			item.accessory = true;
			item.expert = true;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "TheHardCore", 1);
		    recipe.AddIngredient(ItemID.FrostsparkBoots, 1);
			recipe.AddIngredient(ItemID.LavaWaders, 1);
			recipe.AddIngredient(ItemID.GravityGlobe, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.waterWalk = true; 
			player.fireWalk = true; 
			player.canRocket = true;
			player.rocketBoots = 3; 
			player.rocketTimeMax = 25; 
			player.lavaImmune = true; 
			player.iceSkate = true;
			player.moveSpeed += 10f;
			  if(player.controlLeft) 
			  {
				  if(player.velocity.X > 0)
				  {
					  player.velocity.X = -3;
					  };
			  player.velocity.X -= 0.25f;
			  }
			  if(player.controlRight)
			  {
				  if(player.velocity.X < 0)
				  {
					  player.velocity.X = 3;
					  }
			  player.velocity.X += 0.25f;
			  }
			
		}
	}
}
