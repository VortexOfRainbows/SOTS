using Terraria;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Buffs
{
    public class Embattle : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
            Main.persistentBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
		public override void Update(Player player, ref int buffIndex)
		{
            player.GetDamage(DamageClass.Generic) += 0.05f;

            player.buffTime[buffIndex] = 600;
		}
    }
}