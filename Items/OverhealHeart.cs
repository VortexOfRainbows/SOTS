using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class OverhealHeart : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Overheal Heart");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{

			item.width = 18;
			item.height = 18;
			item.value = 0;
			item.rare = 7;
			item.maxStack = 1;
		}
		public override bool OnPickup(Player player)
		{
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			if(modPlayer.HeartSwapDelay == false)
			{
				modPlayer.HeartSwapBonus = 0;
			}
			player.AddBuff(mod.BuffType("HeartDelay2"), 1800);
			for(int i = 0; i < 20; i++)
			{
				if(player.statLife == player.statLifeMax2)
				{
				modPlayer.HeartSwapBonus += 1;
				}
				else
				{
				player.statLife += 1;
				}
			}
			player.HealEffect(20);
			return false;
		}
	
	}
}