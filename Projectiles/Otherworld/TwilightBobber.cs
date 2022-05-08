using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{
    public class TwilightBobber : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BobberGolden);
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Otherworld/TwilightBobberGlow");
            Vector2 drawOrigin = new Vector2(texture.Width/2, projectile.height/2);
            Vector2 drawPos = projectile.Center - Main.screenPosition;
            drawPos.X += 8;
            Color color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0);
            Player player = Main.player[projectile.owner];
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
            if(modPlayer.rainbowGlowmasks)
                for (int k = 0; k < 2; k++)
                {
                    spriteBatch.Draw(texture, drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
                }
            else
                spriteBatch.Draw(texture, drawPos, null, Color.White, projectile.rotation, drawOrigin, projectile.scale, projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
        public override bool PreDrawExtras(SpriteBatch spriteBatch)      //this draws the fishing line correctly
        {
            Lighting.AddLight(projectile.Center, 0.7f, 0.9f, 1.2f);
            Player player = Main.player[projectile.owner];
            if (projectile.bobber && Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem].holdStyle > 0)
            {
                float pPosX = player.MountedCenter.X;
                float pPosY = player.MountedCenter.Y;
                pPosY += Main.player[projectile.owner].gfxOffY;
                int type = Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem].type;
                float gravDir = Main.player[projectile.owner].gravDir;

                if (type == mod.ItemType("TwilightFishingPole"))
                {
                    pPosX += (float)(43 * Main.player[projectile.owner].direction);
                    if (Main.player[projectile.owner].direction < 0)
                    {
                        pPosX -= 13f;
                    }
                    pPosY -= 30f * gravDir;
                }

                if (gravDir == -1f)
                {
                    pPosY -= 12f;
                }
                Vector2 value = new Vector2(pPosX, pPosY);
                value = Main.player[projectile.owner].RotatedRelativePoint(value + new Vector2(8f), true) - new Vector2(8f);
                float projPosX = projectile.position.X + (float)projectile.width * 0.5f - value.X;
                float projPosY = projectile.position.Y + (float)projectile.height * 0.5f - value.Y;
                Math.Sqrt((double)(projPosX * projPosX + projPosY * projPosY));
                float rotation2 = (float)Math.Atan2((double)projPosY, (double)projPosX) - 1.57f;
                bool flag2 = true;
                if (projPosX == 0f && projPosY == 0f)
                {
                    flag2 = false;
                }
                else
                {
                    float projPosXY = (float)Math.Sqrt((double)(projPosX * projPosX + projPosY * projPosY));
                    projPosXY = 12f / projPosXY;
                    projPosX *= projPosXY;
                    projPosY *= projPosXY;
                    value.X -= projPosX;
                    value.Y -= projPosY;
                    projPosX = projectile.position.X + (float)projectile.width * 0.5f - value.X;
                    projPosY = projectile.position.Y + (float)projectile.height * 0.5f - value.Y;
                }
                while (flag2)
                {
                    float num = 12f;
                    float num2 = (float)Math.Sqrt((double)(projPosX * projPosX + projPosY * projPosY));
                    float num3 = num2;
                    if (float.IsNaN(num2) || float.IsNaN(num3))
                    {
                        flag2 = false;
                    }
                    else
                    {
                        if (num2 < 20f)
                        {
                            num = num2 - 8f;
                            flag2 = false;
                        }
                        num2 = 12f / num2;
                        projPosX *= num2;
                        projPosY *= num2;
                        value.X += projPosX;
                        value.Y += projPosY;
                        projPosX = projectile.position.X + (float)projectile.width * 0.5f - value.X;
                        projPosY = projectile.position.Y + (float)projectile.height * 0.1f - value.Y;
                        if (num3 > 12f)
                        {
                            float num4 = 0.3f;
                            float num5 = Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y);
                            if (num5 > 16f)
                            {
                                num5 = 16f;
                            }
                            num5 = 1f - num5 / 16f;
                            num4 *= num5;
                            num5 = num3 / 80f;
                            if (num5 > 1f)
                            {
                                num5 = 1f;
                            }
                            num4 *= num5;
                            if (num4 < 0f)
                            {
                                num4 = 0f;
                            }
                            num5 = 1f - projectile.localAI[0] / 100f;
                            num4 *= num5;
                            if (projPosY > 0f)
                            {
                                projPosY *= 1f + num4;
                                projPosX *= 1f - num4;
                            }
                            else
                            {
                                num5 = Math.Abs(projectile.velocity.X) / 3f;
                                if (num5 > 1f)
                                {
                                    num5 = 1f;
                                }
                                num5 -= 0.5f;
                                num4 *= num5;
                                if (num4 > 0f)
                                {
                                    num4 *= 2f;
                                }
                                projPosY *= 1f + num4;
                                projPosX *= 1f - num4;
                            }
                        }
                        rotation2 = (float)Math.Atan2((double)projPosY, (double)projPosX) - 1.57f;
                        Color color2 = new Color(205, 200, 255);    //fishing line color
                        if (SOTSPlayer.ModPlayer(player).rainbowGlowmasks)
                            color2 = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);

                        Main.spriteBatch.Draw(Main.fishingLineTexture, new Vector2(value.X - Main.screenPosition.X + (float)Main.fishingLineTexture.Width * 0.5f, value.Y - Main.screenPosition.Y + (float)Main.fishingLineTexture.Height * 0.5f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.fishingLineTexture.Width, (int)num)), color2, rotation2, new Vector2((float)Main.fishingLineTexture.Width * 0.5f, 0f), 1f, SpriteEffects.None, 0f);
                    }
                }
            }
            return false;
        }
    }
}