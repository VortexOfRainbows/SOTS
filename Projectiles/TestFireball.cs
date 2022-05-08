using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Utilities;

namespace SOTS.Projectiles 
{    
    public class TestFireball : ModProjectile, IPixellated 
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Accursed Bolt");
            //ProjectileID.Sets.TrailCacheLength[projectile.type] = 30;
            //ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            //Main.projFrames[projectile.type] = 6;
        }
        public override void SetDefaults()
        {
            projectile.damage = 15;
            projectile.width = 33;
            projectile.height = 32;
            projectile.aiStyle = 0;
            projectile.scale = 1f;
            projectile.tileCollide = true;
            projectile.extraUpdates = 1;
            projectile.penetrate = 2;
            projectile.timeLeft = 270;
            projectile.friendly = true;
            projectile.melee = true;
        }
        public override void AI()
        {
            projectile.ai[0] += 0.01f;
            projectile.rotation = projectile.velocity.ToRotation();
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) => false;
        public void Draw(SpriteBatch spriteBatch, Color drawColor)
        {
            #region shader
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            Color color = Color.White;
            color.A = 0;
            Vector4 colorMod = color.ToVector4();
            SOTS.FireballShader.Parameters["colorMod"].SetValue(colorMod);
            SOTS.FireballShader.Parameters["noise"].SetValue(Mod.Assets.Request<Texture2D>("TrailTextures/vnoise").Value);
            SOTS.FireballShader.Parameters["pallette"].SetValue(Mod.Assets.Request<Texture2D>("TrailTextures/Pallette1").Value);
            SOTS.FireballShader.Parameters["opacity2"].SetValue(0.25f);
            SOTS.FireballShader.Parameters["counter"].SetValue(projectile.ai[0]);
            SOTS.FireballShader.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49").Value, (projectile.Center - Main.screenPosition) / 2, null, Color.Pink * projectile.Opacity, projectile.rotation, new Vector2(50, 50), (projectile.scale * new Vector2(4f, 1) / 2) / 2, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            #endregion
        }
    }
}
	
