using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Nature
{    
    public class NatureBeat : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nature Blast");
		}
        public override void SetDefaults()
        {
			projectile.height = 40;
			projectile.width = 40;
            Main.projFrames[projectile.type] = 4;
			projectile.penetrate = -1;
			projectile.hostile = true;
			projectile.timeLeft = 19;
			projectile.tileCollide = false;
			projectile.alpha = 0;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Color color = new Color(100, 120, 100, 0);
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < 4; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(projectile.Center.Y - (int)Main.screenPosition.Y) + y),
				new Rectangle(0, 40 * projectile.frame, 40, 40), color * (1f - (projectile.alpha / 255f)), projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		public override void AI()
        {
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 1.0f / 255f, (255 - projectile.alpha) * 2.5f / 255f, (255 - projectile.alpha) * 1.0f / 255f);
            projectile.frameCounter++;
            if (projectile.frameCounter >= 5)
            {
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 4;
            }
        }
	}
}
		