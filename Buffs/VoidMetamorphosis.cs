using System;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
namespace SOTS.Buffs
{
    public class VoidMetamorphosis : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Void Metamorphosis");
			Description.SetDefault("Decreases flat void regeneration by 6\nLosing void to flat void regeneration will recover life");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
            longerExpertDebuff = false;
        }
    }
}