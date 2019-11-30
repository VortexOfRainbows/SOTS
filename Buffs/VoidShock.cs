using System;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
 
namespace SOTS.Buffs
{
    public class VoidShock : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Void Shock");
			Description.SetDefault("Quickly losing life");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
		
        }
 
		public override void Update(Player player, ref int buffIndex)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			if(player.lifeRegen > 0)
			{
				player.lifeRegen = 0;
			}
			player.lifeRegen -= 7;
			voidPlayer.voidRegen *= 0.1f;
			if(player.buffTime[buffIndex] <= 6 || voidPlayer.voidMeter > 0)
			{
				if(player.buffTime[buffIndex] <= 6 )
				player.AddBuff(mod.BuffType("VoidRecovery"), 100 + voidPlayer.voidMeterMax2);
			
                player.DelBuff(buffIndex);
                buffIndex--;
			}
		}

    }
}