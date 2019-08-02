using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class Neptune : ModBuff
    {
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Neptune");
			Description.SetDefault("More thrown projectiles");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
 
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");		
			
                modPlayer.neptune = 1;
				
            if (modPlayer.neptune == 0)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}