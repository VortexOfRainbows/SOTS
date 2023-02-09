using System;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
 
namespace SOTS.Buffs
{
    public class VoidShock : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Shock");
			Description.SetDefault("Quickly losing life, increases damage taken by 200%");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
        }
		public override void Update(Player player, ref int buffIndex)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidShock = true;
			if (player.endurance > 0)
				player.endurance = 0;
			player.endurance -= 2f;
			if(player.buffTime[buffIndex] <= 6 || voidPlayer.voidMeter > 0)
			{
				if(player.buffTime[buffIndex] <= 6)
					player.AddBuff(ModContent.BuffType<VoidRecovery>(), 120 + voidPlayer.voidMeterMax2);
                player.DelBuff(buffIndex);
                buffIndex--;
			}
		}

    }
}