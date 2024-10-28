using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Celestial
{
    public class SubspaceEyeWall : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.width = 0;
            Projectile.height = 0;
            Projectile.timeLeft = 1200;
            Projectile.hide = true; 
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overWiresUI.Add(index);
        }
        private int FadeInTimer = 0;
        private bool RunOnce = true;
        public override void AI()
        {
            if(RunOnce)
            {
                if(Projectile.knockBack == 1)
                   Projectile.timeLeft = 1400;
                RunOnce = false;
            }
            NPC master = Main.npc[(int)Projectile.ai[0]];
            if (master.active && (master.type == ModContent.NPCType<NPCs.Boss.SubspaceEye>() || master.type == ModContent.NPCType<NPCs.Boss.SubspaceSerpentHead>()) && (master.ai[3] != -1 || Math.Abs(Projectile.ai[1]) <= 1))
            {
                if (Math.Abs(Projectile.ai[1]) > 1)
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
                if (FadeInTimer > 0)
                {
                    FadeInTimer -= 20;
                    if (FadeInTimer <= 0)
                        Projectile.Kill();
                }
            }
            else
            {
                if (FadeInTimer < 255)
                {
                    FadeInTimer++;
                    if (Math.Abs(Projectile.ai[1]) > 1)
                        FadeInTimer++;
                }
                if (FadeInTimer > 255)
                    FadeInTimer = 255;
            }
            Projectile.alpha = FadeInTimer;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.netUpdate = true;
            }
        }
        private int screenHeightOld = 0;
        private int height = 0;
        private int width = 0;
        private Color[] defaultdataColors = null;
        private float scale = 4;
        private void lightsUpdate(bool dark)
        {
            float sizeY = height;
            float sizeX = width;
            int counter = 0;
            for (float localX = 0; localX < sizeX; localX++)
            {
                if(localX < (255 / 2) + 1)
                    counter += 2;
                if (localX > sizeX - ((255 / 2) + 1))
                    counter -= 2;
                for (float localY = 0; localY < sizeY; localY++)
                {
                    if (counter > 255)
                        counter = 255;
                    float alpha = 255 - counter;
                    int x = (int)localX;
                    int y = (int)localY;
                    int loc = x + y * width;
                    if (loc < defaultdataColors.Length && x < width && loc >= 0)
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
        private bool runOnce2 = true;
        private Texture2D ShadowTexture = null;
        public override bool PreDraw(ref Color lightColor)
        {
            if (!Projectile.active)
                return false;
            if (Main.screenHeight != screenHeightOld)
            {
                height = (int)(Main.screenHeight / scale);
                width = (int)(Main.screenWidth / scale) + 200;
                height++;
                defaultdataColors = new Color[width * height];
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        defaultdataColors[x + y * width] = Color.Black;
                    }
                }
                runOnce2 = true;
            }
            if(runOnce2)
            {
                lightsUpdate(false);
                Texture2D TheShadow = new Texture2D(Main.graphics.GraphicsDevice, width, height);
                TheShadow.SetData(0, null, defaultdataColors, 0, width * height);
                ShadowTexture = TheShadow;
                runOnce2 = false;
            }
            NPC master = Main.npc[(int)Projectile.ai[0]];
            if (master.active && (master.type == ModContent.NPCType<NPCs.Boss.SubspaceEye>() || master.type == ModContent.NPCType<NPCs.Boss.SubspaceSerpentHead>()) && master.ai[3] != -1)
            {
                Projectile.Center = master.Center;
            }
            float offset = Projectile.ai[1];
            if (Projectile.ai[1] > 0 && Math.Abs(Projectile.ai[1]) < 1000)
            {
                offset -= Main.screenWidth + 800;
            }
            if(Projectile.ai[1] < 0 && Math.Abs(Projectile.ai[1]) >= 1000)
            {
                offset -= Main.screenWidth + 800;
            }
            Main.spriteBatch.Draw(ShadowTexture, new Vector2(Projectile.Center.X + Projectile.ai[1] + offset - Main.screenPosition.X, 0), null, new Color(FadeInTimer, FadeInTimer, FadeInTimer, FadeInTimer), 0, new Vector2(0, 0), scale, SpriteEffects.None, .2f);
            screenHeightOld = Main.screenHeight;
            return false;
        }
    }
}
		