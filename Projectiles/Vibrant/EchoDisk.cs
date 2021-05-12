using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Void;
using SOTS.Dusts;

namespace SOTS.Projectiles.Vibrant
{    
    public class EchoDisk : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Echo Disc");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;  
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
		public void DustRing()
		{
			for (int i = 0; i < 360; i += 10)
			{
				Vector2 circularLocation = new Vector2(-9, 0).RotatedBy(MathHelper.ToRadians(i));
				int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[num1];
				dust.color = color;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.75f;
				dust.alpha = 70;
				dust.velocity *= 0.1f;
				dust.velocity += circularLocation * 0.25f;
			}
		}
		public override void Kill(int timeLeft)
		{
			color = new Color(80, 120, 220, 0);
			DustRing();
		}
		Color color = new Color(180, 230, 100, 0);
		public override void AI()
		{
			projectile.rotation += 0.47f;
			if(projectile.ai[0] == 1)
			{
				DustRing();
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
			}
			Lighting.AddLight(projectile.Center, color.R / 255f, color.G / 255f, color.B * 1.75f / 255f);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if(projectile.ai[0] != -1)
				projectile.ai[0] = 1;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			DrawTrail(spriteBatch, lightColor);
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, projectile.height * 0.5f);
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawPos = projectile.Center - Main.screenPosition;
			color = projectile.GetAlpha(color);
			for (int j = 0; j < 5; j++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), new Rectangle(0, projectile.height * projectile.frame, projectile.width, projectile.height), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public void DrawTrail(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle(0, projectile.height * projectile.frame, projectile.width, projectile.height), color * 0.5f, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
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
		