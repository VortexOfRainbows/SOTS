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
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = 20;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 50;
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
        public void Draw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture1 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin1 = new Vector2(12 + 6 * Projectile.spriteDirection, 18);
            Vector2 drawPos2 = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture1, drawPos2 + new Vector2(0, Projectile.gfxOffY), null, lightColor, Projectile.rotation, drawOrigin1, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        bool ended = false;
        public override bool PreAI()
        {
            if (Projectile.timeLeft > Projectile.ai[0])
                Projectile.timeLeft = (int)Projectile.ai[0];
            Player player = Main.player[Projectile.owner];
            if (Main.myPlayer == Projectile.owner && Projectile.timeLeft <= 2 && !player.channel)
            {
                ended = true;
                Projectile.netUpdate = true;
            }
            if (!ended && Projectile.timeLeft < 2)
                Projectile.timeLeft = 2;
            Vector2 vector2_1 = player.RotatedRelativePoint(player.MountedCenter, true);
            if (Main.myPlayer == Projectile.owner)
            {
                float num1 =  Projectile.velocity.Length() * Projectile.scale;
                Vector2 vector2_2 = vector2_1;
                float num2 = (float)((double)Main.mouseX + Main.screenPosition.X - vector2_2.X);
                float num3 = (float)((double)Main.mouseY + Main.screenPosition.Y - vector2_2.Y);
                if (player.gravDir == -1.0)
                    num3 = (float)((double)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector2_2.Y);
                float num5 = (float)Math.Sqrt(num2 * (double)num2 + num3 * (double)num3);
                float num6 = num1 / num5;
                float num7 = num2 * num6;
                float num8 = num3 * num6;

                if ((double)num7 != Projectile.velocity.X || (double)num8 != Projectile.velocity.Y)
                    Projectile.netUpdate = true;
                Projectile.velocity.X = num7;
                Projectile.velocity.Y = num8;
            }
            Vector2 trueVelo = new Vector2();
            trueVelo.X = Projectile.velocity.X + Math.Sign(Projectile.velocity.X) * 2;
            trueVelo.Y = Projectile.velocity.Y * (Projectile.velocity.Y < 0 ? 3.5f : 1f);
            if (Projectile.hide == false)
            {
                player.ChangeDir(Projectile.direction);
                player.heldProj = Projectile.whoAmI;
                player.itemRotation = (trueVelo * Projectile.direction).ToRotation() * 1.0f;
                if (!ended && player.itemTime < 2)
                {
                    player.itemTime = 2;
                    player.itemAnimation = 2;
                }
                Projectile.alpha = 0;
            }
            Projectile.hide = false;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.Center = vector2_1;
            Projectile.position += trueVelo + new Vector2(0, 2);
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.PiOver2;
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}