using System;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
namespace SOTS.Buffs
{
    public class VoidRecovery : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Void Recovery");
			Description.SetDefault("Soul returning");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
        }
		public override void Update(Player player, ref int buffIndex)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidRecovery = true;
			player.channel = false;
			if(voidPlayer.voidMeter < -10)
			{
				voidPlayer.voidMeter = -10;
			}
			if(voidPlayer.voidMeter > voidPlayer.voidMeterMax2 / 2)
			{
                player.DelBuff(buffIndex);
                buffIndex--;
			}
		}
    }
}