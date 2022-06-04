using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Inferno
{
    public class Blaspha : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 68;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
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
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Inferno/BlasphaGlow");
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Color color = Color.White;
            spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            if (Main.myPlayer == Projectile.owner)
            {
                texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Inferno/BlasphaChargeReticle");
                drawOrigin = new Vector2(texture.Width / 2, texture.Height - 4);
                for (int i = 0; i < 8; i++)
                {
                    Vector2 circular = new Vector2(0, -GetLength() * (1 - chargePercent)).RotatedBy(MathHelper.ToRadians(45 * i + 180 * chargePercent + 90 * windupPercent));
                    drawPos = Main.MouseWorld - Main.screenPosition + circular;
                    spriteBatch.Draw(texture, drawPos, null, new Color(200, 200, 200, 0) * windupPercent, MathHelper.ToRadians(45 * i + 180 * chargePercent + 90 * windupPercent), drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
                }
            }
        }
        bool runOnce = true;
        bool ended = false;
        int counter = 0;
        float chargePercent = 0;
        float windupPercent = 0;
        public float GetLength()
        {
            Player player = Main.player[Projectile.owner];
            float target = 112;
            float targetRange = 480;
            float rightNow = Vector2.Distance(player.Center, Main.MouseWorld) + 8;
            return target * rightNow / targetRange;
        }
        public Vector2 fireFrom()
        {
            return Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * 32;
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 36, 1.2f, 0.4f);
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < 8; i++)
                {
                    Vector2 circular = new Vector2(0, -GetLength() * (1 - chargePercent)).RotatedBy(MathHelper.ToRadians(45 * i + 360 * chargePercent));
                    Vector2 fireTo = Main.MouseWorld + circular - player.Center + Main.rand.NextVector2Circular(1.5f, 1.5f) * (1 - chargePercent);
                    float velocityMult = Projectile.velocity.Length() * (0.6f + chargePercent * 1.4f) * Main.rand.NextFloat(0.7f + 0.2f * chargePercent, 1.3f - 0.2f * chargePercent);
                    Projectile proj = Projectile.NewProjectileDirect(fireFrom(), fireTo.SafeNormalize(Vector2.Zero) * velocityMult, (int)Projectile.ai[1], Projectile.damage, Projectile.knockBack, Main.myPlayer);
                    proj.GetGlobalProjectile<SOTSProjectile>().affixID = -2; //this sould sync automatically on the SOTSProjectile end
                }
            }
        }
        public override void AI()
        {
            int chargeTime = (int)(Projectile.ai[0] * 1.0f);
            counter++;
            if(counter >= 0)
            {
                chargePercent = counter / (float)chargeTime;
                if (chargePercent > 1)
                    chargePercent = 1;
                windupPercent = 1;
            }
            else
            {
                windupPercent = 1 + counter / Projectile.ai[0];
            }
        }
        public override bool PreAI()
        {
            if(runOnce)
            {
                counter = -(int)(Projectile.ai[0]);
                runOnce = false;
            }
            Player player = Main.player[Projectile.owner];
            if (!player.channel && (counter >= 0))
                ended = true;
            if (!ended)
            {
                player.itemTime = 2;
                player.itemAnimation = 2;
                Projectile.timeLeft = 2;
            }
            Vector2 vector2_1 = player.RotatedRelativePoint(Main.player[Projectile.owner].MountedCenter, true);
            if (Main.myPlayer == Projectile.owner)
            {
                float num1 = 20 * Projectile.scale;
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
                Projectile.velocity = Projectile.velocity;
            }
            if (Projectile.hide == false)
            {
                Main.player[Projectile.owner].heldProj = Projectile.whoAmI;
                Projectile.alpha = 0;
            }
            Projectile.rotation = (float)(Projectile.velocity.ToRotation() + MathHelper.PiOver2);
            if (Main.player[Projectile.owner].channel || Projectile.timeLeft > 50)
            {
                player.ChangeDir(Projectile.direction);
                player.heldProj = Projectile.whoAmI;
                player.itemRotation = MathHelper.WrapAngle(Projectile.rotation + MathHelper.ToRadians(Projectile.direction == -1 ? 90 : -90));
            }
            Projectile.hide = false;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.position.X = (vector2_1.X - (Projectile.width / 2));
            Projectile.position.Y = (vector2_1.Y - (Projectile.height / 2));
            return true;
        }
    }
}