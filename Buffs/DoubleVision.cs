using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class DoubleVision : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Double Vision");
			Description.SetDefault("Extra fishing lines");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
 
        }
        public override void Update(Player player, ref int buffIndex)
        {
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			modPlayer.doubledActive = 1;
        }
    }
}