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
            projectile.width = 24;
            projectile.height = 24;
            projectile.friendly = false;
            projectile.ranged = true;
            projectile.alpha = 0;
            projectile.timeLeft = 20;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.light = 0.8f;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("Projectiles/Laser/PrismLaser");
            Texture2D texture2 = Main.projectileTexture[projectile.type];
            float compression = (100f - projectile.ai[0]) / 100f;
            if (compression < 0)
                compression = 0;
            Vector2 drawPos;
            Color color;
            int i = 0;
            float counter = 160 + Main.GlobalTime;
            float scale = projectile.scale * 1f - 0.5f * compression;
            int offsetDegrees = 35;
            int amt = 4 - (int)(projectile.ai[1] + 9) / 10;
            for (int d = -amt; d <= amt; d++)
            {
                if(d != 0)
                {
                    drawPos = projectile.Center;
                    color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 45 - SOTSWorld.GlobalCounter));
                    color *= ((100f - projectile.alpha) / 255f);
                    color.A = 0;
                    Vector2 dynamicAddition = new Vector2(Main.rand.NextFloat(2, 4) * (1 - 1f * compression), 0).RotatedBy(MathHelper.ToRadians(45f * i) + counter);
                    i++;
                    Vector2 angle = projectile.velocity.RotatedBy(MathHelper.ToRadians(MathHelper.Clamp(Math.Abs(d * offsetDegrees) - 4 * offsetDegrees * (1 - compression), 0, 4 * offsetDegrees) * Math.Sign(d)));
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
            float distance = 32 * ((100f - projectile.ai[0]) / 100f);
            if (distance < 0) distance = 0;

            for (i = 0; i < 8; i++)
            {
                drawPos = projectile.Center;
                color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 45 - SOTSWorld.GlobalCounter));
                color *= ((100f - projectile.alpha) / 255f);
                color.A = 0;
                Vector2 dynamicAddition = new Vector2(2, 0).RotatedBy(MathHelper.ToRadians(45 * i) + counter);
                float angle = projectile.rotation + MathHelper.ToRadians(projectile.ai[0] * 5);
                drawPos += new Vector2(distance, 0).RotatedBy(angle + MathHelper.ToRadians(45* i));
                spriteBatch.Draw(texture2, drawPos - Main.screenPosition + dynamicAddition, null, color, projectile.rotation, new Vector2(texture2.Width / 2, texture2.Height / 2), projectile.scale, SpriteEffects.None, 0f);
            }
            // Vector2 drawOrigin = new Vector2(texture2.Width / 2, texture2.Height / 2);
            // drawPos = projectile.Center - Main.screenPosition;
            // color = Color.White;
            // spriteBatch.Draw(texture2, drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public const float distOffset = 60;
        int ai2 = 0;
        bool ended = false;
        public override void AI()
        {
            projectile.spriteDirection = projectile.direction;
            projectile.ai[0] += 0.35f;
            if (projectile.ai[0] < 100)
            {
                projectile.ai[0] *= 1.035f;
            }
            else
            {
                projectile.ai[0] = 100;
                ai2++;
            }
            Player player = Main.player[projectile.owner];
            if (projectile.owner == Main.myPlayer)
            {
                Vector2 toCursor = Main.MouseWorld - player.Center;
                Vector2 fromPlayer = new Vector2(distOffset, 0).RotatedBy(toCursor.ToRotation());
                projectile.Center = fromPlayer + player.MountedCenter;
                projectile.position += new Vector2(0, 2 * projectile.spriteDirection).RotatedBy(toCursor.ToRotation());
                projectile.velocity = toCursor.SafeNormalize(Vector2.Zero) * projectile.velocity.Length();
                projectile.netUpdate = true;
            }
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            if (!player.channel && projectile.ai[0] >= 100 && ai2 >= 5) ended = true;
            if (!ended && projectile.timeLeft < 33)
                projectile.timeLeft = 33;
            if (ended)
            {
                if((int)projectile.ai[1] % 10 == 0)
                {
                    int num = (int)projectile.ai[1] / 10;
                    if (Main.myPlayer == projectile.owner)
                        Projectile.NewProjectile(projectile.Center, projectile.velocity, ModContent.ProjectileType<HyperlightLaser>(), projectile.damage, projectile.knockBack, Main.myPlayer, num);
                }
                else if(projectile.ai[1] >= 33)
                {
                    projectile.Kill();
                }
                projectile.ai[1]++;
            }
        }
    }
}