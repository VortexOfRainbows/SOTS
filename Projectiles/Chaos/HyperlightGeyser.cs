using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chaos
{
    public class HyperlightGeyser : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hyperlight Geyser");
        }
        public override void SetDefaults()
        {
            projectile.width = 36;
            projectile.height = 78;
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
            Texture2D texture = mod.GetTexture("Projectiles/Chaos/HyperlightGeyser");
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            for (int i = 0; i < 6; i++)
            {
                Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 60 - SOTSWorld.GlobalCounter)) * 0.6f;
                color.A = 0;
                Vector2 rotationAround = new Vector2(4 * projectile.scale, 0).RotatedBy(MathHelper.ToRadians(60 * i + SOTSWorld.GlobalCounter));
                Main.spriteBatch.Draw(texture, projectile.Center + rotationAround - Main.screenPosition, null, color * 1f, projectile.rotation, drawOrigin, projectile.scale * 1f, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            }
            Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, lightColor, projectile.rotation, drawOrigin, projectile.scale * 1f, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            texture = mod.GetTexture("Projectiles/Chaos/HyperlightGeyserGlow");
            Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, Color.White, projectile.rotation, drawOrigin, projectile.scale * 1f, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
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
            if (!ended && projectile.timeLeft < 33)
                projectile.timeLeft = 33;
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
                if (!ended && player.itemTime < 33)
                {
                    player.itemTime = 33;
                    player.itemAnimation = 33;
                }
                projectile.alpha = 0;
            }
            projectile.hide = false;
            projectile.spriteDirection = projectile.direction;
            projectile.position.X = (vector2_1.X - (projectile.width / 2));
            projectile.position.Y = (vector2_1.Y - (projectile.height / 2));
            projectile.position += new Vector2(0, 4 * projectile.spriteDirection).RotatedBy(projectile.velocity.ToRotation());
            projectile.rotation = (float)(Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + MathHelper.PiOver2);
            return false;
        }
    }
}