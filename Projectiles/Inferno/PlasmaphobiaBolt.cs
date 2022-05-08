using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SOTS.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Dusts;
using System.IO;

namespace SOTS.Projectiles.Inferno
{
    public class PlasmaphobiaBolt : ModProjectile
    {
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(projectile.timeLeft);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            projectile.timeLeft = reader.ReadInt32();
        }
        public const int trailLength = 20;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plasma Bolt");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = trailLength;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            projectile.aiStyle = -1;
            projectile.width = 24;
            projectile.height = 24;
            projectile.scale = 1f;
            projectile.tileCollide = true;
            projectile.penetrate = 5;
            projectile.timeLeft = 80;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 90;
            projectile.extraUpdates = 1;
        }
        public void Collide()
        {
            if (projectile.timeLeft > trailLength)
            {
                if (projectile.owner == Main.myPlayer)
                    Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<PlasmaStar>(), projectile.damage, projectile.knockBack, projectile.owner);
                projectile.timeLeft = trailLength;
                projectile.netUpdate = true;
                for(int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
                    dust.velocity *= 1f;
                    dust.velocity += projectile.velocity * 0.2f;
                    dust.noGravity = true;
                    dust.color = new Color(157, 93, 213, 40);
                    dust.fadeIn = 0.1f;
                    dust.scale *= 1.75f;
                    dust.alpha = 100;
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collide();
            projectile.velocity = oldVelocity;
            return false;
        }
        bool runOnce = true;
        public override void AI()
        {
            projectile.ai[0] += 0.01f;
            if (projectile.timeLeft == trailLength + 1)
            {
                Collide();
            }
            if (projectile.timeLeft <= trailLength)
            {
                projectile.tileCollide = false;
                projectile.friendly = false;
                projectile.velocity *= 0f;
                return;
            }
            Player player = Main.player[projectile.owner];
            if(runOnce)
            {
                projectile.scale = 0f;
                runOnce = false;
                for (float j = 0.25f; j <= 0.75f; j += 0.25f)
                {
                    for (int i = 0; i < 360; i += 10)
                    {
                        Vector2 circular = new Vector2(2 + 4 * j, 0).RotatedBy(MathHelper.ToRadians(i));
                        circular.X *= 0.55f;
                        circular = circular.RotatedBy(projectile.velocity.ToRotation());
                        Vector2 fromCenter = projectile.velocity.SafeNormalize(Vector2.Zero) * 32 * j;
                        Dust dust = Dust.NewDustDirect(projectile.Center + projectile.velocity * j + circular - new Vector2(5) + fromCenter, 0, 0, ModContent.DustType<CopyDust4>());
                        dust.velocity = 0.5f * circular + fromCenter;
                        dust.noGravity = true;
                        dust.color = new Color(157, 93, 213, 40);
                        dust.fadeIn = 0.1f;
                        dust.scale *= 1.1f;
                        dust.alpha = 100;
                    }
                }
            }
            if (projectile.scale < 1)
            {
                projectile.scale += 0.05f;
                projectile.scale *= 1.1f;
                projectile.velocity *= 1.08f;
            }
            else
            {
                projectile.scale = 1;
            }
            projectile.rotation = projectile.velocity.ToRotation();
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (projectile.penetrate > 2)
                Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<PlasmaStar>(), projectile.damage, 0, projectile.owner);
            else
                Collide();
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (runOnce)
                return false;
            Color color = Color.White;
            color.A = 0;
            DrawTrail(spriteBatch);
            if(projectile.timeLeft > trailLength)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                Vector4 colorMod = color.ToVector4();
                SOTS.FireballShader.Parameters["colorMod"].SetValue(colorMod);
                SOTS.FireballShader.Parameters["noise"].SetValue(Mod.Assets.Request<Texture2D>("TrailTextures/vnoise").Value);
                SOTS.FireballShader.Parameters["pallette"].SetValue(Mod.Assets.Request<Texture2D>("TrailTextures/Pallette2").Value);
                SOTS.FireballShader.Parameters["opacity2"].SetValue(0.25f);
                SOTS.FireballShader.Parameters["counter"].SetValue(projectile.ai[0]);
                SOTS.FireballShader.CurrentTechnique.Passes[0].Apply();
                Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49").Value, projectile.Center - Main.screenPosition, null, new Color(157, 93, 213, 40) * projectile.Opacity, projectile.rotation, new Vector2(50, 50), projectile.scale * 0.5f * new Vector2(2f, 0.3f), SpriteEffects.None, 0f);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            }
            return false;
        }
        public void DrawTrail(SpriteBatch spriteBatch)
        {
            Color color = new Color(157, 93, 213, 40) * 0.8f;
            Vector2 drawOrigin = new Vector2(50, 50);
            Vector2 original = projectile.position;
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                if(projectile.oldPos[k] != projectile.position)
                {
                    float scale = (projectile.oldPos.Length - k) / (float)projectile.oldPos.Length;
                    Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + projectile.Size / 2 + new Vector2(0f, projectile.gfxOffY);
                    float direction = (original - projectile.oldPos[k]).ToRotation();
                    spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49").Value, drawPos, null, color * scale, direction, drawOrigin, (0.1f + 0.3f * scale) * new Vector2(2f, 0.3f), SpriteEffects.None, 0f);
                    original = projectile.oldPos[k];
                }
            }
        }
    }
    public class PlasmaStar : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plasma Star");
            //ProjectileID.Sets.TrailCacheLength[projectile.type] = 30;
            //ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            //Main.projFrames[projectile.type] = 6;
        }
        public override void SetDefaults()
        {
            projectile.width = 80;
            projectile.height = 80;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 60;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.scale = 1f;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 90;
        }
        public void GenDust(float mult = 1f)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
                dust.velocity *= mult;
                dust.noGravity = true;
                dust.color = new Color(157, 93, 213, 40);
                dust.fadeIn = 0.1f;
                dust.scale *= 1.35f;
                dust.alpha = 100;
            }
        }
        public override void Kill(int timeLeft)
        {
            GenDust();
        }
        bool runOnce = true;
        public override void AI()
        {
            if(runOnce)
            {
                GenDust(2f);
                runOnce = false;
            }
            projectile.rotation += 0.03f;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49").Value;
            float sin = (float)Math.Sin(MathHelper.ToRadians(180 - projectile.timeLeft * 3));
            sin = (float)Math.Pow(sin, 0.9);
            Color color = new Color(157, 93, 213);
            color.A = 0; 
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            SOTS.GodrayShader.Parameters["distance"].SetValue(6 * sin);
            SOTS.GodrayShader.Parameters["colorMod"].SetValue(color.ToVector4());
            SOTS.GodrayShader.Parameters["noise"].SetValue(Mod.Assets.Request<Texture2D>("TrailTextures/noise").Value);
            SOTS.GodrayShader.Parameters["rotation"].SetValue(projectile.rotation + projectile.whoAmI * 1.1f);
            SOTS.GodrayShader.Parameters["opacity2"].SetValue(1f * sin);
            SOTS.GodrayShader.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), projectile.scale * sin, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            for(int i = 0; i < 2; i++)
              spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, i == 1 ? new Color(255, 255, 255, 0) : color * 2, projectile.rotation, new Vector2(texture.Width/2, texture.Height/2), projectile.scale / 2f * sin, SpriteEffects.None, 0f);
            return false;
        }
    }
}