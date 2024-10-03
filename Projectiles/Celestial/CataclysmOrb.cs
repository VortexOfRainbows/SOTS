using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Lightning;
using SOTS.Dusts;

namespace SOTS.Projectiles.Celestial
{    
    public class CataclysmOrb : ModProjectile 
    {
		private Color dustColor => Color.Lerp(new Color(210, 255, 205, 100), new Color(30, 150, 60, 100), Main.rand.NextFloat(1));
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(48);
            AIType = 48; 
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 900;
			Projectile.alpha = 0;
		}
        public override void PostDraw(Color lightColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			Color color = new Color(100, 155, 100, 0);
			for (int i = 0; i < 360; i += 60)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(2.5f, 4f), 0).RotatedBy(MathHelper.ToRadians(i));
				Main.spriteBatch.Draw(texture, drawPos + circular, null, color * ((255f - Projectile.alpha) / 255f), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0.0f);
			}
			color = Color.White;
			Main.spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
        private bool RunOnce = true;
        public override void AI()
		{
			if(Projectile.timeLeft >= 800)
			{
				Projectile.timeLeft = Main.rand.Next(32, 48);
            }
            if (RunOnce)
            {
                RunOnce = false;
                for (int i = 0; i < 5; i++)
                    PixelDust.Spawn(Projectile.Center, 0, 0, Main.rand.NextVector2Square(-1.2f, 1.2f) + Projectile.velocity * 0.5f, dustColor, 6).scale = Main.rand.NextFloat(1.5f, 2f);
            }
            if (!Main.rand.NextBool(4))
            {
                PixelDust.Spawn(Projectile.Center, 0, 0, Main.rand.NextVector2Square(-0.17f, 0.17f) + Projectile.velocity * 0.65f, dustColor, 6).scale = Main.rand.NextFloat(1f, 1.5f);
            }
        }
		public override void OnKill(int timeLeft)
        {
			SOTSUtils.PlaySound(SoundID.NPCHit53, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.625f);
			for (int i = 0; i < 10; i++)
			{
				var num371 = Dust.NewDust(Projectile.Center - new Vector2(5) - new Vector2(10, 10), 24, 24, ModContent.DustType<CopyDust4>(), 0, 0, Projectile.alpha, dustColor, 1.6f);
				Dust dust = Main.dust[num371];
				dust.velocity += Projectile.velocity * 0.1f;
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
			}
			SOTSUtils.PlaySound(SoundID.Item94, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.55f, 0.1f);
			if (Projectile.owner == Main.myPlayer)
			{
				int amt = 1;
				if (Main.rand.NextBool(3))
					amt++;
				if (Main.rand.NextBool(3))
					amt++;
				for (int i = 0; i < amt; i++)
				{
					Vector2 circular = -Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, circular.X, circular.Y, ModContent.ProjectileType<CataclysmLightning>(), Projectile.damage, 0, Projectile.owner, 0);
				}
			}
		}
	}
}
			