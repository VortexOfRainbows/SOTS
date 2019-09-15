using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;

namespace SOTS.Items.Pyramid
{
	public class EmeraldBracelet : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Emerald Bracelet");
			Tooltip.SetDefault("Increases void regen by 0.75");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 34;     
            item.height = 28;   
            item.value = Item.sellPrice(0, 1, 50, 0);
            item.rare = 4;
			item.accessory = true;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidRegen += 0.075f;
		}
	}
}