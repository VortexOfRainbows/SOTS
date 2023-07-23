using System;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
namespace SOTS.Buffs
{
    public class CyberneticEnhancements : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
			Main.debuff[Type] = false;
        }
		public override void Update(Player player, ref int buffIndex)
		{
            player.GetDamage<VoidGeneric>() += 0.1f;
            player.buffTime[buffIndex] = 3000;
		}
    }
}