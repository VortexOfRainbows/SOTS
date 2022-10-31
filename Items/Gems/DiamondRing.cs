using SOTS.Void;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Gems
{
	public class DiamondRing : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Light-Bringer's Ring");
			Tooltip.SetDefault("Increases damage by (defense)%");
			this.SetResearchCost(1);
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips) //goes through each tooltip line
			{
				if (line.Mod == "Terraria" && line.Name == "Tooltip0") //checks the name of the tootip line
				{
					int defenseStat = SOTSPlayer.ModPlayer(Main.LocalPlayer).previousDefense;
					line.Text = "Increases damage by " + defenseStat + "%\nDefense is set to " + (int)Math.Sqrt(defenseStat);
					return;
				}
			}
			base.ModifyTooltips(tooltips);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 22;     
            Item.height = 20;   
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer.ModPlayer(player).DevilRing = true;
		}
	}
}