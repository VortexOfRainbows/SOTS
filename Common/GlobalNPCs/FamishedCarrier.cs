using SOTS.Dusts;
using SOTS.NPCs.AbandonedVillage;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SOTS.Common.GlobalNPCs
{
    public class FamishedCarrier : GlobalNPC
    {
        private bool Infected = false;
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            if(lateInstantiation)
            {
                bool validNPC = entity.type == ModContent.NPCType<CorpseBloom>();
                return validNPC;
            }
            return false;
        }
        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            bitWriter.WriteBit(Infected);
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            Infected = bitReader.ReadBit();
        }
        private bool RunOnce = true;
        public override bool PreAI(NPC npc)
        {
            if(RunOnce)
            {
                RunOnce = false;
                Infected = Main.rand.NextBool(2);
                npc.netUpdate = true;
            }
            if(Infected && Main.rand.NextBool(3))
            {
                int type = WorldGen.crimson ? ModContent.DustType<FamishedDustCrimson>() : ModContent.DustType<FamishedDustCorruption>();
                Dust.NewDust(npc.position, npc.width, npc.height, type);
            }
            return true;
        }
        public override void OnKill(NPC npc)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient && Infected)
            {
                NPC npc2 = NPC.NewNPCDirect(npc.GetSource_Death(), (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<Famished>());
                npc2.velocity += Main.rand.NextVector2Circular(15, 15);
                npc2.netUpdate = true;
            }
        }
    }
}
