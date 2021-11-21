using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{
    public class ThundershockShortbow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Thundershock Crossbow");
        }
        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 62;
            projectile.aiStyle = 20;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ranged = true;
            projectile.timeLeft = 120;
            projectile.hide = true;
            projectile.alpha = 255;
            projectile.extraUpdates = 1;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Otherworld/ThundershockShortbowGlow");
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = projectile.Center - Main.screenPosition;
            Color color = Color.White;
            spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, drawColor, projectile.rotation, drawOrigin, projectile.scale, projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            DrawBall(spriteBatch);
            return false;
        }
        public void DrawBall(SpriteBatch spriteBatch)
        {
            if (projectile.ai[0] != 0 && counter > 0)
            {
                float percent = counter / projectile.ai[0];
                if(counter > projectile.ai[0] - 6)
                {
                    percent *= 0.2f + 0.8f * (projectile.ai[0] - counter) / 6f;
                }    
                if(percent > 0)
                {
                    Vector2 fireFrom = projectile.Center + projectile.velocity.SafeNormalize(Vector2.Zero) * 28;
                    Texture2D texture = mod.GetTexture("Effects/Masks/Extra_49");
                    Color color = new Color(120, 200, 255);
                    color.A = 0;
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                    SOTS.GodrayShader.Parameters["distance"].SetValue(8 * percent);
                    SOTS.GodrayShader.Parameters["colorMod"].SetValue(color.ToVector4());
                    SOTS.GodrayShader.Parameters["noise"].SetValue(mod.GetTexture("TrailTextures/noise"));
                    SOTS.GodrayShader.Parameters["rotation"].SetValue(MathHelper.ToRadians(percent * 360));
                    SOTS.GodrayShader.Parameters["opacity2"].SetValue(1f * percent);
                    SOTS.GodrayShader.CurrentTechnique.Passes[0].Apply();
                    Main.spriteBatch.Draw(texture, fireFrom - Main.screenPosition, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), percent * 0.6f, SpriteEffects.None, 0f);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
                    for (int i = 0; i < 2; i++)
                        spriteBatch.Draw(texture, fireFrom - Main.screenPosition, null, i == 1 ? new Color(255, 255, 255, 0) : color * 2, 0f, new Vector2(texture.Width / 2, texture.Height / 2), percent * 0.3f, SpriteEffects.None, 0f);
                }
            }
        }
        float counter = 0;
        bool ended = false;
        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            counter += 0.5f;
            if (!Main.player[projectile.owner].channel && counter <= 0)
                ended = true;
            if (!ended)
                projectile.timeLeft = 2;
            Vector2 vector2_1 = Main.player[projectile.owner].RotatedRelativePoint(Main.player[projectile.owner].MountedCenter, true);
            if (Main.myPlayer == projectile.owner)
            {
                Item selectedItem = player.inventory[Main.player[projectile.owner].selectedItem];
                float num1 = selectedItem.shootSpeed * projectile.scale;
                Vector2 vector2_2 = vector2_1;
                float num2 = (float)((double)Main.mouseX + Main.screenPosition.X - vector2_2.X);
                float num3 = (float)((double)Main.mouseY + Main.screenPosition.Y - vector2_2.Y);
                if ((double)Main.player[projectile.owner].gravDir == -1.0)
                    num3 = (float)((double)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector2_2.Y);
                float num4 = (float)Math.Sqrt(num2 * (double)num2 + num3 * (double)num3);
                float num5 = (float)Math.Sqrt(num2 * (double)num2 + num3 * (double)num3);
                float num6 = num1 / num5;
                float num7 = num2 * num6;
                float num8 = num3 * num6;

                if ((double)num7 != projectile.velocity.X || (double)num8 != projectile.velocity.Y || projectile.ai[0] != selectedItem.useTime)
                    projectile.netUpdate = true;
                projectile.velocity.X = num7;
                projectile.velocity.Y = num8;
                projectile.ai[0] = selectedItem.useTime;
            }
            if (projectile.ai[0] != 0 && counter > 0)
            {
                Vector2 fireFrom = projectile.Center + projectile.velocity.SafeNormalize(Vector2.Zero) * 28;
                if (counter == (int)projectile.ai[0] / 3)
                    Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 15, 1.1f, 0.1f);
                float percent = counter / projectile.ai[0];
                for (int k = -1; k <= 1; k += 2)
                {
                    Vector2 circularLocation = new Vector2(0, k * (24 * (1 - percent) + 24)).RotatedBy(MathHelper.ToRadians(480 * percent));
                    circularLocation.Y *= 1.1f;
                    circularLocation.X *= 0.7f;
                    circularLocation = circularLocation.RotatedBy(projectile.velocity.ToRotation());
                    Dust dust = Dust.NewDustDirect(fireFrom + circularLocation + new Vector2(-4, -4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, new Color(120, 200, 255));
                    dust.noGravity = true;
                    dust.scale = 1.5f - 1f * percent;
                    dust.velocity = -circularLocation * 0.1f + player.velocity;
                    dust.fadeIn = 0.1f;
                }
                if (counter >= projectile.ai[0])
                {
                    Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 94, 0.7f, 0.3f);
                    for (int k = 0; k < 60; k++)
                    {
                        Dust dust = Dust.NewDustDirect(fireFrom + new Vector2(-4, -4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, new Color(120, 200, 255));
                        dust.noGravity = true;
                        if (k > 20)
                        {
                            dust.velocity *= Main.rand.NextFloat(0.1f, 1.2f);
                            dust.velocity += projectile.velocity.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat((k - 20) * 0.1f, 12 + k * 0.2f);
                            dust.scale *= 2f;
                        }
                        else
                        {
                            dust.velocity *= Main.rand.NextFloat(0.0f, 0.1f * k);
                            dust.scale *= 2.5f;
                        }
                        dust.fadeIn = 0.1f;
                    }
                    fireFrom = projectile.Center + projectile.velocity.SafeNormalize(Vector2.Zero) * 16;
                    counter = -(int)(projectile.ai[0] * 0.5f);
                    if (Main.myPlayer == projectile.owner)
                    {
                        Projectile.NewProjectile(fireFrom, projectile.velocity, ModContent.ProjectileType<ArcLightning>(), projectile.damage, projectile.knockBack, Main.myPlayer);
                        Projectile.NewProjectile(fireFrom, projectile.velocity, ModContent.ProjectileType<ArcLightning>(), projectile.damage, projectile.knockBack, Main.myPlayer, 1);
                    }
                }
            }
            if (projectile.hide == false)
            {
                player.ChangeDir(projectile.direction);
                player.heldProj = projectile.whoAmI;
                player.itemRotation = (projectile.velocity * projectile.direction).ToRotation();
                if (!ended && player.itemTime < 2)
                {
                    player.itemTime = 2;
                    player.itemAnimation = 2;
                }
                projectile.alpha = 0;
            }
            projectile.hide = false;
            projectile.spriteDirection = projectile.direction;
            projectile.position.X = (vector2_1.X - (projectile.width / 2));
            projectile.position.Y = (vector2_1.Y - (projectile.height / 2));
            projectile.rotation = (float)(Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57000005245209);
            return false;
        }
    }
}