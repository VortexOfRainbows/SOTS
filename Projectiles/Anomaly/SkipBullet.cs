using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using SOTS.Dusts;

namespace SOTS.Projectiles.Anomaly
{    
    public class SkipBullet : ModProjectile
	{
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Anomaly/SkipBullet");
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, Color.White, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return true;
		}
		public override void SetDefaults()
        {
			Projectile.CloneDefaults(ProjectileID.Bullet);
			Projectile.DamageType = ModContent.GetInstance<Void.VoidRanged>();
			Projectile.light *= 0.25f;
			Projectile.aiStyle = -1;
			Projectile.penetrate = 1;
			Projectile.width = 16;
			Projectile.height = 16;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 8;
			height = 8;
            return true;
        }
        public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(5, 5), 0, 0, ModContent.DustType<PixelDust>(), 0, 0);
				d.velocity *= 0.2f;
				d.velocity += Projectile.velocity * 0.2f;
				d.fadeIn = 8f;
				d.noGravity = true;
				d.scale *= 1f;
				d.color = ColorHelpers.VoidAnomaly;
				d.color.A = 0;
			}
			base.Kill(timeLeft);
        }
        public override void AI()
		{
			Projectile.spriteDirection = Projectile.direction;
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(5, 5), 0, 0, ModContent.DustType<PixelDust>(), 0, 0);
			d.velocity *= 0.1f;
			d.fadeIn = 8f;
			d.noGravity = true;
			d.scale *= 1f;
			d.color = ColorHelpers.VoidAnomaly;
			d.color.A = 0;
		}
	}
}
		
			