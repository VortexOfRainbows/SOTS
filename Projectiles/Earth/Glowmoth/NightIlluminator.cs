using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Helpers;
using SOTS.Void;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Earth.Glowmoth
{
    public class NightIlluminator : ModProjectile
    {
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Earth/Glowmoth/NightIlluminatorGlow");
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture, drawPos, null, Color.White, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
        public override void SetDefaults()
        {
            Projectile.width = 58;
            Projectile.height = 58;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 40;
            Projectile.hide = true;
            Projectile.alpha = 255;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(ended);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            ended = reader.ReadBoolean();
        }
        bool ended = false;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 vector2_1 = Main.player[Projectile.owner].RotatedRelativePoint(Main.player[Projectile.owner].MountedCenter, true);
            if (Main.myPlayer == Projectile.owner)
            {
                float num1 = 32 * Projectile.scale;
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
                Main.player[Projectile.owner].heldProj = Projectile.whoAmI;
                Projectile.alpha = 0;
            }
            Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.Pi / 2);
            if (!ended)
            {
                player.ChangeDir(Projectile.direction);
                player.heldProj = Projectile.whoAmI;
                player.itemRotation = MathHelper.WrapAngle(Projectile.velocity.ToRotation());
                if(player.HeldItem.type == ModContent.ItemType<Items.Earth.Glowmoth.NightIlluminator>())
                {
                    player.HeldItem.noUseGraphic = true; //this is needed for multiplayer
                }
                player.itemTime = 2;
                player.itemAnimation = 2;
                Projectile.timeLeft = 2;
                if(Main.rand.NextBool(5))
                {
                    Color color2 = ColorHelper.VibrantColorGradient(Main.rand.NextFloat(180), true);
                    Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(16, 16) + Projectile.velocity.SafeNormalize(Vector2.Zero) * 16, 24, 24, ModContent.DustType<CopyDust4>());
                    dust.color = color2;
                    dust.noGravity = true;
                    dust.fadeIn = 0.1f;
                    dust.scale = 0.5f * dust.scale + 0.7f;
                    dust.alpha = Projectile.alpha;
                    dust.velocity *= 0.5f;
                    dust.velocity += Projectile.velocity * 0.22f;
                }
            }
            Projectile.rotation -= MathHelper.ToRadians(Projectile.direction * 45);
            if(player.whoAmI == Main.myPlayer)
            {
                if (!Main.mouseRight)
                {
                    ended = true;
                    Projectile.netUpdate = true;
                }
            }
            Projectile.hide = false;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.position.X = (vector2_1.X - (Projectile.width / 2));
            Projectile.position.Y = (vector2_1.Y - (Projectile.height / 2));
            return false;
        }
    }
}