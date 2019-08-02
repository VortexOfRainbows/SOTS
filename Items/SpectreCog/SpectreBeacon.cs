using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpectreCog
{
	public class SpectreBeacon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ghostly Reception Beacon");
			Tooltip.SetDefault("You see your skeleton reflected into the sky\nSummons the 4 reanimated horrors of the dungeon");
		}
		public override void SetDefaults()
		{

			item.width = 60;
			item.height = 69;
			item.value = 0;
			item.rare = 10;
			item.maxStack = 1;
			item.useAnimation = 30;
			item.useTime = 30;
			item.useStyle = 4;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ReanimationMaterial", 4);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool CanUseItem(Player player)
		{
		return !NPC.AnyNPCs(mod.NPCType("SpectreEoC")) && player.ZoneDungeon;
	
		}
		public override bool UseItem(Player player)
		{
		NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("SpectreKingSlime"));
		NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("SpectrePrime"));
		NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("TechnoWormHead"));
		NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("SpectreEoC"));
		NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("SpectrePrimeLaser"));
		NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("SpectrePrimeVice"));
		Main.PlaySound(0, (int)player.position.X, (int)player.position.Y, 0);
		
			return true;
		
		
		
		}
	}
}