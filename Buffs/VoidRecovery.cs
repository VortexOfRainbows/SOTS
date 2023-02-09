using System;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
namespace SOTS.Buffs
{
    public class VoidRecovery : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
        }
		public override void Update(Player player, ref int buffIndex)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidRecovery = true;
			player.channel = false;
		}
    }
}