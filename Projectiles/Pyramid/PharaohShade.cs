using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs.Boss.Curse;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Pyramid
{
    public class PharaohShade : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.width = 0;
            Projectile.height = 0;
            Projectile.timeLeft = 3600;
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
            counter++;
            NPC master = Main.npc[(int)Projectile.ai[0]];
            if (master.active && (master.type == ModContent.NPCType<PharaohsCurse>()))
            {
                PharaohsCurse curse = master.modNPC as PharaohsCurse;
                if(curse.enteredSecondPhase)
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
            if (Main.netMode == NetmodeID.Server)
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
        Vector2 oldPos;
        public override bool PreDraw(ref Color lightColor)
        {
            if (!Projectile.active || Main.LocalPlayer.Distance(Projectile.Center) > 3200)
                return false;
            if(oldPos == null)
            {
                oldPos = Main.LocalPlayer.Center - Main.screenPosition;
            }
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
            Vector2 playerPos = Main.LocalPlayer.Center - Main.screenPosition;
            playerPos = new Vector2((int)(playerPos.X + 0.5f) / 2, (int)(playerPos.Y + 1.0f) / 2);
            bool outRange = Vector2.Distance(playerPos / 2, oldPos) > 1;
            /*if (outRange)
            {
                Main.NewText("out");
                Main.NewText(playerPos.X + " : " + playerPos.Y);
                Main.NewText(width + " : " + height);
            }*/
            if (counter < 5 || outRange)
            {
                counter++;
                lightsUpdate(true); //reset color
                lightSpots = new List<Vector3>();
                if (outRange)
                    oldPos = playerPos / 2;
                else
                    oldPos = new Vector2(width / 2, height / 2);
                lightSpots.Add(new Vector3(oldPos.X, oldPos.Y, 360f));
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
            int parentID = (int)Projectile.ai[0];
            if (parentID >= 0)
            {
                NPC npc = Main.npc[parentID];
                if (npc.active && npc.type == ModContent.NPCType<PharaohsCurse>())
                {
                    PharaohsCurse pharaoh = npc.modNPC as PharaohsCurse;
                    pharaoh.TruePreDraw(spriteBatch, Lighting.GetColor((int)npc.Center.X / 16, (int)npc.Center.Y / 16), fadeInTimer);
                }
            }
        }
    }
}
		