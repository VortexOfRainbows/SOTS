using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{    
    public class StarcoreBullet : ModProjectile 
    {
		Color color = Color.White;
		bool end = false;
		int bounceCount = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starcore Bullet");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 35;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Color color2 = new Color(110, 110, 110, 0).MultiplyRGBA(color);
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				color2 = projectile.GetAlpha(color2) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length) * 0.5f;
				for (int j = 0; j < 5; j++)
				{
					float x = Main.rand.Next(-10, 11) * 0.1f;
					float y = Main.rand.Next(-10, 11) * 0.1f;
					if (!projectile.oldPos[k].Equals(projectile.position))
					{
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color2, projectile.rotation, drawOrigin, (projectile.oldPos.Length - k) / (float)projectile.oldPos.Length, SpriteEffects.None, 0f);
					}
				}
			}
			return false;
		}
		public override void SetDefaults()
        {
			projectile.height = 14;
			projectile.width = 14;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 7200;
			projectile.tileCollide = true;
			projectile.ranged = true;
			projectile.extraUpdates = 5;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 24 * (1 + projectile.extraUpdates);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			bounceCount++;
			if (bounceCount > 3)
				UpdateEnd();
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
			bounceCount++;
			if (bounceCount > 3)
				UpdateEnd();
			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = -oldVelocity.X;
			}
			if (projectile.velocity.Y != oldVelocity.Y)
			{
				projectile.velocity.Y = -oldVelocity.Y;
			}
			initialVelo = projectile.velocity;
		
			return false;
        }
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 16; i++)
			{
				int num2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num2];
				Color color2 = new Color(110, 110, 110, 0).MultiplyRGBA(color);
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				dust.alpha = 255 - (int)(255 * (projectile.timeLeft / 40f));
				dust.velocity += projectile.velocity * 0.2f;
			}
		}
		bool runOnce = true;
		Vector2 initialVelo;
		public void UpdateEnd()
		{
			if (bounceCount > 3)
			{
				if (projectile.timeLeft > 40)
					projectile.timeLeft = 40;
				end = true;
				projectile.velocity *= 0;
				projectile.friendly = false;
				projectile.extraUpdates = 1;
			}
		}
        public override bool PreAI()
		{
			if (runOnce)
			{
				initialVelo = projectile.velocity;
				runOnce = false;
				color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
			}
			if (end == true && projectile.timeLeft > 40)
				projectile.timeLeft = 40;
			if ((Main.rand.NextBool(2) && end) || Main.rand.NextBool(24))
			{
				int num2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y) - new Vector2(4, 4), projectile.width, projectile.height, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num2];
				Color color2 = new Color(110, 110, 110, 0).MultiplyRGBA(color);
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				int alpha = 255 - (int)(255 * (projectile.timeLeft / 40f));
				alpha = alpha > 255 ? 255 : alpha;
				alpha = alpha < 0 ? 0 : alpha;
				dust.alpha = alpha;
			}
			if (bounceCount > 3)
			{
				UpdateEnd();
				return false;
			}
			return base.PreAI();
        }
        public override void AI()
		{
			int red = color.R;
			int green = color.G;
			int blue = color.B;
			projectile.ai[0] += 5f;
			if(red > green && red > blue)
            {
				Vector2 circularLocation = new Vector2(6, 0).RotatedBy(MathHelper.ToRadians(projectile.ai[0]));
				projectile.velocity = initialVelo.RotatedBy(MathHelper.ToRadians(circularLocation.X));
			}
			if (green > red && green > blue)
			{
				Vector2 circularLocation = new Vector2(-6, 0).RotatedBy(MathHelper.ToRadians(projectile.ai[0]));
				projectile.velocity = initialVelo.RotatedBy(MathHelper.ToRadians(circularLocation.X));
			}
		}
	}
}
		