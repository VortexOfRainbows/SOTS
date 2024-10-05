using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using SOTS.Dusts;
using SOTS.Helpers;

namespace SOTS.Projectiles.Chaos
{
    public class ChaosHelixLaser : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Chaos Helix Laser");
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.timeLeft = 80;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        List<Vector2> drawPositionList = new List<Vector2>();
        float startUpTime = 40;
        int counter = 0;
        bool runOnce = true;
        public const float Speed = 15f;
        public const float SeekOutOthersRange = 96f;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if(Projectile.timeLeft < 80 - startUpTime && Projectile.timeLeft >= 30)
            {
                for(int i = 0; i < drawPositionList.Count - 10; i += 3)
                {
                    float otherMult = i / 30f;
                    if (otherMult > 1)
                        otherMult = 1;
                    int width = (int)(44 * otherMult);
                    Rectangle hitBox = new Rectangle((int)drawPositionList[i].X - width / 2, (int)drawPositionList[i].Y - width / 2, width, width);
                    if (hitBox.Intersects(targetHitbox))
                        return true;
                }
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (runOnce)
                return false;
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            Color color = new Color(140, 140, 140, 0);
            float rotation = Projectile.velocity.ToRotation();
            int max = drawPositionList.Count;
            float scale = counter / startUpTime;
            float alphaMult = 1f;
            if (scale < 1)
            {
                alphaMult = 0.15f + 0.2f * scale;
                scale *= 0.5f;
            }
            else
            {
                scale = 1 - 0.3f * (1 - Projectile.timeLeft / 30f);
            }
            for (int i = 1; i < max; i++)
            {
                if (!SOTS.Config.lowFidelityMode || i % 2 == 0)
                {
                    float otherMult = i / 30f;
                    if (otherMult > 1)
                        otherMult = 1;
                    float actualScale = scale * (0.3f + 0.7f * otherMult * otherMult);
                    if(i > 210)
                    {
                        float mult = (i - 210) / 39f;
                        actualScale *= 1 - mult;
                    }
                    Vector2 drawPos = drawPositionList[i];
                    Color otherC = ColorHelper.Pastel(MathHelper.ToRadians(i * 3), false);
                    otherC.A = 0;
                    float sinusoidScaleMult = 1f + 0.2f * (float)Math.Sin(MathHelper.ToRadians(Projectile.ai[1] + i * 3f + Main.GameUpdateCount * -5f));
                    Vector2 sinusoid = new Vector2(0, 24 * otherMult * (float)Math.Sin(MathHelper.ToRadians(Main.GameUpdateCount * -6.5f + i * 3f + Projectile.ai[1]))).RotatedBy(rotation);
                    Main.spriteBatch.Draw(texture, drawPos + sinusoid - Main.screenPosition, null, otherC * ((255 - Projectile.alpha) / 255f) * alphaMult * 0.6f, rotation, origin, new Vector2(3f, actualScale * 1.25f * sinusoidScaleMult) * Projectile.scale, SpriteEffects.None, 0f);
                    Vector2 sinusoid2 = new Vector2(0, 18 * otherMult * (float)Math.Sin(MathHelper.ToRadians(Main.GameUpdateCount * -2f + i * 4f + Projectile.ai[1]))).RotatedBy(rotation);
                    Main.spriteBatch.Draw(texture, drawPos + sinusoid2 - Main.screenPosition, null, otherC * ((255 - Projectile.alpha) / 255f) * alphaMult * 0.6f, rotation, origin, new Vector2(3f, actualScale * 1f * sinusoidScaleMult) * Projectile.scale, SpriteEffects.None, 0f);
                    Vector2 sinusoid3 = new Vector2(0, 12 * otherMult * (float)Math.Sin(MathHelper.ToRadians(Main.GameUpdateCount * 4f + i * 2f + Projectile.ai[1]))).RotatedBy(rotation);
                    Main.spriteBatch.Draw(texture, drawPos + sinusoid3 - Main.screenPosition, null, otherC * ((255 - Projectile.alpha) / 255f) * alphaMult * 0.6f, rotation, origin, new Vector2(3f, actualScale * 0.75f * sinusoidScaleMult) * Projectile.scale, SpriteEffects.None, 0f);
                    if (i != drawPositionList.Count - 1)
                        rotation = (drawPositionList[i + 1] - drawPos).ToRotation();
                }
            }
            return false;
        }
        Vector2 endPosition;
        public void SetupLaser()
        {
            float radians = (float)Projectile.velocity.ToRotation();
            int playerID = (int)Projectile.ai[0];
            Vector2 position = Projectile.Center;
            Vector2 velocity = Projectile.velocity.SafeNormalize(new Vector2(0, 1)) * Speed;
            int counter = 0;
            if (playerID < 0)
                return;
            Player target = Main.player[playerID];
            for(int i = 0; i < 240; i++)
            {
                position += velocity;
                drawPositionList.Add(position);
                velocity = new Vector2(1, 0).RotatedBy(radians) * Speed;
                counter++;
            }
            //Projectile.velocity = velocity;
            endPosition = position;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void AI()
        {
            if (runOnce)
            {
                Projectile.ai[1] = Main.rand.NextFloat(18000);
                SetupLaser();
                runOnce = false;
            }
            counter++;
            if(counter == startUpTime)
            {
                for (int i = 0; i < 12; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>(), 0, 0, 120);
                    dust.velocity += Projectile.velocity * 0.1f;
                    dust.noGravity = true;
                    dust.color = ColorHelper.Pastel(MathHelper.ToRadians(i * 18));
                    dust.noGravity = true;
                    dust.fadeIn = 0.2f;
                    dust.scale *= 2.2f;
                    dust = Dust.NewDustDirect(endPosition - new Vector2(Projectile.width / 2, Projectile.height / 2), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>(), 0, 0, 120);
                    dust.velocity += Projectile.velocity * 0.1f;
                    dust.noGravity = true;
                    dust.color = ColorHelper.Pastel(MathHelper.ToRadians(i * 18));
                    dust.noGravity = true;
                    dust.fadeIn = 0.2f;
                    dust.scale *= 2.2f;
                }
                Player player = Main.LocalPlayer;
                if(player.Distance(Projectile.Center) < 4800)
                    SOTSUtils.PlaySound(SoundID.Item94, (int)player.Center.X, (int)player.Center.Y, 0.75f, 0.2f);
                for (int i = 0; i < drawPositionList.Count; i += 2)
                {
                    if (!Main.rand.NextBool(3))
                    {
                        Dust dust = Dust.NewDustPerfect(drawPositionList[i], ModContent.DustType<CopyDust4>(), Main.rand.NextVector2Circular(3, 3), 120);
                        dust.velocity += Projectile.velocity * 0.1f;
                        dust.noGravity = true;
                        dust.color = ColorHelper.Pastel(Main.rand.NextFloat(0, 6.28f));
                        dust.noGravity = true;
                        dust.fadeIn = 0.2f;
                        dust.scale *= 2.2f;
                    }
                }
            }
            float endPercent = Projectile.timeLeft / 40f;
            if (endPercent > 1)
                endPercent = 1;
            Projectile.alpha = (int)(255 - 255 * endPercent * endPercent);
        }
    }
}