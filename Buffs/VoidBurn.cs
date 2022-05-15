using System;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
namespace SOTS.Buffs
{
    public class VoidBurn : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Burn");
			Description.SetDefault("Increases void drain by 2");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
            longerExpertDebuff = false;
        }
    }
}