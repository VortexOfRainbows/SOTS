using Terraria;
using Terraria.ModLoader;

namespace SOTS.Buffs
{
    public class SulfurBurn : ModBuff
    {
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Sulfur Burn");
			Description.SetDefault("Soul power draining");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
			Main.debuff[Type] = true;
        }
    }
}