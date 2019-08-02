using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.PLunar
{
	public class NebulousSynchronizer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebulous Crystal");
			Tooltip.SetDefault("It's pulsing with nebulous energy\nNot consumable\nP.S.			Don't use while in multiplayer unless both players have a beastly PC");
		}
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 26;
			item.value = 0;
			item.rare = 12;
			item.maxStack = 1;
			item.useAnimation = 30;
			item.useTime = 30;
			item.useStyle = 4;
			item.expert = true;
			
		}
		public override void AddRecipes()
		{
			/*
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "NightmareFuel", 1000);
			recipe.AddIngredient(null, "TheHardCore", 10);
			recipe.AddIngredient(null, "SpectreWarpCore", 3);
			recipe.AddIngredient(null, "CelestialConsentration", 3);
			recipe.AddIngredient(ItemID.Amethyst, 3);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
			*/
		}
		public override bool CanUseItem(Player player)
		{
		return !NPC.AnyNPCs(mod.NPCType("Nebby"));
		return !Main.dayTime;

	
		}
		public override bool UseItem(Player player)
		{
			if(!Main.dayTime)
			{
						Main.NewText("This boss is a WIP", 255, 255, 255);
		NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("Nebby"));
		Main.PlaySound(0, (int)player.position.X, (int)player.position.Y, 0);
		
			}
		return true;
		
		}
	}
}