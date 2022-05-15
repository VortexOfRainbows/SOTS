using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class Harmony : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Harmony"); //as per its name the sprite should be music related
			Description.SetDefault("Buff companionship");   
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false;
        }
    }
}