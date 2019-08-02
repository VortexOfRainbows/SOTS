using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Challenges
{
	public class BrittleGlass : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brittle as Glass");
			Tooltip.SetDefault("Halves max health, increases damage by 30%\nChallenge: B");
		}
		public override void SetDefaults()
		{
            item.width = 20;    
            item.height = 24; 
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
			SOTSWorld.challengeGlass = true;
			
			return true;
		}
	}
}
