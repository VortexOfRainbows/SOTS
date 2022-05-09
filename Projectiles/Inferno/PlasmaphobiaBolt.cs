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
            writer.Write(Projectile.timeLeft);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.timeLeft = reader.ReadInt32();
        }
        public const int trailLength = 20;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plasma Bolt");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = trailLength;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 80;
            Projectile.friendly = true;
            Projectile.ranged = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 90;
            Projectile.extraUpdates = 1;
        }
        public void Collide()
        {
            if (Projectile.timeLeft > trailLength)
            {
                if (Projectile.owner == Main.myPlayer)
                    Projectile.NewProjectile(Projectile.Center, Vector2.Zero, ModContent.ProjectileType<PlasmaStar>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                Projectile.timeLeft = trailLength;
                Projectile.netUpdate = true;
                for(int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
                    dust.velocity *= 1f;
                    dust.velocity += Projectile.velocity * 0.2f;
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
            Projectile.velocity = oldVelocity;
            return false;
        }
        bool runOnce = true;
        public override void AI()
        {
            Projectile.ai[0] += 0.01f;
            if (Projectile.timeLeft == trailLength + 1)
            {
                Collide();
            }
            if (Projectile.timeLeft <= trailLength)
            {
                Projectile.tileCollide = false;
                Projectile.friendly = false;
                Projectile.velocity *= 0f;
                return;
            }
            Player player = Main.player[Projectile.owner];
            if(runOnce)
            {
                Projectile.scale = 0f;
                runOnce = false;
                for (float j = 0.25f; j <= 0.75f; j += 0.25f)
                {
                    for (int i = 0; i < 360; i += 10)
                    {
                        Vector2 circular = new Vector2(2 + 4 * j, 0).RotatedBy(MathHelper.ToRadians(i));
                        circular.X *= 0.55f;
                        circular = circular.RotatedBy(Projectile.velocity.ToRotation());
                        Vector2 fromCenter = Projectile.velocity.SafeNormalize(Vector2.Zero) * 32 * j;
                        Dust dust = Dust.NewDustDirect(Projectile.Center + Projectile.velocity * j + circular - new Vector2(5) + fromCenter, 0, 0, ModContent.DustType<CopyDust4>());
                        dust.velocity = 0.5f * circular + fromCenter;
                        dust.noGravity = true;
                        dust.color = new Color(157, 93, 213, 40);
                        dust.fadeIn = 0.1f;
                        dust.scale *= 1.1f;
                        dust.alpha = 100;
                    }
                }
            }
            if (Projectile.scale < 1)
            {
                Projectile.scale += 0.05f;
                Projectile.scale *= 1.1f;
                Projectile.velocity *= 1.08f;
            }
            else
            {
                Projectile.scale = 1;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.penetrate > 2)
                Projectile.NewProjectile(Projectile.Center, Vector2.Zero, ModContent.ProjectileType<PlasmaStar>(), Projectile.damage, 0, Projectile.owner);
            else
                Collide();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (runOnce)
                return false;
            Color color = Color.White;
            color.A = 0;
            DrawTrail(spriteBatch);
            if(Projectile.timeLeft > trailLength)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                Vector4 colorMod = color.ToVector4();
                SOTS.FireballShader.Parameters["colorMod"].SetValue(colorMod);
                SOTS.FireballShader.Parameters["noise"].SetValue(Mod.Assets.Request<Texture2D>("TrailTextures/vnoise").Value);
                SOTS.FireballShader.Parameters["pallette"].SetValue(Mod.Assets.Request<Texture2D>("TrailTextures/Pallette2").Value);
                SOTS.FireballShader.Parameters["opacity2"].SetValue(0.25f);
                SOTS.FireballShader.Parameters["counter"].SetValue(Projectile.ai[0]);
                SOTS.FireballShader.CurrentTechnique.Passes[0].Apply();
                Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49").Value, Projectile.Center - Main.screenPosition, null, new Color(157, 93, 213, 40) * Projectile.Opacity, Projectile.rotation, new Vector2(50, 50), Projectile.scale * 0.5f * new Vector2(2f, 0.3f), SpriteEffects.None, 0f);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            }
            return false;
        }
        public void DrawTrail(SpriteBatch spriteBatch)
        {
            Color color = new Color(157, 93, 213, 40) * 0.8f;
            Vector2 drawOrigin = new Vector2(50, 50);
            Vector2 original = Projectile.position;
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                if(Projectile.oldPos[k] != Projectile.position)
                {
                    float scale = (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;
                    Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + Projectile.Size / 2 + new Vector2(0f, Projectile.gfxOffY);
                    float direction = (original - Projectile.oldPos[k]).ToRotation();
                    spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49").Value, drawPos, null, color * scale, direction, drawOrigin, (0.1f + 0.3f * scale) * new Vector2(2f, 0.3f), SpriteEffects.None, 0f);
                    original = Projectile.oldPos[k];
                }
            }
        }
    }
    public class PlasmaStar : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plasma Star");
            //ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            //ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            //Main.projFrames[Projectile.type] = 6;
        }
        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.friendly = true;
            Projectile.ranged = true;
            Projectile.scale = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 90;
        }
        public void GenDust(float mult = 1f)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
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
            Projectile.rotation += 0.03f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49").Value;
            float sin = (float)Math.Sin(MathHelper.ToRadians(180 - Projectile.timeLeft * 3));
            sin = (float)Math.Pow(sin, 0.9);
            Color color = new Color(157, 93, 213);
            color.A = 0; 
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            SOTS.GodrayShader.Parameters["distance"].SetValue(6 * sin);
            SOTS.GodrayShader.Parameters["colorMod"].SetValue(color.ToVector4());
            SOTS.GodrayShader.Parameters["noise"].SetValue(Mod.Assets.Request<Texture2D>("TrailTextures/noise").Value);
            SOTS.GodrayShader.Parameters["rotation"].SetValue(Projectile.rotation + Projectile.whoAmI * 1.1f);
            SOTS.GodrayShader.Parameters["opacity2"].SetValue(1f * sin);
            SOTS.GodrayShader.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale * sin, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            for(int i = 0; i < 2; i++)
              spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, i == 1 ? new Color(255, 255, 255, 0) : color * 2, Projectile.rotation, new Vector2(texture.Width/2, texture.Height/2), Projectile.scale / 2f * sin, SpriteEffects.None, 0f);
            return false;
        }
    }
}