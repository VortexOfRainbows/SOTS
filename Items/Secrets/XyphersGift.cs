using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SOTS.Items.Secrets
{
	public class XyphersGift : ModItem
	{	int Probe = -1;
	int timer = -1;
	int mr = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Xypher's Gift");
			Tooltip.SetDefault("Secret Item\nMagic Locked");
		}
		public override void SetDefaults()
		{
      
            item.width = 32;     
            item.height = 30;   
            item.value = 0;
            item.rare = -12;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.thrownDamage -= player.thrownDamage;
			player.rangedDamage -= player.rangedDamage;
			player.meleeDamage -= player.meleeDamage;
			player.magicDamage += 0.25f;
			player.minionDamage -= player.minionDamage;
			player.manaCost -= 100;
	}
		
		
		
		
		
		}
		
	}

