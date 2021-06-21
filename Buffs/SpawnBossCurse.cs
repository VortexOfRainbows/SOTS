using Terraria;
using Terraria.ModLoader;

namespace SOTS.Buffs
{
    public class SpawnBossCurse : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Debug");
			Description.SetDefault("This is a work around since I don't know how to program multiplayer");   
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			Main.debuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
			if(!NPC.AnyNPCs(mod.NPCType("PharaohsCurse")))
			{
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("PharaohsCurse"));
				for(int king = 0; king < 200; king++)
				{
					NPC npc = Main.npc[king];
					if(npc.type == mod.NPCType("PharaohsCurse"))
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