using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.Xml.Schema;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Steamworks;

namespace SOTS.NPCs
{
    public class DebuffNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        int PlatinumCurse = 0;
        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
        {
            if(item.type == mod.ItemType("PlatinumScythe"))
            {
                if (PlatinumCurse < 10)
                    PlatinumCurse++;
            }
            base.OnHitByItem(npc, player, item, damage, knockback, crit);
        }
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if(PlatinumCurse > 0)
            {
                npc.lifeRegen -= PlatinumCurse * 6;
                damage = PlatinumCurse;
            }
            base.UpdateLifeRegen(npc, ref damage);
        }
        public override void NPCLoot(NPC npc)
        {
            base.NPCLoot(npc);
        }
    }
}