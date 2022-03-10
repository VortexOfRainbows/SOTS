using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using SOTS.Dusts;

namespace SOTS.Projectiles.Chaos
{
    public class ChaosHelixLaser : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chaos Helix Laser");
        }
        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.timeLeft = 80;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }
        List<Vector2> drawPositionList = new List<Vector2>();
        float startUpTime = 40;
        int counter = 0;
        bool runOnce = true;
        public const float Speed = 15f;
        public const float SeekOutOthersRange = 96f;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if(projectile.timeLeft < 80 - startUpTime && projectile.timeLeft > 14)
            {
                for(int i = 0; i < drawPositionList.Count - 10; i += 3)
                {
                    float otherMult = i / 30f;
                    if (otherMult > 1)
                        otherMult = 1;
                    int width = (int)(48 * otherMult);
                    Rectangle hitBox = new Rectangle((int)drawPositionList[i].X - width / 2, (int)drawPositionList[i].Y - width / 2, width, width);
                    if (hitBox.Intersects(targetHitbox))
                        return true;
                }
            }
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (runOnce)
                return false;
            Texture2D texture = Main.projectileTexture[projectile.type];
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            Color color = new Color(140, 140, 140, 0);
            float rotation = projectile.velocity.ToRotation();
            int max = drawPositionList.Count;
            float scale = counter / startUpTime;
            float alphaMult = 1f;
            if (scale < 1)
            {
                alphaMult = 0.1f + 0.15f * scale;
                scale *= 0.5f;
            }
            else
            {
                scale = 1 - 0.3f * (1 - projectile.timeLeft / 30f);
            }
            for (int i = 1; i < max; i++)
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
                Color otherC = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 3), false);
                otherC.A = 0;
                float sinusoidScaleMult = 1f + 0.2f * (float)Math.Sin(MathHelper.ToRadians(projectile.ai[1] + i * 3f + Main.GameUpdateCount * -5f));
                Vector2 sinusoid = new Vector2(0, 24 * otherMult * (float)Math.Sin(MathHelper.ToRadians(Main.GameUpdateCount * -6.5f + i * 3f + projectile.ai[1]))).RotatedBy(rotation);
                Vector2 sinusoid2 = new Vector2(0, 18 * otherMult * (float)Math.Sin(MathHelper.ToRadians(Main.GameUpdateCount * -2f + i * 4f + projectile.ai[1]))).RotatedBy(rotation);
                Vector2 sinusoid3 = new Vector2(0, 12 * otherMult * (float)Math.Sin(MathHelper.ToRadians(Main.GameUpdateCount * 4f + i * 2f + projectile.ai[1]))).RotatedBy(rotation);
                spriteBatch.Draw(texture, drawPos + sinusoid - Main.screenPosition, null, otherC * ((255 - projectile.alpha) / 255f) * alphaMult * 0.6f, rotation, origin, new Vector2(3f, actualScale * 1.25f * sinusoidScaleMult) * projectile.scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(texture, drawPos + sinusoid2 - Main.screenPosition, null, otherC * ((255 - projectile.alpha) / 255f) * alphaMult * 0.6f, rotation, origin, new Vector2(3f, actualScale * 1f * sinusoidScaleMult) * projectile.scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(texture, drawPos + sinusoid3 - Main.screenPosition, null, otherC * ((255 - projectile.alpha) / 255f) * alphaMult * 0.6f, rotation, origin, new Vector2(3f, actualScale * 0.75f * sinusoidScaleMult) * projectile.scale, SpriteEffects.None, 0f);
                if (i != drawPositionList.Count - 1)
                    rotation = (drawPositionList[i + 1] - drawPos).ToRotation();
            }
            return false;
        }
        Vector2 endPosition;
        public void SetupLaser()
        {
            float radians = (float)projectile.velocity.ToRotation();
            int playerID = (int)projectile.ai[0];
            Vector2 position = projectile.Center;
            Vector2 velocity = projectile.velocity.SafeNormalize(new Vector2(0, 1)) * Speed;
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
            //projectile.velocity = velocity;
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
                projectile.ai[1] = Main.rand.NextFloat(18000);
                SetupLaser();
                runOnce = false;
            }
            counter++;
            if(counter == startUpTime)
            {
                for (int i = 0; i < 12; i++)
                {
                    Dust dust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<CopyDust4>(), 0, 0, 120);
                    dust2.velocity += projectile.velocity * 0.1f;
                    dust2.noGravity = true;
                    dust2.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 18));
                    dust2.noGravity = true;
                    dust2.fadeIn = 0.2f;
                    dust2.scale *= 2.2f;
                    dust2 = Dust.NewDustDirect(endPosition - new Vector2(projectile.width / 2, projectile.height / 2), projectile.width, projectile.height, ModContent.DustType<CopyDust4>(), 0, 0, 120);
                    dust2.velocity += projectile.velocity * 0.1f;
                    dust2.noGravity = true;
                    dust2.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 18));
                    dust2.noGravity = true;
                    dust2.fadeIn = 0.2f;
                    dust2.scale *= 2.2f;
                }
                Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 94, 0.75f, 0.2f);
                for (int i = 0; i < drawPositionList.Count; i += 2)
                {
                    if (!Main.rand.NextBool(3))
                    {
                        Dust dust2 = Dust.NewDustPerfect(drawPositionList[i], ModContent.DustType<CopyDust4>(), Main.rand.NextVector2Circular(3, 3), 120);
                        dust2.velocity += projectile.velocity * 0.1f;
                        dust2.noGravity = true;
                        dust2.color = VoidPlayer.pastelAttempt(Main.rand.NextFloat(0, 6.28f));
                        dust2.noGravity = true;
                        dust2.fadeIn = 0.2f;
                        dust2.scale *= 2.2f;
                    }
                }
            }
            float endPercent = projectile.timeLeft / 30f;
            if (endPercent > 1)
                endPercent = 1;
            projectile.alpha = (int)(255 - 255 * endPercent * endPercent);
        }
    }
}