using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Helpers;
using SOTS.Void;
using System;

namespace SOTS.Projectiles.AbandonedVillage
{
    public class BridgeburnerFlame : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_85"; //Flamethrower projectile sprite
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Main.projFrames[Type] = 7;
        }
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 1800;
			Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.aiStyle = 0;
            Projectile.alpha = 255;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 32;
            height = 32;
            return true;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            VoidPlayer.VoidBurn(Mod, target, 10, 420);
        }
        private bool RunOnce = true;
        public override bool PreAI()
        {
            if(RunOnce)
            {
                RunOnce = false;
                Projectile.velocity *= 0.5f;
                float r = Projectile.velocity.ToRotation();
                float total = 30f;
                for (int i = 0; i < total; i++)
                {
                    Vector2 circular = new Vector2(1, 0).RotatedBy(i / total * MathHelper.TwoPi);
                    circular.X *= 0.5f;
                    circular = circular.RotatedBy(r);
                    Vector2 drawPos = Projectile.Center;
                    Dust dust = Dust.NewDustDirect(drawPos + new Vector2(-5), 0, 0, ModContent.DustType<PixelDust>(), 0, 0, 0, ColorHelper.RedEvilColor);
                    dust.noGravity = true;
                    dust.scale = 1.5f;
                    dust.velocity *= 0.05f;
                    dust.velocity += Projectile.velocity * 1.0f + circular * 5f;
                    dust.fadeIn = 6f;
                    dust.color.A = 0;
                    dust.alpha = 120;
                }
            }
            return base.PreAI();
        }
        public override void AI()       
        {
            Projectile.rotation += MathHelper.ToRadians(8) * Projectile.direction;
            Projectile.velocity.Y += 0.01f;
            Projectile.ai[0]++;
            if (Projectile.ai[2] < 5 && Projectile.ai[0] > 12)
            {
                Projectile.ai[0] = 0;
                Projectile.ai[2]++;
            }
            if (Projectile.ai[2] >= 5)
            {
                Projectile.velocity *= 0.9f;
                Projectile.alpha += 10;
                if (Projectile.alpha > 255)
                    Projectile.Kill();
                if (Projectile.alpha > 45)
                    Projectile.hostile = false;
            }
            else
            {
                Projectile.alpha -= 10;
                if (Projectile.alpha < 0)
                    Projectile.alpha = 0;
            }
            int size = 54;
            if (Main.rand.NextBool(3))
            {
                int i = Main.rand.Next(Projectile.oldPos.Length);
                float scale = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2;
                Dust dust = Dust.NewDustDirect(drawPos - new Vector2(5 + size), 2 * size, 2 * size, ModContent.DustType<CopyDust4>(), 0, 0, 0, ColorHelper.RedEvilColor * scale);
                dust.noGravity = true;
                dust.scale = 1.5f * scale;
                dust.velocity *= Main.rand.NextFloat(0.2f);
                dust.velocity += Projectile.oldVelocity * Main.rand.NextFloat(0.3f);
                dust.fadeIn = 0.1f;
                dust.color.A = 0;
                dust.alpha = Projectile.alpha;
            }
            else if(Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5 + size), 2 * size, 2 * size, ModContent.DustType<PixelDust>(), 0, 0, 0, ColorHelper.RedEvilColor, 1f);
                dust.noGravity = true;
                dust.velocity = dust.velocity * Main.rand.NextFloat(0.2f) + Projectile.velocity * Main.rand.NextFloat(.3f);
                dust.fadeIn = 7;
                dust.scale = Main.rand.Next(4, 7) / 4f;
                dust.color.A = 0;
                dust.alpha = Projectile.alpha;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            Color color = Projectile.GetAlpha(ColorHelper.RedEvilColor);
            color.A = 0;
            SpriteEffects effects = Projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f / Main.projFrames[Type]);
            Rectangle rect = new Rectangle(0, (int)Projectile.ai[2] * texture.Height / Main.projFrames[Type], texture.Width, texture.Height / Main.projFrames[Type]);
            int trailLen = Projectile.oldPos.Length;
            Vector2 oldPos = Projectile.Center;
            float scale2 = 1.1f - 0.5f * MathF.Sin(Projectile.alpha / 255f * MathF.PI);
            for (int i = 0; i < trailLen; i++)
            {
                if (Projectile.oldPos[i] != Vector2.Zero)
                {
                    float percent =  1 - (float)i / trailLen;
                    float scale = Projectile.scale * (trailLen - i) / Projectile.oldPos.Length * 1f;
                    Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2;
                    Main.EntitySpriteDraw(texture, drawPos - Main.screenPosition, rect, color * scale * 0.25f * percent, Projectile.rotation, drawOrigin, scale2, SpriteEffects.None, 0f);
                }
            }
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, rect, Projectile.GetAlpha(Color.Red), Projectile.rotation, drawOrigin, scale2, effects, 0);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, rect, Projectile.GetAlpha(new Color(255, 170, 40, 80)), Projectile.rotation, drawOrigin, 0.75f * scale2, effects, 0);
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity *= 0.9f;
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 12; i > 0; i--)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(44, 44), 78, 78, ModContent.DustType<PixelDust>(), 0, 0, 0, ColorHelper.RedEvilColor * 0.25f, 1f);
                dust.noGravity = true;
                dust.velocity = dust.velocity * Main.rand.NextFloat(0.75f) + Projectile.velocity * Main.rand.NextFloat(1f);
                dust.fadeIn = 8;
                dust.scale = Main.rand.Next(4, 9) / 4f;
                dust.color.A = 0;
            }
        }
    }
}