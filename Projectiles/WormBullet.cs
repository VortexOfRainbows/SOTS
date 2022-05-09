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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color = new Color(110, 110, 110, 0);
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				color = Projectile.GetAlpha(color) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * 0.5f;
				for (int j = 0; j < 5; j++)
				{
					float x = Main.rand.Next(-10, 11) * 0.1f;
					float y = Main.rand.Next(-10, 11) * 0.1f;
					if (!Projectile.oldPos[k].Equals(Projectile.position))
					{
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, Projectile.rotation, drawOrigin, (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length, SpriteEffects.None, 0f);
					}
				}
			}
			return false;
		}
		public override void SetDefaults()
        {
			Projectile.height = 18;
			Projectile.width = 18;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 7200;
			Projectile.tileCollide = true;
			Projectile.hostile = false;
			Projectile.magic = true;
			Projectile.extraUpdates = 1;
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			bounceCount++;
			if(bounceCount > 5)
			{
				if(Projectile.timeLeft > 60)
					Projectile.timeLeft = 60;
				end = true;
				Projectile.velocity *= 0;
			}
			else
			{
				if (oldVelocity.Y > 0)
					direction = 0.03f;
				if (Projectile.velocity.X != oldVelocity.X)
				{
					Projectile.velocity.X = -oldVelocity.X;
				}
				if (Projectile.velocity.Y != oldVelocity.Y)
				{
					Projectile.velocity.Y = -oldVelocity.Y;
				}
			}
			return false;
        }
        public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 30; i++)
			{
				int num2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num2];
				dust.color = new Color(250, 100, 190, 40);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				dust.alpha = 255 - (int)(255 * (Projectile.timeLeft / 60f));
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
			if (end)
				Projectile.friendly = false;
			if (runOnce)
			{
				if(Projectile.velocity.Y < 0)
					direction = -0.03f;
				runOnce = false;
			}
			if(end || Main.rand.NextBool(10))
            {
				int num2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num2];
				dust.color = new Color(250, 100, 190, 40);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				int alpha = 255 - (int)(255 * (Projectile.timeLeft / 60f));
				alpha = alpha > 255 ? 255 : alpha;
				alpha = alpha < 0 ? 0 : alpha;
				dust.alpha = alpha;
				return;
            }
			if(direction > 0)
				Projectile.ai[0] += Projectile.velocity.X * 2;
			else
				Projectile.ai[0] -= Projectile.velocity.X * 2;
			Projectile.velocity += new Vector2(0, direction).RotatedBy(MathHelper.ToRadians(Projectile.ai[0]));
		}
	}
}
		