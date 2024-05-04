using System;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
namespace SOTS.Buffs
{
    public class SoulAccess : ModBuff
    {
        public override void SetStaticDefaults()
        { 
            Main.buffNoTimeDisplay[Type] = false;
        }
		public override void Update(Player player, ref int buffIndex)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidRegenSpeed += 0.1f;
		}
    }
}