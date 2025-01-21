using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Void;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Blades
{
    public class BlinkLightning : ModProjectile
    {
        public override string Texture => "SOTS/Projectiles/Lightning/PinkLightning";
        private Color FizzleColor => new Color(177, 73, 190, 0);
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 0;
            ProjectileID.Sets.TrailCacheLength[Type] = 40;
        }
        public override void SetDefaults()
        {
            Projectile.penetrate = 3;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<VoidMelee>();
            Projectile.timeLeft = 150;
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.extraUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            float TrailWidth = 12;
            Texture2D pixel = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = new Vector2(pixel.Width / 2f, pixel.Height / 2);
            Vector2 previous = Projectile.Center;
            int cap = Math.Min(Projectile.timeLeft, Projectile.oldPos.Length);
            for (int i = 1; i < cap; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                if (Projectile.oldPos[i] == Projectile.position)
                    continue;
                Vector2 center = Projectile.oldPos[i] + Projectile.Size / 2;
                float inverse = i / (float)Projectile.oldPos.Length;
                float perc = 1 - inverse * inverse;
                Vector2 toPrev = previous - center;
                Color c = FizzleColor * perc;
                float len = toPrev.Length() / pixel.Width * (1 + perc * 3);
                Main.spriteBatch.Draw(pixel, center - Main.screenPosition, null, c, toPrev.ToRotation() - MathHelper.PiOver2, origin, new Vector2(TrailWidth * perc / pixel.Height * 0.5f, len), SpriteEffects.None, 0f);
                previous = center;
            }
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 10;
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.ai[0] = -1;
            Projectile.netUpdate = true;
            return false;
        }
        private bool RunOnce = true;
        public override bool PreAI()
        {
            if (RunOnce)
            {
                for (int i = 0; i < 5; i++)
                {
                    Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<PixelDust>(), newColor: FizzleColor * 0.75f);
                    d.velocity *= 1.2f;
                    d.velocity += Projectile.velocity * 0.4f;
                    d.noGravity = true;
                    d.fadeIn = 8f;
                    d.scale = d.scale * 0.5f + 1.0f;
                }
                RunOnce = false;
            }
            if (Projectile.ai[0] == -1)
            {
                Projectile.velocity *= 0f;
                if (Projectile.timeLeft > 40)
                    Projectile.timeLeft = 40;
            }
            return base.PreAI();
        }
        private int counter = 0;
        public override void AI()
        {
            counter++;
            Color c = FizzleColor;
            if (Main.rand.NextBool(3))
            {
                float veloMult = Projectile.timeLeft > 40f ? 1 : Projectile.timeLeft / 40f;
                Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(5, 5), 0, 0, ModContent.DustType<PixelDust>(), newColor: c);
                d.velocity *= 0.25f + 0.75f * veloMult;
                d.velocity += Projectile.oldVelocity * Main.rand.NextFloat(1, 1 + 2f * veloMult);
                d.noGravity = true;
                d.fadeIn = 13f;
                d.scale = d.scale * 0.5f + .5f;
            }
            if (Projectile.owner == Main.myPlayer && counter % 12 == 0)
            {
                Projectile.ai[1] = Main.rand.NextFloat(4, 24f) * (Main.rand.Next(2) * 2 - 1);
                if (Projectile.velocity.Length() != 0f)
                {
                    Projectile.velocity = new Vector2(Projectile.velocity.Length() + Main.rand.NextFloat(-0.5f, 0.5f), 0).RotatedBy(Projectile.velocity.ToRotation() + MathHelper.ToRadians(Projectile.ai[1]));
                }
                Projectile.netUpdate = true;
            }
            Projectile.velocity *= 0.9875f;
            if (Projectile.timeLeft <= 40)
                Projectile.ai[0] = -1;
        }
        public override void OnKill(int timeLeft)
        {
        }
    }
}