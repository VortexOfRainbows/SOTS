using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class Pluto : ModBuff
    {
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Shrunken");
			Description.SetDefault("You're too small to be hit by attacks that are to small");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
 
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			
                modPlayer.plutoActive = 1;
				
            if (modPlayer.plutoActive == 0)
            {
				
                modPlayer.plutoActive = 0;
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}