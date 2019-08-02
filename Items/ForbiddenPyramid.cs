using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class ForbiddenPyramid : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Forbidden Pyramid");
			Tooltip.SetDefault("The desert sands shake beneath your feet...");
		}
		public override void SetDefaults()
		{

			item.width = 40;
			item.height = 40;
			item.value = 0;
			item.rare = 5;
			item.maxStack = 99;
			item.useAnimation = 30;
			item.useTime = 30;
			item.useStyle = 4;
			item.consumable = true;
			
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(3783, 1);
			recipe.AddIngredient(ItemID.SoulofNight, 6);
			recipe.AddIngredient(ItemID.HallowedBar, 12);
			recipe.AddIngredient(null, "BrassBar", 4);
			recipe.AddIngredient(null, "SteelBar", 4);
			recipe.AddIngredient(null, "ObsidianScale", 24);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 3);
			recipe.AddRecipe();
		}
		public override bool CanUseItem(Player player)
		{
		return !NPC.AnyNPCs(mod.NPCType("Antilion")) && player.ZoneDesert;
	
		}
		public override bool UseItem(Player player)
		{
		NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("Antilion"));
		Main.PlaySound(0, (int)player.position.X, (int)player.position.Y, 0);
		if(!NPC.AnyNPCs(mod.NPCType("Antilion")) && player.ZoneDesert)
		{
		//		 NPC.NewNPC((int)player.Center.X, (int)player.Center.Y - 600, mod.NPCType("Antilion"));	
		}		
			
		
		return true;
		
		}
	}
}