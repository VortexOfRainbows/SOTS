using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Challenges
{
	public class Icarus : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Icarus");
			Tooltip.SetDefault("Gravity increased\nChallenge: B");
		}
		public override void SetDefaults()
		{
            item.width = 22;     
            item.height = 22;   
            item.useTime = 15;  
            item.useAnimation = 15;
            item.useStyle = 5;    
            item.noMelee = true;
            item.knockBack = 0;
            item.value = 0;
            item.rare = -12;
            item.UseSound = SoundID.Item8;
            item.autoReuse = false;
			item.consumable = true;
		}
		public override bool UseItem(Player player)
		{
			SOTSWorld.challengeIcarus = true;
			
			return true;
		}
	}
}
