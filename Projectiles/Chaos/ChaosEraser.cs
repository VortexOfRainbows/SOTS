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
    public class ChaosEraser : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hyperlight Eraser");
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.timeLeft = 60;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        List<Vector2> drawPositionList = new List<Vector2>();
        float startUpTime = 30;
        int counter = 0;
        bool runOnce = true;
        public const float Speed = 10f;
        public const float SeekOutOthersRange = 96f;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if(Projectile.timeLeft < 60 - startUpTime && Projectile.timeLeft > 20)
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
        public override bool PreDraw(ref Color lightColor)
        {
            if (runOnce)
                return false;
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            Color color = new Color(140, 140, 140, 0);
            float rotation = Projectile.velocity.ToRotation();
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
                scale = 1 - 0.3f * (1 - Projectile.timeLeft / 30f);
            }
            for (int i = 0; i < max; i++)
            {
                float actualScale = scale;
                if(i > 600)
                {
                    float mult = (i - 600) / 120f;
                    actualScale *= 1 - mult;
                }
                Vector2 drawPos = drawPositionList[i];
                Color otherC = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 3), false);
                otherC.A = 0;
                Vector2 sinusoid = new Vector2(0, 64 * actualScale * (float)Math.Sin(MathHelper.ToRadians(Main.GameUpdateCount * 3 + i * 3))).RotatedBy(rotation);
                Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition, null, otherC * ((255 - Projectile.alpha) / 255f) * alphaMult * 0.6f, rotation, origin, new Vector2(2f, actualScale * 3.5f) * Projectile.scale, SpriteEffects.None, 0f);
                if (!SOTS.Config.lowFidelityMode)
                    Main.spriteBatch.Draw(texture, drawPos + sinusoid - Main.screenPosition, null, otherC * ((255 - Projectile.alpha) / 255f) * alphaMult * 0.6f, rotation, origin, new Vector2(2f, actualScale * 1.25f) * Projectile.scale, SpriteEffects.None, 0f);
                if (i != drawPositionList.Count - 1)
                    rotation = (drawPositionList[i + 1] - drawPos).ToRotation();
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
            for(int i = 0; i < 720; i++)
            {
                position += velocity;
                drawPositionList.Add(position);
                if(i < 480)
                    radians = Redirect(radians, position, target.Center);
                velocity = new Vector2(1, 0).RotatedBy(radians) * Speed;
                counter++;
            }
            //Projectile.velocity = velocity;
            endPosition = position;
        }
        public float Redirect(float radians, Vector2 pos, Vector2 npc)
        {
            Vector2 toNPC = npc - pos;
            float speed = Projectile.ai[1] * 0.5f;
            Vector2 rnVelo = new Vector2(Speed, 0).RotatedBy(radians);
            rnVelo += toNPC.SafeNormalize(Vector2.Zero) * speed;
            float npcRad = rnVelo.ToRotation();
            return npcRad;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void AI()
        {
            if (runOnce)
            {
                SetupLaser();
                runOnce = false;
            }
            counter++;
            if(counter == startUpTime)
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>(), 0, 0, 120);
                    dust2.velocity += Projectile.velocity * 0.1f;
                    dust2.noGravity = true;
                    dust2.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 18));
                    dust2.noGravity = true;
                    dust2.fadeIn = 0.2f;
                    dust2.scale *= 2.2f;
                    dust2 = Dust.NewDustDirect(endPosition - new Vector2(Projectile.width / 2, Projectile.height / 2), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>(), 0, 0, 120);
                    dust2.velocity += Projectile.velocity * 0.1f;
                    dust2.noGravity = true;
                    dust2.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 18));
                    dust2.noGravity = true;
                    dust2.fadeIn = 0.2f;
                    dust2.scale *= 2.2f;
                }
                int playerID = (int)Projectile.ai[0];
                if (playerID >= 0)
                {
                    Player target = Main.player[playerID];
                    if (target.active)
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)target.Center.X, (int)target.Center.Y, 91, 1.1f, -0.4f);
                }
                for (int i = 0; i < drawPositionList.Count; i += 2)
                {
                    if (Main.rand.NextBool(5))
                    {
                        Dust dust2 = Dust.NewDustPerfect(drawPositionList[i], ModContent.DustType<CopyDust4>(), Main.rand.NextVector2Circular(3, 3), 120);
                        dust2.velocity += Projectile.velocity * 0.1f;
                        dust2.noGravity = true;
                        dust2.color = VoidPlayer.pastelAttempt(Main.rand.NextFloat(0, 6.28f));
                        dust2.noGravity = true;
                        dust2.fadeIn = 0.2f;
                        dust2.scale *= 2.2f;
                    }
                }
            }
            float endPercent = Projectile.timeLeft / 30f;
            if (endPercent > 1)
                endPercent = 1;
            Projectile.alpha = (int)(255 - 255 * endPercent * endPercent);
        }
    }
}