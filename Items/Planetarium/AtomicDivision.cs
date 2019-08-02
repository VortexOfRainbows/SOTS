using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class AtomicDivision : ModItem
	{	int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Atomic Division");
			Tooltip.SetDefault("Killed enemies explode and drop twice the loot\nIf an enemy was to spawn another enemy upon death, the amount of enemies spawned would be doubled aswell");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 42;     
            item.height = 42;   
            item.value = 1000000;
            item.rare = 10;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
            modPlayer.ItemDivision = true;
			
		}
	}
}
