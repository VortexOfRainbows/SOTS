using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class PlagueSpread : ModBuff
    {
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Cosmic Plague");
			Description.SetDefault("Spread the radiation");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
 
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			
                modPlayer.cosmicPlague = 1;
				
            if (modPlayer.cosmicPlague == 0)
            {
				
                modPlayer.cosmicPlague = 0;
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}