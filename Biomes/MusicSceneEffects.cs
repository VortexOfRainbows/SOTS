using SOTS.Helpers;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Biomes
{
	public class SecretFound : ModSceneEffect
	{
        public override int Music => MusicHelper.Secret;
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh + 1;
        public override bool IsSceneEffectActive(Player player)
        {
            if(SOTSWorld.SecretFoundMusicTimer > 0)
            {
                SOTSWorld.SecretFoundMusicTimer--;
                return true;
            }    
            return false;
        }
    }
    public class PyramidBattle : ModSceneEffect
    {
        public override int Music => MusicHelper.PyramidBattle;
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override bool IsSceneEffectActive(Player player)
        {
            if (SOTSPlayer.pyramidBattle) //variable only applies to local player
                return true;
            return false;
        }
    }
    public class Knuckles : ModSceneEffect
    {
        public override int Music => MusicHelper.Knuckles; //balls in you jaws
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override bool IsSceneEffectActive(Player player)
        {
            return NPC.AnyNPCs(ModContent.NPCType<NPCs.knuckles>()) && Main.npc[NPC.FindFirstNPC(ModContent.NPCType<NPCs.knuckles>())].Distance(player.Center) <= 7000f;
        }
    }
}