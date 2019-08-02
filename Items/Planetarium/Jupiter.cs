using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class Jupiter : ModItem
	{	float boost = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jupiter");
			Tooltip.SetDefault("Lowers all non-summon damage by 20%\nFire additional projectiles from the sky");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 28;     
            item.height = 36;   
            item.value = 1000000;
            item.rare = 9;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.thrownDamage -= 0.2f;
			player.meleeDamage -= 0.2f;
			player.rangedDamage -= 0.2f;
			player.magicDamage -= 0.2f;
			
                
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
                modPlayer.jupiter = 1;
			
		}
	}
}
