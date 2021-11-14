using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class Bluefire : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Bluefire");
			Description.SetDefault("Killed enemies explode into flames");   
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
            modPlayer.BlueFire = true;
        }
    }
}