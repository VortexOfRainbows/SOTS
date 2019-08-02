using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class Needle : ModBuff
    {
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Mighty Whistle");
			Description.SetDefault("The knife keeps flying");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
 
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");		
				player.ghostHeal = true;
                modPlayer.needle = true;
				
            if (!modPlayer.HeartSwapDelay)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}