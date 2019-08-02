using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class BloodySapping : ModBuff
    {
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Bloody Tapping");
			Description.SetDefault("Tap into their blood");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
 
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");		
			
                modPlayer.BloodTapping = 1;
				
            if (modPlayer.BloodTapping == 0)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}