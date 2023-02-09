using Terraria;
using Terraria.ModLoader;

namespace SOTS.Buffs
{
    public class Satiated : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Satiated");
			Description.SetDefault("No more eating!");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
        }
    }
}