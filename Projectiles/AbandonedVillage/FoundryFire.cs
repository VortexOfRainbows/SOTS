using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using ReLogic.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using SOTS.Dusts;
using SOTS.Helpers;
using System.IO.Pipelines;
using System;

namespace SOTS.Projectiles.AbandonedVillage
{
    public class FoundryFire : ModProjectile
    {
        private bool runOnce = true;
		private Vector2[] trail => Projectile.oldPos;
        public override string Texture => "SOTS/Projectiles/AbandonedVillage/Meatball";
        public override void SetStaticDefaults()
        {
			ProjectileID.Sets.TrailCacheLength[Type] = 15;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
			Projectile.width = 18;
            Projectile.height = 18;
			Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.timeLeft = 180;
            Projectile.alpha = 0;
		}
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D TrailTexture = SOTSUtils.WhitePixel;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;

			Vector2 drawOrigin = new(0f, TrailTexture.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center;
			for (int i = 0; i < trail.Length; i++)
            {
                if (trail[i] == Vector2.Zero)
                {
                    continue;
                }
                float scale = Projectile.scale * (trail.Length - i) / (float)trail.Length;
				scale *= 1f;
				Color color = Color.Lerp(Color.Red, Color.Gold, scale);
                color.A = 0;
                Vector2 center = Projectile.oldPos[i] + Projectile.Size / 2;
                float perc = 1 - i / (float)Projectile.oldPos.Length;
                Vector2 toPrev = previousPosition - center;
                Main.spriteBatch.Draw(TrailTexture, center - Main.screenPosition, null, color * perc, toPrev.ToRotation(), new Vector2(0, 1), new Vector2(toPrev.Length() / 2f, 4 * perc), SpriteEffects.None, 0f);
                previousPosition = center;
			}
            Color color2 = Color.Lerp(Color.Red, Color.Gold, 0.5f);
            color2.A = 0;
            for(int i = 0; i < 4; i++)
            {
                Vector2 circular = new Vector2(2, 0).RotatedBy(i * MathHelper.PiOver2);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + circular, null, color2 * 0.5f, Projectile.rotation, texture.Size() / 2f, 1.05f, SpriteEffects.None, 0f);
            }
            return true;
        }

        public override void AI()
        {
            if(runOnce && Projectile.ai[0] == -1)
            {
                SOTSUtils.PlaySound(SoundID.Item89, Projectile.Center, 1.1f, -0.6f, 0.1f);
                runOnce = false;
                float r = Projectile.velocity.ToRotation();
                float total = 24f;
                for(float j = 0.5f; j < 1.0f; j += 0.3f)
                {
                    for (int i = 0; i < total; i++)
                    {
                        Color color = Color.Lerp(Color.Red, Color.Gold, Main.rand.NextFloat(1));
                        color.A = 0;
                        Vector2 circular = new Vector2(1, 0).RotatedBy(i / total * MathHelper.TwoPi);
                        circular.X *= 0.5f;
                        circular = circular.RotatedBy(r);
                        Vector2 drawPos = Projectile.Center;
                        Dust dust = Dust.NewDustDirect(drawPos + new Vector2(-5), 0, 0, ModContent.DustType<PixelDust>(), 0, 0, 0, color);
                        dust.noGravity = true;
                        dust.scale = 2f;
                        dust.velocity *= 0.1f;
                        dust.velocity += Projectile.velocity * 0.8f * j + circular * 5f * j;
                        dust.fadeIn = 4;
                        dust.color.A = 0;
                        dust.alpha = 120;
                    }
                }
            }
            else if(Main.rand.NextBool(7))
            {
                Color color = Color.Lerp(Color.Red, Color.Gold, Main.rand.NextFloat(1));
                color.A = 0;
                Dust dust = Dust.NewDustDirect(Projectile.Center + new Vector2(-4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, color);
                dust.noGravity = true;
                dust.scale = 1.5f;
                dust.velocity *= 0.2f;
                dust.velocity -= Projectile.oldVelocity * Main.rand.NextFloat(0.3f);
                dust.fadeIn = 0.1f;
                dust.color.A = 0;
            }
            Projectile.velocity.Y += 0.25f;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}
		public override void OnKill(int timeLeft)
        {
            for (float i = 0; i < Projectile.oldPos.Length; i += 0.5f)
            {
                float scale = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                scale *= 1f;
                Vector2 drawPos = Projectile.oldPos[(int)i] + Projectile.Size / 2 - ((i % 1) * Projectile.oldVelocity);
                Color color = Color.Lerp(Color.Red, Color.Gold, scale);
                color.A = 0;
                Dust dust = Dust.NewDustDirect(drawPos + new Vector2(-4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, color);
                dust.noGravity = true;
                dust.scale = 2.25f * scale;
                dust.velocity *= 0.3f;
                dust.velocity += Projectile.oldVelocity * Main.rand.NextFloat(0.3f);
                dust.fadeIn = 0.1f;
                dust.color.A = 0;
            }
            for (int i = 25; i > 0; i--)
            {
                Color c = Color.Lerp(Color.Red, Color.Gold, Main.rand.NextFloat(1));
                c.A = 0;
                PixelDust.Spawn(Projectile.Center, 0, 0, Projectile.oldVelocity * 0.1f *Main.rand.NextFloat(1) + Main.rand.NextVector2Circular(7, 7), c, Main.rand.Next(4, 7)).scale = Main.rand.Next(4, 9) / 4f;
            }
            for (int i = 25; i > 0; i--)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5, 5), 0, 0, Main.rand.NextBool() ? ModContent.DustType<FamishedDustCorruption>() : ModContent.DustType<FamishedDustCrimson>(), 0, 0, 0);
                dust.velocity = dust.velocity * 1.0f;
                dust.velocity.Y -= Main.rand.NextFloat(3);
                dust.scale *= 1.25f;
            }
            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
			if(Projectile.owner == Main.myPlayer)
			{
                Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FoundryFireBoom>(), Projectile.damage, 0f, Main.myPlayer);
            }
        }
    }
}