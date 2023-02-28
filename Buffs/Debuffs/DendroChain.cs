using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs.Debuffs
{
    public class DendroChain : ModBuff //All the actual updates for this file will be ran in NPC
    {	
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
        }
    }
    public static class DendroChainNPCOperators
    {
        public static void PullOtherNPCs(NPC npc)
        {

        }
        public static void HurtOtherNPCs(NPC npc)
        {

        }
        public static void InitiateNPCDamageStats(NPC npc, ref int outDamage)
        {

        }
    }
}