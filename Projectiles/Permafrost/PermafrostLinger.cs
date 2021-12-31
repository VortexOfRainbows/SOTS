using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Void;

namespace SOTS.Projectiles.Permafrost
{    
    public class PermafrostLinger : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lingering Permafrost");
		}
        public override void SetDefaults()
        {
			projectile.width = 42;
			projectile.height = 40;
			Main.projFrames[projectile.type] = 4;
			projectile.penetrate = -1;
			projectile.ranged = true;
			projectile.friendly = false;
			projectile.timeLeft = 180;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.alpha = 0;
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			VoidPlayer.VoidBurn(mod, target, 180);
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
				new Rectangle(0, 40 * projectile.frame, 40, 42), color * (1f - (projectile.alpha / 255f)), projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		public override bool PreAI()
		{
			projectile.rotation = projectile.ai[0];
			return base.PreAI();
		}
		public override void AI()
		{
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.15f / 255f, (255 - projectile.alpha) * 0.25f / 255f, (255 - projectile.alpha) * 0.65f / 255f);
			projectile.frameCounter++;
            if (projectile.frameCounter >= 8 && projectile.frame != 3)
            {
				projectile.friendly = false;
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 4;
            }
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item50, (int)projectile.Center.X, (int)projectile.Center.Y);
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 circularLocation = new Vector2(-Main.rand.NextFloat(5, 12), 0).RotatedBy(MathHelper.ToRadians(i) + projectile.rotation);
				int dust2 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, DustID.RainbowMk2);
				Dust dust = Main.dust[dust2];
				dust.velocity = circularLocation * 0.15f;
				dust.color = new Color(65, 136, 164);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
			}
		}
	}
}