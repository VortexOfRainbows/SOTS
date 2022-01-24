using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Inferno
{
    public class IncineratorGloveProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Incinerator Glove");
        }
        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.aiStyle = 20;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ranged = true;
            projectile.timeLeft = 50;
            projectile.hide = true;
            projectile.alpha = 255;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(ended);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            ended = reader.ReadBoolean();
        }
        public void Draw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture1 = Main.projectileTexture[projectile.type];
            Vector2 drawOrigin1 = new Vector2(12 + 6 * projectile.spriteDirection, 18);
            Vector2 drawPos2 = projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture1, drawPos2 + new Vector2(0, projectile.gfxOffY), null, lightColor, projectile.rotation, drawOrigin1, projectile.scale, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
        bool ended = false;
        public override bool PreAI()
        {
            if (projectile.timeLeft > projectile.ai[0])
                projectile.timeLeft = (int)projectile.ai[0];
            Player player = Main.player[projectile.owner];
            if (Main.myPlayer == projectile.owner && projectile.timeLeft <= 2 && !player.channel)
            {
                ended = true;
                projectile.netUpdate = true;
            }
            if (!ended && projectile.timeLeft < 2)
                projectile.timeLeft = 2;
            Vector2 vector2_1 = player.RotatedRelativePoint(player.MountedCenter, true);
            if (Main.myPlayer == projectile.owner)
            {
                float num1 =  projectile.velocity.Length() * projectile.scale;
                Vector2 vector2_2 = vector2_1;
                float num2 = (float)((double)Main.mouseX + Main.screenPosition.X - vector2_2.X);
                float num3 = (float)((double)Main.mouseY + Main.screenPosition.Y - vector2_2.Y);
                if (player.gravDir == -1.0)
                    num3 = (float)((double)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector2_2.Y);
                float num5 = (float)Math.Sqrt(num2 * (double)num2 + num3 * (double)num3);
                float num6 = num1 / num5;
                float num7 = num2 * num6;
                float num8 = num3 * num6;

                if ((double)num7 != projectile.velocity.X || (double)num8 != projectile.velocity.Y)
                    projectile.netUpdate = true;
                projectile.velocity.X = num7;
                projectile.velocity.Y = num8;
            }
            Vector2 trueVelo = new Vector2();
            trueVelo.X = projectile.velocity.X + Math.Sign(projectile.velocity.X) * 2;
            trueVelo.Y = projectile.velocity.Y * (projectile.velocity.Y < 0 ? 3.5f : 1f);
            if (projectile.hide == false)
            {
                player.ChangeDir(projectile.direction);
                player.heldProj = projectile.whoAmI;
                player.itemRotation = (trueVelo * projectile.direction).ToRotation() * 1.0f;
                if (!ended && player.itemTime < 2)
                {
                    player.itemTime = 2;
                    player.itemAnimation = 2;
                }
                projectile.alpha = 0;
            }
            projectile.hide = false;
            projectile.spriteDirection = projectile.direction;
            projectile.Center = vector2_1;
            projectile.position += trueVelo + new Vector2(0, 2);
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + MathHelper.PiOver2;
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}