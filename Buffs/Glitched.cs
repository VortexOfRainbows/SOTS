using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class Glitched : ModBuff
    {
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Glitched");
			Description.SetDefault("haahjrz johunl lultf zahaz");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
 
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");		
			
                modPlayer.EndreEdit = 1;
				
            if (modPlayer.EndreEdit != 1)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}