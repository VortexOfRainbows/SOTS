using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Void;
using System;
using System.Diagnostics.Contracts;
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
			Projectile.alpha = 35;
			Projectile.timeLeft = 160;
			Projectile.tileCollide = false;
			Projectile.idStaticNPCHitCooldown = 10;
			Projectile.usesIDStaticNPCImmunity = true;
		}
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Texture2D pixel = ModContent.Request<Texture2D>("SOTS/Items/Secrets/WhitePixel").Value;
            Color color = Projectile.GetAlpha(lightColor);
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 previous = Projectile.Center;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                float perc = 1 - i / (float)Projectile.oldPos.Length;
                Vector2 center = Projectile.oldPos[i] + Projectile.Size / 2;
                Vector2 toPrev = previous - center;
                Main.spriteBatch.Draw(pixel, center - Main.screenPosition, null, new Color(179, 143, 97) * perc * 1.5f, toPrev.ToRotation(), new Vector2(0, 1), new Vector2(toPrev.Length() / 2f, 3f * perc), Projectile.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
                previous = center;
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, drawOrigin, 0.5f, SpriteEffects.None, 0f);
            return false;
        }
        private Vector2 originalVelo = Vector2.Zero;
		private int counter = 0;
        public override void AI()
        {
			if(originalVelo == Vector2.Zero)
				originalVelo = Projectile.velocity;
            Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X - 5, Projectile.position.Y - 5), 40, 40, ModContent.DustType<ModSandDust>());
            dust.noGravity = true;
            dust.velocity *= 0.1f;
            Projectile.alpha++;
			Projectile.ai[0] += 6;
			counter++;
            Vector2 circular = new Vector2(0, (1.0f + counter / 15f) * MathF.Sin(MathHelper.ToRadians(Projectile.ai[0]))).RotatedBy(originalVelo.ToRotation());
			Projectile.position += circular;
            Projectile.direction = SOTSUtils.SignNoZero(Projectile.velocity.X);
            Projectile.rotation += Projectile.direction * 0.2f;
		}
		public override void OnKill(int timeLeft)
		{
			for(int i = 0; i < 22; i++)
			{
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X - 5, Projectile.position.Y - 5), 40, 40, ModContent.DustType<ModSandDust>());
				dust.noGravity = true;
				dust.velocity *= 1.2f;
                dust.scale *= 1.2f;
			}
		}
	}
}
		