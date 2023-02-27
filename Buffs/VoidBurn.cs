using System;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.ID;

namespace SOTS.Buffs
{
    public class VoidBurn : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }
    }
}