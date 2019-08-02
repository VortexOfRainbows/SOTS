using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class CrimCruptSmell : ModBuff
    {
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Crim Crupt Smell");
			Description.SetDefault("You smell like them");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
 
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");		
			
                modPlayer.corruptSmell = 2;
				
            if (modPlayer.corruptSmell == 0)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}