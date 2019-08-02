using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace SOTS.Items.Secrets
{
	public class MicrosoftEdge : ModItem
	{	float speedUp = 0f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Microsoft Edge");
			Tooltip.SetDefault("Gets more broken each swing");
		}
		public override void SetDefaults()
		{

			item.damage = 100;
			item.melee = true;
			item.width = 120;
			item.height = 120;
			item.useTime = 80;
			item.useAnimation = 80;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 100000;
			item.rare = 7;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;   
			item.useTurn = true;

		}
		public override bool UseItem(Player player)
		{
				if(speedUp < 74)
				{
				speedUp += 2f;
				}
				else
				{
				speedUp = 76;
				}
				player.AddBuff(mod.BuffType("Glitched"), 360, false);
				return true;
		}
		public override void UpdateInventory(Player player)
		{
				item.useAnimation = (int)(80 - speedUp);
				item.useTime = (int)(80 - speedUp);
				
				speedUp -= 0.005f;
				if(speedUp < 0)
				{
					speedUp = 0;
				}
		}
	}
}