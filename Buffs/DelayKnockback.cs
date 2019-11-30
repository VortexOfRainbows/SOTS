using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class DelayKnockback : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Delay Knockback");
			Description.SetDefault("Stuck for a bit then knocked back");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
        }
		public override void Update(Player player, ref int buffIndex)
		{
			player.velocity.Y *= 0.9f;
			player.velocity.X *= 0.9f;
		}
		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.velocity.Y *= 0.93f;
			npc.velocity.X *= 0.93f;
			if(npc.buffTime[buffIndex] <= 5)
			{
				if(!npc.boss)
				{
					npc.velocity.Y *= -1.75f;
					npc.velocity.X *= -5.5f;
				}
				else
				{
					npc.velocity.Y *= -1.15f;
					npc.velocity.X *= -1.25f;
				}
				npc.DelBuff(buffIndex);
			}
		}
    }
}