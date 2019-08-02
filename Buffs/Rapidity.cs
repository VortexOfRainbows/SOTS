using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class Rapidity : ModBuff
    {
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Rapidity");
			Description.SetDefault("Fire faster");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
 
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			
                modPlayer.rapidity = true;
				
            if (modPlayer.rapidity == false)
            {
				
                modPlayer.rapidity = false;
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}