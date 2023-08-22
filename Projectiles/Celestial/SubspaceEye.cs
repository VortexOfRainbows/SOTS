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
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.width = 0;
            Projectile.height = 0;
            Projectile.timeLeft = 1400;
            Projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overWiresUI.Add(index);
        }
        private List<Vector3> lightSpots = new List<Vector3>();
        private int fadeInTimer = 0;
        int counter = 0;
        public override void AI()
        {
            //Main.NewText(Projectile.timeLeft);
            NPC master = Main.npc[(int)Projectile.ai[0]];
            if(master.active && (master.type == ModContent.NPCType<NPCs.Boss.SubspaceEye>() || master.type == ModContent.NPCType<NPCs.Boss.SubspaceSerpentHead>()) && (master.ai[3] != -1 || Projectile.ai[1] == -1))
            {
                if ((int)Projectile.ai[1] != -1)
                    Projectile.timeLeft = 257;
                Projectile.Center = master.Center;
            }
            else
            {
                if (Projectile.timeLeft > 257)
                    Projectile.timeLeft = 257;
            }
            if (Projectile.timeLeft < 255)
            {
                if (fadeInTimer > 0)
                {
                    fadeInTimer--;
                    if (fadeInTimer <= 0)
                        Projectile.Kill();
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
            Projectile.alpha = fadeInTimer;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.netUpdate = true;
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
                float sizeX = size;
                float sizeY = size;
                int alphaBase = 255;
                int alphaScale = 4;
                if ((int)Projectile.ai[1] == -1)
                {
                    alphaBase = 0;
                    alphaScale = 3;
                    sizeX = Main.screenWidth / 2 / scale + 200;
                }
                for (float localX = -sizeX; localX < sizeX; localX++)
                {
                    for (float localY = -sizeY; localY < sizeY; localY++)
                    {
                        float distFromCenter = new Vector2(localX, localY).Length();
                        if ((int)Projectile.ai[1] == -1)
                            distFromCenter = Math.Abs(localY);
                        if (distFromCenter < size)
                        {
                            float alpha = size + alphaBase - (distFromCenter * alphaScale);
                            if (alpha > 255)
                                alpha = 255;
                            if (alpha < 0)
                                alpha = 0;
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
        Texture2D ShadowTexture = null;
        public override bool PreDraw(ref Color lightColor)
        {
            if (!Projectile.active)
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
                counter = 0;
            }
            if(counter < 4 || Projectile.timeLeft < 5)
            {
                lightsUpdate(true); //reset color
                lightSpots = new List<Vector3>();
                if((int)Projectile.ai[1] == 0)
                    lightSpots.Add(new Vector3((drawPlayer.Center.X - Main.screenPosition.X) / scale, (drawPlayer.Center.Y - Main.screenPosition.Y) / scale, 1560f));
                else if ((int)Projectile.ai[1] == -1)
                    lightSpots.Add(new Vector3((drawPlayer.Center.X - Main.screenPosition.X) / scale, (drawPlayer.Center.Y - Main.screenPosition.Y) / scale, 1280f));
                lightsUpdate(false); //now that we have lights make them transparent
                Texture2D TheShadow = new Texture2D(Main.graphics.GraphicsDevice, width, height);
                TheShadow.SetData(0, null, defaultdataColors, 0, width * height);
                ShadowTexture = TheShadow;
            }
            counter++;
            if (ShadowTexture != null)
                Main.spriteBatch.Draw(ShadowTexture, Vector2.Zero, null, new Color(fadeInTimer, fadeInTimer, fadeInTimer, fadeInTimer), 0, Vector2.Zero, scale, SpriteEffects.None, .2f);
            screenWidthOld = Main.screenWidth;
            screenHeightOld = Main.screenHeight;
            return false;
        }
    }
}
		