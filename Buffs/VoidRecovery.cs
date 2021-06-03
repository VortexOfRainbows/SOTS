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
			voidPlayer.voidRegen += 1.25f;
			voidPlayer.voidRegen += 0.0075f * voidPlayer.voidMeterMax2;
			voidPlayer.voidRegen += 0.0125f * Math.Abs(voidPlayer.voidMeter);	
			player.channel = false;
			if(voidPlayer.voidMeter < 0)
			{
				voidPlayer.voidRegen += 0.0375f * Math.Abs(voidPlayer.voidMeter);	
				voidPlayer.voidRegen *= 1.5f;
			}
			if(voidPlayer.voidMeter > voidPlayer.voidMeterMax2 - 10)
			{
                player.DelBuff(buffIndex);
                buffIndex--;
			}
			voidPlayer.voidRegen *= 8f;
		}

    }
}