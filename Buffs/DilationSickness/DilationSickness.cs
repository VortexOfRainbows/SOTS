using Terraria;
using Terraria.ModLoader;

namespace SOTS.Buffs.DilationSickness
{
    public class DilationSickness : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Dilation Sickness");
			Description.SetDefault("");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
			Main.debuff[Type] = true;
        }
    }
}