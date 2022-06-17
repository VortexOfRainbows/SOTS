using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS
{
    public static class ModGores //From Aequus 1.4 - Thanks NALYDDD
    {
        public static int GoreType(string name)
        {
            int index = name.LastIndexOf("/"); //this is somewhat a bandaid fix, but also means that finding gores from code is easier too!
            if(index != -1)
                name = name.Substring(index + 1);
            return SOTS.Instance.Find<ModGore>(name).Type;
        }
        /// <summary>
        /// Spawns gore at the top left of the NPC
        /// </summary>
        public static Gore DeathGore(this NPC npc, string name, Vector2 offset = default(Vector2), Vector2? velocity = null)
        {
            return Gore.NewGoreDirect(npc.GetSource_Death("SOTS:Gore"), npc.position + offset, velocity ?? npc.velocity, GoreType(name));
        }
        /// <summary>
        /// Spawns gore at the center of the NPC
        /// </summary>
        public static Gore DeathGoreCenter(this NPC npc, string name, Vector2 offset = default(Vector2), Vector2? velocity = null)
        {
            return Gore.NewGoreDirect(npc.GetSource_Death("SOTS:Gore"), npc.Center + offset, velocity ?? npc.velocity, GoreType(name));
        }
        /// <summary>
        /// Spawns gore at the designated position
        /// </summary>
        public static Gore DeathGoreAtPosition(this NPC npc, string name, Vector2 position, Vector2? velocity = null)
        {
            return Gore.NewGoreDirect(npc.GetSource_Death("SOTS:Gore"), position, velocity ?? npc.velocity, GoreType(name));
        }
    }
}