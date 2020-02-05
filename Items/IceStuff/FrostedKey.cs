using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.IceStuff
{
	public class FrostedKey : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Key");
			Tooltip.SetDefault("It's cold to the touch\nYou'd better get near something warm, like a campfire");
		}
		public override void SetDefaults()
		{

			item.width = 34;
			item.height = 30;
			item.value = 400000;
			item.rare = 6;
			item.maxStack = 1;
			
		}
		public override void UpdateInventory(Player player) 
		{
			if(player.FindBuffIndex(87) < 0) //Checking for campfire buff
			{
				if(!SOTSWorld.downedAmalgamation)
				{
					player.AddBuff(46, 6); //adding chilled
					player.AddBuff(44, 6); //adding frostburn
				}
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "FragmentOfPermafrost", 5);
			recipe.AddIngredient(ItemID.HallowedBar, 5);
			recipe.AddIngredient(ItemID.FrostCore, 1);
			recipe.AddIngredient(ItemID.SoulofSight, 3);
			recipe.AddIngredient(ItemID.SoulofMight, 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}