using Terraria;
using Terraria.ModLoader;

namespace SOTS.Buffs
{
    public class SpawnBossCurse : ModBuff
    {
        public override void SetStaticDefaults()
        {
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			Main.debuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
			if(!NPC.AnyNPCs(Mod.Find<ModNPC>("PharaohsCurse").Type))
			{
				NPC.SpawnOnPlayer(player.whoAmI, Mod.Find<ModNPC>("PharaohsCurse").Type);
				for(int king = 0; king < 200; king++)
				{
					NPC npc = Main.npc[king];
					if(npc.type == Mod.Find<ModNPC>("PharaohsCurse").Type)
					{
						npc.position.X = player.Center.X - npc.width/2;
						npc.position.Y = player.Center.Y - npc.height/2 - 200;
					}
				}
			}
			player.DelBuff(buffIndex);
        }
    }
}