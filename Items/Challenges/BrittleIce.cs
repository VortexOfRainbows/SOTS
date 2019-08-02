using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Challenges
{
	public class BrittleIce : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brittle as Ice");
			Tooltip.SetDefault("Lowers max health by 80\nGetting hurt permanently lowers max health\nEnemies can drop items that can increase max health\nChallenge: S");
		}
		public override void SetDefaults()
		{
            item.width = 24;     
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
			SOTSWorld.challengeIce = true;
			return true;
		}
	}
}
