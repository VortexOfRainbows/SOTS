using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Projectiles.Otherworld
{    
    public class AvaritianExplosion : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Avaritian Explosion");
			Main.projFrames[Projectile.type] = 5;
		}
        public override void SetDefaults()
        {
			Projectile.height = 70;
			Projectile.width = 70;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.timeLeft = 24;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.alpha = 0;
		}
        public override bool PreDraw(ref Color lightColor)
        {
			return false;
		}
        public override void PostDraw(Color lightColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.25f;
				float y = Main.rand.Next(-10, 11) * 0.25f;
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(Projectile.Center.Y - (int)Main.screenPosition.Y) + y),
				new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), color * (1f - (Projectile.alpha / 255f)), Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
			base.PostDraw(lightColor);
		}
		public override void AI()
        {
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 1.25f / 255f, (255 - Projectile.alpha) * 1.25f / 255f, (255 - Projectile.alpha) * 1.5f / 255f);
			Projectile.knockBack = 3.5f;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
				Projectile.friendly = false;
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 5;
            }
        }
	}
}