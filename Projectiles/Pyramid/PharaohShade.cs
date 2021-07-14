using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs.Boss.Curse;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Pyramid
{
    public class PharaohShade : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.tileCollide = false;
            projectile.width = 0;
            projectile.height = 0;
            projectile.timeLeft = 3600;
            projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.
        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsOverWiresUI.Add(index);
        }
        private List<Vector3> lightSpots = new List<Vector3>();
        private int fadeInTimer = 0;
        int counter = 0;
        public override void AI()
        {
            //Main.NewText(projectile.timeLeft);
            counter++;
            NPC master = Main.npc[(int)projectile.ai[0]];
            if (master.active && (master.type == ModContent.NPCType<PharaohsCurse>()))
            {
                PharaohsCurse curse = master.modNPC as PharaohsCurse;
                if(curse.enteredSecondPhase)
                {
                    if ((int)projectile.ai[1] != -1)
                        projectile.timeLeft = 257;
                    projectile.Center = master.Center;
                }
                else
                {
                    if (projectile.timeLeft > 257)
                        projectile.timeLeft = 257;
                }
            }
            else
            {
                if (projectile.timeLeft > 257)
                    projectile.timeLeft = 257;
            }
            if (projectile.timeLeft < 255)
            {
                if (fadeInTimer > 0)
                {
                    fadeInTimer--;
                    if (fadeInTimer <= 0)
                        projectile.Kill();
                }
            }
            else
            {
                if (fadeInTimer < 255)
                {
                    fadeInTimer++;
                }
                if (fadeInTimer > 255)
                    fadeInTimer = 255;
            }
            projectile.alpha = fadeInTimer;
            if (Main.netMode != 1)
            {
                projectile.netUpdate = true;
            }
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
                int alphaScale = 3;
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
                lightSpots.Add(new Vector3(width / 2, height / 2, 360));
                lightsUpdate(false); //now that we have lights make them transparent
            }
            Texture2D TheShadow = new Texture2D(Main.graphics.GraphicsDevice, width, height);
            TheShadow.SetData(0, null, defaultdataColors, 0, width * height);
            spriteBatch.Draw(TheShadow, Vector2.Zero, null, new Color(fadeInTimer, fadeInTimer, fadeInTimer, fadeInTimer), 0, Vector2.Zero, scale, SpriteEffects.None, .2f);
            screenWidthOld = Main.screenWidth;
            screenHeightOld = Main.screenHeight;
            DrawPharaoh(spriteBatch, lightColor);
            return false;
        }
        public void DrawPharaoh(SpriteBatch spriteBatch, Color lightColor)
        {
            int parentID = (int)projectile.ai[0];
            if (parentID >= 0)
            {
                NPC npc = Main.npc[parentID];
                if (npc.active && npc.type == ModContent.NPCType<PharaohsCurse>())
                {
                    PharaohsCurse pharaoh = npc.modNPC as PharaohsCurse;
                    pharaoh.TruePreDraw(spriteBatch, lightColor, fadeInTimer);
                }
            }
        }
    }
}
		