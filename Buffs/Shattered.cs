using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class Shattered : ModBuff
    {	
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Shattered");
			Description.SetDefault("Next hit will hurt a lot!");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
        }
    }
}