using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class FrozenThroughTime : ModBuff
    {	
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Paralyzed");
			Description.SetDefault("Can't... move...");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
        }
		public override void Update(Player player, ref int buffIndex)
		{
			player.velocity.Y = 0;
			player.velocity.X = 0;
		}
		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.velocity.Y = 0;
			npc.velocity.X = 0;
		}
    }
}