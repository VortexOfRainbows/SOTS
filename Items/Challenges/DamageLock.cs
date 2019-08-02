using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Challenges
{
	public class DamageLock : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Damage Lock");
			Tooltip.SetDefault("Every minute, a random damage type will be unlocked and the rest will be locked\nChallenge: A");
		}
		public override void SetDefaults()
		{
            item.width = 36;     
            item.height = 30;   
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
			SOTSWorld.challengeLock = true;
			
			return true;
		}
	}
}
