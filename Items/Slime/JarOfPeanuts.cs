using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs.Boss;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Slime
{
	public class JarOfPeanuts : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 26;
			Item.value = 0;
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 30;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = 4;
			Item.consumable = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Bottle, 1).AddIngredient<Peanut>(15).AddTile(TileID.Anvils).Register();
		}
		public override bool CanUseItem(Player player)
		{
			return !NPC.AnyNPCs(ModContent.NPCType<PutridPinky1>()) && !NPC.AnyNPCs(ModContent.NPCType<PutridPinkyPhase2>());
		}
		public override bool? UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<PutridPinky1>());
			//SOTSUtils.PlaySound(0, (int)player.position.X, (int)player.position.Y, 0);
			//if(!NPC.AnyNPCs(ModContent.NPCType<PutridPinky1>()) && !NPC.AnyNPCs(Mod.Find<ModNPC>("PutridPinkyPhase2").Type))
			//{
			//		 NPC.NewNPC((int)player.Center.X, (int)player.Center.Y - 600, mod.NPCType("PutridPinky1"));	
			//}
			return true;
		}
	}
}