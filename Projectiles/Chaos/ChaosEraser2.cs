using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using SOTS.Dusts;
using SOTS.NPCs.Boss.Lux;

namespace SOTS.Projectiles.Chaos
{
    public class ChaosEraser2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Redlight Eraser");
        }
        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.timeLeft = 900;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }
        List<Vector2> drawPositionList = new List<Vector2>();
        float startUpTime = 30;
        int counter = 0;
        bool runOnce = true;
        public const float Speed = 10f;
        public const float SeekOutOthersRange = 96f;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if(projectile.timeLeft < 900 - startUpTime && projectile.timeLeft > 90)
            {
                int width = 64;
                for(int i = 0; i < drawPositionList.Count - 60; i += 3)
                {
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
            float scale = counter / 30f;
            float alphaMult = 1f;
            if (scale < 1)
            {
                alphaMult = 0.1f + 0.15f * scale;
                scale *= 0.5f;
            }
            else
            {
                scale = 1 - 0.3f * (1 - projectile.timeLeft / 900f);
            }
            for (int i = 0; i < max; i++)
            {
                if (!SOTS.Config.lowFidelityMode || i % 2 == 0)
                {
                    float actualScale = scale;
                    if (i > 600)
                    {
                        float mult = (i - 600) / 120f;
                        actualScale *= 1 - mult;
                    }
                    Vector2 drawPos = drawPositionList[i];
                    Color otherC = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 3), new Color(255, 70, 70));
                    otherC.A = 0;
                    Vector2 sinusoid = new Vector2(0, 64 * ((255 - projectile.alpha) / 255f) * actualScale * (float)Math.Sin(MathHelper.ToRadians(Main.GameUpdateCount * 8 + i * 5))).RotatedBy(rotation);
                    float sinusoid2 = (float)Math.Sin(MathHelper.ToRadians(Main.GameUpdateCount * 3 + i * 4));
                    spriteBatch.Draw(texture, drawPos - Main.screenPosition, null, otherC * ((255 - projectile.alpha) / 255f) * alphaMult * 1.1f, rotation, origin, new Vector2(2f, actualScale * 2.5f * (1 + sinusoid2 * 0.3f) * ((255 - projectile.alpha) / 255f)) * projectile.scale, SpriteEffects.None, 0f);
                    spriteBatch.Draw(texture, drawPos + sinusoid - Main.screenPosition, null, otherC * ((255 - projectile.alpha) / 255f) * alphaMult * 1.1f, rotation, origin, new Vector2(2f, actualScale * 1.0f * (1 + sinusoid2 * 0.3f) * ((255 - projectile.alpha) / 255f)) * projectile.scale, SpriteEffects.None, 0f);
                    if (i != drawPositionList.Count - 1)
                        rotation = (drawPositionList[i + 1] - drawPos).ToRotation();
                }
            }
            return false;
        }
        Vector2 endPosition;
        public void SetupLaser()
        {
            float radians = (float)projectile.velocity.ToRotation();
            int parentID = (int)projectile.ai[0];
            Vector2 position = projectile.Center;
            Vector2 velocity = projectile.velocity.SafeNormalize(new Vector2(0, 1)) * Speed;
            if (parentID >= 0)
            {
                NPC npc2 = Main.npc[parentID];
                if (npc2.active && npc2.type == ModContent.NPCType<FakeLux>())
                {
                    radians = npc2.rotation + MathHelper.ToRadians(90);
                    projectile.velocity = velocity = new Vector2(1, 0).RotatedBy(radians);
                    projectile.Center = npc2.Center + velocity * 32 + npc2.velocity;
                }
                else
                {
                    projectile.Kill();
                }
            }
            int counter = 0;
            drawPositionList = new List<Vector2>();
            for(int i = 0; i < 720; i++)
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
                runOnce = false;
            }
            SetupLaser();
            counter++;
            if(counter == startUpTime)
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<CopyDust4>(), 0, 0, 120);
                    dust2.velocity += projectile.velocity * 0.1f;
                    dust2.noGravity = true;
                    dust2.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 18), new Color(255, 100, 100));
                    dust2.noGravity = true;
                    dust2.fadeIn = 0.2f;
                    dust2.scale *= 2.2f;
                    dust2 = Dust.NewDustDirect(endPosition - new Vector2(projectile.width / 2, projectile.height / 2), projectile.width, projectile.height, ModContent.DustType<CopyDust4>(), 0, 0, 120);
                    dust2.velocity += projectile.velocity * 0.1f;
                    dust2.noGravity = true;
                    dust2.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 18), new Color(255, 100, 100));
                    dust2.noGravity = true;
                    dust2.fadeIn = 0.2f;
                    dust2.scale *= 2.2f;
                }
                Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 94, 1.6f, -0.4f);
                for (int i = 0; i < drawPositionList.Count; i += 2)
                {
                    if (Main.rand.NextBool(4))
                    {
                        Dust dust2 = Dust.NewDustPerfect(drawPositionList[i], ModContent.DustType<CopyDust4>(), Main.rand.NextVector2Circular(3, 3), 120);
                        dust2.velocity += projectile.velocity * 0.1f;
                        dust2.noGravity = true;
                        dust2.color = VoidPlayer.pastelAttempt(Main.rand.NextFloat(0, 6.28f), new Color(255, 100, 100));
                        dust2.noGravity = true;
                        dust2.fadeIn = 0.2f;
                        dust2.scale *= 2.2f;
                    }
                }
            }
            for (int i = 0; i < drawPositionList.Count; i += 2)
            {
                if (Main.rand.NextBool(1000))
                {
                    Dust dust2 = Dust.NewDustPerfect(drawPositionList[i], ModContent.DustType<CopyDust4>(), Main.rand.NextVector2Circular(3, 3), 120);
                    dust2.velocity += projectile.velocity * 0.1f;
                    dust2.noGravity = true;
                    dust2.color = VoidPlayer.pastelAttempt(Main.rand.NextFloat(0, 6.28f), new Color(255, 100, 100));
                    dust2.noGravity = true;
                    dust2.fadeIn = 0.2f;
                    dust2.scale *= 2.2f;
                }
            }
            float endPercent = projectile.timeLeft / 120f;
            if (endPercent > 1)
                endPercent = 1;
            projectile.alpha = (int)(255 - 255 * endPercent * endPercent);
        }
    }
}