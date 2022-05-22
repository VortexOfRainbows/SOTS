using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
 
 
namespace SOTS.Projectiles
{
    public class PurpleBobber : ModProjectile
    {	
		int rodBobberType = -1;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.LightPet[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(ProjectileID.BobberGolden); 
			Projectile.bobber = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 2000;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = true;
            return true;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            base.AI();
        }
        public override bool PreAI()
        {
            if (rodBobberType == -1)
			{
				rodBobberType = (int)Projectile.knockBack;
				Projectile.aiStyle = 61;
				AIType = (int)Projectile.knockBack;
				Projectile.timeLeft = 2000;
				return false;
			}
			return true;
		}
        public override bool PreDrawExtras()
        {
            Player player1 = Main.player[Projectile.owner];
            Lighting.AddLight(Projectile.Center, 0.3f, 0.1f, 0.24f);
			Player owner = null;
            if (Projectile.owner != -1)
            {
                owner = Main.player[Projectile.owner];
            }
            else if (Projectile.owner == 255)
            {
                owner = Main.LocalPlayer;
            }
            var player = owner;
            if (Projectile.bobber && Main.player[Projectile.owner].inventory[Main.player[Projectile.owner].selectedItem].holdStyle > 0)
            {
                float pPosX = player.MountedCenter.X;
                float pPosY = player.MountedCenter.Y;
                pPosY += Main.player[Projectile.owner].gfxOffY;
                int type = Main.player[Projectile.owner].inventory[Main.player[Projectile.owner].selectedItem].type;
                float gravDir = Main.player[Projectile.owner].gravDir;
 
                pPosX += (float)(-20 * Main.player[Projectile.owner].direction);
                if (Main.player[Projectile.owner].direction > 0)
                {
					pPosX -= 13f;
                }
                pPosY -= 40f * gravDir;
 
                if (gravDir == -1f)
                {
                    pPosY -= 12f;
                }
                Vector2 value = new Vector2(pPosX, pPosY);
                value = Main.player[Projectile.owner].RotatedRelativePoint(value + new Vector2(8f), true) - new Vector2(8f);
                float projPosX = Projectile.position.X + (float)Projectile.width * 0.25f - value.X;
                float projPosY = Projectile.position.Y + (float)Projectile.height * 0.5f - value.Y;
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
                    projPosX = Projectile.position.X + (float)Projectile.width * 0.25f - value.X;
                    projPosY = Projectile.position.Y + (float)Projectile.height * 0.5f - value.Y;
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
                        projPosX = Projectile.position.X + (float)Projectile.width * 0.25f - value.X;
                        projPosY = Projectile.position.Y + (float)Projectile.height * 0.1f - value.Y;
                        if (num3 > 12f)
                        {
                            float num4 = 0.3f;
                            float num5 = Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y);
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
                            num5 = 1f - Projectile.localAI[0] / 100f;
                            num4 *= num5;
                            if (projPosY > 0f)
                            {
                                projPosY *= 1f + num4;
                                projPosX *= 1f - num4;
                            }
                            else
                            {
                                num5 = Math.Abs(Projectile.velocity.X) / 3f;
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
                        Color color2 = Lighting.GetColor((int)value.X / 16, (int)(value.Y / 16f), new Color(132, 121, 174));    //fishing line color
                        Texture2D fishingLineTexture = (Texture2D)TextureAssets.FishingLine;
                        Main.spriteBatch.Draw(fishingLineTexture, new Vector2(value.X - Main.screenPosition.X + fishingLineTexture.Width * 0.5f, value.Y - Main.screenPosition.Y + fishingLineTexture.Height * 0.5f), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, 0, fishingLineTexture.Width, (int)num)), color2, rotation2, new Vector2(fishingLineTexture.Width * 0.5f, 0f), 1f, SpriteEffects.None, 0f);
                    }
                }
            }
            return false;
        }
    }
}