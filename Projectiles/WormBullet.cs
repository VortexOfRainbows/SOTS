using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles
{    
    public class WormBullet : ModProjectile 
    {
		bool end = false;
		int bounceCount = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wormwood Bullet");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 50;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Color color = new Color(110, 110, 110, 0);
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				color = projectile.GetAlpha(color) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length) * 0.5f;
				for (int j = 0; j < 5; j++)
				{
					float x = Main.rand.Next(-10, 11) * 0.1f;
					float y = Main.rand.Next(-10, 11) * 0.1f;
					if (!projectile.oldPos[k].Equals(projectile.position))
					{
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, projectile.rotation, drawOrigin, (projectile.oldPos.Length - k) / (float)projectile.oldPos.Length, SpriteEffects.None, 0f);
					}
				}
			}
			return false;
		}
		public override void SetDefaults()
        {
			projectile.height = 18;
			projectile.width = 18;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 7200;
			projectile.tileCollide = true;
			projectile.hostile = false;
			projectile.magic = true;
			projectile.extraUpdates = 1;
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			bounceCount++;
			if(bounceCount > 5)
			{
				if(projectile.timeLeft > 60)
					projectile.timeLeft = 60;
				end = true;
				projectile.velocity *= 0;
			}
			else
			{
				if (oldVelocity.Y > 0)
					direction = 0.03f;
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y;
				}
			}
			return false;
        }
        public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 30; i++)
			{
				int num2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num2];
				dust.color = new Color(250, 100, 190, 40);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				dust.alpha = 255 - (int)(255 * (projectile.timeLeft / 60f));
			}
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
			width = 16;
			height = 16;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
		bool runOnce = true;
		float direction = 0.03f;
        public override void AI()
		{
			if (runOnce)
			{
				if(projectile.velocity.Y < 0)
					direction = -0.03f;
				runOnce = false;
			}
			if(end || Main.rand.NextBool(10))
            {
				int num2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num2];
				dust.color = new Color(250, 100, 190, 40);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				int alpha = 255 - (int)(255 * (projectile.timeLeft / 60f));
				alpha = alpha > 255 ? 255 : alpha;
				alpha = alpha < 0 ? 0 : alpha;
				dust.alpha = alpha;
				return;
            }
			if(direction > 0)
				projectile.ai[0] += projectile.velocity.X * 2;
			else
				projectile.ai[0] -= projectile.velocity.X * 2;
			projectile.velocity += new Vector2(0, direction).RotatedBy(MathHelper.ToRadians(projectile.ai[0]));
		}
	}
}
		