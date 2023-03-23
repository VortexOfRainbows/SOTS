using System;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
namespace SOTS.Buffs.ConduitBoosts
{
    public class NatureBoosted : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
		public override void Update(Player player, ref int buffIndex)
		{
            if(player.buffTime[buffIndex] < 30)
               player.buffTime[buffIndex] = 30;
		}
    }
}