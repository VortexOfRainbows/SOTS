using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class IntimidatingPresence : ModBuff
    {
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			Main.debuff[Type] = true;
		}
    }
}