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
			Description.SetDefault("Void regeneration decreased by 60");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
            longerExpertDebuff = false;
        }
    }
}