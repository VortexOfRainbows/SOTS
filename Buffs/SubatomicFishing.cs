using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class SubatomicFishing : ModBuff
    {
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Atomic Fishing");
			Description.SetDefault("Fish out anything");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
 
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			
                modPlayer.subFish = 1;
				
            if (modPlayer.subFish == 0)
            {
				
                modPlayer.subFish = 0;
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}