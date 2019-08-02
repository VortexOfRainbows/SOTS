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


namespace SOTS.Items.Planetarium
{
	public class Oblivion : ModItem
	{	float boost = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Oblivion");
			Tooltip.SetDefault("All projectiled attacks increase enemy defense\nProjectiled attacks damage scale with defense");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 28;     
            item.height = 54;   
            item.value = 1000000;
            item.rare = 9;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
                
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
                modPlayer.Oblivion = true;
			
		}
	}
}
