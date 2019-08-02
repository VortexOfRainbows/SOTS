using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class PewDiePie : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pewdiepie");
			Tooltip.SetDefault("Summons the one and only....");
		}
		public override void SetDefaults()
		{

			item.width = 46;
			item.height = 56;
			item.rare = 12;
			item.expert = true;
			item.maxStack = 420;
			item.useAnimation = 30;
			item.useTime = 30;
			item.useStyle = 4;
			item.consumable = true;
			
		}
		public override bool CanUseItem(Player player)
		{
		return !NPC.AnyNPCs(mod.NPCType("knuckles"));
	
		}
		public override bool UseItem(Player player)
		{
		NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("knuckles"));
		Main.PlaySound(0, (int)player.position.X, (int)player.position.Y, 0);
		
		return true;
		
		}
	}
}