using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class PlanetariumDiamond : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Planetarium Diamond");
			Tooltip.SetDefault("Summons the Ethereal Entity\nUse in planetarium biome\nMight not spawn if there isn't enough blocks for the boss to spawn on\nMake sure the majority of the blocks are outside of your screen");
		}
		public override void SetDefaults()
		{

			item.width = 26;
			item.height = 26;
			item.value = 0;
			item.rare = 9;
			item.maxStack = 30;
			item.useAnimation = 30;
			item.useTime = 30;
			item.useStyle = 4;
			item.consumable = true;
			
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "EmptyPlanetariumOrb", 3);
			recipe.AddIngredient(ItemID.Cloud, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool CanUseItem(Player player)
		{
        SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
		return !NPC.AnyNPCs(mod.NPCType("EtherealEntity")) && !NPC.AnyNPCs(mod.NPCType("EtherealEntity2")) && !NPC.AnyNPCs(mod.NPCType("EtherealEntity3")) && modPlayer.PlanetariumBiome;
	
		}
		public override bool UseItem(Player player)
		{
        SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
				
		NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("EtherealEntity"));
		Main.PlaySound(0, (int)player.position.X, (int)player.position.Y, 0);
		if(!NPC.AnyNPCs(mod.NPCType("EtherealEntity")) && !NPC.AnyNPCs(mod.NPCType("EtherealEntity2")) && !NPC.AnyNPCs(mod.NPCType("EtherealEntity3")) && modPlayer.PlanetariumBiome)
		{
		//		 NPC.NewNPC((int)player.Center.X, (int)player.Center.Y + 600, mod.NPCType("EtherealEntity"));	
		}
			
		return true;
		
		}
	}
}