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
	public class OmegasWish : ModItem
	{	int Probe = -1;
	int timer = -1;
	int mr = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Omega's Wish");
			Tooltip.SetDefault("Secret Item\nMelee Locked");
		}
		public override void SetDefaults()
		{
      
            item.width = 30;     
            item.height = 30;   
            item.value = 0;
            item.rare = -12;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.thrownDamage -= player.thrownDamage;
			player.rangedDamage -= player.rangedDamage;
			player.meleeDamage += 0.25f;
			player.magicDamage -= player.magicDamage;
			player.minionDamage -= player.minionDamage;
			player.statLifeMax2 += 100;
			player.AddBuff(mod.BuffType("BloodySapping"), 300);
	}
		
		
		
		
		
		}
		
	}

