using System;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
 
namespace SOTS.Buffs
{
    public class DEFEBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
			Main.debuff[Type] = false;
        }
		public override void Update(Player player, ref int buffIndex)
		{
			SOTSPlayer sPlayer = SOTSPlayer.ModPlayer(player);
            sPlayer.noMoreConstructs = true;
		}
    }
}