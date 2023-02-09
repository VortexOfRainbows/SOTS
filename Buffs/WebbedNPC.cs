using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class WebbedNPC : ModBuff
    {	
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
        }
    }
}