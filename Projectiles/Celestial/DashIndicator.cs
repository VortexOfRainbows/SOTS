using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Celestial
{    
    public class DashIndicator : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursespire");
		}
        public override bool PreDraw(ref Color lightColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 velo = Projectile.velocity.SafeNormalize(Vector2.Zero) * texture.Height;
			Color color = new Color(100, 255, 100, 0);
			if (Projectile.ai[1] == -1)
				color = new Color(255, 100, 100, 0);
			for(int i = -40; i < 40; i++)
            {
				for(int j = 0; j < 2; j++)
				{
					float alphaMult = 0.04f;
					float dir = j * 2 - 1;
					for(int k = 0; k < 12; k++)
					{
						alphaMult += 0.08f;
						Vector2 offset = new Vector2((12 + k * 2) * dir, 0).RotatedBy(Projectile.rotation);
						Vector2 drawPos = Projectile.Center + velo * i;
						Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition + offset, null, color * alphaMult * ((255f - Projectile.alpha) / 255f), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.0f);
					}
				}
				for (int j = 0; j < 2; j++)
				{
					float bonusAlphaMult = 1 - 1 * (counter / 16f);
					float alphaMult = 0.04f;
					float dir = j * 2 - 1;
					for (int k = 0; k < 12; k++)
					{
						alphaMult += 0.04f;
						Vector2 offset = new Vector2((counter + 12 + k * 2) * dir, 0).RotatedBy(Projectile.rotation);
						Vector2 drawPos = Projectile.Center + velo * i;
						Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition + offset, null, color * alphaMult * bonusAlphaMult * ((255f - Projectile.alpha) / 255f), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.0f);
					}
				}
			}
			return false;
        }
        public override void SetDefaults()
        {
			Projectile.width = 60;
			Projectile.height = 60;
			Projectile.friendly = false;
			Projectile.timeLeft = 152;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 255;
		}
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
		int counter = 0;
        public override void AI()
		{
			counter++;
			if (counter > 16)
				counter = 0;
			if (Projectile.timeLeft > 38)
				Projectile.alpha--;
			else
				Projectile.alpha += 3;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
		}
	}
}
		