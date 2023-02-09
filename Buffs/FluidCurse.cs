using Terraria;
using Terraria.ModLoader;

namespace SOTS.Buffs
{
    public class FluidCurse : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fluid Curse");
			Description.SetDefault("");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
			Main.debuff[Type] = true;
        }
    }
}