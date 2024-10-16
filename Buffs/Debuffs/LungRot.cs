using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs.Debuffs
{
    public class LungRot : ModBuff
    {
        public const float LifeRegenWhileMoving = 0.2f;
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
			Main.debuff[Type] = true;
        }
    }
    //The effects for this buff are handled in AVPlayer due to requiring a different update order.
}