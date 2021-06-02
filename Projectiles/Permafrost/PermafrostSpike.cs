using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Permafrost
{    
    public class PermafrostSpike : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Permafrost Spike");
		}
        public override void SetDefaults()
        {
			projectile.penetrate = 1;
			projectile.width = 20;
			projectile.height = 20;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.tileCollide = true;
		}
		public override void AI()
		{
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.15f / 255f, (255 - projectile.alpha) * 0.25f / 255f, (255 - projectile.alpha) * 0.65f / 255f);
			projectile.rotation += 0.04f;
			projectile.ai[0]--;
			if(projectile.ai[0] <= 0)
			{
				projectile.position += projectile.velocity;
			}
			if(Main.rand.NextBool(72))
			{
				int dust2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y) - new Vector2(5), projectile.width, projectile.height, 267);
				Dust dust = Main.dust[dust2];
				dust.color = new Color(65, 136, 164);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Color color = new Color(80, 80, 80, 0);
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.3f;
				float y = Main.rand.Next(-10, 11) * 0.3f;
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(projectile.Center.Y - (int)Main.screenPosition.Y) + y),
				null, color * (1f - (projectile.alpha / 255f)), projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		public override void Kill(int timeLeft)
        {
			if(projectile.owner == Main.myPlayer)
			{
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("PermafrostLinger"), projectile.damage, 0, projectile.owner, projectile.rotation);
			}
		}
	}
}
		
			