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
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.tileCollide = false;
            projectile.width = 0;
            projectile.height = 0;
            projectile.timeLeft = 60;
            projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.
        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsOverWiresUI.Add(index);
        }
        private List<Vector3> lightSpots = new List<Vector3>();
        private int fadeInTimer = 0;
        public override void AI()
        {
            //Main.NewText(projectile.timeLeft);
            Player player = Main.player[projectile.owner];
            Projectile master = Main.projectile[(int)projectile.ai[0]];
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
            if (player.active && !player.dead)
            {
                projectile.Center = master.Center;
            }
            if (modPlayer.FluidCurse && player.active)
            {
                projectile.timeLeft = (int)modPlayer.FluidCurseMult;
                Vector2 toPlayer = player.Center - projectile.Center;
                float dist = toPlayer.Length();
                float speed = (0.4f + dist * 0.1f / (float)Math.Pow(modPlayer.FluidCurseMult, 0.5f));
                if (speed > dist)
                    speed = dist;
                projectile.Center += toPlayer.SafeNormalize(Vector2.Zero) * speed;
            }
            else
            {
                projectile.Kill();
            }
            int destinationAlpha = (int)(projectile.timeLeft * 255f / 60f);
            if (projectile.alpha < destinationAlpha)
                projectile.alpha++;
            else if (projectile.alpha > destinationAlpha)
                projectile.alpha--;
            fadeInTimer = 10 + (int)(projectile.alpha * 0.98f);
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
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (!projectile.active || Main.LocalPlayer.whoAmI != projectile.owner)
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
            spriteBatch.Draw(TheShadow, Vector2.Zero, null, new Color(fadeInTimer, fadeInTimer, fadeInTimer, fadeInTimer), 0, Vector2.Zero, scale, SpriteEffects.None, .2f);
            screenWidthOld = Main.screenWidth;
            screenHeightOld = Main.screenHeight;
            return false;
        }
    }
}
		