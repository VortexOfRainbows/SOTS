using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Dusts;
using SOTS.NPCs;
using SOTS.Prim.Trails;
using SOTS.Void;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Inferno
{    
    public class InfernoBolt : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Inferno Bolt");
		}
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20; 
            Projectile.timeLeft = 180;
            Projectile.penetrate = 1; 
            Projectile.friendly = false; 
            Projectile.hostile = true; 
            Projectile.tileCollide = false;    
            Projectile.ignoreWater = true; 
		}
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Color color = new Color(100, 100, 100, 0);
            float scale = Projectile.scale;
            for(int i = 0; i < 5; i++)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + Main.rand.NextVector2Circular(1, 1), null, color, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        bool runOnce = true;
        public override bool PreAI()
        {
            bool other = Projectile.ai[1] == -1 || Projectile.ai[1] == -2;
            if (runOnce)
            {
                if(!other)
                    SOTSUtils.PlaySound(SoundID.Item34, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.9f, 0.5f);
                else if(Projectile.ai[1] == -2)
                    SOTSUtils.PlaySound(SoundID.Item62, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.7f, 0.2f);
                if (Main.netMode != NetmodeID.Server)
                    SOTS.primitives.CreateTrail(new StarTrail(Projectile, ColorHelpers.InfernoColorAttempt(0.4f), ColorHelpers.InfernoColorAttempt(0.4f), 10));
                runOnce = false;
                for (int i = 0; i < 5; i++)
                {
                    int width = (int)(Projectile.width * Projectile.scale * 1.0f);
                    Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(width, width) / 2, width, width, ModContent.DustType<CopyDust4>());
                    dust.velocity = dust.velocity * 0.8f + Projectile.velocity.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1f) * (float)Math.Sqrt(Projectile.velocity.Length());
                    dust.noGravity = true;
                    dust.fadeIn = 0.2f;
                    dust.color = ColorHelpers.InfernoColorAttempt(Main.rand.NextFloat(1));
                    dust.scale *= 1.4f;
                }
            }
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            if(Main.netMode != NetmodeID.Server)
            {
                for(int i = 0; i < 7; i++)
                {
                    int width = (int)(Projectile.width * Projectile.scale * 1.0f);
                    Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(width, width) / 2, width, width, ModContent.DustType<CopyDust4>());
                    dust.velocity = dust.velocity * 0.8f + Projectile.velocity.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1f) * (float)Math.Sqrt(Projectile.velocity.Length());
                    dust.noGravity = true;
                    dust.fadeIn = 0.2f;
                    dust.color = ColorHelpers.InfernoColorAttempt(Main.rand.NextFloat(1));
                    dust.scale *= 1.4f;
                }
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if(Main.rand.NextBool(3))
                target.AddBuff(BuffID.OnFire, 180, false);
            else
                VoidPlayer.VoidBurn(Mod, target, 30, 270);
        }
        public override void AI()
        {
            bool other = Projectile.ai[1] == -1 || Projectile.ai[1] == -2;
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(-0.9f * Projectile.ai[0]));
            Projectile.velocity += Projectile.velocity.SafeNormalize(Vector2.Zero) * 0.07f;
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2 * (other ? -1 : 1);
            if (Main.rand.NextBool(8))
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
                dust.velocity *= 0.5f;
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
                dust.color = ColorHelpers.InfernoColorAttempt(Main.rand.NextFloat(1));
                dust.scale *= 1.2f;
                Projectile.velocity += Projectile.velocity.SafeNormalize(Vector2.Zero) * 0.36f;
            }
        }
	}	
}
			