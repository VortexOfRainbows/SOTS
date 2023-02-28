using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs.Debuffs
{
    public class DendroChain : ModBuff //All the actual updates for this file will be ran in NPC
    {	
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dendro Chain");
			Description.SetDefault("");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
        }
    }
    public static class DendroChainNPCOperators
    {
        public const int DendroChainStandardDuration = 1200;
        public static void PullOtherNPCs(NPC npc)
        {

        }
        public static void HurtOtherNPCs(NPC npc)
        {

        }
        public static void InitiateNPCDamageStats(NPC npc, ref int outDamage)
        {
            if(npc.HasBuff(ModContent.BuffType<DendroChain>()))
            {
                int buffIndex = npc.FindBuffIndex(ModContent.BuffType<DendroChain>());
                int currentBuffTime = npc.buffTime[buffIndex];
                int damageToHave = currentBuffTime - DendroChainStandardDuration;
                outDamage = damageToHave;
                npc.buffTime[buffIndex] = DendroChainStandardDuration;
            }
        }
    }
}