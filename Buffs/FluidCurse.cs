using Terraria;
using Terraria.ModLoader;

namespace SOTS.Buffs
{
    public class FluidCurse : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
			Main.debuff[Type] = true;
        }
    }
}