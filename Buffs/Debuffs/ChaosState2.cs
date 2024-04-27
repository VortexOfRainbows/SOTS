using SOTS.NPCs.Town;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs.Debuffs
{
    public class ChaosState2 : ModBuff
    {	
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
        }
        public override void Update(Player p, ref int buffIndex)
        {
            p.GetAttackSpeed(DamageClass.Melee) += 0.4f;
        }
    }
}