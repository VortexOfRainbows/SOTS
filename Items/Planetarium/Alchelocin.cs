using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class Alchelocin : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mercurius");
			Tooltip.SetDefault("Immunity to many debuffs\nHitting enemies has a chance to inflict a debuff, lasting 7 seconds\nThe debuff is random");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 26;     
            item.height = 38;   
            item.value = 1000000;
            item.rare = 9;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{ 
		SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
                modPlayer.mercuryActive = 1;
			
			player.buffImmune[20] = true;
			player.buffImmune[22] = true;
			player.buffImmune[23] = true;
			player.buffImmune[24] = true;
			player.buffImmune[30] = true;
			player.buffImmune[31] = true;
			player.buffImmune[32] = true;
			player.buffImmune[33] = true;
			player.buffImmune[30] = true;
			player.buffImmune[35] = true;
			player.buffImmune[36] = true;
			player.buffImmune[39] = true;
			player.buffImmune[46] = true;
			player.buffImmune[47] = true;
			player.buffImmune[67] = true;
			player.buffImmune[68] = true;
			player.buffImmune[69] = true;
			player.buffImmune[70] = true;
			player.buffImmune[80] = true;
			player.buffImmune[94] = true;
			player.buffImmune[103] = true;
			player.buffImmune[137] = true;
			player.buffImmune[144] = true;
			player.buffImmune[149] = true;
			player.buffImmune[153] = true;
			player.buffImmune[156] = true;
			player.buffImmune[160] = true;
			player.buffImmune[163] = true;
			player.buffImmune[164] = true;
			player.buffImmune[169] = true;
			player.buffImmune[192] = true;
			player.buffImmune[194] = true;
			
		}
	}
}
