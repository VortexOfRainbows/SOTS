using Terraria;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Buffs
{
    public class Frenzy : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false;
        }
		public override void Update(Player player, ref int buffIndex)
		{
            if (player.HeldItem.CountsAsClass(DamageClass.Melee))
                SOTSPlayer.ModPlayer(player).attackSpeedMod += 1;
		}
    }
}