using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class WaveSkipped : ModBuff
    {
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Wave Skipped");
			Description.SetDefault("You just skipped all the waves!");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
 
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");		
			
                modPlayer.chessSkip = true;
				
            if (!modPlayer.chessSkip)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}