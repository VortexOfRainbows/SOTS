using Microsoft.Xna.Framework;
using SOTS.NPCs.Boss.Polaris;
using SOTS.NPCs.Boss.Polaris.NewPolaris;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Buffs
{
    public class SpawnBossIce : ModBuff
    {
        public override void SetStaticDefaults()
        {  
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
			Main.debuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
			if(!NPC.AnyNPCs(ModContent.NPCType<Polaris>()) && !NPC.AnyNPCs(ModContent.NPCType<Polaris>()))
			{
				NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<NewPolaris>());
				for(int king = 0; king < 200; king++)
				{
					NPC npc = Main.npc[king];
					if(npc.type == ModContent.NPCType<Polaris>() || npc.type == ModContent.NPCType<NewPolaris>())
					{
						npc.Center = player.Center + new Vector2(0, -1000);
					}
				}
			}
            player.DelBuff(buffIndex);
        }
    }
}