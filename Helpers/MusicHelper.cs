using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using tModPorter;

namespace SOTS.Helpers
{
    public static class MusicHelper
    {
        public static int AltPutridPinky => MusicLoader.GetMusicSlot("SOTS/Sounds/Music/BananaLizard/AltPutridPinky");
        public static int AltAdvisor => MusicLoader.GetMusicSlot("SOTS/Sounds/Music/BananaLizard/AltAdvisor");
        public static int AltPyramid => MusicLoader.GetMusicSlot("SOTS/Sounds/Music/BananaLizard/AltPyramid");
        public static int AltPlanetarium => MusicLoader.GetMusicSlot("SOTS/Sounds/Music/BananaLizard/Observatory");
        public static SOTSConfig Config => SOTS.Config;
        public static int Glowmoth => MusicLoader.GetMusicSlot("SOTS/Sounds/Music/Glowmoth");
        public static int PutridPinky => Config.alternativeMusic ? AltPutridPinky : MusicLoader.GetMusicSlot("SOTS/Sounds/Music/PutridPinky");
        public static int Advisor => Config.alternativeMusic ? AltAdvisor : MusicLoader.GetMusicSlot("SOTS/Sounds/Music/Advisor");
        public static int Polaris => MusicLoader.GetMusicSlot("SOTS/Sounds/Music/Polaris");
        public static int Sanctuary => MusicLoader.GetMusicSlot("SOTS/Sounds/Music/BananaLizard/Sanctuary");
        public static int SubspaceSerpent => MusicLoader.GetMusicSlot("SOTS/Sounds/Music/SubspaceSerpent");
        public static int Pyramid => Config.alternativeMusic ? AltPyramid : MusicLoader.GetMusicSlot("SOTS/Sounds/Music/CursedPyramid");
        public static int PyramidBattle => MusicLoader.GetMusicSlot("SOTS/Sounds/Music/PyramidBattle");
        public static int Planetarium => Config.alternativeMusic ? AltPlanetarium : MusicLoader.GetMusicSlot("SOTS/Sounds/Music/Planetarium");
        public static int Knuckles => MusicLoader.GetMusicSlot("SOTS/Sounds/Music/KnucklesTheme");
        public static int Lux => MusicLoader.GetMusicSlot("SOTS/Sounds/Music/Lux");
        public static int PharaohsCurse => MusicLoader.GetMusicSlot("SOTS/Sounds/Music/PharaohsCurse");
        public static int AbandonedVillageSurface => MusicLoader.GetMusicSlot("SOTS/Sounds/Music/BananaLizard/AVSurface");
        public static int AbandonedVillageUnderground =>  MusicLoader.GetMusicSlot("SOTS/Sounds/Music/BananaLizard/AVUnderground");
        public static int Secret =>  MusicLoader.GetMusicSlot("SOTS/Sounds/Music/SecretFound");
        public static int Excavate => MusicLoader.GetMusicSlot("SOTS/Sounds/Music/BananaLizard/Excavate");
    }
}