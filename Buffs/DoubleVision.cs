using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class DoubleVision : ModBuff
    {
        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Double Vision");
			Description.SetDefault("Extra fishing lines");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);	
			modPlayer.doubledActive = 1;
        }
    }
}