using Terraria;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Buffs
{
    public class AtlantisBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false;
        }
		public override void Update(Player player, ref int buffIndex)
		{
            player.sotsPlayer().StatShareMeleeAndMagic = true;
            player.moveSpeed += 0.2f;
            player.endurance += 0.1f;
		}
    }
}