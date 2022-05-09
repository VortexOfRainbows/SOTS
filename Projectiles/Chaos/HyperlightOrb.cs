using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Laser;
using SOTS.Void;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Chaos
{
    public class HyperlightOrb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hyperlight Orb");
        }
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = false;
            Projectile.ranged = true;
            Projectile.alpha = 0;
            Projectile.timeLeft = 20;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.light = 0.8f;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Laser/PrismLaser").Value;
            Texture2D texture2 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            float compression = (100f - Projectile.ai[0]) / 100f;
            if (compression < 0)
                compression = 0;
            Vector2 drawPos;
            Color color;
            int i = 0;
            float counter = 160 + Main.GlobalTime;
            float scale = Projectile.scale * 1f - 0.5f * compression;
            int offsetDegrees = 35;
            int amt = 4 - (int)(Projectile.ai[1] + 9) / 10;
            for (int d = -amt; d <= amt; d++)
            {
                if(d != 0)
                {
                    drawPos = Projectile.Center;
                    color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 45 - SOTSWorld.GlobalCounter));
                    color *= ((100f - Projectile.alpha) / 255f);
                    color.A = 0;
                    Vector2 dynamicAddition = new Vector2(Main.rand.NextFloat(2, 4) * (1 - 1f * compression), 0).RotatedBy(MathHelper.ToRadians(45f * i) + counter);
                    i++;
                    Vector2 angle = Projectile.velocity.RotatedBy(MathHelper.ToRadians(MathHelper.Clamp(Math.Abs(d * offsetDegrees) - 4 * offsetDegrees * (1 - compression), 0, 4 * offsetDegrees) * Math.Sign(d)));
                    float rotation = angle.ToRotation();
                    for (int a = 0; a < 200; a++)
                    {
                        drawPos += new Vector2(texture.Height * scale, 0).RotatedBy(angle.ToRotation());
                        int k = (int)drawPos.X / 16;
                        int j = (int)drawPos.Y / 16;
                        spriteBatch.Draw(texture, drawPos - Main.screenPosition + dynamicAddition, null, color, rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
                        if (SOTSWorldgenHelper.TrueTileSolid(k, j))
                        {
                            break;
                        }
                    }
                }
            }
            float distance = 32 * ((100f - Projectile.ai[0]) / 100f);
            if (distance < 0) distance = 0;

            for (i = 0; i < 8; i++)
            {
                drawPos = Projectile.Center;
                color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 45 - SOTSWorld.GlobalCounter));
                color *= ((100f - Projectile.alpha) / 255f);
                color.A = 0;
                Vector2 dynamicAddition = new Vector2(2, 0).RotatedBy(MathHelper.ToRadians(45 * i) + counter);
                float angle = Projectile.rotation + MathHelper.ToRadians(Projectile.ai[0] * 5);
                drawPos += new Vector2(distance, 0).RotatedBy(angle + MathHelper.ToRadians(45* i));
                spriteBatch.Draw(texture2, drawPos - Main.screenPosition + dynamicAddition, null, color, Projectile.rotation, new Vector2(texture2.Width / 2, texture2.Height / 2), Projectile.scale, SpriteEffects.None, 0f);
            }
            // Vector2 drawOrigin = new Vector2(texture2.Width / 2, texture2.Height / 2);
            // drawPos = Projectile.Center - Main.screenPosition;
            // color = Color.White;
            // spriteBatch.Draw(texture2, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public const float distOffset = 56;
        int ai2 = 0;
        bool ended = false;
        public override void AI()
        {
            Projectile.spriteDirection = Projectile.direction;
            Projectile.ai[0] += 0.35f;
            if (Projectile.ai[0] < 100)
            {
                Projectile.ai[0] *= 1.035f;
            }
            else
            {
                Projectile.ai[0] = 100;
                ai2++;
            }
            Player player = Main.player[Projectile.owner];
            if (Projectile.owner == Main.myPlayer)
            {
                Vector2 toCursor = Main.MouseWorld - player.Center;
                Vector2 fromPlayer = new Vector2(distOffset, 0).RotatedBy(toCursor.ToRotation());
                Projectile.Center = fromPlayer + player.MountedCenter;
                Projectile.position += new Vector2(0, 2 * Projectile.spriteDirection).RotatedBy(toCursor.ToRotation());
                Projectile.velocity = toCursor.SafeNormalize(Vector2.Zero) * Projectile.velocity.Length();
                Projectile.netUpdate = true;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            if (!player.channel && Projectile.ai[0] >= 100 && ai2 >= 5) ended = true;
            if (!ended && Projectile.timeLeft < 33)
                Projectile.timeLeft = 33;
            if (ended)
            {
                if((int)Projectile.ai[1] % 10 == 0)
                {
                    int num = (int)Projectile.ai[1] / 10;
                    if (Main.myPlayer == Projectile.owner)
                        Projectile.NewProjectile(Projectile.Center, Projectile.velocity, ModContent.ProjectileType<HyperlightLaser>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, num);
                }
                else if(Projectile.ai[1] >= 33)
                {
                    Projectile.Kill();
                }
                Projectile.ai[1]++;
            }
        }
    }
}