using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Helpers;
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
            // DisplayName.SetDefault("Hyperlight Geyser");
        }
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 78;
            Projectile.aiStyle = 20;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 20;
            Projectile.hide = true;
            Projectile.alpha = 255;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Chaos/HyperlightGeyser").Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            for (int i = 0; i < 6; i++)
            {
                Color color = ColorHelper.Pastel(MathHelper.ToRadians(i * 60 - SOTSWorld.GlobalCounter)) * 0.6f;
                color.A = 0;
                Vector2 rotationAround = new Vector2(4 * Projectile.scale, 0).RotatedBy(MathHelper.ToRadians(60 * i + SOTSWorld.GlobalCounter));
                Main.spriteBatch.Draw(texture, Projectile.Center + rotationAround - Main.screenPosition, null, color * 1f, Projectile.rotation, drawOrigin, Projectile.scale * 1f, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale * 1f, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            texture = Mod.Assets.Request<Texture2D>("Projectiles/Chaos/HyperlightGeyserGlow").Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, drawOrigin, Projectile.scale * 1f, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            return false;
        }
        bool ended = false;
        int ai2 = 0;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.ai[1] += 0.35f;
            if (Projectile.ai[1] < 100)
            {
                Projectile.ai[1] *= 1.035f;
            }
            else
            {
                Projectile.ai[1] = 100;
                ai2++;
            }
            if (!player.channel && Projectile.ai[1] >= 100 && ai2 >= 5) ended = true;
            if (!ended && Projectile.timeLeft < 33)
                Projectile.timeLeft = 33;
            Vector2 vector2_1 = Main.player[Projectile.owner].RotatedRelativePoint(Main.player[Projectile.owner].MountedCenter, true);
            if (Main.myPlayer == Projectile.owner)
            {
                float num1 = Main.player[Projectile.owner].inventory[Main.player[Projectile.owner].selectedItem].shootSpeed * Projectile.scale;
                Vector2 vector2_2 = vector2_1;
                float num2 = (float)((double)Main.mouseX + Main.screenPosition.X - vector2_2.X);
                float num3 = (float)((double)Main.mouseY + Main.screenPosition.Y - vector2_2.Y);
                if ((double)Main.player[Projectile.owner].gravDir == -1.0)
                    num3 = (float)((double)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector2_2.Y);
                float num4 = (float)Math.Sqrt(num2 * (double)num2 + num3 * (double)num3);
                float num5 = (float)Math.Sqrt(num2 * (double)num2 + num3 * (double)num3);
                float num6 = num1 / num5;
                float num7 = num2 * num6;
                float num8 = num3 * num6;

                if ((double)num7 != Projectile.velocity.X || (double)num8 != Projectile.velocity.Y)
                    Projectile.netUpdate = true;
                Projectile.velocity.X = num7;
                Projectile.velocity.Y = num8;
                Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[0]);
            }
            if (Projectile.hide == false)
            {
                player.ChangeDir(Projectile.direction);
                player.heldProj = Projectile.whoAmI;
                player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
                if (!ended && player.itemTime < 33)
                {
                    player.itemTime = 33;
                    player.itemAnimation = 33;
                }
                Projectile.alpha = 0;
            }
            Projectile.hide = false;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.position.X = (vector2_1.X - (Projectile.width / 2));
            Projectile.position.Y = (vector2_1.Y - (Projectile.height / 2));
            Projectile.position += new Vector2(0, 4 * Projectile.spriteDirection).RotatedBy(Projectile.velocity.ToRotation());
            Projectile.rotation = (float)(Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.PiOver2);
            return false;
        }
    }
}