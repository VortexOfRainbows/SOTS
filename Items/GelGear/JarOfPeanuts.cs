using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GelGear
{
	public class JarOfPeanuts : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jar Of Peanuts");
			Tooltip.SetDefault("Summons the Putrid Pinky");
		}
		public override void SetDefaults()
		{

			item.width = 20;
			item.height = 26;
			item.value = 0;
			item.rare = 3;
			item.maxStack = 30;
			item.useAnimation = 30;
			item.useTime = 30;
			item.useStyle = 4;
			item.consumable = true;
			
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Bottle, 1);
			recipe.AddIngredient(null,"Peanut", 15);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
		}
		public override bool CanUseItem(Player player)
		{
			return !NPC.AnyNPCs(mod.NPCType("PutridPinky1")) && !NPC.AnyNPCs(mod.NPCType("PutridPinkyPhase2"));
		}
		public override bool UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("PutridPinky1"));
			Main.PlaySound(0, (int)player.position.X, (int)player.position.Y, 0);
			if(!NPC.AnyNPCs(mod.NPCType("PutridPinky1")) && !NPC.AnyNPCs(mod.NPCType("PutridPinkyPhase2")))
			{
			//		 NPC.NewNPC((int)player.Center.X, (int)player.Center.Y - 600, mod.NPCType("PutridPinky1"));	
			}
			return true;
		}
	}
}