using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class Ceres : ModBuff
    {
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Ceres");
			Description.SetDefault("Autoswing");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
 
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");		
			
                modPlayer.ceres = true;
				
            if (modPlayer.ceres == false)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}