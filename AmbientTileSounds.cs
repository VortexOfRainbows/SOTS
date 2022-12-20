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
        int timerAlchemy = 0;

        int timerSkyMill = 0;

        int timerAutoham = 0;

        int timerOrb = 0;

        int timerSoli = 0;

        int timerFurnace = 0;

        int timerCookingPot = 0;

        int timerHeart = 0;

        int timerTiForge = 0;

        int timerAdForge = 0;

        int timerHellforge = 0;

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
        public override void DrawEffects(int i, int j, int type, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            if (SOTS.IsSOTSTexturePackEnabled())
            {
                if (type == TileID.AlchemyTable)
                {
                    if (timerAlchemy == 0)
                    {
                        SoundEngine.PlaySound(alchtable, new Vector2(i * 16, j * 16));
                    }
                    timerAlchemy++;
                    if (timerAlchemy == 594)
                    {
                        timerAlchemy = 0;
                    }
                }
                if (type == TileID.SkyMill)
                {
                    if (timerSkyMill == 0)
                    {
                        SoundEngine.PlaySound(skymill, new Vector2(i * 16, j * 16));
                    }
                    timerSkyMill++;
                    if (timerSkyMill == 61)
                    {
                        timerSkyMill = 0;
                    }
                }
                if (type == TileID.Autohammer)
                {
                    if (timerAutoham == 0)
                    {
                        SoundEngine.PlaySound(autoham, new Vector2(i * 16, j * 16));
                    }
                    timerAutoham++;
                    if (timerAutoham == 58)
                    {
                        timerAutoham = 0;
                    }
                }
                if (type == TileID.Solidifier)
                {
                    if (timerSoli == 0)
                    {
                        SoundEngine.PlaySound(solidifier, new Vector2(i * 16, j * 16));
                    }
                    timerSoli++;
                    if (timerSoli == 291)
                    {
                        timerSoli = 0;
                    }
                }
                if (type == TileID.Furnaces)
                {
                    if (timerFurnace == 0)
                    {
                        SoundEngine.PlaySound(furnace, new Vector2(i * 16, j * 16));
                    }
                    timerFurnace++;
                    if (timerFurnace == 600)
                    {
                        timerFurnace = 0;
                    }
                }
                if (type == TileID.CookingPots)
                {
                    if (timerCookingPot == 0)
                    {
                        SoundEngine.PlaySound(cookingpot, new Vector2(i * 16, j * 16));
                    }
                    timerCookingPot++;
                    if (timerCookingPot == 1530)
                    {
                        timerCookingPot = 0;
                    }
                }
                if (type == TileID.ShadowOrbs)
                {
                    if (WorldGen.crimson)
                    {
                        if (timerHeart == 0)
                        {
                            SoundEngine.PlaySound(heart, new Vector2(i * 16, j * 16));
                        }
                        timerHeart++;
                        if (timerHeart == 74)
                        {
                            timerHeart = 0;
                        }
                    }
                    else
                    {
                        if (timerOrb == 0)
                        {
                            SoundEngine.PlaySound(orb, new Vector2(i * 16, j * 16));
                        }
                        timerOrb++;
                        if (timerOrb == 59)
                        {
                            timerOrb = 0;
                        }
                    }

                }
                if (type == TileID.AdamantiteForge)
                {
                    if (timerAdForge == 0)
                    {
                        SoundEngine.PlaySound(adama, new Vector2(i * 16, j * 16));
                    }
                    timerAdForge++;
                    if (timerAdForge == 4740) //3780 for titanium
                    {
                        timerAdForge = 0;
                    }
                }
                if (type == TileID.Hellforge)
                {
                    if (timerHellforge == 0)
                    {
                        SoundEngine.PlaySound(hellforge, new Vector2(i * 16, j * 16));
                    }
                    timerHellforge++;
                    if (timerHellforge == 3840)
                    {
                        timerHellforge = 0;
                    }
                }
            }
        }

    }

}