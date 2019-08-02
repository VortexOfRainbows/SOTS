using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class BackupBow : ModBuff
    {
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Backup Bow");
			Description.SetDefault("Bow behind the back");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
 
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");		
			
                modPlayer.libraActive = 1;
				
            if (modPlayer.libraActive == 0)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}