using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class HeartDelay2 : ModBuff
    {
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Heart Boost");
			Description.SetDefault("Overhealed");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
 
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
			if(player.FindBuffIndex(mod.BuffType("HeartDelay")) <= -1)
			{
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");		
			player.statLifeMax2 += modPlayer.HeartSwapBonus;
			
                modPlayer.HeartSwapDelay = true;
				
            if (!modPlayer.HeartSwapDelay || modPlayer.HeartSwapBonus <= 0)
            {
				
                modPlayer.HeartSwapBonus = 0;
                player.DelBuff(buffIndex);
                buffIndex--;
            }
			else
			{
			player.statLife = player.statLifeMax2;
			}
			}
        }
    }
}