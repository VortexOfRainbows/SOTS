using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class Doubled : ModBuff
    {
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Doubled");
			Description.SetDefault("Your eyes... they desieve you...");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
 
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			
                modPlayer.doubledActive = 1;
				
            if (modPlayer.doubledActive == 0)
            {
				
                modPlayer.doubledActive = 0;
				modPlayer.doubledAmount = 0;
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}