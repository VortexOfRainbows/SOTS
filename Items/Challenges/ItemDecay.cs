using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Challenges
{
	public class ItemDecay : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Item Decay");
			Tooltip.SetDefault("Every minute and a-half, a random non-placeable, non-tool, item will be purged from your inventory\nChallenge: S");
		}
		public override void SetDefaults()
		{
            item.width = 26;     
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
			SOTSWorld.challengeDecay = true;
			
			return true;
		}
	}
}
