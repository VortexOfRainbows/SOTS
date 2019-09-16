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

namespace SOTS.Items.Pyramid
{
	public class SunlightAmulet : ModItem
	{	int timer = 0;
		float boost = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sunlight Amulet");
			Tooltip.SetDefault("Grants permanent hunter and dangersense effects");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 30;     
            item.height = 30;   
            item.value = Item.sellPrice(0, 1, 50, 0);
            item.rare = 4;
			item.accessory = true;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.detectCreature = true;
			player.dangerSense = true;
		}
	}
}