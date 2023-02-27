using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class RippleBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false;
        }
		public override void Update(Player player, ref int buffIndex)
        {
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
            modPlayer.rippleBonusDamage += 2;
            modPlayer.rippleEffect = true;
            modPlayer.rippleTimer++;
        }
    }
}