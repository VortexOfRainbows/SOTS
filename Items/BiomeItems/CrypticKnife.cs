using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace SOTS.Items.BiomeItems
{
	public class CrypticKnife : ModItem
	{ 	float speedUp = 0;
		bool overheated = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryptic Knife");
			Tooltip.SetDefault("Increase melee damage by 2% and melee crit by 1% while in the inventory\nMultiple Knives can be active at a time");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 6));
		}
		public override void SetDefaults()
		{
            
            item.width = 58;     
            item.height = 44;     
            item.value = 110000;
            item.rare = 6;
			item.expert = true;

		}
		
		public override void UpdateInventory(Player player)
		{
			player.meleeDamage += 0.02f;
			player.meleeCrit += 1;
		}
	}
}
