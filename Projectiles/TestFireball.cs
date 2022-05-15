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
            //ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            //ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            //Main.projFrames[Projectile.type] = 6;
        }
        public override void SetDefaults()
        {
            Projectile.damage = 15;
            Projectile.width = 33;
            Projectile.height = 32;
            Projectile.aiStyle = 0;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 270;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
        }
        public override void AI()
        {
            Projectile.ai[0] += 0.01f;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override bool PreDraw(ref Color lightColor) => false;
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
            SOTS.FireballShader.Parameters["counter"].SetValue(Projectile.ai[0]);
            SOTS.FireballShader.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49").Value, (Projectile.Center - Main.screenPosition) / 2, null, Color.Pink * Projectile.Opacity, Projectile.rotation, new Vector2(50, 50), (Projectile.scale * new Vector2(4f, 1) / 2) / 2, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            #endregion
        }
    }
}
	
