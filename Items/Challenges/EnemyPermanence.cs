using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Challenges
{
	public class EnemyPermanence : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enemy Permanence");
			Tooltip.SetDefault("Every half-minute, a random enemy will be healed and made immune\nPrioritizes low health enemies over bosses\nChallenge: C");
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
			SOTSWorld.challengePermanence = true;
			
			return true;
		}
	}
}
