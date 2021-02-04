using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Laser
{
    public class PhotonGeyser : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Photon Geyser");
        }
        public override void SetDefaults()
        {
            projectile.width = 36;
            projectile.height = 74;
            projectile.aiStyle = 20;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ranged = true;
            projectile.timeLeft = 20;
            projectile.hide = true;
            projectile.alpha = 255;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = mod.GetTexture("Projectiles/Laser/PhotonGeyser_Glow");
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            float counter = Main.GlobalTime * 160;
            //int bonus = (int)(counter / 360f);
            for (int i = 0; i < 6; i++)
            {
                Color color = new Color(255, 0, 0, 0);
                switch (i)
                {
                    case 0:
                        color = new Color(255, 0, 0, 0);
                        break;
                    case 1:
                        color = new Color(255, 140, 0, 0);
                        break;
                    case 2:
                        color = new Color(255, 255, 0, 0);
                        break;
                    case 3:
                        color = new Color(0, 255, 0, 0);
                        break;
                    case 4:
                        color = new Color(0, 0, 255, 0);
                        break;
                    case 5:
                        color = new Color(140, 0, 255, 0);
                        break;
                }
                Vector2 rotationAround = new Vector2(4 * projectile.scale, 0).RotatedBy(MathHelper.ToRadians(60 * i + counter));
                Main.spriteBatch.Draw(texture, projectile.Center + rotationAround - Main.screenPosition, null, color * 1f, projectile.rotation, drawOrigin, projectile.scale * 1f, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            }
            texture = mod.GetTexture("Projectiles/Laser/PhotonGeyser");
            Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, lightColor, projectile.rotation, drawOrigin, projectile.scale * 1f, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            return false;
        }
        bool ended = false;
        int ai2 = 0;
        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            projectile.ai[1] += 0.35f;
            if (projectile.ai[1] < 100)
            {
                projectile.ai[1] *= 1.035f;
            }
            else
            {
                projectile.ai[1] = 100;
                ai2++;
            }
            if (!player.channel && projectile.ai[1] >= 100 && ai2 >= 5) ended = true;
            if (!ended && projectile.timeLeft < 2)
                projectile.timeLeft = 2;
            Vector2 vector2_1 = Main.player[projectile.owner].RotatedRelativePoint(Main.player[projectile.owner].MountedCenter, true);
            if (Main.myPlayer == projectile.owner)
            {
                float num1 = Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem].shootSpeed * projectile.scale;
                Vector2 vector2_2 = vector2_1;
                float num2 = (float)((double)Main.mouseX + Main.screenPosition.X - vector2_2.X);
                float num3 = (float)((double)Main.mouseY + Main.screenPosition.Y - vector2_2.Y);
                if ((double)Main.player[projectile.owner].gravDir == -1.0)
                    num3 = (float)((double)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector2_2.Y);
                float num4 = (float)Math.Sqrt(num2 * (double)num2 + num3 * (double)num3);
                float num5 = (float)Math.Sqrt(num2 * (double)num2 + num3 * (double)num3);
                float num6 = num1 / num5;
                float num7 = num2 * num6;
                float num8 = num3 * num6;

                if ((double)num7 != projectile.velocity.X || (double)num8 != projectile.velocity.Y)
                    projectile.netUpdate = true;
                projectile.velocity.X = num7;
                projectile.velocity.Y = num8;
                projectile.velocity = projectile.velocity.RotatedBy(projectile.ai[0]);
            }
            if (projectile.hide == false)
            {
                player.ChangeDir(projectile.direction);
                player.heldProj = projectile.whoAmI;
                player.itemRotation = (projectile.velocity * projectile.direction).ToRotation();
                if (!ended && player.itemTime < 2)
                {
                    player.itemTime = 2;
                    player.itemAnimation = 2;
                }
                projectile.alpha = 0;
            }
            projectile.hide = false;
            projectile.spriteDirection = projectile.direction;
            projectile.position.X = (vector2_1.X - (projectile.width / 2));
            projectile.position.Y = (vector2_1.Y - (projectile.height / 2));
            projectile.rotation = (float)(Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57000005245209);
            return false;
        }
    }
}