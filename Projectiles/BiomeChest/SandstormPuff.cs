using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Void;
using Steamworks;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.BiomeChest
{    
    public class SandstormPuff : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.height = 40;
			Projectile.width = 40;
			Projectile.DamageType = ModContent.GetInstance<VoidRanged>();
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.alpha = 20;
			Projectile.timeLeft = 100;
			Projectile.tileCollide = true;
			Projectile.idStaticNPCHitCooldown = 10;
			Projectile.usesIDStaticNPCImmunity = true;
            Projectile.extraUpdates = 1;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 12;
            height = 12;
            return true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Texture2D pixel = ModContent.Request<Texture2D>("SOTS/Items/Secrets/WhitePixel").Value;
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 veloOffset = originalVelo.SNormalize() * Sinusoid * -8;
            float scaleMod = 0.8f + 0.2f * Sinusoid;
            float alphaMult = MathF.Sqrt(1 - Projectile.alpha / 255f);
            Vector2 previous = Projectile.Center;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                float perc = 1 - i / (float)Projectile.oldPos.Length;
                Vector2 center = Projectile.oldPos[i] + Projectile.Size / 2;
                Vector2 toPrev = previous - center;
                Main.spriteBatch.Draw(pixel, center - Main.screenPosition + veloOffset, null, Lighting.GetColor((int)center.X / 16, (int)center.Y / 16, ColorHelpers.SandstormPouchColor) * perc * 1.0f * alphaMult, toPrev.ToRotation(), new Vector2(0, 1), new Vector2(toPrev.Length() / 2f, 3.75f * perc * scaleMod), Projectile.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
                previous = center;
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + veloOffset, null, lightColor * alphaMult, Projectile.rotation, drawOrigin, 0.75f * scaleMod, SpriteEffects.None, 0f);
            return false;
        }
        private Vector2 originalVelo = Vector2.Zero;
		private int counter = 0;
        private bool RunOnce = true;
        private float Sinusoid => MathF.Sin(MathHelper.ToRadians(Projectile.ai[0] * Projectile.direction));
        public override void AI()
        {
            if(RunOnce)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i == 0 || Main.rand.NextBool(4))
                    {
                        Dust dust = PixelDust.Spawn(Projectile.Center - Projectile.velocity.SNormalize() * 17f, 0, 0, Projectile.velocity * 0.85f + Main.rand.NextVector2Circular(3.5f, 3.5f), new Color(191, 78, 0, 0), 12);
                        dust.scale = Main.rand.NextFloat(1, 2);
                    }
                    else
                    {
                        Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 15, Projectile.Center.Y - 15), 20, 20, ModContent.DustType<ModSandDust>(), newColor: ColorHelpers.SandstormPouchColor * 0.5f);
                        dust.noGravity = true;
                        dust.velocity = dust.velocity * 0.6f + Projectile.velocity * 0.5f;
                        dust.scale *= 1.1f;
                        dust.alpha = Projectile.alpha;
                    }
                }
                RunOnce = false;
            }
			if(originalVelo == Vector2.Zero)
				originalVelo = Projectile.velocity;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Main.rand.NextBool(12))
                {
                    float perc = 1 - i / (float)Projectile.oldPos.Length;
                    Vector2 center = Projectile.oldPos[i] + Projectile.Size / 2;
                    Dust dust = Dust.NewDustDirect(center + new Vector2(-5, -5), 0, 0, ModContent.DustType<ModSandDust>(), newColor: ColorHelpers.SandstormPouchColor * perc * 0.5f);
                    dust.noGravity = true;
                    dust.velocity = dust.velocity * 0.1f + Projectile.velocity * 0.5f;
                    dust.alpha = (int)(Projectile.alpha + 255 * (1 - perc));
                }
            }
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 5, Projectile.Center.Y - 5), 0, 0, ModContent.DustType<ModSandDust>(), newColor: ColorHelpers.SandstormPouchColor * 0.5f);
                dust.noGravity = true;
                dust.velocity *= 0.2f;
                dust.alpha = Projectile.alpha;
            }
            Projectile.alpha++;
            Projectile.ai[0] += 5.5f;
			counter++;
            Vector2 circular = new Vector2(0, (0.85f + counter / 17f) * Sinusoid).RotatedBy(originalVelo.ToRotation());
			Projectile.position += circular * Projectile.ai[1];
            Projectile.direction = SOTSUtils.SignNoZero(Projectile.velocity.X);
            Projectile.rotation += Projectile.direction * 0.2f;
		}
		public override void OnKill(int timeLeft)
		{
			for(int i = 0; i < 12; i++)
			{
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 15, Projectile.Center.Y - 15), 20, 20, ModContent.DustType<ModSandDust>(), newColor: ColorHelpers.SandstormPouchColor * 0.5f);
				dust.noGravity = true;
                dust.velocity = dust.velocity * 0.6f + Projectile.velocity * 0.75f;
                dust.scale *= 1.3f;
                dust.alpha = Projectile.alpha;
            }
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float perc = 1 - i / (float)Projectile.oldPos.Length;
                Vector2 center = Projectile.oldPos[i] + Projectile.Size / 2;
                Dust dust = Dust.NewDustDirect(center + new Vector2(-5, -5), 0, 0, ModContent.DustType<ModSandDust>(), newColor: ColorHelpers.SandstormPouchColor * perc * 0.75f);
                dust.noGravity = true;
                dust.velocity = dust.velocity * 0.1f + Projectile.velocity * 0.75f;
                dust.scale = perc + 0.5f;
                dust.alpha = (int)(Projectile.alpha + 255 * (1 - perc));
            }
        }
	}
}
		