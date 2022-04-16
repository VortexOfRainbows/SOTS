using Terraria;
using Terraria.ModLoader;

namespace SOTS.Buffs.DilationSickness
{
    public class DilationSickness : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Dilation Sickness");
			Description.SetDefault("Decreases void regeneration speed by 90%");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
        }
    }
}