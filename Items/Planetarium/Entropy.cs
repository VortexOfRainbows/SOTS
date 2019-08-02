using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class Entropy : ModItem
	{	int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Entropy");
			Tooltip.SetDefault("Evens out all percent damage boosts\nDoes not split boosts gained from accessories that are below this in the accessory equip lineup\nOnly effects accessory and armor boosts\nDecreases enemy health by 5% and slowly drains enemy health overtime");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 20;     
            item.height = 30;   
            item.value = 1000000;
            item.rare = 9;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
            modPlayer.StartingDamage += 5;
				
			float totalBuff = (float)(player.meleeDamage + player.magicDamage + player.minionDamage + player.rangedDamage + player.thrownDamage)/5f;
			
			timer += 5;
			timer += (int)(totalBuff - 1) * 100;
			if(timer >= 210)
			{
				for(int i = 0; i < 200; i++)
				{
					NPC target = Main.npc[i];
					if(target.life > 5 && !target.friendly)
					target.life--;
				}
				timer = 0;
			}
			player.meleeDamage = totalBuff;
			player.magicDamage = totalBuff;
			player.minionDamage = totalBuff;
			player.thrownDamage = totalBuff;
			player.rangedDamage = totalBuff;
		}
	}
}
