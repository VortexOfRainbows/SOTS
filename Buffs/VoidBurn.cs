using System;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
namespace SOTS.Buffs
{
    public class VoidBurn : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Void Burn");
			Description.SetDefault("Flat void regeneration decreased by 2");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
            longerExpertDebuff = false;
        }
    }
}