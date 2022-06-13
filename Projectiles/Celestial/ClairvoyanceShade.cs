using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Celestial
{
    public class ClairvoyanceShade : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.width = 0;
            Projectile.height = 0;
            Projectile.timeLeft = 60;
            Projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overWiresUI.Add(index);
        }
        private List<Vector3> lightSpots = new List<Vector3>();
        private int fadeInTimer = 0;
        public override void AI()
        {
            //Main.NewText(Projectile.timeLeft);
            Player player = Main.player[Projectile.owner];
            Projectile master = Main.projectile[(int)Projectile.ai[0]];
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
            if (player.active && !player.dead)
            {
                Projectile.Center = master.Center;
            }
            if (modPlayer.FluidCurse && player.active)
            {
                Projectile.timeLeft = (int)modPlayer.FluidCurseMult;
                Vector2 toPlayer = player.Center - Projectile.Center;
                float dist = toPlayer.Length();
                float speed = (0.4f + dist * 0.1f / (float)Math.Pow(modPlayer.FluidCurseMult, 0.5f));
                if (speed > dist)
                    speed = dist;
                Projectile.Center += toPlayer.SafeNormalize(Vector2.Zero) * speed;
            }
            else
            {
                Projectile.Kill();
            }
            int destinationAlpha = (int)(Projectile.timeLeft * 255f / 60f);
            if (Projectile.alpha < destinationAlpha)
                Projectile.alpha++;
            else if (Projectile.alpha > destinationAlpha)
                Projectile.alpha--;
            fadeInTimer = 10 + (int)(Projectile.alpha * 0.98f);
            if (fadeInTimer > 255)
                fadeInTimer = 255;
        }
        private int screenWidthOld = 0;
        private int screenHeightOld = 0;
        private int height = 0;
        private int width = 0;
        private Color[] defaultdataColors = null;
        private float scale = 4;
        private void lightsUpdate(bool dark)
        {
            foreach (Vector3 spot in lightSpots)
            {
                Vector2 spotCoords = new Vector2(spot.X, spot.Y);
                float size = spot.Z / scale;
                float sizeX = width;
                float sizeY = height;
                int alphaScale = 2;
                for (float localX = -sizeX; localX < sizeX; localX++)
                {
                    for (float localY = -sizeY; localY < sizeY; localY++)
                    {
                        float distFromCenter = new Vector2(localX, localY).Length();
                        if (distFromCenter < size)
                        {
                            float alpha = size - (distFromCenter * alphaScale);
                            int x = (int)spotCoords.X + (int)localX;
                            int y = (int)spotCoords.Y + (int)localY;
                            int loc = x + y * width;
                            if (loc < defaultdataColors.Length && x < width && x > 0 && loc >= 0)
                            {
                                if (dark)
                                {
                                    defaultdataColors[loc] = Color.Black;
                                }
                                else
                                {
                                    float mult = ((255f - alpha) / 255f);
                                    if (mult < 0)
                                        mult = 0;
                                    defaultdataColors[loc] = Color.Black * mult;
                                }
                            }
                        }
                    }
                }
            }
        }
        int counter = 0;
        public override bool PreDraw(ref Color lightColor)
        {
            if (!Projectile.active || Main.LocalPlayer.whoAmI != Projectile.owner)
                return false;
            Player drawPlayer = Main.LocalPlayer;
            if (Main.screenWidth != screenWidthOld || Main.screenHeight != screenHeightOld)
            {
                height = (int)(Main.screenHeight / scale);
                width = (int)(Main.screenWidth / scale);
                height++;
                defaultdataColors = new Color[width * height];
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        defaultdataColors[x + y * width] = Color.Black;
                    }
                }
            }

            if (counter < 5)
            {
                counter++;
                lightsUpdate(true); //reset color
                lightSpots = new List<Vector3>();
                lightSpots.Add(new Vector3(width / 2, height / 2, 800));
                lightsUpdate(false); //now that we have lights make them transparent
            }
            Texture2D TheShadow = new Texture2D(Main.graphics.GraphicsDevice, width, height);
            TheShadow.SetData(0, null, defaultdataColors, 0, width * height);
            Main.spriteBatch.Draw(TheShadow, Vector2.Zero, null, new Color(fadeInTimer, fadeInTimer, fadeInTimer, fadeInTimer), 0, Vector2.Zero, scale, SpriteEffects.None, .2f);
            screenWidthOld = Main.screenWidth;
            screenHeightOld = Main.screenHeight;
            return false;
        }
    }
}
		