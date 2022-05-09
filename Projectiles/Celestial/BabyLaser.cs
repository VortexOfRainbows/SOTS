using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Celestial
{
	public class BabyLaser : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Green Laser");
		}
		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.alpha = 80;
			Projectile.penetrate = -1;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(ModContent.BuffType<AbyssalInferno>(), 60, false);
		}
		public override bool ShouldUpdatePosition()
        {
			return false;
        }
		int counter = 0;
        public override void AI()
		{
			counter++;
			//Projectile.Center = npc.Center;
			if (Projectile.alpha <= 100)
            {
				for (int i = 0; i < 10; i++)
				{
					int dust3 = Dust.NewDust(Projectile.Center - new Vector2(12, 12) - new Vector2(5), 24, 24, ModContent.DustType<CopyDust4>());
					Dust dust4 = Main.dust[dust3];
					dust4.velocity *= 0.55f;
					dust4.velocity += Projectile.velocity.SafeNormalize(Vector2.Zero) * 2f;
					dust4.color = new Color(100, 255, 100, 0);
					dust4.noGravity = true;
					dust4.fadeIn = 0.1f;
					dust4.scale *= 2.75f;
				}
			}
			Projectile.alpha += 7;
			if (Projectile.alpha > 120)
			{
				Projectile.hostile = false;
			}
			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (!Projectile.hostile)
				return false;
			float laserDist = 200;
			Vector2 center = Projectile.Center;
			for (int i = 0; i < laserDist; i++)
			{
				Rectangle rect = new Rectangle((int)center.X - 9, (int)center.Y - 9, 18, 18);
				center += Projectile.velocity.SafeNormalize(new Vector2(1, 0)) * Projectile.width * 1.5f;
				if (rect.Intersects(targetHitbox))
					return true;
			}
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			float laserDist = 200;
			Vector2 center = Projectile.Center;
			for (int i = 0; i < laserDist; i++)
            {
				center += Projectile.velocity.SafeNormalize(new Vector2(1, 0)) * Projectile.width * 1.5f;
				spriteBatch.Draw(texture, center - Main.screenPosition, null, new Color(100, 255, 100, 0) * ((255f - Projectile.alpha) / 255f), Projectile.velocity.ToRotation(), origin, 1.5f, SpriteEffects.None, 0f);
				for (int j = 0; j < 2; j++)
				{
					float bonusAlphaMult = 1 - 1 * (counter / 28f);
					float dir = j * 2 - 1;
					Vector2 offset = new Vector2(counter * 1f * dir, 0).RotatedBy(Projectile.velocity.ToRotation() + MathHelper.ToRadians(90));
					Main.spriteBatch.Draw(texture, center - Main.screenPosition + offset, null, new Color(100, 255, 100, 0) * bonusAlphaMult * ((255f - Projectile.alpha) / 255f), Projectile.velocity.ToRotation(), origin, 1.5f, SpriteEffects.None, 0.0f);
				}
			}
			return false;
		}
	}
}