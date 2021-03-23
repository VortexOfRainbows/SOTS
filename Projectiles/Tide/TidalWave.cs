using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Tide
{    
    public class TidalWave : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tidal Wave");
		}
        public override void SetDefaults()
        {
			projectile.penetrate = 1;
			projectile.width = 24;
			projectile.height = 24;
			projectile.timeLeft = 720;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.tileCollide = false;
		}
		bool runOnce = true;
        public override void AI()
		{
			if(runOnce)
			{
				for (int i = 0; i < 4; i++)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 267);
					dust.color = new Color(64, 72, 178);
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.scale *= 2f;
					dust.velocity *= 1.1f;
					dust.velocity += projectile.velocity * 1.1f;
				}
				runOnce = false;
            }
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.15f / 255f, (255 - projectile.alpha) * 0.25f / 255f, (255 - projectile.alpha) * 0.65f / 255f);
			projectile.rotation += 0.04f;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(projectile.Center.Y - (int)Main.screenPosition.Y) + y),
				null, color * (1f - (projectile.alpha / 255f)), projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 20; i++)
			{
				Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 267);
				dust.color = new Color(64, 72, 178);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				dust.velocity *= 1.4f;
				dust.velocity += projectile.velocity * 0.7f;
			}
		}
	}
}
		
			