using System;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.ID;

namespace SOTS.Buffs
{
    public class VoidMetamorphosis : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Metamorphosis");
			Description.SetDefault("Increases void drain by 6\nLosing void to void drain will recover life");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }
    }
}