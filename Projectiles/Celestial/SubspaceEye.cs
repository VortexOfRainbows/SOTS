using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Celestial
{
    public class SubspaceEye : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.tileCollide = false;
            projectile.width = 0;
            projectile.height = 0;
            projectile.timeLeft = 1200;
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
            projectile.ai[1]++;
            NPC master = Main.npc[(int)projectile.ai[0]];
            if(master.active && master.type == ModContent.NPCType<NPCs.Boss.SubspaceEye>())
            {
                projectile.timeLeft = 257;
                projectile.Center = master.Center;
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
                for (float localX = -size; localX < size; localX++)
                {
                    for (float localY = -size; localY < size; localY++)
                    {
                        float distFromCenter = new Vector2(localX, localY).Length();
                        if (distFromCenter < size)
                        {
                            float alpha = size + 255 - (distFromCenter * 4);
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
            if (!projectile.active)
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

            if(projectile.ai[1] < 5 || projectile.timeLeft < 5)
            {
                lightsUpdate(true); //reset color
                lightSpots = new List<Vector3>();
                lightSpots.Add(new Vector3((drawPlayer.Center.X - Main.screenPosition.X) / scale, (drawPlayer.Center.Y - Main.screenPosition.Y) / scale, 1560f));
                NPC master = Main.npc[(int)projectile.ai[0]];
                if (master.ai[3] > 0)
                {
                    lightSpots.Add(new Vector3((master.Center.X - Main.screenPosition.X) / scale, (master.Center.Y - Main.screenPosition.Y) / scale, master.ai[3]));
                }
                /* for (int i = 0; i < Main.projectile.Length; i++)
                {
                    if (Main.projectile[i].active && (Main.projectile[i].type == mod.ProjectileType("EtimsicCannon") || Main.projectile[i].type == mod.ProjectileType("EtimsicWall")) && Main.projectile[i].ai[1] == 1)
                    {
                        lightSpots.Add(new Vector3((Main.projectile[i].Center.X - Main.screenPosition.X) / scale, (Main.projectile[i].Center.Y - Main.screenPosition.Y) / scale, 40));
                    }
                } */
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
		