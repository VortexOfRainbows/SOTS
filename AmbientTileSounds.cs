using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace SOTS
{
    public class AmbientTileSounds : GlobalTile
    {
        public const string SoundsPath = "SOTS/Sounds/Tiles/";

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

        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            alchtable.IsLooped = skymill.IsLooped = autoham.IsLooped = orb.IsLooped = solidifier.IsLooped = furnace.IsLooped = cookingpot.IsLooped = heart.IsLooped = titan.IsLooped = adama.IsLooped = hellforge.IsLooped = true;
            alchtable.MaxInstances = skymill.MaxInstances = autoham.MaxInstances = orb.MaxInstances = solidifier.MaxInstances = furnace.MaxInstances = cookingpot.MaxInstances = heart.MaxInstances = titan.MaxInstances = adama.MaxInstances = hellforge.MaxInstances = 1;
            alchtable.SoundLimitBehavior = skymill.SoundLimitBehavior = autoham.SoundLimitBehavior = orb.SoundLimitBehavior = solidifier.SoundLimitBehavior = furnace.SoundLimitBehavior = cookingpot.SoundLimitBehavior = heart.SoundLimitBehavior = titan.SoundLimitBehavior = adama.SoundLimitBehavior = hellforge.SoundLimitBehavior = SoundLimitBehavior.IgnoreNew;
            if (!fail)
            {
                if (type == TileID.AlchemyTable)
                {
                    ActiveSound? snd;
                    snd = SoundEngine.FindActiveSound(alchtable);
                    if (snd != null)
                        snd.Stop();
                    return;
                }
                if (type == TileID.SkyMill)
                {
                    ActiveSound? snd;
                    snd = SoundEngine.FindActiveSound(skymill);
                    if (snd != null)
                        snd.Stop();
                    return;
                }
                if (type == TileID.Autohammer)
                {
                    ActiveSound? snd;
                    snd = SoundEngine.FindActiveSound(autoham);
                    if (snd != null)
                        snd.Stop();
                    return;
                }
                if (type == TileID.ShadowOrbs)
                {
                    if (WorldGen.crimson)
                    {
                        ActiveSound? snd;
                        snd = SoundEngine.FindActiveSound(heart);
                        if (snd != null)
                            snd.Stop();
                        return;
                    }
                    else
                    {
                        ActiveSound? snd;
                        snd = SoundEngine.FindActiveSound(orb);
                        if (snd != null)
                            snd.Stop();
                        return;
                    }
                }
                if (type == TileID.Solidifier)
                {
                    ActiveSound? snd;
                    snd = SoundEngine.FindActiveSound(solidifier);
                    if (snd != null)
                        snd.Stop();
                    return;
                }
                if (type == TileID.Furnaces)
                {
                    ActiveSound? snd;
                    snd = SoundEngine.FindActiveSound(furnace);
                    if (snd != null)    
                        snd.Stop();
                    return;
                }
                if (type == TileID.CookingPots)
                {
                    ActiveSound? snd;
                    snd = SoundEngine.FindActiveSound(cookingpot);
                    if (snd != null)
                        snd.Stop();
                    return;
                }
                if (type == TileID.AdamantiteForge)
                {
                    ActiveSound? snd;
                    snd = SoundEngine.FindActiveSound(adama);
                    if (snd != null)
                        snd.Stop();
                    return;
                }
                if (type == TileID.Hellforge)
                {
                    ActiveSound? snd;
                    snd = SoundEngine.FindActiveSound(hellforge);
                    if (snd != null)
                        snd.Stop();
                    return;
                }
            }
            
        }
        public override void PostDraw(int i, int j, int type, SpriteBatch spriteBatch)
        {
            alchtable.IsLooped = skymill.IsLooped = autoham.IsLooped = orb.IsLooped = solidifier.IsLooped = furnace.IsLooped = cookingpot.IsLooped = heart.IsLooped = titan.IsLooped = adama.IsLooped = hellforge.IsLooped = true;
            alchtable.MaxInstances = skymill.MaxInstances = autoham.MaxInstances = orb.MaxInstances = solidifier.MaxInstances = furnace.MaxInstances = cookingpot.MaxInstances = heart.MaxInstances = titan.MaxInstances = adama.MaxInstances = hellforge.MaxInstances = 1;
            alchtable.SoundLimitBehavior = skymill.SoundLimitBehavior = autoham.SoundLimitBehavior = orb.SoundLimitBehavior = solidifier.SoundLimitBehavior = furnace.SoundLimitBehavior = cookingpot.SoundLimitBehavior = heart.SoundLimitBehavior = titan.SoundLimitBehavior = adama.SoundLimitBehavior = hellforge.SoundLimitBehavior = SoundLimitBehavior.IgnoreNew;
            if (SOTS.SOTSTexturePackEnabled)
            {
                if (type == TileID.AlchemyTable)
                {
                    if (SoundEngine.SoundPlayer.FindActiveSound(alchtable) == null)
                        SoundEngine.PlaySound(alchtable, new Vector2(i * 16, j * 16));
                    return;               
                }
                if (type == TileID.SkyMill)
                {
                    if (SoundEngine.SoundPlayer.FindActiveSound(skymill) == null)
                        SoundEngine.PlaySound(skymill, new Vector2(i * 16, j * 16));
                    return;
                }
                if (type == TileID.Autohammer)
                {
                    if (SoundEngine.FindActiveSound(autoham) == null)
                        SoundEngine.PlaySound(autoham, new Vector2(i * 16, j * 16));
                    return;
                }
                if (type == TileID.Solidifier)
                {
                    if (SoundEngine.SoundPlayer.FindActiveSound(solidifier) == null)
                        SoundEngine.PlaySound(solidifier, new Vector2(i * 16, j * 16));
                    return;
                }
                if (type == TileID.Furnaces)
                {
                    if (SoundEngine.SoundPlayer.FindActiveSound(furnace) == null)
                        SoundEngine.PlaySound(furnace, new Vector2(i * 16, j * 16));
                    return;
                }
                if (type == TileID.CookingPots)
                {
                    if (SoundEngine.SoundPlayer.FindActiveSound(cookingpot) == null)
                        SoundEngine.PlaySound(cookingpot, new Vector2(i * 16, j * 16));
                    return;
                }
                if (type == TileID.ShadowOrbs)
                {
                    if (WorldGen.crimson)
                    { 
                        if (SoundEngine.SoundPlayer.FindActiveSound(heart) == null)
                            SoundEngine.PlaySound(heart, new Vector2(i * 16, j * 16));
                        return;
                    }
                    else
                    {
                        if (SoundEngine.SoundPlayer.FindActiveSound(orb) == null)
                            SoundEngine.PlaySound(orb, new Vector2(i * 16, j * 16));
                        return;
                    }
                }
                if (type == TileID.AdamantiteForge)
                {
                    if (SoundEngine.SoundPlayer.FindActiveSound(adama) == null)
                        SoundEngine.PlaySound(adama, new Vector2(i * 16, j * 16));
                    return;
                }
                if (type == TileID.Hellforge)
                {
                    if (SoundEngine.SoundPlayer.FindActiveSound(hellforge) == null)
                        SoundEngine.PlaySound(hellforge, new Vector2(i * 16, j * 16));
                    return;
                }
            }
        }

    }

}