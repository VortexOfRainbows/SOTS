using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems
{
	public class MargritArk: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Margrit Ark");
			Tooltip.SetDefault("Vibrates at a certain frequency");
		}
		public override void SetDefaults()
		{

			item.width = 32;
			item.height = 24;
			item.value = 0;
			item.rare = 6;
			item.maxStack = 30;
			item.useAnimation = 30;
			item.useTime = 30;
			item.useStyle = 4;
			item.consumable = true;
			
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(3081, 12);
			recipe.AddIngredient(3086, 12);
			recipe.AddIngredient(ItemID.Obsidian, 50);
			recipe.AddIngredient(ItemID.Bone, 24);
			recipe.AddIngredient(null, "ObsidianScale", 16);
			recipe.AddTile(TileID.Furnaces);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool CanUseItem(Player player)
		{
		return !NPC.AnyNPCs(mod.NPCType("CrypticCarver1")) && !NPC.AnyNPCs(mod.NPCType("CrypticCarver2"));
	
		}
		public override bool UseItem(Player player)
		{
		NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("CrypticCarver1"));
		Main.PlaySound(0, (int)player.position.X, (int)player.position.Y, 0);
		if(!NPC.AnyNPCs(mod.NPCType("CrypticCarver1")) && !NPC.AnyNPCs(mod.NPCType("CrypticCarver2")))
		{
		//		 NPC.NewNPC((int)player.Center.X, (int)player.Center.Y - 600, mod.NPCType("CrypticCarver1"));	
		}
		return true;
		
		}
	}
}