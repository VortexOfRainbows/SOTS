using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using SOTS.Void;
using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class VoidRift : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Rift");
			Tooltip.SetDefault("Grants a better access to the void\nIncreases void damage by 30%, max void by 40, void regeneration by 2, and decreases void usage by 15%");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 30;     
            item.height = 26;   
            item.value = 1000000;
            item.rare = 10;
			item.defense = 10;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidMeterMax2 += 40;
			voidPlayer.voidDamage += 0.30f;
			voidPlayer.voidRegen += 0.2f;
			voidPlayer.voidCost -= 0.15f;
		}
	}
}
