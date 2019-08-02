using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class GreatFilter : ModItem
	{	int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Great Filter");
			Tooltip.SetDefault("Lowers enemy health by 25%");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 36;     
            item.height = 38;   
            item.value = 1000000;
            item.rare = 10;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
            modPlayer.StartingDamage += 25;
			
		}
	}
}
