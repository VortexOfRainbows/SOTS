using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Celestial
{    
    public class DashIndicator2 : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursespire");
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 velo = projectile.velocity.SafeNormalize(Vector2.Zero) * texture.Height;
			Color color = new Color(255, 100, 100, 0);
			for(int i = -40; i < 40; i++)
            {
				for(int j = 0; j < 2; j++)
				{
					float alphaMult = 0.04f;
					float dir = j * 2 - 1;
					for(int k = 0; k < 12; k++)
					{
						alphaMult += 0.08f;
						Vector2 offset = new Vector2((12 + k * 2) * dir, 0).RotatedBy(projectile.rotation);
						Vector2 drawPos = projectile.Center + velo * i;
						Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition + offset, null, color * alphaMult * ((255f - projectile.alpha) / 255f), projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);
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
						Vector2 offset = new Vector2((counter + 12 + k * 2) * dir, 0).RotatedBy(projectile.rotation);
						Vector2 drawPos = projectile.Center + velo * i;
						Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition + offset, null, color * alphaMult * bonusAlphaMult * ((255f - projectile.alpha) / 255f), projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);
					}
				}
			}
			return false;
        }
        public override void SetDefaults()
        {
			projectile.width = 60;
			projectile.height = 60;
			projectile.friendly = false;
			projectile.timeLeft = 142;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 255;
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
			if (projectile.timeLeft > 38)
				projectile.alpha--;
			else
				projectile.alpha += 3;
			if (projectile.timeLeft <= 10)
				projectile.alpha++;
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
			if(projectile.timeLeft == 26)
			{
				//SoundEngine.PlaySound(SoundID.Item119, (int)projectile.Center.X, (int)projectile.Center.Y);
				if (Main.netMode != 1)
				{
					Vector2 center = projectile.Center - projectile.velocity.SafeNormalize(new Vector2(1, 0)) * 1500 * projectile.ai[0];
					Vector2 velo = projectile.velocity.SafeNormalize(new Vector2(1, 0)) * 56 * projectile.ai[0];
					Projectile.NewProjectile(center, velo, ModContent.ProjectileType<EnergySerpentHead2>(), projectile.damage, 0, Main.myPlayer, 51, -2);
				}
			}
		}
	}
}
		