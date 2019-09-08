using System;
using Terraria;
using Terraria.ModLoader;
 
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace SOTS.Buffs
{
    public class SpawnBossIce : ModBuff
    { int regenTimer = 0;
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Debug");
			Description.SetDefault("This is a work around since I don't know how to program multiplayer");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
			Main.debuff[Type] = true;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("ShardKing"));
			
					for(int king = 0; king < 200; king++)
					{
						NPC npc = Main.npc[king];
						if(npc.type == mod.NPCType("ShardKing"))
						{
						npc.position.X = player.Center.X - npc.width/2;
						npc.position.Y = player.Center.Y - npc.height/2 - 1200;
						}
					}
                player.DelBuff(buffIndex);
        }
    }
}