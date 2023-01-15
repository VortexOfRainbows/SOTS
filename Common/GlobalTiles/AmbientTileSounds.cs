using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ObjectData;

namespace SOTS.Common.GlobalTiles
{
    public class AmbientTileSounds : GlobalTile
    {
        public override bool IsLoadingEnabled(Mod mod) //Disabling ambient audio because it is very WIP and may not work with the audio system.
        {
            return false;
        }
        public const string SoundsPath = "SOTS/Sounds/Tiles/";
        private static bool WereSoundsOn = false;

        SoundStyle alchtable = new(SoundsPath + "alchemytable", SoundType.Ambient);
        SoundStyle skymill = new(SoundsPath + "skymill", SoundType.Ambient);
        SoundStyle autoham = new(SoundsPath + "autoham", SoundType.Ambient);
        SoundStyle orb = new(SoundsPath + "orb", SoundType.Ambient);
        SoundStyle solidifier = new(SoundsPath + "solidifier", SoundType.Ambient);
        SoundStyle furnace = new(SoundsPath + "furnace", SoundType.Ambient);
        SoundStyle cookingpot = new(SoundsPath + "cookingpot", SoundType.Ambient);
        SoundStyle heart = new(SoundsPath + "crimheart", SoundType.Ambient);
        SoundStyle titan = new(SoundsPath + "titanforge", SoundType.Ambient);
        SoundStyle adama = new(SoundsPath + "adaforge", SoundType.Ambient);
        SoundStyle hellforge = new(SoundsPath + "hellforge", SoundType.Ambient);
        public override void SetStaticDefaults()
        {
            alchtable.IsLooped = skymill.IsLooped = autoham.IsLooped = orb.IsLooped = solidifier.IsLooped = furnace.IsLooped = cookingpot.IsLooped = heart.IsLooped = titan.IsLooped = adama.IsLooped = hellforge.IsLooped = true;
            alchtable.MaxInstances = skymill.MaxInstances = autoham.MaxInstances = orb.MaxInstances = solidifier.MaxInstances = furnace.MaxInstances = cookingpot.MaxInstances = heart.MaxInstances = titan.MaxInstances = adama.MaxInstances = hellforge.MaxInstances = 1;
            alchtable.SoundLimitBehavior = skymill.SoundLimitBehavior = autoham.SoundLimitBehavior = orb.SoundLimitBehavior = solidifier.SoundLimitBehavior = furnace.SoundLimitBehavior = cookingpot.SoundLimitBehavior = heart.SoundLimitBehavior = titan.SoundLimitBehavior = adama.SoundLimitBehavior = hellforge.SoundLimitBehavior = SoundLimitBehavior.IgnoreNew;
        }
        public void KillAmbiance(int type, bool fail, bool all = false)
        {
            if (!fail)
            {
                if (all || type == TileID.AlchemyTable)
                {
                    TurnOffSound(alchtable);
                }
                if (all || type == TileID.SkyMill)
                {
                    TurnOffSound(skymill);
                }
                if (all || type == TileID.Autohammer)
                {
                    TurnOffSound(autoham);
                }
                if (all || type == TileID.ShadowOrbs)
                {
                    if (WorldGen.crimson)
                    {
                        TurnOffSound(heart);
                    }
                    else
                    {
                        TurnOffSound(orb);
                    }
                }
                if (all || type == TileID.Solidifier)
                {
                    TurnOffSound(solidifier);
                }
                if (all || type == TileID.Furnaces)
                {
                    TurnOffSound(furnace);
                }
                if (all || type == TileID.CookingPots)
                {
                    TurnOffSound(cookingpot);
                }
                if (all || type == TileID.AdamantiteForge)
                {
                    TurnOffSound(adama);
                }
                if (all || type == TileID.Hellforge)
                {
                    TurnOffSound(hellforge);
                }
            }
        }
        public void TurnOffSound(SoundStyle sound)
        {
            ActiveSound? snd = SoundEngine.FindActiveSound(sound);
            if (snd != null)
            {
                snd.Sound.Stop(true);
            }
        }
        public void TurnOnSound(SoundStyle sound, int i, int j)
        {
            SoundEngine.PlaySound(sound, new Vector2(i * 16, j * 16));
        }
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            KillAmbiance(type, fail);
        }
        public override void PostDraw(int i, int j, int type, SpriteBatch spriteBatch)
        {
            if (SOTS.SOTSTexturePackEnabledAudio && !Main.gamePaused)
            {
                WereSoundsOn = true;
                if (type == TileID.AlchemyTable)
                {
                    TurnOnSound(alchtable, i, j);
                }
                if (type == TileID.SkyMill)
                {
                    TurnOnSound(skymill, i, j);
                }
                if (type == TileID.Autohammer)
                {
                    TurnOnSound(autoham, i, j);
                }
                if (type == TileID.Solidifier)
                {
                    TurnOnSound(solidifier, i, j);
                }
                if (type == TileID.Furnaces)
                {
                    TurnOnSound(furnace, i, j);
                }
                if (type == TileID.CookingPots)
                {
                    TurnOnSound(cookingpot, i, j);
                }
                if (type == TileID.ShadowOrbs)
                {
                    if (WorldGen.crimson)
                    {
                        TurnOnSound(heart, i, j);
                    }
                    else
                    {
                        TurnOnSound(orb, i, j);
                    }
                }
                if (type == TileID.AdamantiteForge)
                {
                    TurnOnSound(adama, i, j);
                }
                if (type == TileID.Hellforge)
                {
                    TurnOnSound(hellforge, i, j);
                }
            }
            else
            {
                if(WereSoundsOn)
                    KillAmbiance(type, false, true);
                WereSoundsOn = false;
            }
        }
    }
}