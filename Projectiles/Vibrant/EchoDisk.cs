using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Vibrant
{    
    public class EchoDisk : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Echo Disk");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 20;  
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
			Main.projFrames[projectile.type] = 2;
		}
        public override void SetDefaults()
        {
			projectile.height = 26;
			projectile.width = 26;
			projectile.penetrate = -1;
			projectile.ranged = true;
			projectile.tileCollide = true;
			projectile.timeLeft = 720;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.extraUpdates = 2;
			projectile.alpha = 0;
		}
		public override void Kill(int timeLeft)
		{
			if (projectile.frame == 1)
			{
				for (int i = 0; i < 360; i += 8)
				{
					Vector2 circularLocation = new Vector2(-10, 0).RotatedBy(MathHelper.ToRadians(i));

					int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, DustID.Lead);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].scale = 1.25f;
					Main.dust[num1].velocity = circularLocation * 0.35f;
				}
			}
			else
			{
				for (int i = 0; i < 360; i += 8)
				{
					Vector2 circularLocation = new Vector2(-10, 0).RotatedBy(MathHelper.ToRadians(i));

					int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 44);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].scale = 1.25f;
					Main.dust[num1].velocity = circularLocation * 0.35f;
					Main.dust[num1].alpha = 200;
				}
			}
		}
		public override void AI()
		{
			projectile.rotation += 0.47f;
			if(projectile.ai[0] == 1)
			{
				for (int i = 0; i < 360; i += 8)
				{
					Vector2 circularLocation = new Vector2(-10, 0).RotatedBy(MathHelper.ToRadians(i));

					int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, DustID.Lead);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].scale = 1.25f;
					Main.dust[num1].velocity = circularLocation * 0.35f;
				}
				projectile.timeLeft = 90;
				projectile.frame = 0;
				projectile.ai[0] = -1;
				projectile.alpha = 255;
				projectile.position -= projectile.velocity * 40;
				projectile.penetrate = 2;
				projectile.tileCollide = false;
				if (Main.myPlayer == projectile.owner)
					projectile.netUpdate = true;
			}
			if (projectile.ai[0] != -1)
			{
				projectile.frame = 1;
			}
			else
			{
				if(projectile.timeLeft > 45)
				{
					projectile.alpha -= 6;
				}
				else
				{
					projectile.alpha += 6;
				}
				if (projectile.timeLeft <= 52)
				{
					projectile.tileCollide = true;
				}
				Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 1.5f / 255f, (255 - projectile.alpha) * 1.75f / 255f, (255 - projectile.alpha) * 0.2f / 255f);
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if(projectile.ai[0] != -1)
				projectile.ai[0] = 1;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle(0, projectile.height * projectile.frame, projectile.width, projectile.height), color * 0.5f, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.ai[0] != -1)
				projectile.ai[0] = 1;
			projectile.velocity = oldVelocity;
			Main.PlaySound(SoundID.Item10, projectile.position);
			return projectile.ai[0] == -1;
		}
	}
}
		