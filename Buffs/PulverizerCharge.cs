using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class PulverizerCharge : ModBuff
    {
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Pulverizer Charging");
			Description.SetDefault("");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
 
        }
    }
}