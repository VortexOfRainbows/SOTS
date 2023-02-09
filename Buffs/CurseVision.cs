using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class CurseVision : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Curse Vision");
			Description.SetDefault("Afflicted?");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
			Main.debuff[Type] = true;
        }
    }
}